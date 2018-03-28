using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionDiary.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Ημερολόγιο Κινήσεων";
        }
        private DelegateCommand _faciliesButtonCommand;
        public DelegateCommand FacilitiesButtonCommand =>
            _faciliesButtonCommand ?? (_faciliesButtonCommand = new DelegateCommand(ExecuteFacilitiesButtonCommand));

        void ExecuteFacilitiesButtonCommand()
        {
            NavigationService.NavigateAsync("FacilitiesPage");
        }

        private DelegateCommand _transactorsButtonCommand;
        public DelegateCommand TransactorsButtonCommand =>
            _transactorsButtonCommand ?? (_transactorsButtonCommand = new DelegateCommand(ExecuteTransactorsButtonCommand));
        void ExecuteTransactorsButtonCommand()
        {
            NavigationService.NavigateAsync("AutoCompletePage");
        }

        private DelegateCommand _transactionsButtonCommand;
        public DelegateCommand TransactionsButtonCommand =>
            _transactionsButtonCommand ?? (_transactionsButtonCommand = new DelegateCommand(ExecuteTransactionsButtonCommand));

        void ExecuteTransactionsButtonCommand()
        {
            NavigationService.NavigateAsync("TransactionsPage");
        }
    }
}
