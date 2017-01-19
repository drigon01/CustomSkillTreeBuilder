using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomSkillTreeBuilder
{
  public class SkillToMarginConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      return new Thickness(0);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
