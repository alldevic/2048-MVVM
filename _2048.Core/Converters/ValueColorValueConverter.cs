using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using _2048.Core.Model;

namespace _2048.Core.Converters
{
    public class ValueColorValueConverter : MvxValueConverter<int, int>
    {
        private IMvxValueConverter _mvxValueConverterImplementation;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Colors.One;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}