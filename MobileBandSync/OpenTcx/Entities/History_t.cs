// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.History_t
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
  public class History_t
  {
    private HistoryFolder_t runningField;
    private HistoryFolder_t bikingField;
    private HistoryFolder_t otherField;
    private MultiSportFolder_t multiSportField;

    public HistoryFolder_t Running
    {
      get => this.runningField;
      set => this.runningField = value;
    }

    public HistoryFolder_t Biking
    {
      get => this.bikingField;
      set => this.bikingField = value;
    }

    public HistoryFolder_t Other
    {
      get => this.otherField;
      set => this.otherField = value;
    }

    public MultiSportFolder_t MultiSport
    {
      get => this.multiSportField;
      set => this.multiSportField = value;
    }
  }
}
