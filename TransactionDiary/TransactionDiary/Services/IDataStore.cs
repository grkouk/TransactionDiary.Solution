using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionDiary.Models;

namespace TransactionDiary.Services
{/// <summary>
/// Data store Interface
/// <typeparamref name="T1"/> is List Type 
/// <typeparamref name="T2"/> Create DTO Type
/// <typeparamref name="T3"/> Modify DTO Type
/// </summary>
/// <typeparam name="T1">List type</typeparam>
/// <typeparam name="T2">Create Param</typeparam>
/// <typeparam name="T3">Modify Param</typeparam>
    public interface IDataStore<T1,T2,T3>
    {
        Task<IEnumerable<T1>> GetItemsAsync();
        Task<IEnumerable<T1>> GetItemsInPeriodAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<SearchListItem>> GetItemsSearchListAsync();

        Task<bool> AddItemAsync(T2 item);
        Task<T1> AddItemAsync2(T2 item);
        Task<T3> ModifyItemAsync(int id, T3 item);
        Task<bool> DeleteItemAsync(int id);
    }
}
