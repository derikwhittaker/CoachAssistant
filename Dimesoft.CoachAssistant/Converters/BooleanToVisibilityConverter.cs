﻿using System;
using System.Windows;
using System.Windows.Data;

namespace Dimesoft.CoachAssistant.Converters
{
	public class BooleanToVisibilityConverter : IValueConverter
	{
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public BooleanToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            if (value == null)
            {
                return Visibility.Collapsed;
            }

			return ((bool) value) ? TrueValue : FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
