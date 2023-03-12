// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Folders_t
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
  public class Folders_t
  {
    private History_t historyField;
    private Workouts_t workoutsField;
    private Courses_t coursesField;

    public History_t History
    {
      get => this.historyField;
      set => this.historyField = value;
    }

    public Workouts_t Workouts
    {
      get => this.workoutsField;
      set => this.workoutsField = value;
    }

    public Courses_t Courses
    {
      get => this.coursesField;
      set => this.coursesField = value;
    }
  }
}
