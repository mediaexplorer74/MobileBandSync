// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Workout_t
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
  public class Workout_t
  {
    private string nameField;
    private AbstractStep_t[] stepField;
    private DateTime[] scheduledOnField;
    private string notesField;
    private AbstractSource_t creatorField;
    private Sport_t sportField;

    [XmlElement(DataType = "token")]
    public string Name
    {
      get => this.nameField;
      set => this.nameField = value;
    }

    [XmlElement("Step")]
    public AbstractStep_t[] Step
    {
      get => this.stepField;
      set => this.stepField = value;
    }

    [XmlElement("ScheduledOn", DataType = "date")]
    public DateTime[] ScheduledOn
    {
      get => this.scheduledOnField;
      set => this.scheduledOnField = value;
    }

    public string Notes
    {
      get => this.notesField;
      set => this.notesField = value;
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
