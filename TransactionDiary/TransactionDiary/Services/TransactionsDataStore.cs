using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GrKouk.Api.Dtos;
using Newtonsoft.Json;
using TransactionDiary.Models;

namespace TransactionDiary.Services
{
    public class TransactionsDataStore:IDataStore<TransactionDto,TransactionCreateDto,TransactionDto>
    {
        private const string BaseUrl = "http://testapi.potos.tours/api/transactions";
       //private const string BaseUrl = "http://localhost:60928/api/transactions/";

        public async Task<IEnumerable<TransactionDto>> GetItemsAsync()
        {
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(BaseUrl);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<TransactionDto>>(jsonContent);
                    return itemsList;

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

        public async Task<IEnumerable<TransactionDto>> GetItemsInPeriodAsync(DateTime fromDate, DateTime toDate)
        {
            var httpClient = new HttpClient();

            try
            {
                var fDate = fromDate.ToString("yyyy-MM-dd");

                var tDate = toDate.ToString("yyyy-MM-dd");

                var uri = new Uri(BaseUrl + $"/transactionsinperiod?fromdate={fDate}&todate={tDate}");

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<TransactionDto>>(jsonContent);
                    return itemsList;

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
               // return null;
                throw e;
            }
        }

        public Task<IEnumerable<SearchListItem>> GetItemsSearchListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddItemAsync(TransactionCreateDto item)
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
                    return await Task.FromResult(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(false);
                //throw;
            }
            return await Task.FromResult(false);
        }

        public async Task<TransactionDto> AddItemAsync2(TransactionCreateDto item)
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
                    var newItem = JsonConvert.DeserializeObject<TransactionDto>(jsonContent);
                    return newItem;
                }
                else
                {
                    var e =new Exception(response.ToString());
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

        public Task<TransactionDto> ModifyItemAsync(int id, TransactionDto item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
