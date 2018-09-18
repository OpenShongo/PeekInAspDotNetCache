using System.Collections.Generic;
using System.Net;
using System.Runtime.Caching;
using System.Threading.Tasks;
using BasicSampleSite.Models;
using BasicSampleSite.Services;
using Microsoft.AspNetCore.Mvc;
using PeekInCache.AspDotNetCore;

namespace BasicSampleSite.Controllers.Dictionary
{
    [Route("api/dictionary/person")]
    [Route("api/dict/person")]
    public class PersonController : Controller
    {
        [PeekInCache("Dict_"+ nameof(PersonMemoryCache))]
        private static readonly MemoryCache PersonMemoryCache = MemoryCache.Default;

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
            if (PersonMemoryCache.Contains(key))
                return (IEnumerable<Person>)PersonMemoryCache[key];

            var persons = await _personService.FetchRandomSampleOfPersons(limit);
            PersonMemoryCache[key] = persons;
            return (IEnumerable<Person>)PersonMemoryCache[key];
        }
    }
}