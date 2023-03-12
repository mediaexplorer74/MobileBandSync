// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Version_t
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
  public class Version_t
  {
    private ushort versionMajorField;
    private ushort versionMinorField;
    private ushort buildMajorField;
    private bool buildMajorFieldSpecified;
    private ushort buildMinorField;
    private bool buildMinorFieldSpecified;

    public ushort VersionMajor
    {
      get => this.versionMajorField;
      set => this.versionMajorField = value;
    }

    public ushort VersionMinor
    {
      get => this.versionMinorField;
      set => this.versionMinorField = value;
    }

    public ushort BuildMajor
    {
      get => this.buildMajorField;
      set => this.buildMajorField = value;
    }

    [XmlIgnore]
    public bool BuildMajorSpecified
    {
      get => this.buildMajorFieldSpecified;
      set => this.buildMajorFieldSpecified = value;
    }

    public ushort BuildMinor
    {
      get => this.buildMinorField;
      set => this.buildMinorField = value;
    }

    [XmlIgnore]
    public bool BuildMinorSpecified
    {
      get => this.buildMinorFieldSpecified;
      set => this.buildMinorFieldSpecified = value;
    }
  }
}
