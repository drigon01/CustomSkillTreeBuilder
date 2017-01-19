using System;
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
      ViewModel.IsSkillContextMenuOpen = false;
      var wSelectedItem = (Skill)mSideMenu.SelectedItem;
      wSelectedItem.Name += "__" + mSelectedSkillFamilyName;
      ViewModel.AddComponent(wSelectedItem);
    }

    private void OnEditSkill(object sender, RoutedEventArgs e)
    {
      ViewModel.IsSkillContextMenuOpen = false;
      ViewModel.EditComponent(mSideMenu.SelectedItem);
    }

    private void OnSkillSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var wSource = (e.OriginalSource as TreeViewItem);
      ViewModel.IsSkillContextMenuOpen = true;
    }

    private void OnSkillFamilySelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      //((TreeViewItem)sender).IsExpanded = !((TreeViewItem)sender).IsExpanded;
      e.Handled = true;
    }

    private void OnSelectedItemChanged(object sender, RoutedEventArgs e)
    {
      var wParent = GetSelectedTreeViewItemParent(e.OriginalSource as TreeViewItem);
      if (wParent != null)
        mSelectedSkillFamilyName = ((SkillFamily)(wParent.Header)).Name;
    }

    public TreeViewItem GetSelectedTreeViewItemParent(TreeViewItem item)
    {
      DependencyObject parent = System.Windows.Media.VisualTreeHelper.GetParent(item);
      while (parent != null
        && !(parent is TreeViewItem || parent is TreeView))
      {
        parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
      }

      return parent == null ? null : parent as TreeViewItem;
    }
  }
}
