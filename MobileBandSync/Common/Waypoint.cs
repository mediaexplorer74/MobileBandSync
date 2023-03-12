// Decompiled with JetBrains decompiler
// Type: MobileBandSync.Common.Waypoint
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;

namespace MobileBandSync.Common
{
  public class Waypoint
  {
    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public double SpeedOverGround { get; set; }

    public double ElevationFromMeanSeaLevel { get; set; }

    public double EstimatedHorizontalError { get; set; }

    public double EstimatedVerticalError { get; set; }

    public DateTime TimeStamp { get; set; }
  }
}
