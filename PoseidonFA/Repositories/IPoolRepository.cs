using PoseidonFA.Models;

namespace PoseidonFA.Repositories
{
    public interface IPoolRepository
    {
        Pool Get(int id);
    }
}