using LR.Application.AppResult.ResultData;
using LR.Application.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace LR.Persistance.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Paginates the given <see cref="IQueryable{TItem}"/> by applying skip and take based on the specified page number and page size.
        /// </summary>
        /// <typeparam name="TItem">The entity type of the query. Must be a class.</typeparam>
        /// <param name="query">The source query to paginate.</param>
        /// <param name="pageNumber">The 1-based page number. Values less than 1 will skip 0 items.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="ct">Optional <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>
        /// A <see cref="PaginatedResult{TItem}"/> containing the current page, page size, total count of items, and the items for the current page.
        /// </returns>
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

        /// <summary>
        /// Applies a text search filter to the given <see cref="IQueryable{TItem}"/> based on the specified fields.
        /// </summary>
        /// <typeparam name="TItem">The entity type of the query.</typeparam>
        /// <param name="query">The source query to filter.</param>
        /// <param name="search">The search string to match. If null or whitespace, the query is returned unmodified.</param>
        /// <param name="fields">
        /// The collection of string fields to search in. Each field is an expression selecting a string property from <typeparamref name="TItem"/>.
        /// If empty, the query is returned unmodified.
        /// </param>
        /// <returns>
        /// A filtered <see cref="IQueryable{TItem}"/> where any of the specified fields contain the search string, ignoring null values.
        /// Multiple fields are combined with logical OR.
        /// </returns>
        public static IQueryable<TItem> ApplyTextSearch<TItem>(this IQueryable<TItem> query, string? search, IReadOnlyCollection<Expression<Func<TItem, string?>>> fields)
        {
            if (string.IsNullOrWhiteSpace(search) || fields.Count == 0)
                return query;

            var parameter = Expression.Parameter(typeof(TItem), "x");
            Expression? body = null;

            foreach (var field in fields)
            {
                var replaced = ReplaceParameter(field, parameter);

                var notNull = Expression.NotEqual(
                    replaced,
                    Expression.Constant(null, typeof(string))
                );

                var contains = Expression.Call(
                    replaced,
                    nameof(string.Contains),
                    Type.EmptyTypes,
                    Expression.Constant(search)
                );

                var condition = Expression.AndAlso(notNull, contains);

                body = body is null
                    ? condition
                    : Expression.OrElse(body, condition);
            }

            var lambda = Expression.Lambda<Func<TItem, bool>>(body!, parameter);

            return query.Where(lambda);
        }

        private static Expression ReplaceParameter<T>(Expression<Func<T, string?>> expr, ParameterExpression parameter)
        {
            return new ParameterReplaceVisitor(expr.Parameters[0], parameter)
                .Visit(expr.Body)!;
        }

        private sealed class ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
        {
            private readonly ParameterExpression _from = from;
            private readonly ParameterExpression _to = to;

            protected override Expression VisitParameter(ParameterExpression node)
                => node == _from ? _to : base.VisitParameter(node);
        }

        /// <summary>
        /// Applies multi-column sorting to the given <see cref="IQueryable{TItem}"/> based on the provided sort descriptors.
        /// </summary>
        /// <typeparam name="TItem">The entity type of the query.</typeparam>
        /// <param name="query">The source query to sort.</param>
        /// <param name="sort">
        /// A list of <see cref="SortDescriptorDto"/> describing the properties to sort by and the sort direction.
        /// If null or empty, the query is returned unmodified.
        /// </param>
        /// <returns>
        /// A sorted <see cref="IQueryable{TItem}"/> according to the specified descriptors. 
        /// If none of the descriptors are valid sortable properties, the original query is returned.
        /// Multiple descriptors are applied in the order they appear in the list.
        /// </returns>
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