using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomSkillTreeBuilder
{
  /// <summary>
  /// Interaction logic for SkillDialog.xaml
  /// </summary>
  public partial class SkillDialog : DragableControlBase
  {
    static RoutedEvent AddFamilyEvent = EventManager.RegisterRoutedEvent
      ("AddFamily", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SkillDialog));

    static RoutedEvent AddSkillEvent = EventManager.RegisterRoutedEvent
      ("AddSkill", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SkillDialog));

    static RoutedEvent EditSkillEvent = EventManager.RegisterRoutedEvent
      ("EditSkill", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SkillDialog));

    public event RoutedEventHandler AddFamily
    {
      add { AddHandler(AddFamilyEvent, value); }
      remove { RemoveHandler(AddFamilyEvent, value); }
    }

    public event RoutedEventHandler AddSkill
    {
      add { AddHandler(AddSkillEvent, value); }
      remove { RemoveHandler(AddSkillEvent, value); }
    }

    public event RoutedEventHandler EditSkill
    {
      add { AddHandler(EditSkillEvent, value); }
      remove { RemoveHandler(EditSkillEvent, value); }
    }

    SkillDialogViewModel ViewModel;
    string mImage;

    public SkillDialog()
    {
      Visibility = Visibility.Hidden;
      InitializeComponent();

      Loaded += (s, e) =>
        IsVisibleChanged += OnVisibleChanged;
    }

    private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if ((bool)e.NewValue)
      {
        SetValue(Canvas.LeftProperty, (mParent.ActualWidth / 2) - (ActualWidth));
        SetValue(Canvas.TopProperty, (mParent.ActualHeight / 2) - (ActualHeight / 2));
        SetValue(Canvas.ZIndexProperty, 1000);

        ViewModel = (SkillDialogViewModel)mGrid.DataContext;
        GenerateContextMenu((string)Tag);
      }
      else
      { ViewModel.Items.Clear(); }
    }

    private void GenerateContextMenu(string type)
    {
      var wAdd = new Button();
      wAdd.Content = "Add";
      wAdd.Width = 84;
      wAdd.Margin = new Thickness(50, 0, 0, 0);

      var wNameBox = new TextBox();
      wNameBox.Width = 240;
      wNameBox.Height = 34;
      wNameBox.Text = "Enter Skill Family name here.";
      wNameBox.GotFocus += (s, e) => wNameBox.Text = wNameBox.Text == "Enter Skill Family name here." ? "" : wNameBox.Text;

      switch (type)
      {
        case "Skill":
          wAdd.Click += (s, e) => RaiseEvent(new RoutedEventArgs(AddSkillEvent));

          var wEdit = new Button();
          wEdit.Content = "Edit";
          wEdit.Width = 84;
          wEdit.Margin = new Thickness(50, 0, 0, 0);
          wEdit.Click += (s, e) => RaiseEvent(new RoutedEventArgs(EditSkillEvent));

          wEdit.SetValue(Grid.ColumnProperty, 1);
          wAdd.SetValue(Grid.ColumnProperty, 0);
          ViewModel.Items.Add(wAdd);
          ViewModel.Items.Add(wEdit);
          break;

        case "NewFamily":
          wAdd.Click += (s, e) => RaiseEvent(new SkillFamilyAdditionEventArgs(wNameBox.Text, AddFamilyEvent));

          wAdd.SetValue(Grid.ColumnProperty, 1);
          wNameBox.SetValue(Grid.ColumnProperty, 0);
          ViewModel.Items.Add(wAdd);
          ViewModel.Items.Add(wNameBox);
          break;

        case "NewSkill":
          var wImageBrowser = new Button();
          wImageBrowser.Content = "add image";
          wImageBrowser.Click += (s, e) => GetImage();

          wAdd.Click += (s, e) => RaiseEvent(new SkillAdditionEventArgs(wNameBox.Text, null, mImage, AddFamilyEvent));

          ViewModel.Items.Add(wAdd);
          ViewModel.Items.Add(wNameBox);
          ViewModel.Items.Add(wImageBrowser);
          break;

        default:
          var wError = new TextBlock();
          wError.Text = "Oops, something went wrong.";
          Grid.SetColumnSpan(wError, 2);
          ViewModel.Items.Add(wError);
          break;
      }
    }


    private void GetImage()
    {
      var wImageBrowser = new OpenFileDialog();
      wImageBrowser.Title = "Select a picture";
      wImageBrowser.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
        "Portable Network Graphic (*.png)|*.png";
      if (wImageBrowser.ShowDialog() == true)
      {
        mImage = wImageBrowser.FileName;
      }

    }
  }
  class SkillFamilyAdditionEventArgs : RoutedEventArgs
  {
    public string Name { get; set; }

    internal SkillFamilyAdditionEventArgs(string name, RoutedEvent routedEvent)
    {
      Name = name;
      RoutedEvent = routedEvent;
    }
  }

  class SkillAdditionEventArgs : RoutedEventArgs
  {
    public string Name { get; set; }
    public string[] Effects { get; set; }
    public string Image { get; set; }

    internal SkillAdditionEventArgs(string name, string[] effects, string imgPath, RoutedEvent routedEvent)
    {
      Name = name;
      Effects = effects;
      Image = imgPath;

      RoutedEvent = routedEvent;
    }
  }
}

