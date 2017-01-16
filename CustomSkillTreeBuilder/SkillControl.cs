using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;

namespace CustomSkillTreeBuilder
{
  class SkillControl : Button
  {
    Canvas mParent;
    Point mStartPosition;
    
    bool mIsDragging;

    public SkillControl(Skill skill)
    {
      Height = 34;
      Width = 48;
      Content = skill.Name;

      ContextMenu = new ContextMenu();
      ContextMenu.Items.Add(new MenuItem { Header = "Connect" });
      ContextMenu.Items.Add(new MenuItem { Header = "Delete" });
      MouseMove += OnMouseMove;
      Loaded += OnLoaded;

      RenderTransformOrigin = new Point(0.5, 0.5);

      mIsDragging = false;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      mParent = (Canvas)Parent;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      mIsDragging = true;
      mStartPosition = e.GetPosition(this);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      mIsDragging = false;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
      mIsDragging = false;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
      if (mIsDragging)
      {
        var newX = e.GetPosition(mParent).X - mStartPosition.X;
        var newY = e.GetPosition(mParent).Y - mStartPosition.Y;

        SetValue(Canvas.TopProperty, newX);
        SetValue(Canvas.LeftProperty, newY);

      }

    }


  }
}
