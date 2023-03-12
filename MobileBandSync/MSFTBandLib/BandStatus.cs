// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.BandStatus
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.IO;

namespace MobileBandSync.MSFTBandLib
{
  public class BandStatus
  {
    private const int serializedByteCount = 6;

    private BandStatus()
    {
    }

    public ushort PacketType { get; private set; }

    public uint Status { get; private set; }

    public static int GetSerializedByteCount() => 12;

    public static BandStatus DeserializeFromBytes(byte[] data) => new BandStatus()
    {
      PacketType = BitConverter.ToUInt16(data, 0),
      Status = BitConverter.ToUInt32(data, 2)
    };

    internal void SerializeToBytes(ref byte[] data)
    {
      MemoryStream output = new MemoryStream(BandStatus.GetSerializedByteCount());
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write(this.PacketType);
      binaryWriter.Write(this.Status);
      data = output.ToArray();
    }

    internal byte[] SerializeToByteArray()
    {
      int offset1 = 0;
      byte[] buffer = new byte[BandMetadataRange.GetSerializedByteCount()];
      BandBitConverter.GetBytes(this.PacketType, buffer, offset1);
      int offset2 = offset1 + 2;
      BandBitConverter.GetBytes(this.Status, buffer, offset2);
      return buffer;
    }
  }
}
