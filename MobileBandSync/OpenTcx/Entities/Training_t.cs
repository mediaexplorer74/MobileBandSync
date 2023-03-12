// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Training_t
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
  public class Training_t
  {
    private QuickWorkout_t quickWorkoutResultsField;
    private Plan_t planField;
    private bool virtualPartnerField;

    public QuickWorkout_t QuickWorkoutResults
    {
      get => this.quickWorkoutResultsField;
      set => this.quickWorkoutResultsField = value;
    }

    public Plan_t Plan
    {
      get => this.planField;
      set => this.planField = value;
    }

    [XmlAttribute]
    public bool VirtualPartner
    {
      get => this.virtualPartnerField;
      set => this.virtualPartnerField = value;
    }
  }
}
