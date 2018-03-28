using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.Api.Dtos;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using TransactionDiary.Helpers;
using TransactionDiary.Models;
using TransactionDiary.Services;
using Prism.Navigation;
using Prism.Services;

namespace TransactionDiary.ViewModels
{
    public class TransactionsPageViewModel : ViewModelBase
    {
        private readonly ITransactionDataStore<TransactionDto, TransactionCreateDto, TransactionModifyDto> _itemsDs;
        private readonly IPageDialogService _dialogService;
        private static ISettings AppSettings => CrossSettings.Current;
        #region DateFilters

        private ObservableCollection<DateFilter> _dateFilters;
        public ObservableCollection<DateFilter> DateFilters
        {
            get => _dateFilters;
            set => SetProperty(ref _dateFilters, value);
        }

        #endregion

        #region IsBusy

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion
        public TransactionsPageViewModel(INavigationService navigationService, ITransactionDataStore<TransactionDto, TransactionCreateDto, TransactionModifyDto> itemsDs, IPageDialogService dialogService) : base(navigationService)
        {
            _itemsDs = itemsDs;
            _dialogService = dialogService;
            DateFilters = HelperFunctions.CreateDateFilters(DateTime.Now);
            Title = "Κινήσεις Ημερολογίου";

        }

        private ObservableCollection<TransactionDto> _itemsCollection;

        public ObservableCollection<TransactionDto> ItemsCollection
        {
            get => _itemsCollection;
            set => SetProperty(ref _itemsCollection, value);
        }

        #region AddCommand

        private DelegateCommand _addCommand;
        public DelegateCommand AddCommand =>
            _addCommand ?? (_addCommand = new DelegateCommand(AddItemCommand, () => !IsBusy))
            .ObservesProperty(() => IsBusy);

        void AddItemCommand()
        {
            NavigationService.NavigateAsync("ExpenceTransactionPage");
        }

        #endregion

        #region RefreshCommand

        private DelegateCommand _refreshCommand;
        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(async () => await RefreshDataCommand(), () => !IsBusy))
            .ObservesProperty(() => IsBusy);

        async Task RefreshDataCommand()
        {
            await RefreshDataAsync();
        }

        #endregion

        #region ModifyCommand
        private DelegateCommand _modifyCommand;
        public DelegateCommand ModifyCommand =>
            _modifyCommand ?? (_modifyCommand = new DelegateCommand(async () => await ModifyCmd(), () => ((SelectedRowIndex > -1) && !IsBusy)))
            .ObservesProperty(() => SelectedRowIndex)
            .ObservesProperty(() => IsBusy);
        async Task ModifyCmd()
        {
            if (SelectedGridItem == null)
            {
                return;
            }
            var par = new NavigationParameters();
            par.Add("ItemToModify", SelectedGridItem);
            await NavigationService.NavigateAsync("ExpenceTransactionPage", par);
            //await RefreshDataAsync();
        }

        #endregion
        #region DeleteCommand
        private DelegateCommand _deleteCommand;
        public DelegateCommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new DelegateCommand(async () => await DeleteCmd(), () => ((SelectedRowIndex > -1) && !IsBusy)))
            .ObservesProperty(() => SelectedRowIndex)
            .ObservesProperty(() => IsBusy);
        async Task DeleteCmd()
        {
            if (SelectedGridItem == null)
            {
                return;
            }


            try
            {
                if ( await _itemsDs.DeleteItemAsync(SelectedGridItem.Id))
                {
                    ItemsCollection.Remove(SelectedGridItem);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
            }

        }
        #endregion

        private async Task RefreshDataAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                if (ItemsCollection == null)
                {
                    ItemsCollection = new ObservableCollection<TransactionDto>();
                }
                ItemsCollection.Clear();
                var items = await GetItemsAsync();
                foreach (var item in items)
                {
                    ItemsCollection.Add(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "ok");

                //throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task<IEnumerable<TransactionDto>> GetItemsAsync()
        {

            if (_selectedDateFilterItem != null)
            {
                return await _itemsDs.GetItemsInPeriodAsync(_selectedDateFilterItem.FromDate, _selectedDateFilterItem.ToDate);
            }
            else
            {
                return await _itemsDs.GetItemsAsync();
            }
        }

        #region OnNavigatedTo

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            var iF = SettingDateFilter;
            if (iF != -1)
            {
                SelectedDateFilterIndex = SettingDateFilter;
                this._selectedDateFilterItem = DateFilters[iF];
            }
            else
            {
                this._selectedDateFilterItem = DateFilters[2];
                this.SelectedDateFilterIndex = 2;
            }
            if (ItemsCollection == null)
            {

                await RefreshDataAsync();
            }

            if (parameters != null)
            {
                if (parameters.ContainsKey("RefreshView"))
                {
                    await RefreshDataAsync();
                }
            }

        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            SettingDateFilter = SelectedDateFilterIndex;
        }

        #endregion
        public static int SettingDateFilter
        {
            get => AppSettings.GetValueOrDefault(nameof(SettingDateFilter), -1);
            set => AppSettings.AddOrUpdateValue(nameof(SettingDateFilter), value);
        }

        private int _selectedDateFilterIndex;
        public int SelectedDateFilterIndex
        {
            get => _selectedDateFilterIndex;
            set => SetProperty(ref _selectedDateFilterIndex, value);
        }

        public DateFilter SelectedDateFilterItem
        {
            get => _selectedDateFilterItem;
            set => SetProperty(ref _selectedDateFilterItem, value);
        }

        private DateFilter _selectedDateFilterItem;


        private DelegateCommand _pickerSelectedIndexChangedCommand;
        public DelegateCommand PickerSelectedIndexChangedCommand =>
            _pickerSelectedIndexChangedCommand ?? (_pickerSelectedIndexChangedCommand = new DelegateCommand(PickerSelectedIndexChangedCmd));

        private async void PickerSelectedIndexChangedCmd()
        {
            SettingDateFilter = SelectedDateFilterIndex;
        }
        #region GridBinding Properties

        private int _selectedRowIndex;

        public int SelectedRowIndex
        {
            get => _selectedRowIndex;
            set => SetProperty(ref _selectedRowIndex, value);
        }

        private TransactionDto _selectedGridItem;

        public TransactionDto SelectedGridItem
        {
            get => _selectedGridItem;
            set => SetProperty(ref _selectedGridItem, value);
        }

        #endregion
    }
}
