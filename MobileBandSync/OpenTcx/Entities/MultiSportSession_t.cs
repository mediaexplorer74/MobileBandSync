// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.MultiSportSession_t
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
  public class MultiSportSession_t
  {
    private DateTime idField;
    private FirstSport_t firstSportField;
    private NextSport_t[] nextSportField;
    private string notesField;

    public DateTime Id
    {
      get => this.idField;
      set => this.idField = value;
    }

    public FirstSport_t FirstSport
    {
      get => this.firstSportField;
      set => this.firstSportField = value;
    }

    [XmlElement("NextSport")]
    public NextSport_t[] NextSport
    {
      get => this.nextSportField;
      set => this.nextSportField = value;
    }

    public string Notes
    {
      get => this.notesField;
      set => this.notesField = value;
    }
  }
}
