using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CustomSkillTreeBuilder
{
  /// <summary>
  /// Describes the collection of Skill families
  /// </summary>
  [XmlRoot("SkillTreeComponents")]
  public class SkillTreeComponents : List<SkillFamily> { }

  /// <summary>
  /// Describes a Skill family
  /// </summary>
  public class SkillFamily : IEquatable<SkillFamily>
  {
    public string Name { get; set; }
    public List<Skill> Skills { get; set; }

    public SkillFamily Copy()
    {
      var wCopy = new SkillFamily();
      wCopy.Name = this.Name;
      wCopy.Skills = this.Skills;

      return wCopy;
    }

    public bool Equals(SkillFamily other)
    {
      return Name == other.Name && ((other.Skills == null && Skills == null) ||
        !(other.Skills == null || Skills == null) && Skills.SequenceEqual(other.Skills));
    }
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
