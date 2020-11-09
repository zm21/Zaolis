using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace ZaolisUI
{
    /// <summary>
    /// A converter that takes in a boolean if a message was sent by me, and returns the
    /// correct background color
    /// </summary>
    public class SentByMeToBackgroundConverter : BaseValueConverter<SentByMeToBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = new System.Windows.Media.BrushConverter();
            return (bool)value ? (Brush)converter.ConvertFromString("#FFE4E1") : (Brush)converter.ConvertFromString("#00BFFF");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
