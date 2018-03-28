using System;
using System.Collections.ObjectModel;
using TransactionDiary.Models;

namespace TransactionDiary.Helpers
{
    public static class HelperFunctions
    {
       public static ObservableCollection<DateFilter> CreateDateFilters(DateTime currentDate)
        {
            ObservableCollection<DateFilter> filters = new ObservableCollection<DateFilter>();

            filters.Add(new DateFilter()
            {
                Name = "Τρέχων Μήνας",
                SystemCode = "CURMONTH",
                FromDate = new DateTime(currentDate.Year, currentDate.Month, 1),
                ToDate = currentDate
            });

            filters.Add(new DateFilter()
            {
                Name = "30 Ημέρες",
                SystemCode = "LAST30DAYS",
                FromDate = currentDate.AddDays(-30),
                ToDate = currentDate
            });

            filters.Add(new DateFilter()
            {
                Name = "60 Ημέρες",
                SystemCode = "LAST60DAYS",
                FromDate = currentDate.AddDays(-60),
                ToDate = currentDate
            });
            filters.Add(new DateFilter()
            {
                Name = "Τρέχων Ετος",
                SystemCode = "CURYEAR",
                FromDate = new DateTime(currentDate.Year, 01,01),
                ToDate = currentDate
            });
            return filters;
        }
    }
}
