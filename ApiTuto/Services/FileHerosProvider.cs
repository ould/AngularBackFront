using System;
using System.Collections.Generic;
using ApiTuto.Models;
using Newtonsoft.Json.Linq;

namespace ApiTuto.Services
{
	public class FileHerosProvider : IHerosProvider
	{
        private string filePathDb = "./db.txt";       

        public async Task<IEnumerable<IHeros>> GetAllHeroes()
        {
            var herosDbFromFile = await File.ReadAllTextAsync(filePathDb);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Hero>>(herosDbFromFile) ?? new List<Hero>();
        }

        public async Task<IHeros> GetHeroById(int id)
        {
            return (await GetAllHeroes()).Where(h => h.id == id).FirstOrDefault();
        }

        public async Task<IHeros> PostHero(IHeros value)
        {
            var listeHerosAvantPost = await GetAllHeroes();
            var lastId = listeHerosAvantPost.LastOrDefault()?.id ?? 0;
            var newHero = value;
            newHero.id = lastId + 1;

            var listeHerosApresPost = listeHerosAvantPost.Append(newHero).ToList();
            var newListeHerosText = Newtonsoft.Json.JsonConvert.SerializeObject(listeHerosApresPost);
            System.IO.File.WriteAllText(filePathDb, newListeHerosText);
            return newHero;
        }

        public async Task<IHeros> PutHero(int id, IHeros value)
        {
            var listeHeros = await GetAllHeroes();
            var heroAUpdate = listeHeros.Where(h => h.id == id).FirstOrDefault();

            if(heroAUpdate == null)
            {
                return await PostHero(value);
            }
            else
            {
                heroAUpdate = value;
                listeHeros.Where(x => x.id == id).ToList()
                    .ForEach(x => x.name = value.name); //TODO : faire en sorte que ça remplace tout le heros

                var newListeHerosText = Newtonsoft.Json.JsonConvert.SerializeObject(listeHeros);
                System.IO.File.WriteAllText(filePathDb, newListeHerosText);
                return heroAUpdate;
            }

        }


        public async Task DeleteHero(int id)
        {
            var listeHerosAvantPost = await GetAllHeroes();
            var nouvelleListeSansHeroDeleted = listeHerosAvantPost.Where(h => h.id != id).ToList();

            if (nouvelleListeSansHeroDeleted.Count() != listeHerosAvantPost.Count())
            {
                var newListeHerosText = Newtonsoft.Json.JsonConvert.SerializeObject(nouvelleListeSansHeroDeleted);
                System.IO.File.WriteAllText(filePathDb, newListeHerosText);
            }          
        }
    }
}

