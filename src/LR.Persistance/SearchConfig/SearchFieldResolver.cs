using System.Linq.Expressions;

namespace LR.Persistance.SearchConfig
{
    public static class SearchFieldResolver
    {
        /// <summary>
        /// Resolves a set of entity string fields for searching based on requested fields and a predefined map.
        /// </summary>
        /// <typeparam name="TEntity">The entity type containing the fields.</typeparam>
        /// <typeparam name="TField">An enum type representing the searchable fields.</typeparam>
        /// <param name="requested">
        /// The collection of requested fields from the client. 
        /// Only fields present in the map will be used. If null or empty, all fields in the map are returned.
        /// </param>
        /// <param name="map">
        /// A dictionary mapping enum values of <typeparamref name="TField"/> to expressions selecting string properties from <typeparamref name="TEntity"/>.
        /// </param>
        /// <returns>
        /// An array of expressions representing the entity fields that should be searched.
        /// </returns>
        public static Expression<Func<TEntity, string?>>[] Resolve<TEntity, TField>(
            IReadOnlyCollection<TField>? requested,
            IReadOnlyDictionary<TField, Expression<Func<TEntity, string?>>> map)
            where TField : struct, Enum
        {
            if (requested is { Count: > 0 })
                return requested
                    .Distinct()
                    .Where(map.ContainsKey)
                    .Select(f => map[f])
                    .ToArray();

            return map.Values.ToArray();
        }
    }
}
