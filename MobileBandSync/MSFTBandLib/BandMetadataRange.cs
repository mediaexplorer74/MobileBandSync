// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.BandMetadataRange
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.IO;

namespace MobileBandSync.MSFTBandLib
{
  public class BandMetadataRange
  {
    private const int serializedByteCount = 12;

    private BandMetadataRange()
    {
    }

    public uint StartingSeqNumber { get; set; }

    public uint EndingSeqNumber { get; set; }

    public uint ByteCount { get; set; }

    public static int GetSerializedByteCount() => 12;

    public static BandMetadataRange DeserializeFromBytes(byte[] data) => new BandMetadataRange()
    {
      StartingSeqNumber = BitConverter.ToUInt32(data, 0),
      EndingSeqNumber = BitConverter.ToUInt32(data, 4),
      ByteCount = BitConverter.ToUInt32(data, 8)
    };

    internal void SerializeToBytes(ref byte[] data)
    {
      MemoryStream output = new MemoryStream(BandMetadataRange.GetSerializedByteCount());
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write(this.StartingSeqNumber);
      binaryWriter.Write(this.EndingSeqNumber);
      binaryWriter.Write(this.ByteCount);
      data = output.ToArray();
    }

    internal byte[] SerializeToByteArray()
    {
      int offset1 = 0;
      byte[] buffer = new byte[BandMetadataRange.GetSerializedByteCount()];
      BandBitConverter.GetBytes(this.StartingSeqNumber, buffer, offset1);
      int offset2 = offset1 + 4;
      BandBitConverter.GetBytes(this.EndingSeqNumber, buffer, offset2);
      int offset3 = offset2 + 4;
      BandBitConverter.GetBytes(this.ByteCount, buffer, offset3);
      return buffer;
    }
  }
}
