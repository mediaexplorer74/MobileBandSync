// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Notifications.NotificationsEnum
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

namespace MobileBandSync.MSFTBandLib.Notifications
{
  public enum NotificationsEnum : ushort
  {
    SMS = 1,
    Email = 2,
    Messaging = 8,
    CallIncoming = 11, // 0x000B
    CallAnswered = 12, // 0x000C
    CallMissed = 13, // 0x000D
    CallHangup = 14, // 0x000E
    Voicemail = 15, // 0x000F
    CalendarAddEvent = 16, // 0x0010
    CalendarClear = 17, // 0x0011
    GenericDialog = 100, // 0x0064
    GenericUpdate = 101, // 0x0065
    GenericTileClear = 102, // 0x0066
    GenericPageClear = 103, // 0x0067
  }
}
