using LR.Application.AppResult.ResultData;
using LR.Application.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace LR.Persistance.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<TItem>> ToPagedAsync<TItem>(this IQueryable<TItem> query, int pageNumber, int pageSize, CancellationToken ct = default)
            where TItem : class
        {
            var totalCount = await query.CountAsync(ct);

            var pagedList = await query
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PaginatedResult<TItem>(pageNumber, pageSize, totalCount, pagedList);
        }

        public static IQueryable<TItem> ApplySorting<TItem>(this IQueryable<TItem> query, IReadOnlyList<SortDescriptorDto>? sort)
        {
            if (sort == null || sort.Count == 0)
                return query;

            IOrderedQueryable<TItem>? orderedQuery = null;

            foreach (var (descriptor, index) in sort.Select((d, i) => (d, i)))
            {
                var propInfo = typeof(TItem).GetProperty(descriptor.Property);

                if (!IsSortableType(propInfo))
                    continue;

                orderedQuery = ApplyOrder(
                    index == 0 ? query : orderedQuery!,
                    descriptor.Property,
                    descriptor.Desc,
                    index == 0
                );
            }

            return orderedQuery ?? query;
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, bool desc, bool first)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var member = Expression.PropertyOrField(parameter, property);
            var keySelector = Expression.Lambda(member, parameter);

            var method = first
                ? (desc ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy))
                : (desc ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy));

            var call = Expression.Call(
                typeof(Queryable),
                method,
                [typeof(T), member.Type],
                source.Expression,
                Expression.Quote(keySelector)
            );

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        private static bool IsSortableType(PropertyInfo? propertyInfo)
        {
            if (propertyInfo == null)
                return false;

            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(propertyInfo.PropertyType)
            && propertyInfo.PropertyType != typeof(string))
                return false;

            var underlying = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

            return underlying.IsPrimitive
                   || underlying == typeof(string)
                   || underlying == typeof(DateTime)
                   || underlying == typeof(decimal)
                   || underlying == typeof(Guid)
                   || underlying == typeof(DateTimeOffset);
        }
    }
}