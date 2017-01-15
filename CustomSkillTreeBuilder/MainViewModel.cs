using CustomSkillTreeBuilder.Properties;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CustomSkillTreeBuilder
{
  class MainViewModel
  {
    XmlReader mXmlReader;
    XmlWriter mXmlWriter;
    OpenFileDialog mOpenFileDialog;
    string mNodeConfigPath;

    public MainViewModel()
    {
      mOpenFileDialog = new OpenFileDialog();
      mOpenFileDialog.DefaultExt = "xml";
      mOpenFileDialog.InitialDirectory = Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().Location);

      mNodeConfigPath = mOpenFileDialog.InitialDirectory + "\\" + Resources.defaultFile;
    }

    public void Open()
    {
      if (mOpenFileDialog.ShowDialog() == true)
        mNodeConfigPath = File.ReadAllText(mOpenFileDialog.FileName);
      BuildNodes();
    }

    public void Save()
    {
      var wObject = new SkillTreeComponents
      {
        SkillFamilies = new System.Collections.Generic.List<SkillFamily>
        {
          new SkillFamily {Name="test",
              Skills = new System.Collections.Generic.List<Skill>
              {
                new Skill { Name="TestSkill",
                  Effects = new[] {"ability1","ability2" } }
              }
          }
        }
      };

      mXmlWriter = XmlWriter.Create("asd.xml", new XmlWriterSettings
      {
        Indent = true,
        NewLineOnAttributes = true
      });
      new XmlSerializer(typeof(SkillTreeComponents)).Serialize(mXmlWriter, wObject);

    }

    private void BuildNodes()
    {
      mXmlReader = XmlReader.Create(mNodeConfigPath);
      mXmlReader.ReadContentAs(typeof(SkillTreeComponents), null);
    }
  }
}
