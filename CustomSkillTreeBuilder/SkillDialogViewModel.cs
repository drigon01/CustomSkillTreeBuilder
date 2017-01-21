using System.Collections.ObjectModel;
using System.Windows;

namespace CustomSkillTreeBuilder
{
  public class SkillDialogViewModel : PropertyAware
  {
    bool mIsOpen;
    public bool IsOpen
    {
      get { return mIsOpen; }
      set
      {
        mIsOpen = value;
        NotifyPropertyChanged(this.NameOf(p => p.IsOpen));
      }
    }

    public ObservableCollection<UIElement> Items
    {
      get; set;
    }

    public SkillDialogViewModel() { Items = new ObservableCollection<UIElement>(); }

  }
}
