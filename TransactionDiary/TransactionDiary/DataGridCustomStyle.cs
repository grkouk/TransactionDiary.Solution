using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;

namespace TransactionDiary
{
    public class DataGridCustomStyle:DataGridStyle
    {
        public override Color GetAlternatingRowBackgroundColor()
        {
            return Color.LightGray;
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.Blue;
        }

        public override Color GetHeaderForegroundColor()
        {
            return Color.White;
        }
    }
}
