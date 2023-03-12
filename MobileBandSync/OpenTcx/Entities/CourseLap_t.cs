// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.CourseLap_t
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
  public class CourseLap_t
  {
    private double totalTimeSecondsField;
    private double distanceMetersField;
    private Position_t beginPositionField;
    private double beginAltitudeMetersField;
    private bool beginAltitudeMetersFieldSpecified;
    private Position_t endPositionField;
    private double endAltitudeMetersField;
    private bool endAltitudeMetersFieldSpecified;
    private HeartRateInBeatsPerMinute_t averageHeartRateBpmField;
    private HeartRateInBeatsPerMinute_t maximumHeartRateBpmField;
    private Intensity_t intensityField;
    private byte cadenceField;
    private bool cadenceFieldSpecified;

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

    public Position_t BeginPosition
    {
      get => this.beginPositionField;
      set => this.beginPositionField = value;
    }

    public double BeginAltitudeMeters
    {
      get => this.beginAltitudeMetersField;
      set => this.beginAltitudeMetersField = value;
    }

    [XmlIgnore]
    public bool BeginAltitudeMetersSpecified
    {
      get => this.beginAltitudeMetersFieldSpecified;
      set => this.beginAltitudeMetersFieldSpecified = value;
    }

    public Position_t EndPosition
    {
      get => this.endPositionField;
      set => this.endPositionField = value;
    }

    public double EndAltitudeMeters
    {
      get => this.endAltitudeMetersField;
      set => this.endAltitudeMetersField = value;
    }

    [XmlIgnore]
    public bool EndAltitudeMetersSpecified
    {
      get => this.endAltitudeMetersFieldSpecified;
      set => this.endAltitudeMetersFieldSpecified = value;
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
  }
}
