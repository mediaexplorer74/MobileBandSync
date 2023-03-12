// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.ActivityList_t
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
  public class ActivityList_t
  {
    private Activity_t[] activityField;
    private MultiSportSession_t[] multiSportSessionField;

    [XmlElement("Activity")]
    public Activity_t[] Activity
    {
      get => this.activityField;
      set => this.activityField = value;
    }

    [XmlElement("MultiSportSession")]
    public MultiSportSession_t[] MultiSportSession
    {
      get => this.multiSportSessionField;
      set => this.multiSportSessionField = value;
    }
  }
}
