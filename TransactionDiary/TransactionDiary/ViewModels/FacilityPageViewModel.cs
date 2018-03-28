using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using TransactionDiary.Models;
using TransactionDiary.Services;
using Prism.Navigation;

namespace TransactionDiary.ViewModels
{
    public class FacilityPageViewModel : BindableBase
    {
        private readonly IDataStore<Facility,Facility,Facility> _facilityDs;
        private readonly INavigationService _navigationService;

        public FacilityPageViewModel(IDataStore<Facility,Facility,Facility> facilityDs, INavigationService navigationService)
        {
            _facilityDs = facilityDs;
            _navigationService = navigationService;
        }

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

        private string _shortDescription;

        public string ShortDescription
        {
            get { return _shortDescription; }
            set { SetProperty(ref _shortDescription, value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand));

        private async void ExecuteSaveCommand()
        {
            var facility = new Facility
            {
                Name = _name,
                Code = _code,
                Description = _description,
                ShortDescription = _shortDescription
            };

            var result= await _facilityDs.AddItemAsync(facility);
            //Inform user for result
            
            Debug.WriteLine(result ? "Facility added succesfully" : "Facility addition failed");
            await _navigationService.GoBackAsync();
        }
    }
}
