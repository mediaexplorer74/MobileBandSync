// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Helpers.TimeHelper
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Command;
using System;
using System.Collections.Generic;

namespace MobileBandSync.MSFTBandLib.Helpers
{
  public static class TimeHelper
  {
    public static DateTime DateTimeUshorts(ushort[] t) => new DateTime((int) t[0], (int) t[1], (int) t[2], (int) t[3], (int) t[4], (int) t[5]);

    public static DateTime DateTimeResponse(CommandResponse response, int position = 0)
    {
      List<ushort> ushortList = new List<ushort>((IEnumerable<ushort>) response.GetByteStream().GetUshorts(8, position));
      ushortList.RemoveAt(2);
      return TimeHelper.DateTimeUshorts(ushortList.ToArray());
    }
  }
}
