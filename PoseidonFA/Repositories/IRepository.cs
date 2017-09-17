namespace PoseidonFA.Repositories
{
    public interface IRepository<T>
    {
        void Add(T model);
        void Update(string id, T model);
    }
}
