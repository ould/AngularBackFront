using ApiTuto.Models;

namespace ApiTuto
{
    public interface IHerosProvider
    {
        Task<IEnumerable<IHero>> GetAllHeroes();
        Task<IHero> GetHeroById(int id);
        Task<IHero> PostHero(IHero value);
        Task<IHero> PutHero(int id, IHero value);
        Task DeleteHero(int id);
    }
}