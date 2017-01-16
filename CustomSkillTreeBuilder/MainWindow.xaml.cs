using System.Windows;
using System.Windows.Controls;

namespace CustomSkillTreeBuilder
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Open_Clicked(object sender, RoutedEventArgs e)
    {
      var wViewModel = (DataContext as MainViewModel);
      if ((string)(sender as MenuItem).CommandParameter == "Components")
      {
        wViewModel.LoadComponents();
      }
    }

    private void Save_Clicked(object sender, RoutedEventArgs e)
    {
      var wViewModel = (DataContext as MainViewModel);
      if ((string)(sender as MenuItem).CommandParameter == "Components")
      {
        wViewModel.SaveComponents();
      }
      else
      {
        wViewModel.SaveSkillTree();
      }
    }

    private void SkillSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var wViewModel = (DataContext as MainViewModel);
      var wSkill = ((TextBlock)sender).Text;
      switch (MessageBox.Show("Select Action, Yes=>Add, No=>Edit, Cancel=>Cancel", "Add/Edit", MessageBoxButton.YesNoCancel))
      {
        case MessageBoxResult.Yes:
          wViewModel.AddComponent(mCanvas, wSkill);
          break;
        case MessageBoxResult.No:
          break;
        default:
          break;
      }
    }
  }
}
