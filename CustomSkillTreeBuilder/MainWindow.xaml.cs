using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CustomSkillTreeBuilder
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    string mSelectedSkillFamilyName;

    MainViewModel ViewModel { get; set; }

    public MainWindow()
    {
      InitializeComponent();
      ViewModel = (DataContext as MainViewModel);
      ViewModel.Canvas = mCanvas;
    }

    private void Open_Clicked(object sender, RoutedEventArgs e)
    {
      if ((string)(sender as MenuItem).CommandParameter == "Components")
      {
        ViewModel.LoadComponents();
      }
    }

    private void Save_Clicked(object sender, RoutedEventArgs e)
    {
      if ((string)(sender as MenuItem).CommandParameter == "Components")
      {
        ViewModel.SaveComponents();
      }
      else
      {
        ViewModel.SaveSkillTree();
      }
    }

    private void OnAddSkill(object sender, RoutedEventArgs e)
    {
      if (mSideMenu.SelectedItem is Skill)
      {
        var wSelectedItem = (Skill)mSideMenu.SelectedItem;
        wSelectedItem.Name += "__" + mSelectedSkillFamilyName;
        ViewModel.AddSkill(wSelectedItem);
      }
    }

    private void OnEditSkill(object sender, RoutedEventArgs e)
    {
      ViewModel.IsContextMenuOpen = false;
      ViewModel.EditComponent(mSideMenu.SelectedItem);
    }

    private void OnSkillSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (!AddNewCompnent(((TextBlock)sender).Text, "Skill"))
      {
        ViewModel.Context = "Skill";
        ViewModel.IsContextMenuOpen = true;
      }
    }

    private void OnSkillFamilySelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (!AddNewCompnent(((TextBlock)sender).Text, "Family"))
        e.Handled = true;
    }

    private void OnSelectedItemChanged(object sender, RoutedEventArgs e)
    {
      var wParent = (e.OriginalSource as DependencyObject).FindParent<TreeViewItem>();
      if (wParent is TreeViewItem)
        mSelectedSkillFamilyName = ((SkillFamily)(wParent.Header)).Name;
    }

    private void AddNewFamily(object sender, RoutedEventArgs e)
    {
      var wArgs = (SkillFamilyAdditionEventArgs)e;
      ViewModel.AddNewComponent(new SkillFamily { Name = wArgs.Name });
      mSelectedSkillFamilyName = wArgs.Name;
    }

    private bool AddNewCompnent(string text, string type)
    {
      if (text == Properties.Resources.AddNew)
      {
        ViewModel.AddNewComponent(type == "Skill" ? new SkillFamily
        {
          Skills = new List<Skill>(),
          Name = mSelectedSkillFamilyName
        } : null);
        return true;
      }
      return false;
    }

    private void mCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      ViewModel.Context = null;
      ViewModel.IsContextMenuOpen = false;
    }
  }
}
