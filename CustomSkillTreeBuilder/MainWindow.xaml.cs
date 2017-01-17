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
      ViewModel.AddComponent((string)((MenuItem)sender).CommandParameter);
    }

    private void OnEditSkill(object sender, RoutedEventArgs e)
    {
      ViewModel.EditComponent((string)((MenuItem)sender).CommandParameter);
    }
  }
}
