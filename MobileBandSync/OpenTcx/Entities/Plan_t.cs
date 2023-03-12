// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Plan_t
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
  public class Plan_t
  {
    private string nameField;
    private TrainingType_t typeField;
    private bool intervalWorkoutField;

    [XmlElement(DataType = "token")]
    public string Name
    {
      get => this.nameField;
      set => this.nameField = value;
    }

    [XmlAttribute]
    public TrainingType_t Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    [XmlAttribute]
    public bool IntervalWorkout
    {
      get => this.intervalWorkoutField;
      set => this.intervalWorkoutField = value;
    }
  }
}
