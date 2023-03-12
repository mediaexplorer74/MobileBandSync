// ExportType.cs
// Type: MobileBandSync.Common.ExportType
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

namespace MobileBandSync.Common
{
  public enum ExportType
  {
    Unknown = 0,
    GPX = 1,
    TCX = 2,
    FIT = 4,
    HeartRate = 8,
    Cadence = 16, // 0x00000010
    Temperature = 32, // 0x00000020
    GalvanicSkinResponse = 64, // 0x00000040
  }
}
