using Poseidon.Filters;
using System.Linq;

namespace Poseidon.Helpers
{
    public static class LinqExtension
    {
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
    }
}
