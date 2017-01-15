using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CustomSkillTreeBuilder
{
  public class SkillTreeComponents
  {
    public List<SkillFamily> SkillFamilies { get; set; }
  }
  public class SkillFamily
  {
    public string Name { get; set; }
    public List<Skill> Skills { get; set; }
  }
  public class Skill
  {
    public string Name { get; set; }
    [XmlElement("Effect")]
    public string[] Effects { get; set; }
  }
}
