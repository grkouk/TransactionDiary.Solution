using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TransactionDiary.Models;

namespace TransactionDiary.Services
{
    public class CompanyAutoCompleteDs : IAutoCompleteDataSource<Company>
    {
        private const string BaseUrl = "http://testapi.potos.tours/api/Companies";
        //private const string BaseUrl = "http://localhost:60928/api/Companies/";
        public async Task<IEnumerable<SearchListItem>> GetSearchListItemsAsync()
        {
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(BaseUrl + "/companiessearchlist");

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsSearchList = JsonConvert.DeserializeObject<List<SearchListItem>>(jsonContent);
                    return itemsSearchList;

                }

                var emptyItems = new List<SearchListItem>();
                return emptyItems;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return null;
                throw (e);
            }
        }

        public Task<IList<SearchListItem>> GetSearchListItemsLightAsync()
        {
            throw new NotImplementedException();
        }
    }
}