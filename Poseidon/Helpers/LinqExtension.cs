using Microsoft.AspNet.OData.Query;
using Poseidon.Filters;
using System.Linq;

namespace Poseidon.Helpers
{
    public static class LinqExtension
    {
        #region Poseidon Filters
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IFilter<T> filter)
        {
            return filter.Filter(source);
        }

        public static IQueryable<TResult> Where<TSource, TResult>(this IQueryable<TSource> source, IFilter<TSource, TResult> filter)
        {
            return filter.Filter(source);
        }

        public static T FirstOrDefault<T>(this IQueryable<T> source, IFilter<T> filter)
        {
            return filter.Filter(source).FirstOrDefault();
        }

        public static TResult FirstOrDefault<TSource, TResult>(this IQueryable<TSource> source, IFilter<TSource, TResult> filter)
        {
            return filter.Filter(source).FirstOrDefault();
        }

        public static int Count<T>(this IQueryable<T> source, IFilter<T> filter)
        {
            return filter.Filter(source).Count();
        }

        public static int Count<TSource, TResult>(this IQueryable<TSource> source, IFilter<TSource, TResult> filter)
        {
            return filter.Filter(source).Count();
        }
        #endregion

        #region OData Filters
        private static readonly ODataQuerySettings defaultODataQuerySettings = new ODataQuerySettings();

        public static IQueryable<T> OData<T>(this IQueryable<T> source, ODataQueryOptions queryOptions)
        {
            return queryOptions.ApplyTo(source).Cast<T>();
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, ODataQueryOptions queryOptions)
        {
            return queryOptions.Filter.ApplyTo(source, defaultODataQuerySettings).Cast<T>();
        }

        public static int Count<T>(this IQueryable<T> source, ODataQueryOptions queryOptions)
        {
            return source.Where(queryOptions).Count();
        }

        public static T FirstOrDefault<T>(this IQueryable<T> source, ODataQueryOptions queryOptions)
        {
            return source.Where(queryOptions).FirstOrDefault();
        }

        public static IQueryable Select(this IQueryable source, ODataQueryOptions queryOptions)
        {
            if (queryOptions.SelectExpand == null) return source;

            return queryOptions.SelectExpand.ApplyTo(source, defaultODataQuerySettings);
        }
        #endregion
    }
}
