using CustomSkillTreeBuilder.Properties;
using Microsoft.Win32;
using System;
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
    internal Canvas Canvas;
    private ImageSource mBGImage;
    private OpenFileDialog mOpenFileDialog;
    private string mNodeConfigPath;
    private SkillTreeComponents mSkillTreeComponents;
    private SkillTree mSkilTree = new SkillTree();
    private SkillButton mSelected;

    public MainViewModel()
    {
      BGImage = new BitmapImage(new Uri("../../Resources/bgimg.jpg", UriKind.Relative));
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

    public void AddComponent(string skillName)
    {
      if (Canvas.Children.Count == 0)
      {
        mSkilTree = new SkillTree();
      }

      var wSkill = SkillTreeComponents.SelectMany(f => f.Skills).FirstOrDefault(s => s.Name == skillName);
      if (wSkill == null) { return; }
      if (!mSkilTree.Select(k => k.Name).Contains(wSkill.Name))
      {
        var wUISkill = new UISkill { Effects = wSkill.Effects, Name = wSkill.Name };
        mSkilTree.Add(wUISkill);

        var wButton = new SkillButton(wUISkill);
        wButton.Connect += OnConnect;
        Canvas.Children.Add(wButton);
      }
      else
      { MessageBox.Show("Already added"); }
    }

    private void OnConnect(object sender, RoutedEventArgs e)
    {
      if (mSelected == null)
      {
        mSelected = (SkillButton)sender;
      }
      else
      {
        var wPos1 = GetButtonCenterPosition(mSelected);
        var wPos2 = GetButtonCenterPosition((SkillButton)sender);
        Canvas.Children.Add(
          new System.Windows.Shapes.Line
          {
            X1 = wPos1.X,
            Y1 = wPos1.Y,
            X2 = wPos2.X,
            Y2 = wPos2.Y,
            Stroke = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
            StrokeThickness = 2
          });
        mSelected = null;
      }
    }

    private Point GetButtonCenterPosition(Button button)
    {
      return new Point(Canvas.GetLeft(button) + 32
        , Canvas.GetTop(button) + 32);
    }

    internal void EditComponent(object wSkill)
    {
      throw new NotImplementedException();
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
        new SkillFamily {Name="test",
            Skills = new System.Collections.Generic.List<Skill>
            {
              new Skill { Name="TestSkill",
                Effects = new[] {"ability1","ability2" } }
            }
        }
      };
    }
  }
}

