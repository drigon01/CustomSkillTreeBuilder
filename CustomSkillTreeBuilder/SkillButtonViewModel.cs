using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace CustomSkillTreeBuilder
{
  class SkillButtonViewModel : PropertyAware
  {


    public void Initialze(UISkill skill)
    {
      Skill = skill;
      Skill.CanvasLeft = 0;
      Skill.CanvasTop = 0;
      Skill.ChildSkills = new System.Collections.Generic.List<Skill>();
    }

    public UISkill Skill { get; set; }

    public ImageSource Image
    {
      get
      {
        return new BitmapImage(new Uri("../../Resources/vampire-dracula.png", UriKind.Relative));
      }
    }
  }
}
