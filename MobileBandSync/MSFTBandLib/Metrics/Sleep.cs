// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Metrics.Sleep
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Command;
using MobileBandSync.MSFTBandLib.Includes;
using System;

namespace MobileBandSync.MSFTBandLib.Metrics
{
  public class Sleep
  {
    public uint Calories { get; protected set; }

    public uint Duration { get; protected set; }

    public uint Feeling { get; protected set; }

    public uint RestingHR { get; protected set; }

    public uint TimeAsleep { get; protected set; }

    public uint TimeAwake { get; protected set; }

    public uint TimeToSleep { get; protected set; }

    public uint TimesAwoke { get; protected set; }

    public DateTime Timestamp { get; protected set; }

    public DateTime WokeUp { get; protected set; }

    public ushort Version { get; protected set; }

    public Sleep()
    {
    }

    public Sleep(CommandResponse response)
    {
      ByteStream byteStream = response.GetByteStream();
      this.Calories = byteStream.GetUint32(26);
      this.Duration = byteStream.GetUint32(10) / 1000U;
      this.Feeling = byteStream.GetUint32(50);
      this.RestingHR = byteStream.GetUint32(30);
      this.TimeAsleep = byteStream.GetUint32(22) / 1000U;
      this.TimeAwake = byteStream.GetUint32(18) / 1000U;
      this.TimeToSleep = byteStream.GetUint32(46) / 1000U;
      this.TimesAwoke = byteStream.GetUint32(14);
      this.Timestamp = DateTime.FromFileTime((long) byteStream.GetUint64());
      this.WokeUp = DateTime.FromFileTime((long) byteStream.GetUint64(38));
      this.Version = byteStream.GetUshort(8);
    }
  }
}
