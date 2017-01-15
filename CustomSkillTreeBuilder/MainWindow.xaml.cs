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
    }
  }
}
