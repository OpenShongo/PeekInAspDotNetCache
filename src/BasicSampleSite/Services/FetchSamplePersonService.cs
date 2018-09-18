using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BasicSampleSite.Models;
using Newtonsoft.Json;

namespace BasicSampleSite.Services
{
    public class FetchSamplePersonService
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<IEnumerable<Name>> FetchRandomSampleOfNames(int limit)
        {
            var response = await HttpClient.GetAsync($"https://randomuser.me/api/?results={limit}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseResult>(content);
            return result.Results.Select(x => x.Name);
        }

        public async Task<IEnumerable<Person>> FetchRandomSampleOfPersons(int limit)
        {
            var response = await HttpClient.GetAsync($"https://randomuser.me/api/?results={limit}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseResult>(content);
            return result.Results;
        }
    }
}
