// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Activity_t
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
  public class Activity_t
  {
    private DateTime idField;
    private ActivityLap_t[] lapField;
    private string notesField;
    private Training_t trainingField;
    private AbstractSource_t creatorField;
    private Sport_t sportField;

    public DateTime Id
    {
      get => this.idField;
      set => this.idField = value;
    }

    [XmlElement("Lap")]
    public ActivityLap_t[] Lap
    {
      get => this.lapField;
      set => this.lapField = value;
    }

    public string Notes
    {
      get => this.notesField;
      set => this.notesField = value;
    }

    public Training_t Training
    {
      get => this.trainingField;
      set => this.trainingField = value;
    }

    public AbstractSource_t Creator
    {
      get => this.creatorField;
      set => this.creatorField = value;
    }

    [XmlAttribute]
    public Sport_t Sport
    {
      get => this.sportField;
      set => this.sportField = value;
    }
  }
}
