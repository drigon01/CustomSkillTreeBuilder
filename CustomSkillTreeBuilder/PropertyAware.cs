using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace CustomSkillTreeBuilder
{
  public class PropertyAware : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  static class Extensions
  {
    public static string NameOf<T, TProp>(this T o, Expression<Func<T, TProp>> propertySelector)
    {
      MemberExpression body = (MemberExpression)propertySelector.Body;
      return body.Member.Name;
    }

    public static T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
      //get parent item
      DependencyObject parentObject = VisualTreeHelper.GetParent(child);

      //we've reached the end of the tree
      if (parentObject == null) return null;

      //check if the parent matches the type we're looking for
      T parent = parentObject as T;
      if (parent != null)
        return parent;
      else
        //enter recursion
        return FindParent<T>(parentObject);
    }
  }
}
