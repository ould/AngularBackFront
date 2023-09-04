using System;
using ApiTuto.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace ApiTuto.Services
{
	public class DbHerosProvider : IHerosProvider
	{

        private readonly IConfiguration _configuration;
        private readonly ILogger<DbHerosProvider> _logger;
        private readonly string _connectionString;

        public DbHerosProvider(IConfiguration configuration, ILogger<DbHerosProvider> logger)
		{
            _configuration = configuration;
            this._logger = logger;
            _connectionString = _configuration.GetConnectionString("dbTestVincent") ?? "";
        }
        

        public async Task<IEnumerable<IHero>> GetAllHeroes()
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                IEnumerable<IHero> heros = await connection.QueryAsync<Hero>("select * from Heros");
                return heros;
            }
        }

        public async Task<IHero> GetHeroById(int id)
        {
                IHero heros = await QueryFistHeroOrDefautAsync("select * from Heros where id = @id ", new Hero { id = id });
                return heros;          
        }

        public async Task<IHero> PostHero(IHero value)
        {
                var result = await ExecuteAsyncHeros("insert into Heros (Name) values (@name)", value);
                if (result == 1)return await QueryFistHeroOrDefautAsync("select * from Heros where Name = @name order by Id Desc ", value);

                throw new Exception("Erreur dans la requete Post Heros");
            
        }

        public async Task<IHero> PutHero(int id, IHero value)
        {
            
            var heroExistant = await GetHeroById(id);

            //Si le hero existe, alors on l'update, sinon il faut le créer 
            if (heroExistant.id != 0)
            {
                var result = await ExecuteAsyncHeros("update Heros set Name = @name where Id = @id", value);
                if (result == 1) return await GetHeroById(id);
            }
            else
            {
                return await PostHero(value);
            }

            throw new Exception("Erreur dans la requete Put Heros");
        }

        public async Task DeleteHero(int id)
        {
            var result = await ExecuteAsyncHeros("delete from Heros where Id = @id", new Hero {id = id });
            if (result != 1) _logger.LogWarning("Error in {methodName} : Could not find id {id} ", nameof(DeleteHero), id ); //TODO gestion des erreurs
        }


        private async Task<int> ExecuteAsyncHeros(string query, IHero heros)
        {
            using SqlConnection connection = new(_connectionString);
            return await connection.ExecuteAsync(query, new { heros.id,  heros.name});

        }

        private async Task<IHero> QueryFistHeroOrDefautAsync(string query, IHero heros)
        {
            using SqlConnection connection = new(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Hero>(query, new { heros.id, heros.name });

        }

    }
}

