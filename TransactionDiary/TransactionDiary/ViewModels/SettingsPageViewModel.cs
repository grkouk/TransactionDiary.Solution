using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Syncfusion.Data;

namespace TransactionDiary.ViewModels
{
	public class SettingsPageViewModel : BindableBase
	{
	    private static ISettings AppSettings => CrossSettings.Current;
        public SettingsPageViewModel()
        {

        }

	    public static string WebApiBaseAddress
        {
            get => AppSettings.GetValueOrDefault(nameof(WebApiBaseAddress), "http://testapi.potos.tours/api");
            set => AppSettings.AddOrUpdateValue(nameof(WebApiBaseAddress), value);
	    }

	    private string _webAddress;
        public string WebAddress
        {
            get
            {
                _webAddress = WebApiBaseAddress;
                return _webAddress;
            }
            set
            {
                WebApiBaseAddress = value;
                SetProperty(ref _webAddress, value);
            }
        }
	}
}
