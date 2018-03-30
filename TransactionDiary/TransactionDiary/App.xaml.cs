using GrKouk.Api.Dtos;
using Prism;
using Prism.Ioc;
using TransactionDiary.ViewModels;
using TransactionDiary.Views;
using TransactionDiary.Models;
using TransactionDiary.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.DryIoc;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TransactionDiary
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync(nameof(MenuPage) + "/" + nameof(NavigationPage) + "/" + nameof(Views.MainPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDataStore<Facility, Facility, Facility>, FacilitiesDataStore>();
            containerRegistry.Register<IDataStore<Transactor, Transactor, Transactor>, TransactorsDataStore>();
            containerRegistry.Register<IDataStore<TransactionDto, TransactionCreateDto, TransactionDto>, TransactionsDataStore>();
            containerRegistry.Register<IDataStore<Category, Category, Category>, CategoryDataSource>();
            containerRegistry.Register<IDataStore<Company, Company, Company>, CompanyDataStore>();
            containerRegistry.Register<IDataStore<CostCentre, CostCentre, CostCentre>, CostCentreDataStore>();
            containerRegistry.Register<IAutoCompleteDataSource<Transactor>, TransactorsAutoCompleteDs>();
            containerRegistry.Register<IAutoCompleteDataSource<Category>, CategoryAutoCompleteDs>();
            containerRegistry.Register<IAutoCompleteDataSource<Company>, CompanyAutoCompleteDs>();
            containerRegistry.Register<IAutoCompleteDataSource<CostCentre>, CostCentreAutoCompleteDs>();
            containerRegistry.Register<IAutoCompleteDataSource<RevenueCentre>, RevenueCentreAutoCompleteDs>();
            containerRegistry.Register<ITransactionDataStore<TransactionDto, TransactionCreateDto, TransactionModifyDto>, TransactionsDataStoreEx>();

            containerRegistry.RegisterForNavigation<MenuPage>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();

            containerRegistry.RegisterForNavigation<FacilitiesPage>();
            containerRegistry.RegisterForNavigation<FacilityPage>();
            containerRegistry.RegisterForNavigation<AutoCompletePage>();
            containerRegistry.RegisterForNavigation<TransactionsPage>();
            containerRegistry.RegisterForNavigation<ExpenceTransactionPage>();
            containerRegistry.RegisterForNavigation<TransactorsSearchListPage>();
            containerRegistry.RegisterForNavigation<TransactorPage>();
            containerRegistry.RegisterForNavigation<CategorySearchListPage>();
            containerRegistry.RegisterForNavigation<CategoryPage>();
            containerRegistry.RegisterForNavigation<TestPage>();
            containerRegistry.RegisterForNavigation<CostCentresSearchListPage>();
            containerRegistry.RegisterForNavigation<CompaniesSearchListPage>();
            containerRegistry.RegisterForNavigation<CostCentrePage>();
            containerRegistry.RegisterForNavigation<CompanyPage>();

            containerRegistry.RegisterForNavigation<SettingsPage>();
        }
    }
}
