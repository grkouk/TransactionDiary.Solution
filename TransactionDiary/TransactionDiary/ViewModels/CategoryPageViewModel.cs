using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.Api.Dtos;
using Prism.Services;
using TransactionDiary.Models;
using TransactionDiary.Services;
using TransactionDiary.ViewModels;

namespace TransactionDiary.ViewModels
{
    public class CategoryPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly IDataStore<Category, Category, Category> _itemDs;

        public CategoryPageViewModel(INavigationService navigationService
            , IPageDialogService dialogService
             , IDataStore<Category, Category, Category> itemDs
                ) : base(navigationService)
        {
            _dialogService = dialogService;
            _itemDs = itemDs;
            Title = "Εισαγωγή Κατηγορίας";
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

            var newCategory = new Category
            {
                Name = _name,
                Code = _code
            };
            try
            {
                var newItem = await _itemDs.AddItemAsync2(newCategory);
                navigationParams.Add("RefreshView", "True");
                navigationParams.Add("NewCategory", newItem);
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
                if (parameters.ContainsKey("CategoryName"))
                {
                    Name = parameters["CategoryName"].ToString();
                }
            }
        }

        #endregion
    }
}
