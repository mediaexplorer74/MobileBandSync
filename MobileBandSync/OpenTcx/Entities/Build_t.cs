// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Build_t
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
  public class Build_t
  {
    private Version_t versionField;
    private BuildType_t typeField;
    private bool typeFieldSpecified;
    private string timeField;
    private string builderField;

    public Version_t Version
    {
      get => this.versionField;
      set => this.versionField = value;
    }

    public BuildType_t Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    [XmlIgnore]
    public bool TypeSpecified
    {
      get => this.typeFieldSpecified;
      set => this.typeFieldSpecified = value;
    }

    [XmlElement(DataType = "token")]
    public string Time
    {
      get => this.timeField;
      set => this.timeField = value;
    }

    [XmlElement(DataType = "token")]
    public string Builder
    {
      get => this.builderField;
      set => this.builderField = value;
    }
  }
}
