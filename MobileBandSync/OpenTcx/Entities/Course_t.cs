// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Course_t
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
  public class Course_t
  {
    private string nameField;
    private CourseLap_t[] lapField;
    private Trackpoint_t[] trackField;
    private string notesField;
    private CoursePoint_t[] coursePointField;
    private AbstractSource_t creatorField;

    [XmlElement(DataType = "token")]
    public string Name
    {
      get => this.nameField;
      set => this.nameField = value;
    }

    [XmlElement("Lap")]
    public CourseLap_t[] Lap
    {
      get => this.lapField;
      set => this.lapField = value;
    }

    [XmlArrayItem("Trackpoint", typeof (Trackpoint_t), IsNullable = false)]
    public Trackpoint_t[] Track
    {
      get => this.trackField;
      set => this.trackField = value;
    }

    public string Notes
    {
      get => this.notesField;
      set => this.notesField = value;
    }

    [XmlElement("CoursePoint")]
    public CoursePoint_t[] CoursePoint
    {
      get => this.coursePointField;
      set => this.coursePointField = value;
    }

    public AbstractSource_t Creator
    {
      get => this.creatorField;
      set => this.creatorField = value;
    }
  }
}
