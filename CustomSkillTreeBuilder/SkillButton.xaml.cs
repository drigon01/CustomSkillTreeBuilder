using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomSkillTreeBuilder
{
  /// <summary>
  /// Interaction logic for SkillButton.xaml
  /// </summary>
  public partial class SkillButton
  {
    static RoutedEvent ConnectEvent = EventManager.RegisterRoutedEvent
      ("Connect", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SkillButton));

    static RoutedEvent MovingEvent = EventManager.RegisterRoutedEvent
      ("MovingSkill", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SkillButton));

    static RoutedEvent DeleteEvent = EventManager.RegisterRoutedEvent
      ("Delete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SkillButton));

    private bool mIsDragging;
    private Canvas mParent;
    private Point mStartPosition;
    private SkillButtonViewModel ViewModel;

    public event RoutedEventHandler Connect
    {
      add { AddHandler(ConnectEvent, value); }
      remove { RemoveHandler(ConnectEvent, value); }
    }

    public event RoutedEventHandler Moving
    {
      add { AddHandler(MovingEvent, value); }
      remove { RemoveHandler(MovingEvent, value); }
    }

    public event RoutedEventHandler Delete
    {
      add { AddHandler(DeleteEvent, value); }
      remove { RemoveHandler(DeleteEvent, value); }
    }

    public SkillButton(UISkill skill)
    {
      Name = skill.Name;
      InitializeComponent();
      mIsDragging = false;
      ViewModel = (DataContext as SkillButtonViewModel);
      ViewModel.Initialze(skill);
      SetValue(Canvas.ZIndexProperty, 800);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      mParent = (Canvas)Parent;

      SetValue(Canvas.LeftProperty, (mParent.ActualWidth / 2) - (ActualWidth));
      SetValue(Canvas.TopProperty, (mParent.ActualHeight / 2) - (ActualHeight / 2));
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
      if (mIsDragging)
      {
        RaiseEvent(new RoutedEventArgs(MovingEvent));
        var wPos = this.DragOnCanvas(mParent, mStartPosition, e);

        ViewModel.Skill.CanvasLeft = wPos.X;
        ViewModel.Skill.CanvasTop = wPos.Y;
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

    protected override void OnMouseLeave(MouseEventArgs e)
    {
      mIsDragging = false;
      e.Handled = true;
    }

    private void OnConnect(object sender, RoutedEventArgs e)
    {
      RaiseEvent(new RoutedEventArgs(ConnectEvent));
    }

    private void OnDelete(object sender, RoutedEventArgs e)
    {
      RaiseEvent(new RoutedEventArgs(DeleteEvent));
    }
  }
}
