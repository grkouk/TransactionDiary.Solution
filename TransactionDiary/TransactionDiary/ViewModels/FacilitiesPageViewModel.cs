using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TransactionDiary.Models;
using TransactionDiary.Services;
using Prism.Navigation;

namespace TransactionDiary.ViewModels
{
    public class FacilitiesPageViewModel : ViewModelBase
    {
        private IDataStore<Facility,Facility,Facility> _facilityDS;

        //private bool _dsInitialized = false;

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public FacilitiesPageViewModel(INavigationService navigationService, IDataStore<Facility,Facility,Facility> facilityDs) : base(navigationService)
        {
            _facilityDS = facilityDs;
        }
        
        private ObservableCollection<Facility> _facilitiesCollection;

        public ObservableCollection<Facility> FacilitiesCollection
        {
            get => _facilitiesCollection;
            set => SetProperty(ref _facilitiesCollection, value);
        }


        private DelegateCommand _addCommand;
        public DelegateCommand AddCommand =>
            _addCommand ?? (_addCommand = new DelegateCommand(AddFacilityCommand));

        void AddFacilityCommand()
        {
            NavigationService.NavigateAsync("FacilityPage");
        }

        private DelegateCommand _refreshCommand;
        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(async () => await RefreshDataCommand()));

        async Task RefreshDataCommand()
        {
            await RefreshDataAsync();
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
                if (FacilitiesCollection == null)
                {
                    FacilitiesCollection = new ObservableCollection<Facility>();
                }
                FacilitiesCollection.Clear();
                var items = await GetItemsAsync();
                foreach (var item in items)
                {
                    FacilitiesCollection.Add(item);
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
        private async Task<IEnumerable<Facility>> GetItemsAsync()
        {
            return await _facilityDS.GetItemsAsync();
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (FacilitiesCollection == null)
            {
                //FacilitiesCollection = new ObservableCollection<Facility>(await GetItemsAsync());
                await RefreshDataAsync();
            }
        }
    }
}
