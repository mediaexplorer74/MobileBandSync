// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.Trackpoint_t
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
  public class Trackpoint_t
  {
    private DateTime timeField;
    private Position_t positionField;
    private double altitudeMetersField;
    private bool altitudeMetersFieldSpecified;
    private double distanceMetersField;
    private bool distanceMetersFieldSpecified;
    private HeartRateInBeatsPerMinute_t heartRateBpmField;
    private byte cadenceField;
    private bool cadenceFieldSpecified;
    private SensorState_t sensorStateField;
    private bool sensorStateFieldSpecified;

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

    public double DistanceMeters
    {
      get => this.distanceMetersField;
      set => this.distanceMetersField = value;
    }

    [XmlIgnore]
    public bool DistanceMetersSpecified
    {
      get => this.distanceMetersFieldSpecified;
      set => this.distanceMetersFieldSpecified = value;
    }

    public HeartRateInBeatsPerMinute_t HeartRateBpm
    {
      get => this.heartRateBpmField;
      set => this.heartRateBpmField = value;
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

    public SensorState_t SensorState
    {
      get => this.sensorStateField;
      set => this.sensorStateField = value;
    }

    [XmlIgnore]
    public bool SensorStateSpecified
    {
      get => this.sensorStateFieldSpecified;
      set => this.sensorStateFieldSpecified = value;
    }
  }
}
