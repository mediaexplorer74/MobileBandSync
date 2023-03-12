// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.TrainingCenterDatabase_t
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
  [XmlRoot("TrainingCenterDatabase", IsNullable = false, Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
  public class TrainingCenterDatabase_t
  {
    private Folders_t foldersField;
    private ActivityList_t activitiesField;
    private Workout_t[] workoutsField;
    private Course_t[] coursesField;
    private AbstractSource_t authorField;

    public Folders_t Folders
    {
      get => this.foldersField;
      set => this.foldersField = value;
    }

    public ActivityList_t Activities
    {
      get => this.activitiesField;
      set => this.activitiesField = value;
    }

    [XmlArrayItem("Workout", IsNullable = false)]
    public Workout_t[] Workouts
    {
      get => this.workoutsField;
      set => this.workoutsField = value;
    }

    [XmlArrayItem("Course", IsNullable = false)]
    public Course_t[] Courses
    {
      get => this.coursesField;
      set => this.coursesField = value;
    }

    public AbstractSource_t Author
    {
      get => this.authorField;
      set => this.authorField = value;
    }
  }
}
