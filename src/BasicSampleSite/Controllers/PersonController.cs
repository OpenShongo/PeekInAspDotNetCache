using System.Collections.Generic;
using System.Net;
using System.Runtime.Caching;
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
        [PeekInCache(nameof(NameCache))]
        private static readonly Dictionary<string, IEnumerable<Name>> NameCache = new Dictionary<string, IEnumerable<Name>>();

        [PeekInCache(nameof(PersonMemoryCache))]
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

        [HttpGet("names/list/{limit}")]
        [ProducesResponseType(typeof(IEnumerable<Person>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetNameListAsync(int limit)
        {
            var key = $"{nameof(GetNameListAsync)}_{limit}";
            var persons = await GetNames(key, limit);

            return Ok(persons);
        }

        private async Task<IEnumerable<Name>> GetNames(string key, int limit)
        {
            if (NameCache.ContainsKey(key))
                return NameCache[key];

            var names = await _personService.FetchRandomSampleOfNames(limit);
            NameCache[key] = names;
            return NameCache[key];
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