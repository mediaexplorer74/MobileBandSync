// Decompiled with JetBrains decompiler
// Type: MobileBandSync.Common.WorkoutPoint
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;

namespace MobileBandSync.Common
{
  public class WorkoutPoint
  {
    public DateTime Time { get; set; }

    public int HeartRateBpm { get; set; }

    public GpsPosition Position { get; set; }

    public double Elevation { get; set; }

    public uint GalvanicSkinResponse { get; set; }

    public double SkinTemperature { get; set; }

    public uint Cadence { get; set; }
  }
}
