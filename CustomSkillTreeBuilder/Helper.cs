using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

    public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
    {
      DependencyObject parentObject = VisualTreeHelper.GetParent(child);

      if (parentObject == null) return null;

      T parent = parentObject as T;
      if (parent != null)
        return parent;
      else
        //enter recursion
        return FindParent<T>(parentObject);
    }

    public static Point DragOnCanvas<T>(this T o, Canvas parent, Point startPoint, MouseEventArgs args) where T : DependencyObject
    {
      var wPos = args.GetPosition(parent);
      var wX = wPos.X - startPoint.X;
      var wY = wPos.Y - startPoint.Y;

      o.SetValue(Canvas.LeftProperty, 0 < wX && wX < parent.ActualWidth ? wX : wPos.X);
      o.SetValue(Canvas.TopProperty, 0 < wY && wY < parent.ActualHeight ? wY : wPos.Y);

      return
        new Point((double)o.GetValue(Canvas.LeftProperty),
        (double)o.GetValue(Canvas.TopProperty));
    }

  }

  public class EqualityComparer<T> : IEqualityComparer<T>
  {
    public EqualityComparer(Func<T, T, bool> cmp) { Compare = cmp; }
    public bool Equals(T x, T y) { return Compare(x, y); }
    public int GetHashCode(T obj) { return obj.GetHashCode(); }
    public Func<T, T, bool> Compare { get; set; }
  }
}
