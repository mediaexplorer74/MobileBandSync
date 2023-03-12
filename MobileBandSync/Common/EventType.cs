// EventType.cs
// Type: MobileBandSync.Common.EventType
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

namespace MobileBandSync.Common
{
  public enum EventType
  {
    Unknown = -2, // 0xFFFFFFFE
    None = 0,
    Running = 4,
    Biking = 6,
    Walking = 16, // 0x00000010
    Sleeping = 21, // 0x00000015
    Hike = 32, // 0x00000020
    Workout = 99, // 0x00000063
  }
}
