using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTuto.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiTuto.Controllers
{
    [EnableCors("HeroesPolicy")]
    [Route("api/[controller]")]
    public class HerosController : Controller
    {

        private readonly IHerosProvider herosProvider;

        public HerosController(IHerosProvider herosProvider)
        {            
            this.herosProvider = herosProvider;
        }


        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allHeroes = await herosProvider.GetAllHeroes();

            return Json(allHeroes);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var herosResult = await herosProvider.GetHeroById(id);

            return Json(herosResult);
        }

        //POST api/values
       [HttpPost]
        public async Task<IActionResult> Post([FromBody] Hero value) 
        {
            var idResult = await herosProvider.PostHero(value);

            return Json(idResult);

        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Hero value)
        {
            
            var result = await herosProvider.PutHero(value.id, value);

            return Json(result);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await herosProvider.DeleteHero(id);
            return Ok();
        }
    }
}

