// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.ActivityLap_t
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
  public class ActivityLap_t
  {
    private double totalTimeSecondsField;
    private double distanceMetersField;
    private double maximumSpeedField;
    private bool maximumSpeedFieldSpecified;
    private ushort caloriesField;
    private HeartRateInBeatsPerMinute_t averageHeartRateBpmField;
    private HeartRateInBeatsPerMinute_t maximumHeartRateBpmField;
    private Intensity_t intensityField;
    private byte cadenceField;
    private bool cadenceFieldSpecified;
    private TriggerMethod_t triggerMethodField;
    private Trackpoint_t[] trackField;
    private string notesField;
    private DateTime startTimeField;

    public double TotalTimeSeconds
    {
      get => this.totalTimeSecondsField;
      set => this.totalTimeSecondsField = value;
    }

    public double DistanceMeters
    {
      get => this.distanceMetersField;
      set => this.distanceMetersField = value;
    }

    public double MaximumSpeed
    {
      get => this.maximumSpeedField;
      set => this.maximumSpeedField = value;
    }

    [XmlIgnore]
    public bool MaximumSpeedSpecified
    {
      get => this.maximumSpeedFieldSpecified;
      set => this.maximumSpeedFieldSpecified = value;
    }

    public ushort Calories
    {
      get => this.caloriesField;
      set => this.caloriesField = value;
    }

    public HeartRateInBeatsPerMinute_t AverageHeartRateBpm
    {
      get => this.averageHeartRateBpmField;
      set => this.averageHeartRateBpmField = value;
    }

    public HeartRateInBeatsPerMinute_t MaximumHeartRateBpm
    {
      get => this.maximumHeartRateBpmField;
      set => this.maximumHeartRateBpmField = value;
    }

    public Intensity_t Intensity
    {
      get => this.intensityField;
      set => this.intensityField = value;
    }

    public byte Cadence
    {
      get => this.cadenceField;
      set => this.cadenceField = value;
    }

    [XmlIgnore]
    public bool CadenceSpecified
    {
      get => this.cadenceFieldSpecified;
      set => this.cadenceFieldSpecified = value;
    }

    public TriggerMethod_t TriggerMethod
    {
      get => this.triggerMethodField;
      set => this.triggerMethodField = value;
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

    [XmlAttribute]
    public DateTime StartTime
    {
      get => this.startTimeField;
      set => this.startTimeField = value;
    }
  }
}
