// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.CoursePoint_t
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
  public class CoursePoint_t
  {
    private string nameField;
    private DateTime timeField;
    private Position_t positionField;
    private double altitudeMetersField;
    private bool altitudeMetersFieldSpecified;
    private CoursePointType_t pointTypeField;
    private string notesField;

    [XmlElement(DataType = "token")]
    public string Name
    {
      get => this.nameField;
      set => this.nameField = value;
    }

    public DateTime Time
    {
      get => this.timeField;
      set => this.timeField = value;
    }

    public Position_t Position
    {
      get => this.positionField;
      set => this.positionField = value;
    }

    public double AltitudeMeters
    {
      get => this.altitudeMetersField;
      set => this.altitudeMetersField = value;
    }

    [XmlIgnore]
    public bool AltitudeMetersSpecified
    {
      get => this.altitudeMetersFieldSpecified;
      set => this.altitudeMetersFieldSpecified = value;
    }

    public CoursePointType_t PointType
    {
      get => this.pointTypeField;
      set => this.pointTypeField = value;
    }

    public string Notes
    {
      get => this.notesField;
      set => this.notesField = value;
    }
  }
}
