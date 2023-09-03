using ApiTuto.Models;

namespace ApiTuto
{
    public interface IHerosProvider
    {
        Task<IEnumerable<IHeros>> GetAllHeroes();
        Task<IHeros> GetHeroById(int id);
        Task<IHeros> PostHero(IHeros value);
        Task<IHeros> PutHero(int id, IHeros value);
        Task DeleteHero(int id);
    }
}