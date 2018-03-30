using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TransactionDiary.Views;
using Prism.Navigation;
using Xamarin.Forms;
using TransactionDiary.Models;


namespace TransactionDiary.ViewModels
{
	public class MenuPageViewModel : BindableBase
	{
	    private INavigationService _navigationService;
	    public ObservableCollection<MyMenuItem> MenuItems { get; set; }
	    private MyMenuItem selectedMenuItem;
	    public MyMenuItem SelectedMenuItem
	    {
	        get => selectedMenuItem;
	        set => SetProperty(ref selectedMenuItem, value);
	    }

	    public DelegateCommand NavigateCommand { get; private set; }

	    public MenuPageViewModel(INavigationService navigationService)
	    {
	        _navigationService = navigationService;

	        MenuItems = new ObservableCollection<MyMenuItem>();

	        MenuItems.Add(new MyMenuItem()
	        {
	            Icon = "ic_viewa",
	            PageName = nameof(TransactionsPage),
	            Title = "Transactions"
	        });

	        MenuItems.Add(new MyMenuItem()
	        {
	            Icon = "ic_viewb",
	            PageName = nameof(TransactorPage),
	            Title = "Transactors"
	        });
	        MenuItems.Add(new MyMenuItem()
	        {
	            Icon = "ic_viewb",
	            PageName = nameof(CategoryPage),
	            Title = "Categories"
	        });
	        MenuItems.Add(new MyMenuItem()
	        {
	            Icon = "ic_viewb",
	            PageName = nameof(SettingsPage),
	            Title = "Settings"
	        });
            NavigateCommand = new DelegateCommand(Navigate);
	    }
	    async void Navigate()
	    {
	        await _navigationService.NavigateAsync(nameof(NavigationPage) + "/" + SelectedMenuItem.PageName);
	    }
    }
}
