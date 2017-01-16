using CustomSkillTreeBuilder.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace CustomSkillTreeBuilder
{
  class MainViewModel : PropertyAware
  {
    private ImageSource mBGImage;
    private OpenFileDialog mOpenFileDialog;
    private string mNodeConfigPath;
    private SkillTreeComponents mSkillTreeComponents;
    private SkillTree mSkilTree = new SkillTree();

    public MainViewModel()
    {
      BGImage = new BitmapImage(new Uri("bgimg.jpg", UriKind.RelativeOrAbsolute));
      mOpenFileDialog = new OpenFileDialog();
      mOpenFileDialog.DefaultExt = "xml";
      mOpenFileDialog.InitialDirectory = Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().Location);

      mNodeConfigPath = mOpenFileDialog.InitialDirectory + "\\" + Resources.defaultFile;
    }

    public ImageSource BGImage
    {
      get { return mBGImage; }
      set
      {
        mBGImage = value;
        NotifyPropertyChanged(this.NameOf(p => p.BGImage));
      }
    }

    public SkillTreeComponents SkillTreeComponents
    {
      get { return mSkillTreeComponents; }
      set
      {
        mSkillTreeComponents = value;
        NotifyPropertyChanged(this.NameOf(p => p.SkillTreeComponents));
      }
    }

    public void AddComponent(Canvas canvas, string skillName)
    {
      if (canvas.Children.Count == 0)
      {
        mSkilTree = new SkillTree();
      }

      var wSkill = SkillTreeComponents.SkillFamilies.SelectMany(f => f.Skills).FirstOrDefault(s => s.Name == skillName);
      if (wSkill == null) { return; }
      if (!mSkilTree.Select(k => k.Name).Contains(wSkill.Name))
      {
        wSkill.ChildSkills = new List<Skill>();
        mSkilTree.Add(wSkill);
        canvas.Children.Add(new SkillControl(wSkill));
      }
      else
      { MessageBox.Show("Already added"); }
    }

    public ScrollViewer Menu { get; set; }

    public void LoadComponents()
    {
      if (mOpenFileDialog.ShowDialog() == true)
        mNodeConfigPath = mOpenFileDialog.FileName;
      BuildNodes();
    }

    public void SaveComponents()
    {
      var wObject = TestTree();

      using (var wXmlWriter = XmlWriter.Create("asd.xml",
        new XmlWriterSettings { Indent = true, NewLineOnAttributes = true }))
      {
        new XmlSerializer(typeof(SkillTreeComponents)).Serialize(wXmlWriter, wObject);
      }
    }

    public void SaveSkillTree()
    {
      using (var wXmlWriter = XmlWriter.Create("asd_tree.xml",
        new XmlWriterSettings { Indent = true, NewLineOnAttributes = true }))
      {
        new XmlSerializer(typeof(SkillTree)).Serialize(wXmlWriter, mSkilTree);
      }
    }

    private void BuildNodes()
    {
      SkillTreeComponents wObejct;
      try
      {
        var wSerializer = new XmlSerializer(typeof(SkillTreeComponents));

        using (var wStream = new StringReader(File.ReadAllText(mNodeConfigPath)))
        using (var wReader = XmlReader.Create(wStream))
        {
          wObejct = (SkillTreeComponents)wSerializer.Deserialize(wReader);
        }
        SkillTreeComponents = wObejct;
      }
      catch (Exception wException)
      {
        MessageBox.Show(wException.Message, "Xml Parsing Error");
      }
    }

    private SkillTreeComponents TestTree()
    {
      return new SkillTreeComponents
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
    }
  }
}
