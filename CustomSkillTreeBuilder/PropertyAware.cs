using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CustomSkillTreeBuilder
{
  class PropertyAware : INotifyPropertyChanged
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
  }
}
