using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.Api.Dtos;
using TransactionDiary.Models;
using TransactionDiary.Services;
using Prism.Services;

namespace TransactionDiary.ViewModels
{
    public class TransactorPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly IDataStore<Transactor, Transactor, Transactor> _itemsDs;


        public TransactorPageViewModel(INavigationService navigationService
            , IPageDialogService dialogService
            , IDataStore<Transactor, Transactor, Transactor> itemsDs
                            ) : base(navigationService)
        {
            _dialogService = dialogService;
            _itemsDs = itemsDs;
        }

        #region Save Command
        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () => await SaveDataCommand()));

        private async Task SaveDataCommand()
        {
            
            NavigationParameters navigationParams = new NavigationParameters();

            var newTransactor = new Transactor
            {
                Code = _code,
                Name = _name,
                Address = _address,
                City = _city,
                Zip = _zip,
                PhoneWork = _phoneWork,
                PhoneMobile = _phoneMobile,
                PhoneFax = _phoneFax,
                EMail = _email
            };
            try
            {
                var newItem = await _itemsDs.AddItemAsync2(newTransactor);
                //navigationParams.Add("RefreshView", "True");
                navigationParams.Add("NewTransactor", newItem);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                //throw;
            }

            await NavigationService.GoBackAsync(navigationParams);

        }
        #endregion
        #region Data Bind Properties
        private string _code;
        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        private string _city;
        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }
        private int _zip;
        public int Zip
        {
            get => _zip;
            set => SetProperty(ref _zip, value);
        }
        private string _phoneWork;
        public string PhoneWork
        {
            get => _phoneWork;
            set => SetProperty(ref _phoneWork, value);
        }
        private string _phoneMobile;
        public string PhoneMobile
        {
            get => _phoneMobile;
            set => SetProperty(ref _phoneMobile, value);
        }
        private string _phoneFax;
        public string PhoneFax
        {
            get => _phoneFax;
            set => SetProperty(ref _phoneFax, value);
        }
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        #endregion

        
    }
}
