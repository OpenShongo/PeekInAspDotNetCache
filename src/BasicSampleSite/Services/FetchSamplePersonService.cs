using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BasicSampleSite.Models;
using Newtonsoft.Json;

namespace BasicSampleSite.Services
{
    public class FetchSamplePersonService
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<IEnumerable<Person>> FetchRandomSampleOfPersons(int limit)
        {
            var personList = new List<Person>();


            var response = await HttpClient.GetAsync($"https://randomuser.me/api/?results={limit}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseResult>(content);
            personList.AddRange(result.Results);

            return personList;
        }
    }
}
