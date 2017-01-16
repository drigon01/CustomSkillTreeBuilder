using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomSkillTreeBuilder
{
  /// <summary>
  /// Interaction logic for SkillButton.xaml
  /// </summary>
  public partial class SkillButton : Button
  {
    private bool mIsDragging;
    private Canvas mParent;
    private Point mStartPosition;
    private SkillButtonViewModel ViewModel;


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
        var newX = wPos.X - mStartPosition.X;
        var newY = wPos.Y - mStartPosition.Y;

        SetValue(Canvas.TopProperty, 0 < newY && newY < mParent.ActualHeight ? newY : wPos.Y);
        SetValue(Canvas.LeftProperty, 0 < newX && newX < mParent.ActualWidth ? newX : wPos.X);
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
  }
}
