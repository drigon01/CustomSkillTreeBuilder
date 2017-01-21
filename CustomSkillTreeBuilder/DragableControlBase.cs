using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomSkillTreeBuilder
{
  public class DragableControlBase : UserControl
  {
    private bool mIsDragging;
    protected Canvas mParent;
    private Point mStartPosition;

    static RoutedEvent MovingEvent = EventManager.RegisterRoutedEvent
          ("Moving", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SkillButton));

    protected DragableControlBase()
    {
      Loaded += OnLoaded;
      MouseMove += OnMouseMove;
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

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
      if (mIsDragging)
      {
        RaiseEvent(new RoutedEventArgs(MovingEvent));
        var wPos = this.DragOnCanvas(mParent, mStartPosition, e);
      }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      mParent = this.FindParent<Canvas>();
    }
  }
}
