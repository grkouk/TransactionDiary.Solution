﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TransactionDiary.Models;

namespace TransactionDiary.Services
{
    public class TransactorsDataStore : IDataStore<Transactor,Transactor,Transactor>
    {
        private const string BaseUrl = "http://testapi.potos.tours/api/transactors";
        //private const string BaseUrl = "http://localhost:60928/api/transactors/";
        public async Task<IEnumerable<Transactor>> GetItemsAsync()
        {
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(BaseUrl);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var transactorList = JsonConvert.DeserializeObject<List<Transactor>>(jsonContent);
                    return transactorList;

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

        public Task<IEnumerable<Transactor>> GetItemsInPeriodAsync(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SearchListItem>> GetItemsSearchListAsync()
        {
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(BaseUrl + "/transactorssearchlist");

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsSearchList = JsonConvert.DeserializeObject<List<SearchListItem>>(jsonContent);
                    return itemsSearchList;

                }
                //TODO: Should not return null
                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
                //throw;
            }
        }

        public Task<bool> AddItemAsync(Transactor item)
        {
            throw new NotImplementedException();
        }

        public async Task<Transactor> AddItemAsync2(Transactor item)
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
                    var newItem = JsonConvert.DeserializeObject<Transactor>(jsonContent);
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

        public Task<Transactor> ModifyItemAsync(int id, Transactor item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
