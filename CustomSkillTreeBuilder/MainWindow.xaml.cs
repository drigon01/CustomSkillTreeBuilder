using System;
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
      ViewModel.IsSkillContextMenuOpen = false;
      var wSelectedItem = (Skill)mSideMenu.SelectedItem;
      wSelectedItem.Name += "__" + mSelectedSkillFamilyName;
      ViewModel.AddSkill(wSelectedItem);
    }

    private void OnEditSkill(object sender, RoutedEventArgs e)
    {
      ViewModel.IsSkillContextMenuOpen = false;
      ViewModel.EditComponent(mSideMenu.SelectedItem);
    }

    private void OnSkillSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (((TextBlock)sender).Text == Properties.Resources.AddNew)
      {
        ViewModel.AddNewComponent(new SkillFamily
        {
          Skills = new List<Skill>() { ((Skill)mSideMenu.SelectedItem) },
          Name = mSelectedSkillFamilyName
        });
      }
      else { ViewModel.IsSkillContextMenuOpen = true; }
    }

    private void OnSkillFamilySelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var wFamilyName = ((TextBlock)sender).Text;
      if (wFamilyName == Properties.Resources.AddNew)
      {
        ViewModel.AddNewComponent(null);
      }
      else { e.Handled = true; }
    }

    private void OnSelectedItemChanged(object sender, RoutedEventArgs e)
    {
      var wParent = (e.OriginalSource as DependencyObject).FindParent<TreeViewItem>();
      if (wParent is TreeViewItem)
        mSelectedSkillFamilyName = ((SkillFamily)(wParent.Header)).Name;
    }

    private void AddNewFamily(object sender, RoutedEventArgs e)
    {
      ViewModel.AddNewComponent(new SkillFamily { Name = mNewName.Text });
      mSelectedSkillFamilyName = mNewName.Text;
    }
  }
}
