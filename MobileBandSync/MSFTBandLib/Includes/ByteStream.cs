// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Includes.ByteStream
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.IO;

namespace MobileBandSync.MSFTBandLib.Includes
{
  public class ByteStream : IDisposable
  {
    public MemoryStream MemoryStream;
    public BinaryReader BinaryReader;
    public BinaryWriter BinaryWriter;

    public bool Disposed { get; protected set; }

    public ByteStream()
    {
      this.MemoryStream = new MemoryStream();
      this.BinaryReader = new BinaryReader((Stream) this.MemoryStream);
      this.BinaryWriter = new BinaryWriter((Stream) this.MemoryStream);
    }

    public ByteStream(byte[] bytes)
      : this()
    {
      this.Write(bytes);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.Disposed)
        return;
      this.BinaryReader.Dispose();
      this.BinaryWriter.Dispose();
      this.MemoryStream.Dispose();
      this.Disposed = true;
    }

    public void Write(byte[] bytes) => this.BinaryWriter.Write(bytes);

    public byte[] GetBytes() => this.MemoryStream.ToArray();

    public uint GetUint32(int position = 0)
    {
      this.BinaryReader.BaseStream.Position = (long) position;
      return this.BinaryReader.ReadUInt32();
    }

    public ulong GetUint64(int position = 0)
    {
      this.BinaryReader.BaseStream.Position = (long) position;
      return this.BinaryReader.ReadUInt64();
    }

    public string GetString(int position = 0, long chars = 0)
    {
      if (chars == 0L)
        chars = this.BinaryReader.BaseStream.Length;
      this.BinaryReader.BaseStream.Position = (long) position;
      return new string(this.BinaryReader.ReadChars((int) chars));
    }

    public byte GetByte(int position = 0)
    {
      this.BinaryReader.BaseStream.Position = (long) position;
      return this.BinaryReader.ReadByte();
    }

    public ushort GetUshort(int position = 0)
    {
      this.BinaryReader.BaseStream.Position = (long) position;
      return this.BinaryReader.ReadUInt16();
    }

    public ushort[] GetUshorts(int count, int pos = 0)
    {
      ushort[] ushorts = new ushort[count];
      for (int index = 0; index < count; ++index)
      {
        pos = index == 0 ? pos : pos + 2;
        ushorts[index] = this.GetUshort(pos);
      }
      return ushorts;
    }
  }
}
