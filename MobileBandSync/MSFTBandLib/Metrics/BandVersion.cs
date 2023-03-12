// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Metrics.BandVersion
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Command;
using MobileBandSync.MSFTBandLib.Includes;

namespace MobileBandSync.MSFTBandLib.Metrics
{
  public class BandVersion
  {
    public string AppName { get; protected set; }

    public byte PCBId { get; protected set; }

    public ushort VersionMajor { get; protected set; }

    public ushort VersionMinor { get; protected set; }

    public uint Revision { get; protected set; }

    public uint BuildNumber { get; protected set; }

    public byte DebugBuild { get; protected set; }

    public BandVersion()
    {
    }

    public BandVersion(CommandResponse response)
    {
      ByteStream byteStream1 = response.GetByteStream();
      int num = 0;
      string str = "";
      ByteStream byteStream2 = byteStream1;
      int position1 = num;
      int position2 = position1 + 1;
      for (char ch = (char) byteStream2.GetByte(position1); ch != char.MinValue; ch = (char) byteStream1.GetByte(position2++))
        str += ch.ToString();
      this.AppName = str;
      this.PCBId = byteStream1.GetByte(position2);
      this.VersionMajor = byteStream1.GetUshort(position2 + 1);
      this.VersionMinor = byteStream1.GetUshort(position2 + 3);
      this.Revision = byteStream1.GetUint32(position2 + 5);
      this.BuildNumber = byteStream1.GetUint32(position2 + 9);
      this.DebugBuild = byteStream1.GetByte(position2 + 13);
    }
  }
}
