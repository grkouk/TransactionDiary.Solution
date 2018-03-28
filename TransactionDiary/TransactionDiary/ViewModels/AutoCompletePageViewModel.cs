using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Navigation;
using TransactionDiary.Models;
using TransactionDiary.Services;

namespace TransactionDiary.ViewModels
{
	public class AutoCompletePageViewModel : ViewModelBase
    {
        private IDataStore<Transactor,Transactor,Transactor> _transactorDS;

        //private bool _dsInitialized = false;

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public AutoCompletePageViewModel(INavigationService navigationService, IDataStore<Transactor,Transactor,Transactor> transactorDs) : base(navigationService)
        {
            _transactorDS = transactorDs;
        }
        private ObservableCollection<Transactor> _transactorsCollection;

        public ObservableCollection<Transactor> TransactorsCollection
        {
            get => _transactorsCollection;
            set => SetProperty(ref _transactorsCollection, value);
        }
        private async Task RefreshDataAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                if (TransactorsCollection == null)
                {
                    TransactorsCollection = new ObservableCollection<Transactor>();
                }
                TransactorsCollection.Clear();
                var items = await GetItemsAsync();
                foreach (var item in items)
                {
                    TransactorsCollection.Add(item);
                    //await Task.Delay(5000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task<IEnumerable<Transactor>> GetItemsAsync()
        {
            return await _transactorDS.GetItemsAsync();
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (TransactorsCollection == null)
            {
               
                await RefreshDataAsync();
            }
        }
    }
}
