﻿
namespace RigoFunc.Render.Converters {
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class DoubleRoundConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return Math.Round((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}
