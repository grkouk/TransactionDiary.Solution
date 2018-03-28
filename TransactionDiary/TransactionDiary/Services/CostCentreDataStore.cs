using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TransactionDiary.Models;

namespace TransactionDiary.Services
{
    public class CostCentreDataStore:IDataStore<CostCentre,CostCentre,CostCentre>
    {
        private const string BaseUrl = "http://testapi.potos.tours/api/CοstCentres";
        //private const string BaseUrl = "http://localhost:60928/api/CostCentres/";

        public async Task<IEnumerable<CostCentre>> GetItemsAsync()
        {
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(BaseUrl);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var facilitiesList = JsonConvert.DeserializeObject<List<CostCentre>>(jsonContent);
                    return facilitiesList;

                }

                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
                //throw;
            }
        }

        public Task<IEnumerable<CostCentre>> GetItemsInPeriodAsync(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SearchListItem>> GetItemsSearchListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddItemAsync(CostCentre item)
        {
            throw new NotImplementedException();
        }

        public async Task<CostCentre> AddItemAsync2(CostCentre item)
        {
            var httpClient = new HttpClient();
            var jsonItem = JsonConvert.SerializeObject(item);
            try
            {

                var uri = new Uri(BaseUrl);
                HttpContent httpContent = new StringContent(jsonItem);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpClient.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync(uri, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var newItem = JsonConvert.DeserializeObject<CostCentre>(jsonContent);
                    return newItem;
                }
                else
                {
                    var e = new Exception(response.ToString());
                    throw e;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return await Task.FromResult(null);
                throw e;

            }
        }

        public Task<CostCentre> ModifyItemAsync(int id, CostCentre item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
