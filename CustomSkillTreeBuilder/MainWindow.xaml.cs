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

    private void SkillSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var wSkill = ((TextBlock)sender).Text;
      switch (MessageBox.Show("Select Action, Yes=>Add, No=>Edit, Cancel=>Cancel", "Add/Edit", MessageBoxButton.YesNoCancel))
      {
        case MessageBoxResult.Yes:
          ViewModel.AddComponent(mCanvas, wSkill);
          break;
        case MessageBoxResult.No:
          break;
        default:
          break;
      }
    }
  }
}
