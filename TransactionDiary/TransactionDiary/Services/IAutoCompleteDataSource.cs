using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionDiary.Models;

namespace TransactionDiary.Services
{
    public interface IAutoCompleteDataSource<T>
    {
        Task<IEnumerable<SearchListItem>> GetSearchListItemsAsync();
        Task<IList<SearchListItem>> GetSearchListItemsLightAsync();
    }
}
