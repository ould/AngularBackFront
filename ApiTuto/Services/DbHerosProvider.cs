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
        private readonly string _connectionString;

        public DbHerosProvider(IConfiguration configuration)
		{
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("dbTestVincent") ?? "";
        }
        

        public async Task<IEnumerable<IHeros>> GetAllHeroes()
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                IEnumerable<IHeros> heros = await connection.QueryAsync<Hero>("select * from Heros");
                return heros;
            }
        }

        public async Task<IHeros> GetHeroById(int id)
        {
                IHeros heros = await QueryFistHeroOrDefautAsync("select * from Heros where id = @id ", new Hero { id = id });
                return heros;          
        }

        public async Task<IHeros> PostHero(IHeros value)
        {
                var result = await ExecuteAsyncHeros("insert into Heros (Name) values (@name)", value);
                if (result == 1)return await QueryFistHeroOrDefautAsync("select * from Heros where Name = @name order by Id Desc ", value);

                throw new Exception("Erreur dans la requete Post Heros");
            
        }

        public async Task<IHeros> PutHero(int id, IHeros value)
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
            if (result != 1) throw new Exception("Erreur dans la requete Delete Heros");
        }


        private async Task<int> ExecuteAsyncHeros(string query, IHeros heros)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(query, new { heros.id,  heros.name});

        }

        private async Task<IHeros> QueryFistHeroOrDefautAsync(string query, IHeros heros)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Hero>(query, new { heros.id, heros.name });

        }

    }
}

