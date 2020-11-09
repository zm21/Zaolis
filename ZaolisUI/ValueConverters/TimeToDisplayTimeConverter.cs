using System;
using System.Globalization;
using System.Windows;

namespace ZaolisUI
{
    /// <summary>
    /// A converter that takes in date and converts it to a user friendly time
    /// </summary>
    public class TimeToDisplayTimeConverter : BaseValueConverter<TimeToDisplayTimeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the time passed in
            var time = (DateTime)value;

            // If it is today...
            if (time.Date == DateTime.UtcNow.Date)
                // Return just time
                return time.ToShortTimeString();

            // Otherwise, return a full date
            return $"{time.ToShortTimeString()}, {time.ToShortDateString()}";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
