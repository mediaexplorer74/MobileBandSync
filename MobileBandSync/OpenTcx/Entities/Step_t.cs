// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Step_t
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
  public class Step_t : AbstractStep_t
  {
    private string nameField;
    private Duration_t durationField;
    private Intensity_t intensityField;
    private Target_t targetField;

    [XmlElement(DataType = "token")]
    public string Name
    {
      get => this.nameField;
      set => this.nameField = value;
    }

    public Duration_t Duration
    {
      get => this.durationField;
      set => this.durationField = value;
    }

    public Intensity_t Intensity
    {
      get => this.intensityField;
      set => this.intensityField = value;
    }

    public Target_t Target
    {
      get => this.targetField;
      set => this.targetField = value;
    }
  }
}
