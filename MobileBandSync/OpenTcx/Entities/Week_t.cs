// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Week_t
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MobileBandSync.OpenTcx.Entities
{
  [GeneratedCode("xsd", "4.0.30319.1")]
  [DebuggerStepThrough]
  [XmlType(Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
  public class Week_t
  {
    private string notesField;
    private DateTime startDayField;

    public string Notes
    {
      get => this.notesField;
      set => this.notesField = value;
    }

    [XmlAttribute(DataType = "date")]
    public DateTime StartDay
    {
      get => this.startDayField;
      set => this.startDayField = value;
    }
  }
}
