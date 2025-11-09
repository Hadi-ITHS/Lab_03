using Lab_03.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Lab_03.Converters
{
    internal class IntToDifficultyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Difficulty.Easy:
                    return 0;
                case Difficulty.Medium:
                    return 1;
                case Difficulty.Hard:
                    return 2;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case 0:
                    return Difficulty.Easy;
                case 1:
                    return Difficulty.Medium;
                case 2:
                    return Difficulty.Hard;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
