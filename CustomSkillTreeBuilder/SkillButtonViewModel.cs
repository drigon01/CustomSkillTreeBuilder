using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace CustomSkillTreeBuilder
{
  public class SkillButtonViewModel : PropertyAware
  {
    private UISkill mSkill;

    public SkillButtonViewModel() { }

    public SkillButtonViewModel(UISkill skill)
    {
      Skill = skill;
      Skill.CanvasLeft = 0;
      Skill.CanvasTop = 0;
      Skill.ChildSkills = new System.Collections.Generic.List<string>();
    }

    public UISkill Skill
    {
      get { return mSkill; }
      set { mSkill = value; NotifyPropertyChanged(this.NameOf(p => p.Skill)); }
    }

    public ImageSource Image
    {
      get
      {
        return new BitmapImage(new Uri("../../Resources/vampire-dracula.png", UriKind.Relative));
      }
    }
  }
}
