using System;
using System.Linq;

namespace Poseidon.Filters
{
    public interface IFilter<T> : ICloneable
    {
        IQueryable<T> Filter(IQueryable<T> source);
    }

    public interface IFilter<TSource, TResult> : ICloneable
    {
        IQueryable<TResult> Filter(IQueryable<TSource> source);
    }
}
