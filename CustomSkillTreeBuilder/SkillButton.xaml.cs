using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace CustomSkillTreeBuilder
{
  /// <summary>
  /// Interaction logic for SkillButton.xaml
  /// </summary>
  public partial class SkillButton : Button
  {
    static RoutedEvent ConnectEvent = EventManager.RegisterRoutedEvent
      ("Connect", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SkillButton));


    private bool mIsDragging;
    private Canvas mParent;
    private Point mStartPosition;
    private SkillButtonViewModel ViewModel;


    public event RoutedEventHandler Connect
    {
      add { AddHandler(ConnectEvent, value); }
      remove { RemoveHandler(ConnectEvent, value); }
    }

    public SkillButton(UISkill skill)
    {
      InitializeComponent();
      mIsDragging = false;
      ViewModel = (DataContext as SkillButtonViewModel);
      ViewModel.Initialze(skill);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      mParent = (Canvas)Parent;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
      if (mIsDragging)
      {
        var wPos = e.GetPosition(mParent);
        var wX = wPos.X - mStartPosition.X;
        var wY = wPos.Y - mStartPosition.Y;

        ViewModel.Skill.CanvasLeft = wX;
        ViewModel.Skill.CanvasTop = wY;

        SetValue(Canvas.LeftProperty, 0 < wX && wX < mParent.ActualWidth ? wX : wPos.X);
        SetValue(Canvas.TopProperty, 0 < wY && wY < mParent.ActualHeight ? wY : wPos.Y);
      }
      e.Handled = true;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      mIsDragging = true;
      mStartPosition = e.GetPosition(this);
      e.Handled = true;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      mIsDragging = false;
      e.Handled = true;
    }

    private void OnConnect(object sender, RoutedEventArgs e)
    {
      RaiseEvent(new RoutedEventArgs(ConnectEvent));
    }
  }
}
