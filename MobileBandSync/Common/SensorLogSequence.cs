// Decompiled with JetBrains decompiler
// Type: MobileBandSync.Common.SensorLogSequence
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.Collections.Generic;

namespace MobileBandSync.Common
{
  public class SensorLogSequence
  {
    public SensorLogSequence(long lFileTime)
    {
      this.TimeStamp = lFileTime <= 0L ? new DateTime?(DateTime.MinValue) : new DateTime?(DateTime.FromFileTime(lFileTime));
      this.HeartRates = new List<HeartRate>();
      this.Waypoints = new List<Waypoint>();
      this.AdditionalData = new List<UnknownData>();
      this.Counters = new List<Counter>();
      this.Temperatures = new List<SkinTemperature>();
      this.WorkoutMarkers = new List<WorkoutMarker>();
      this.WorkoutMarkers2 = new List<WorkoutMarker2>();
      this.SensorValues1 = new List<SensorValueCollection1>();
      this.SensorValues2 = new List<SensorValueCollection2>();
      this.SensorValues3 = new List<SensorValueCollection3>();
      this.StepSnapshots = new List<Steps>();
      this.SleepSummaries = new List<SleepSummary>();
      this.WorkoutSummaries = new List<WorkoutSummary>();
      this.DailySummaries = new List<DailySummary>();
      this.SensorList = new List<Sensor>();
    }

    public DateTime? TimeStamp { get; set; }

    public TimeSpan? Duration { get; set; }

    public int ID { get; set; }

    public int UtcOffset { get; set; }

    public List<HeartRate> HeartRates { get; }

    public List<Waypoint> Waypoints { get; }

    public List<Counter> Counters { get; }

    public List<SkinTemperature> Temperatures { get; }

    public List<Sensor> SensorList { get; }

    public List<Steps> StepSnapshots { get; }

    public List<SleepSummary> SleepSummaries { get; }

    public List<WorkoutMarker> WorkoutMarkers { get; }

    public List<WorkoutMarker2> WorkoutMarkers2 { get; }

    public List<SensorValueCollection1> SensorValues1 { get; }

    public List<SensorValueCollection2> SensorValues2 { get; }

    public List<SensorValueCollection3> SensorValues3 { get; }

    public List<UnknownData> AdditionalData { get; }

    public List<WorkoutSummary> WorkoutSummaries { get; }

    public List<DailySummary> DailySummaries { get; }
  }
}
