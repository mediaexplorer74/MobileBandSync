// Decompiled with JetBrains decompiler
// Type: MobileBandSync.Common.Workout
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.Collections.Generic;

namespace MobileBandSync.Common
{
  public class Workout
  {
    public WorkoutSummary Summary;
    public SleepSummary SleepSummary;

    public Workout()
    {
      this.Type = EventType.Workout;
      this.TrackPoints = new List<WorkoutPoint>();
    }

    public DateTime StartTime { get; set; }

    public DateTime LastSplitTime { get; set; }

    public DateTime EndTime { get; set; }

    public List<WorkoutPoint> TrackPoints { get; }

    public int LastHR { get; set; }

    public string Notes { get; set; }

    public EventType Type { get; set; }

    public string TCXBuffer { get; set; }

    public string Filename { get; set; }

    public ulong Filesize { get; set; }
  }
}
