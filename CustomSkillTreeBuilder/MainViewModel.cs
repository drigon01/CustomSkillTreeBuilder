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
    internal Canvas Canvas;
    private ImageSource mBGImage;
    private OpenFileDialog mOpenFileDialog;
    private SaveFileDialog mSaveFileDialog;
    public string mSelectedSkillName;
    private string mNodeConfigPath;
    private bool mIsSkillContextMenuOpen;
    private bool mIsAddNewSkillFamilyDialogVisible;
    private SkillTreeComponents mSkillTreeComponents;
    private SkillTree mSkilTree = new SkillTree();
    private SkillButton mSelected;

    private readonly SkillFamily mAddSkillFamily = new SkillFamily { Name = Resources.AddNew };
    private readonly Skill mAddSkill = new Skill
    {
      Name = Resources.AddNew,
      Effects = new[] { "By clicking this you can add a new skill under this family." }
    };

    public MainViewModel()
    {
      mOpenFileDialog = new OpenFileDialog();
      mSaveFileDialog = new SaveFileDialog();
      mSaveFileDialog.DefaultExt = mOpenFileDialog.DefaultExt = "xml";
      mSaveFileDialog.InitialDirectory = mOpenFileDialog.InitialDirectory = Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().Location);

      mIsSkillContextMenuOpen = false;

      mNodeConfigPath = mOpenFileDialog.InitialDirectory + "\\" + Resources.defaultFile;
      BGImage = new BitmapImage(new Uri("../../Resources/bgimg.jpg", UriKind.Relative));
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

    public bool IsSkillContextMenuOpen
    {
      get { return mIsSkillContextMenuOpen; }
      set
      {
        mIsSkillContextMenuOpen = value;
        NotifyPropertyChanged(this.NameOf(p => p.IsSkillContextMenuOpen));
      }
    }

    public bool IsAddNewSkillFamilyDialogOpen
    {
      get { return mIsAddNewSkillFamilyDialogVisible; }
      set
      {
        mIsAddNewSkillFamilyDialogVisible = value;
        NotifyPropertyChanged(this.NameOf(p => p.IsAddNewSkillFamilyDialogOpen));
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

    public void AddSkill(Skill skill)
    {
      if (Canvas.Children.Count == 0)
      {
        mSkilTree = new SkillTree();
      }

      var wSkill = SkillTreeComponents.SelectMany(f => f.Skills).FirstOrDefault(s => s.Name == skill.Name);
      if (wSkill == null) { return; }
      if (!mSkilTree.Select(k => k.Name).Contains(wSkill.Name))
      {
        var wUISkill = new UISkill { Effects = wSkill.Effects, Name = wSkill.Name };
        mSkilTree.Add(wUISkill);

        var wButton = new SkillButton(wUISkill);
        wButton.Connect += OnConnect;
        wButton.Moving += OnDraggingSkill;

        Canvas.Children.Add(wButton);
      }
      else
      { MessageBox.Show("Already added"); }
    }

    private void OnDraggingSkill(object sender, RoutedEventArgs e)
    {
      var wButton = (SkillButton)sender;
      var wCenter = GetButtonCenterPosition(wButton);

      foreach (var wUiElement in Canvas.Children)
      {
        var wLine = wUiElement as System.Windows.Shapes.Line;
        if (wLine != null && wLine.Name.Contains(wButton.Name))
        {
          if (wLine.Name.StartsWith(wButton.Name))
          {
            wLine.X1 = wCenter.X;
            wLine.Y1 = wCenter.Y;
          }
          else
          {
            wLine.X2 = wCenter.X;
            wLine.Y2 = wCenter.Y;
          }
        }
      }
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
        var wConnector =
          new System.Windows.Shapes.Line
          {
            X1 = wPos1.X,
            Y1 = wPos1.Y,
            X2 = wPos2.X,
            Y2 = wPos2.Y,
            Stroke = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
            StrokeThickness = 2,
            Name = string.Format("{0}_{1}", mSelected.Name, ((SkillButton)sender).Name)
          };

        Canvas.Children.Add(wConnector);
        Panel.SetZIndex(wConnector, 0);

        mSelected = null;
      }
    }

    private Point GetButtonCenterPosition(Button button)
    {
      return new Point(Canvas.GetLeft(button) + button.ActualWidth / 2
        , Canvas.GetTop(button) + button.ActualHeight / 2);
    }

    internal void EditComponent(object wSkill)
    {
      throw new NotImplementedException();
    }

    public string SelectedSkillName
    {
      get { return mSelectedSkillName; }
      internal set
      {
        mSelectedSkillName = value;
        NotifyPropertyChanged(this.NameOf(p => p.SelectedSkillName));
      }
    }

    public void LoadComponents()
    {
      if (mOpenFileDialog.ShowDialog() == true)
        mNodeConfigPath = mOpenFileDialog.FileName;
      BuildNodes();
    }

    public void SaveComponents()
    {
      var wList = SkillTreeComponents;
      wList.Remove(mAddSkillFamily);
      wList.ForEach(f => f.Skills.Remove(mAddSkill));

      if (mSaveFileDialog.ShowDialog() == true)
      {
        using (var wXmlWriter = XmlWriter.Create(mSaveFileDialog.FileName,
        new XmlWriterSettings { Indent = true, NewLineOnAttributes = true }))
        {
          new XmlSerializer(typeof(SkillTreeComponents)).Serialize(wXmlWriter, wList);
        }
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
        wObejct.Add(mAddSkillFamily);
        SkillTreeComponents = wObejct;
        SkillTreeComponents.ForEach(f => f.Skills.Add(mAddSkill));
      }
      catch (Exception wException)
      {
        MessageBox.Show(wException.Message, "Xml Parsing Error");
      }
    }

    public void AddNewComponent(SkillFamily family)
    {
      if (family is SkillFamily && family.Skills == null)
      {
        var wComponents = SkillTreeComponents;
        wComponents.Add(family);
        SkillTreeComponents.Clear();
        SkillTreeComponents = wComponents;
        IsAddNewSkillFamilyDialogOpen = false;
      }
      else if (family is SkillFamily && SkillTreeComponents.Contains(family,
        new EqualityComparer<SkillFamily>((f1, f2) => f1.Name == f2.Name)))
      {
        SkillTreeComponents.FirstOrDefault(f => f.Name == family.Name).Skills.AddRange(family.Skills);
      }
      else
      {
        IsAddNewSkillFamilyDialogOpen = true;
      }
    }

    /* private SkillTreeComponents TestTree()
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
     }*/
  }
}

