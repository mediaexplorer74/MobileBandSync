// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.HistoryFolder_t
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MobileBandSync.OpenTcx.Entities
{
  [GeneratedCode("xsd", "4.0.30319.1")]
  [DebuggerStepThrough]
  [XmlType(Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
  public class HistoryFolder_t
  {
    private HistoryFolder_t[] folderField;
    private ActivityReference_t[] activityRefField;
    private Week_t[] weekField;
    private string notesField;
    private string nameField;

    [XmlElement("Folder")]
    public HistoryFolder_t[] Folder
    {
      get => this.folderField;
      set => this.folderField = value;
    }

    [XmlElement("ActivityRef")]
    public ActivityReference_t[] ActivityRef
    {
      get => this.activityRefField;
      set => this.activityRefField = value;
    }

    [XmlElement("Week")]
    public Week_t[] Week
    {
      get => this.weekField;
      set => this.weekField = value;
    }

    public string Notes
    {
      get => this.notesField;
      set => this.notesField = value;
    }

    [XmlAttribute]
    public string Name
    {
      get => this.nameField;
      set => this.nameField = value;
    }
  }
}
