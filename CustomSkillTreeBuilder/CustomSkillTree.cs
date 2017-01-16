using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CustomSkillTreeBuilder
{
  /// <summary>
  /// Describes the collection of Skill families
  /// </summary>
  public class SkillTreeComponents
  {
    public List<SkillFamily> SkillFamilies { get; set; }
  }

  /// <summary>
  /// Describes a Skill family
  /// </summary>
  public class SkillFamily
  {
    public string Name { get; set; }
    public List<Skill> Skills { get; set; }
  }

  /// <summary>
  /// Describes a skill
  /// </summary>
  public class Skill
  {
    public string Name { get; set; }
    [XmlElement("Effect")]
    public string[] Effects { get; set; }
  }

  [XmlRoot(ElementName = "Skill")]
  public class UISkill : Skill
  {
    public double CanvasTop;
    public double CanvasLeft;
    [XmlElement("Skill")]
    public List<Skill> ChildSkills { get; set; }
  }

  /// <summary>
  /// Describes a Skill tree with connections and variosu skills;
  /// </summary>
  [XmlRoot(ElementName = "SkillTree")]
  public class SkillTree : List<UISkill> { }
}
