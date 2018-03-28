using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.Api.Dtos;
using TransactionDiary.Models;
using TransactionDiary.Services;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;


namespace TransactionDiary.ViewModels
{
    public class ExpenceTransactionPageViewModel : ViewModelBase
    {
        #region Constructor and private fields

        #region Private fields

        private readonly IAutoCompleteDataSource<CostCentre> _costCentresAutoCompleteDs;
        private readonly IAutoCompleteDataSource<RevenueCentre> _revenueCentresAutoCompleteDs;
        private readonly ITransactionDataStore<TransactionDto, TransactionCreateDto, TransactionModifyDto> _itemsDs;
        private readonly IAutoCompleteDataSource<Transactor> _transactorsAutoCompleteDs;
        private readonly IAutoCompleteDataSource<Category> _categoriesAutoCompleteDs;
        private readonly IAutoCompleteDataSource<Company> _companiesAutoCompleteDs;
        private readonly IPageDialogService _dialogService;
        private bool isModifyMode = false;
        #endregion
        #region Constructor

        public ExpenceTransactionPageViewModel(INavigationService navigationService
            , ITransactionDataStore<TransactionDto, TransactionCreateDto, TransactionModifyDto> itemsDs
            , IAutoCompleteDataSource<Transactor> transactorsAutoCompleteDs
            , IAutoCompleteDataSource<Category> categoriesAutoCompleteDs
            , IAutoCompleteDataSource<Company> companiesAutoCompleteDs
            , IAutoCompleteDataSource<CostCentre> costCentresAutoCompleteDs
            , IAutoCompleteDataSource<RevenueCentre> revenueCentresAutoCompleteDs
            , IPageDialogService dialogService) : base(navigationService)
        {
            _costCentresAutoCompleteDs = costCentresAutoCompleteDs;
            _revenueCentresAutoCompleteDs = revenueCentresAutoCompleteDs;
            _itemsDs = itemsDs;
            _transactorsAutoCompleteDs = transactorsAutoCompleteDs;
            _categoriesAutoCompleteDs = categoriesAutoCompleteDs;
            _companiesAutoCompleteDs = companiesAutoCompleteDs;
            _dialogService = dialogService;
            _transactionDate = DateTime.Today;
            Title = "Εξοδα";
            isModifyMode = false;
        }

        #endregion

        #endregion

        #region IsBusy region

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Save Command

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () => await SaveDataCommand()));

        private async Task SaveDataCommand()
        {
            int transactorId;
            int categoryId;
            int costCentreId;
            int companyId;
            NavigationParameters par = new NavigationParameters();

            // string transactorName;
            if (_selectedTransactorItem != null)
            {
                transactorId = _selectedTransactorItem.Key;
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Please select a Transactor", "Ok");
                return;
            }

            if (_selectedCategoryItem != null)
            {
                categoryId = _selectedCategoryItem.Key;
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Please select a Category", "Ok");
                return;
            }
            if (_selectedCostCentreItem != null)
            {
                costCentreId = _selectedCostCentreItem.Key;
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Please select a Cost Centre", "Ok");
                return;
            }
            if (_selectedCompanyItem != null)
            {
                companyId = _selectedCompanyItem.Key;
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Please select a Company", "Ok");
                return;
            }

            if (!isModifyMode)
            {
                var expenceTransaction = new TransactionCreateDto
                {
                    TransactionDate = _transactionDate,
                    AmountFpa = _amountFpa,
                    AmountNet = _amountNet,
                    TransactorId = transactorId,
                    ReferenceCode = _referenceCode,
                    Description = _description,
                    CategoryId = categoryId,
                    CompanyId = companyId,
                    RevenueCentreId = 1,      //Πρέπει να του δόσω κάποιο υπαρκτό κλειδί αλλιώς έχει πρόβλημα με το Foreign Key
                    CostCentreId = costCentreId,
                    Kind = (int)TransactionsKinds.Expence
                };
                try
                {

                    var newItem = await _itemsDs.AddItemAsync2(expenceTransaction);
                    par.Add("RefreshView", "True");
                    par.Add("NewItem", newItem);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                    //throw;
                }
            }
            else
            {
                var expenceTransaction = new TransactionModifyDto()
                {
                    Id = _transactionId,
                    TransactionDate = _transactionDate,
                    AmountFpa = _amountFpa,
                    AmountNet = _amountNet,
                    TransactorId = transactorId,
                    ReferenceCode = _referenceCode,
                    Description = _description,
                    CategoryId = categoryId,
                    CompanyId = companyId,
                    RevenueCentreId = 1,      //Πρέπει να του δόσω κάποιο υπαρκτό κλειδί αλλιώς έχει πρόβλημα με το Foreign Key
                    CostCentreId = costCentreId,
                    Timestamp = _timeStamp,
                    Kind = (int)TransactionsKinds.Expence

                };
                try
                {
                    var newItem = await _itemsDs.ModifyItemAsync(_transactionId, expenceTransaction);
                    par.Add("RefreshView", "True");
                    par.Add("NewItem", newItem);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                    //throw;
                }
            }

            await NavigationService.GoBackAsync(par);
        }
        #endregion

        private ObservableCollection<SearchListItem> _transactorsCollection;

        public ObservableCollection<SearchListItem> TransactorsCollection
        {
            get => _transactorsCollection;
            set => SetProperty(ref _transactorsCollection, value);
        }

        private DateTime _transactionDate;
        private DelegateCommand _refreshTransactorsCommand;

        public DelegateCommand RefreshTransactorsCommand =>
            _refreshTransactorsCommand ?? (_refreshTransactorsCommand =
                new DelegateCommand(async () => await RefreshTransactorsCmd()));

        async Task RefreshTransactorsCmd()
        {
            await RefreshTransactorsSearchListAsync();
        }

        private DelegateCommand _refreshCategoriesCommand;

        public DelegateCommand RefreshCategoriesCommand =>
            _refreshCategoriesCommand ?? (_refreshCategoriesCommand =
                new DelegateCommand(async () => await RefreshCategoriesCmd()));

        async Task RefreshCategoriesCmd()
        {
            await RefreshCategoriesSearchListAsync();
        }

        private async Task RefreshTransactorsSearchListAsync()
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
                    TransactorsCollection = new ObservableCollection<SearchListItem>();
                }

                TransactorsCollection.Clear();
                var items = await GetTransactorsSearchListAsync();
                foreach (var item in items)
                {
                    TransactorsCollection.Add(item);
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

        private async Task<IEnumerable<SearchListItem>> GetTransactorsSearchListAsync()
        {
            return await _transactorsAutoCompleteDs.GetSearchListItemsAsync();
        }

        #region Bindable Properties

        private int _transactionId;
        private byte[] _timeStamp;

        public DateTime TransactionDate
        {
            get => _transactionDate;
            set => SetProperty(ref _transactionDate, value);
        }

        private string _referenceCode;
        public string ReferenceCode
        {
            get => _referenceCode;
            set => SetProperty(ref _referenceCode, value);
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private decimal _amountFpa;

        public decimal AmountFpa
        {
            get => _amountFpa;
            set => SetProperty(ref _amountFpa, value);
        }

        private decimal _amountNet;

        public decimal AmountNet
        {
            get => _amountNet;
            set => SetProperty(ref _amountNet, value);
        }


        #endregion
        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (TransactorsCollection == null)
            {
                await RefreshTransactorsSearchListAsync();
            }

            if (CategoriesCollection == null)
            {
                await RefreshCategoriesSearchListAsync();
            }
            if (CompaniesCollection == null)
            {
                await RefreshCompaniesSearchListAsync();
            }
            if (CostCentreCollection == null)
            {
                await RefreshCostCentreSearchListAsync();
            }

            if (parameters != null)
            {
                if (parameters.ContainsKey("ItemToModify"))
                {
                    int i;

                    var itemToModify = (TransactionDto)parameters["ItemToModify"];
                    isModifyMode = true;
                    _transactionId = itemToModify.Id;
                    CategoryText = itemToModify.CategoryName;

                    CostCentreText = itemToModify.CostCentreName;

                    CompanyText = itemToModify.CompanyName;
                    ReferenceCode = itemToModify.ReferenceCode;
                    Description = itemToModify.Description;
                    AmountNet = itemToModify.AmountNet;
                    AmountFpa = itemToModify.AmountFpa;
                    TransactionDate = itemToModify.TransactionDate;
                    TransactorText = itemToModify.TransactorName;
                    _timeStamp = itemToModify.Timestamp;

                    await UpdateSelectedItemsAsync(itemToModify);
                    return;
                }
                if (parameters.ContainsKey("TransactorSelected"))
                {
                    var transactorTapped = (SearchListItem)parameters["TransactorSelected"];
                    TransactorText = transactorTapped.Name;
                    SelectedTransactorItem = transactorTapped;
                    return;
                }
                if (parameters.ContainsKey("CompanySelected"))
                {
                    var itemTapped = (SearchListItem)parameters["CompanySelected"];
                    CompanyText = itemTapped.Name;
                    return;
                    //SelectedTransactorItem = transactorTapped;
                }
                if (parameters.ContainsKey("CοstCentreSelected"))
                {
                    var itemTapped = (SearchListItem)parameters["CοstCentreSelected"];
                    CostCentreText = itemTapped.Name;
                    //SelectedTransactorItem = transactorTapped;
                    return;
                }
                if (parameters.ContainsKey("CategorySelected"))
                {
                    var categoryTapped = (SearchListItem)parameters["CategorySelected"];
                    CategoryText = categoryTapped.Name;
                    SelectedCategoryItem = categoryTapped;
                    return;
                }
                if (parameters.ContainsKey("NewTransactor"))
                {
                    var newTransactor = (Transactor)parameters["NewTransactor"];
                    var searchListItem = new SearchListItem
                    {
                        Key = newTransactor.Id,
                        Name = newTransactor.Name
                    };
                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        TransactorsCollection.Add(new SearchListItem
                        {
                            Key = searchListItem.Key,
                            Name = searchListItem.Name
                        });
                        TransactorText = newTransactor.Name;
                        SelectedTransactorItem = searchListItem;
                    }
                    else
                    {
                        SelectedTransactorItem = searchListItem;
                    }

                    return;
                }
                if (parameters.ContainsKey("NewCategory"))
                {
                    var newCategory = (Category)parameters["NewCategory"];
                    var searchListItem = new SearchListItem
                    {
                        Key = newCategory.Id,
                        Name = newCategory.Name
                    };
                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        CategoriesCollection.Add(new SearchListItem
                        {
                            Key = searchListItem.Key,
                            Name = searchListItem.Name
                        });
                        CategoryText = newCategory.Name;
                        SelectedCategoryItem = searchListItem;
                    }
                    else
                    {
                        SelectedCategoryItem = searchListItem;
                    }

                    return;
                }
            }



        }

        private async Task UpdateSelectedItemsAsync(TransactionDto itemToModify)
        {
            var transItem = TransactorsCollection.Single(p => p.Key == itemToModify.TransactorId);
            SelectedTransactorItem = transItem;
            var catItem = CategoriesCollection.Single(p => p.Key == itemToModify.CategoryId);
            SelectedCategoryItem = catItem;
            var costItem = CostCentreCollection.Single(p => p.Key == itemToModify.CostCentreId);
            SelectedCostCentreItem = costItem;
            var compItem = CompaniesCollection.Single(p => p.Key == itemToModify.CompanyId);
            SelectedCompanyItem = compItem;
            return;
        }

        private SearchListItem _selectedTransactorItem;

        public SearchListItem SelectedTransactorItem
        {
            get => _selectedTransactorItem;
            set => SetProperty(ref _selectedTransactorItem, value);
        }

        private DelegateCommand<object> _transactorValueChangedCommand;
        public DelegateCommand<object> TransactorValueChangedCommand =>
            _transactorValueChangedCommand ?? (_transactorValueChangedCommand = new DelegateCommand<object>(async (t) => await TransactorValueChangedCmd(t)));

        private async Task TransactorValueChangedCmd(object value)
        {
            if (isModifyMode)
            {
                return;
            }
            if (value.GetType() == typeof(string))
            {
                CategoryText = string.Empty;
                CostCentreText = string.Empty;
            }
            else if (value != null)
            {
                var transactorId = (value as SearchListItem).Key;
                var lastCategory = await _itemsDs.GetCategoryIdOfTransactorsLastTransactionAsync(transactorId);
                if (lastCategory != null)
                {
                    CategoryText = lastCategory.Name;
                    SelectedCategoryItem = lastCategory;
                }
                var lastCostCentre = await _itemsDs.GetCostCentreIdOfTransactorsLastTransactionAsync(transactorId);
                if (lastCostCentre != null)
                {
                    CostCentreText = lastCostCentre.Name;
                    SelectedCostCentreItem = lastCostCentre;
                }
            }
            else
            {
                CategoryText = string.Empty;
                CostCentreText = string.Empty;
            }
        }

        private string _categoryText;

        public string CategoryText
        {
            get => _categoryText;
            set => SetProperty(ref _categoryText, value);
        }

        private string _companyText;

        public string CompanyText
        {
            get => _companyText;
            set => SetProperty(ref _companyText, value);
        }

        private string _costCentreText;

        public string CostCentreText
        {
            get => _costCentreText;
            set => SetProperty(ref _costCentreText, value);
        }

        private string _revenueCentreText;

        public string RevenueCentreText
        {
            get => _revenueCentreText;
            set => SetProperty(ref _revenueCentreText, value);
        }

        private string _transactorText;

        public string TransactorText
        {
            get => _transactorText;
            set => SetProperty(ref _transactorText, value);
        }

        private int _selectedTransactorIndex;
        public int SelectedTransactorIndex
        {
            get => _selectedTransactorIndex;
            set => SetProperty(ref _selectedTransactorIndex, value);
        }

        private SearchListItem _selectedCategoryItem;
        public SearchListItem SelectedCategoryItem
        {
            get => _selectedCategoryItem;
            set => SetProperty(ref _selectedCategoryItem, value);
        }

        private int _selectedCategoryIndex;
        public int SelectedCategoryIndex
        {
            get => _selectedCategoryIndex;
            set => SetProperty(ref _selectedCategoryIndex, value);
        }

        private ObservableCollection<SearchListItem> _categoriesCollection;
        public ObservableCollection<SearchListItem> CategoriesCollection
        {
            get => _categoriesCollection;
            set => SetProperty(ref _categoriesCollection, value);
        }
        private ObservableCollection<SearchListItem> _companiesCollection;
        public ObservableCollection<SearchListItem> CompaniesCollection
        {
            get => _companiesCollection;
            set => SetProperty(ref _companiesCollection, value);
        }
        private ObservableCollection<SearchListItem> _costCentreCollection;
        public ObservableCollection<SearchListItem> CostCentreCollection
        {
            get => _costCentreCollection;
            set => SetProperty(ref _costCentreCollection, value);
        }
        private ObservableCollection<SearchListItem> _revenueCentreCollection;
        public ObservableCollection<SearchListItem> RevenueCentreCollection
        {
            get => _revenueCentreCollection;
            set => SetProperty(ref _revenueCentreCollection, value);
        }
        private async Task RefreshCategoriesSearchListAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                if (CategoriesCollection == null)
                {
                    CategoriesCollection = new ObservableCollection<SearchListItem>();
                }

                CategoriesCollection.Clear();
                var items = await GetCategoriesSearchListAsync();
                foreach (var item in items)
                {
                    CategoriesCollection.Add(item);
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
        private async Task RefreshCostCentreSearchListAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                if (CostCentreCollection == null)
                {
                    CostCentreCollection = new ObservableCollection<SearchListItem>();
                }

                CostCentreCollection.Clear();
                var items = await GetCostCentreSearchListAsync();
                foreach (var item in items)
                {
                    CostCentreCollection.Add(item);
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
        private async Task RefreshRevenueCentreSearchListAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                if (RevenueCentreCollection == null)
                {
                    RevenueCentreCollection = new ObservableCollection<SearchListItem>();
                }

                RevenueCentreCollection.Clear();
                var items = await GetRevenueCentreSearchListAsync();
                foreach (var item in items)
                {
                    RevenueCentreCollection.Add(item);
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
        private async Task RefreshCompaniesSearchListAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                if (CompaniesCollection == null)
                {
                    CompaniesCollection = new ObservableCollection<SearchListItem>();
                }

                CompaniesCollection.Clear();
                var items = await GetCompanieSearchListAsync();
                foreach (var item in items)
                {
                    CompaniesCollection.Add(item);
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
        #region SearchTransactors

        private DelegateCommand _searchTransactors;
        public DelegateCommand SearchTransactors =>
            _searchTransactors ?? (_searchTransactors = new DelegateCommand(SearchTransactorsCmd));

        private void SearchTransactorsCmd()
        {
            NavigationService.NavigateAsync("TransactorsSearchListPage");
        }

        #endregion

        #region InsertTransactor

        private DelegateCommand _insertTransactor;
        public DelegateCommand InsertTransactor =>
            _insertTransactor ?? (_insertTransactor = new DelegateCommand(InsertTransactorCmd));

        private void InsertTransactorCmd()
        {
            NavigationService.NavigateAsync("TransactorPage");
        }

        #endregion

        private async Task<IEnumerable<SearchListItem>> GetCategoriesSearchListAsync()
        {
            return await _categoriesAutoCompleteDs.GetSearchListItemsAsync();
        }
        private async Task<IEnumerable<SearchListItem>> GetCostCentreSearchListAsync()
        {
            return await _costCentresAutoCompleteDs.GetSearchListItemsAsync();
        }
        private async Task<IEnumerable<SearchListItem>> GetRevenueCentreSearchListAsync()
        {
            return await _revenueCentresAutoCompleteDs.GetSearchListItemsAsync();
        }
        private async Task<IEnumerable<SearchListItem>> GetCompanieSearchListAsync()
        {
            return await _companiesAutoCompleteDs.GetSearchListItemsAsync();
        }
        #region SearchCategories

        private DelegateCommand _searchCategories;
        public DelegateCommand SearchCategories =>
            _searchCategories ?? (_searchCategories = new DelegateCommand(SearchCategoriesCmd));

        private void SearchCategoriesCmd()
        {
            NavigationService.NavigateAsync("CategorySearchListPage");
        }

        #endregion

        #region SearchCompanies
        private DelegateCommand _searchCompanies;
        public DelegateCommand SearchCompanies =>
            _searchCompanies ?? (_searchCompanies = new DelegateCommand(SearchCompaniesCmd));

        private void SearchCompaniesCmd()
        {
            NavigationService.NavigateAsync("CompaniesSearchListPage");
        }
        #endregion
        #region SearchCostCentres
        private DelegateCommand _searchCostCentres;
        public DelegateCommand SearchCostCentres =>
            _searchCostCentres ?? (_searchCostCentres = new DelegateCommand(SearchCostCentresCmd));

        private void SearchCostCentresCmd()
        {
            NavigationService.NavigateAsync("CostCentresSearchListPage");
        }
        #endregion

        #region Insert Commands

        #region InsertCategory
        private DelegateCommand _insertCategory;
        public DelegateCommand InsertCategory =>
            _insertCategory ?? (_insertCategory = new DelegateCommand(InsertCategoryCmd));

        private void InsertCategoryCmd()
        {
            var par = new NavigationParameters();
            if (CategoryText?.Length > 0)
            {
                par.Add("CategoryName", CategoryText);
            }

            NavigationService.NavigateAsync("CategoryPage", par);
        }
        #endregion
        #region InsertCompany
        private DelegateCommand _insertCompany;
        public DelegateCommand InsertCompany =>
            _insertCompany ?? (_insertCompany = new DelegateCommand(InsertCompanyCmd));

        private void InsertCompanyCmd()
        {
            var par = new NavigationParameters();
            if (CompanyText?.Length > 0)
            {
                par.Add("CompanyName", CompanyText);
            }

            NavigationService.NavigateAsync("CompanyPage", par);
        }
        #endregion

        #region InsertCostCentre
        private DelegateCommand _insertCostCentre;
        public DelegateCommand InsertCostCentre =>
            _insertCostCentre ?? (_insertCostCentre = new DelegateCommand(InsertCostCentreCmd));

        private void InsertCostCentreCmd()
        {
            var par = new NavigationParameters();
            if (CostCentreText?.Length > 0)
            {
                par.Add("CostCentreName", CostCentreText);
            }

            NavigationService.NavigateAsync("CostCentrePage", par);
        }
        #endregion


        #endregion

        private SearchListItem _selectedCompanyItem;
        public SearchListItem SelectedCompanyItem
        {
            get => _selectedCompanyItem;
            set => SetProperty(ref _selectedCompanyItem, value);
        }

        private int _selectedCompanyIndex;
        public int SelectedCompanyIndex
        {
            get => _selectedCompanyIndex;
            set => SetProperty(ref _selectedCompanyIndex, value);
        }

        private SearchListItem _selectedCostCentreItem;
        public SearchListItem SelectedCostCentreItem
        {
            get => _selectedCostCentreItem;
            set => SetProperty(ref _selectedCostCentreItem, value);
        }

        private int _selectedCostCentreIndex;
        public int SelectedCostCentreIndex
        {
            get => _selectedCostCentreIndex;
            set => SetProperty(ref _selectedCostCentreIndex, value);
        }

        private SearchListItem _selecteRevenueCentreItem;
        public SearchListItem SelectedRevenueCentreItem
        {
            get => _selecteRevenueCentreItem;
            set => SetProperty(ref _selecteRevenueCentreItem, value);
        }

        private int _selectedRevenueCentreIndex;
        public int SelectedRevenueCentreIndex
        {
            get => _selectedRevenueCentreIndex;
            set => SetProperty(ref _selectedRevenueCentreIndex, value);
        }
    }
}
