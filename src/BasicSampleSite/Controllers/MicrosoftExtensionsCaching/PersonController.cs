using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BasicSampleSite.Models;
using BasicSampleSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PeekInCache.AspDotNetCore;

namespace BasicSampleSite.Controllers.MicrosoftExtensionsCaching
{
    [Route("api/mec/person")]
    public class PersonController : Controller
    {
        [PeekInCache("Mec.PersonMemoryCache")]
        private readonly IMemoryCache _memoryCache;

        private readonly FetchSamplePersonService _personService;

        public PersonController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
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
            if (_memoryCache.TryGetValue(key, out var output))
                return (IEnumerable<Person>)output;

            var persons = await _personService.FetchRandomSampleOfPersons(limit);
            return _memoryCache.Set(key, persons);
        }
    }
}