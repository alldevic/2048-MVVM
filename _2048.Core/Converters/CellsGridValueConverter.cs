using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using _2048.Core.Model;

namespace _2048.Core.Converters
{
    public class CellsGridValueConverter : MvxValueConverter<Cell[][], int[][]>
    {
        private IMvxValueConverter _mvxValueConverterImplementation;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _mvxValueConverterImplementation.Convert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _mvxValueConverterImplementation.ConvertBack(value, targetType, parameter, culture);
        }   
    }
}