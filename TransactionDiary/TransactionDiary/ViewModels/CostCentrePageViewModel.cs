using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionDiary.Models;
using TransactionDiary.Services;
using Prism.Services;

namespace TransactionDiary.ViewModels
{
	public class CostCentrePageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly IDataStore<CostCentre, CostCentre, CostCentre> _itemDs;

        public CostCentrePageViewModel(INavigationService navigationService
            , IPageDialogService dialogService
            , IDataStore<CostCentre, CostCentre, CostCentre> itemDs

        ) : base(navigationService)
        {
            _dialogService = dialogService;
            _itemDs = itemDs;
            Title = "Εισαγωγή Κ.Κόστους";
        }
        #region IsBusy

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Bindable Properties

        private string _code;

        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        #endregion
        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () => await SaveDataCmd()));

        private async Task SaveDataCmd()
        {
            NavigationParameters navigationParams = new NavigationParameters();

            var newEntity = new CostCentre
            {
                Name = _name,
                Code = _code
            };
            try
            {
                var newItem = await _itemDs.AddItemAsync2(newEntity);
                navigationParams.Add("RefreshView", "True");
                navigationParams.Add("NewCostCentre", newItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                //throw;
            }
            await NavigationService.GoBackAsync(navigationParams);
        }
        #region SaveCommand

        #endregion
        #region OnNavigatedTo

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey("CostCentreName"))
                {
                    Name = parameters["CostCentreName"].ToString();
                }
            }
        }

        #endregion
    }
}
