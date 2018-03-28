using System.Threading.Tasks;
using TransactionDiary.Models;

namespace TransactionDiary.Services
{
    public interface ITransactionDataStore<T1, T2, T3>:IDataStore<T1,T2,T3>
    {
        Task<SearchListItem> GetCategoryIdOfTransactorsLastTransactionAsync(int transactorId);
        Task<SearchListItem> GetCostCentreIdOfTransactorsLastTransactionAsync(int transactorId);
        Task<SearchListItem> GetRevenueCentreIdOfTransactorsLastTransactionAsync(int transactorId);
    }
}
