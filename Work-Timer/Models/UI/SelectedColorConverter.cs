using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using WorkTimer.Models.Enums;

namespace WorkTimer.Models.UI
{
    public class SelectedColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isSelected = System.Convert.ToBoolean(value);
            return isSelected ? App._instance.App.GetThemeBrushFromResource(ColorName.PrimaryColor) : new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
