using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BasicSampleSite.Models;
using BasicSampleSite.Services;
using Microsoft.AspNetCore.Mvc;
using PeekInCache.AspDotNetCore;

namespace BasicSampleSite.Controllers
{
    [Route("api/person")]
    public class PersonController : Controller
    {
        [PeekInCache(nameof(PersonCache))]
        private static readonly Dictionary<string, IEnumerable<Person>> PersonCache = new Dictionary<string, IEnumerable<Person>>();
        private readonly FetchSamplePersonService _personService;

        public PersonController()
        {
            _personService = new FetchSamplePersonService();
        }

        [HttpGet("list/{limit}")]
        [ProducesResponseType(typeof(IEnumerable<Person>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPersonListAsync(int limit)
        {
            var key = $"{nameof(GetPersonListAsync)}_{limit}";
            var persons = await GetPersons(key, limit);
                
            return Ok(persons);
        }

        private async Task<IEnumerable<Person>> GetPersons(string key, int limit)
        {
            if (PersonCache.ContainsKey(key))
                return PersonCache[key];

            var persons = await _personService.FetchRandomSampleOfPersons(limit);
            PersonCache[key] = persons;
            return PersonCache[key];
        }
    }
}