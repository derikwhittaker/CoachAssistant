using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Dimesoft.CoachAssistant.Models;

namespace Dimesoft.CoachAssistant.Converters
{
    public class SelectionStateToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var asType = (SelectionState)Enum.Parse(typeof(SelectionState), value.ToString(), true);


            switch (asType)
            {
                case SelectionState.Selected:
                    return Visibility.Visible;

                default:
                    return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}