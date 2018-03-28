using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TransactionDiary.Models;
using TransactionDiary.Services;
using Prism.Navigation;

namespace TransactionDiary.ViewModels
{
    public class CategorySearchListPageViewModel : ViewModelBase
    {

        private readonly IAutoCompleteDataSource<Category> _itemAutoCompleteDs;

        public CategorySearchListPageViewModel(INavigationService navigationService
                , IAutoCompleteDataSource<Category> itemAutoCompleteDs
        ) : base(navigationService)
        {
            _itemAutoCompleteDs = itemAutoCompleteDs;
            Title = "Επιλογή Κατηγοριών";
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (ItemsList == null)
            {
                await RefreshDataAsync();
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private ObservableCollection<SearchListItem> _itemsList;
        public ObservableCollection<SearchListItem> ItemsList
        {
            get => _itemsList;
            set => SetProperty(ref _itemsList, value);
        }


        private SearchListItem _selectedListItem;


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
                if (ItemsList == null)
                {
                    ItemsList = new ObservableCollection<SearchListItem>();
                }
                ItemsList.Clear();
                var items = await GetItemsAsync();
                foreach (var item in items)
                {
                    ItemsList.Add(item);
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
        private async Task<IEnumerable<SearchListItem>> GetItemsAsync()
        {
            return await _itemAutoCompleteDs.GetSearchListItemsAsync();
        }

        public SearchListItem SelectedListItem
        {
            get => _selectedListItem;
            set => SetProperty(ref _selectedListItem, value);
        }

        private DelegateCommand<SearchListItem> _itemTappedCommand;
        public DelegateCommand<SearchListItem> ItemTappedCommand =>
            _itemTappedCommand ?? (_itemTappedCommand = new DelegateCommand<SearchListItem>(ItemTappedCmd));

        private async void ItemTappedCmd(SearchListItem itemTapped)
        {
            var p = new NavigationParameters();
            p.Add("CategorySelected", itemTapped);

            await NavigationService.GoBackAsync(p);
        }

        private DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand =>
            _searchCommand ?? (_searchCommand = new DelegateCommand(async () => await SearchCmd()));

        private async Task SearchCmd()
        {
            if (_searchText != null)
            {
                if (_searchText.Length > 0)
                {
                    var filteredList = ItemsList.Where(p => p.Name.StartsWith(_searchText, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    ItemsList.Clear();
                    foreach (var item in filteredList)
                    {
                        ItemsList.Add(item);
                    }
                }
                else
                {
                    await RefreshDataAsync();
                }
            }

        }


        private string _searchText;
        public string SearchText { get => _searchText; set => SetProperty(ref _searchText, value); }

        private DelegateCommand<string> _textChangedCommand;
        public DelegateCommand<string> TextChangedCommand =>
            _textChangedCommand ?? (_textChangedCommand = new DelegateCommand<string>(TextChangedCmd));

        private async void TextChangedCmd(string NewTextValue)
        {
            if (NewTextValue==null)
            {
                return;
            }
            if (NewTextValue != string.Empty)
            {
                try
                {
                    var fList = ItemsList.Where(p => p.Name.StartsWith(NewTextValue,StringComparison.CurrentCultureIgnoreCase)).ToList();
                    ItemsList.Clear();
                    foreach (var item in fList)
                    {
                        ItemsList.Add(item);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                await RefreshDataAsync();
            }


        }
    }
}

