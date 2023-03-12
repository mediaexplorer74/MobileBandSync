// Decompiled with JetBrains decompiler
// Type: MobileBandSync.Common.SensorLogType
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

namespace MobileBandSync.Common
{
  public enum SensorLogType
  {
    Timestamp = 0,
    Timestamp3 = 13, // 0x0000000D
    SequenceID = 15, // 0x0000000F
    UtcOffset = 19, // 0x00000013
    SkinTemperature = 66, // 0x00000042
    Waypoint = 83, // 0x00000053
    Sensor = 104, // 0x00000068
    HeartRate = 128, // 0x00000080
    Steps = 130, // 0x00000082
    Sleep = 153, // 0x00000099
    WorkoutSummary = 161, // 0x000000A1
    Counter = 164, // 0x000000A4
    WorkoutMarker = 208, // 0x000000D0
    WorkoutMarker2 = 210, // 0x000000D2
    SleepSummary = 213, // 0x000000D5
    Timestamp2 = 224, // 0x000000E0
    Timestamp4 = 226, // 0x000000E2
    DailySummary = 230, // 0x000000E6
    Unknown = 255, // 0x000000FF
  }
}
