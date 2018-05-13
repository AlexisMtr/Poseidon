using System.Linq;

namespace Poseidon.Filters
{
    public interface IFilter<T>
    {
        IQueryable<T> Filter(IQueryable<T> source);
    }

    public interface IFilter<TSource, TResult>
    {
        IQueryable<TResult> Filter(IQueryable<TSource> source);
    }
}
