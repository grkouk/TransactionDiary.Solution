using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Syncfusion.Data;
using Syncfusion.SfAutoComplete.XForms;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;

namespace Prism.Converters
{
    public class AutoCompleteValueConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var evArgs = value as SelectionChangedEventArgs;

            if (evArgs!=null)
            {
                var item = evArgs.Value;
                if (item == null)
                {
                    throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
                }
                return item;
            }
            else
            {
                throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
            }

            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class GroupCaptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value != null ? value as Group : null;
            if (data != null)
            {
                SfDataGrid dataGrid = (SfDataGrid)parameter;
                var summaryText = SummaryCreator.GetSummaryDisplayTextForRow((value as Group).SummaryDetails, dataGrid.View);

                return summaryText;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
