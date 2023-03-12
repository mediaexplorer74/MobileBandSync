// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.WorkoutFolder_t
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
  public class WorkoutFolder_t
  {
    private WorkoutFolder_t[] folderField;
    private NameKeyReference_t[] workoutNameRefField;
    private string nameField;

    [XmlElement("Folder")]
    public WorkoutFolder_t[] Folder
    {
      get => this.folderField;
      set => this.folderField = value;
    }

    [XmlElement("WorkoutNameRef")]
    public NameKeyReference_t[] WorkoutNameRef
    {
      get => this.workoutNameRefField;
      set => this.workoutNameRefField = value;
    }

    [XmlAttribute]
    public string Name
    {
      get => this.nameField;
      set => this.nameField = value;
    }
  }
}
