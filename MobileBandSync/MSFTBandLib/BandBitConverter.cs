// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.BandBitConverter
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.Text;

namespace MobileBandSync.MSFTBandLib
{
  internal static class BandBitConverter
  {
    private static readonly char[] HexCharTable = "0123456789ABCDEF".ToCharArray();

    public static void GetBytes(short i, byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0 || buffer.Length < offset + 2)
        throw new ArgumentOutOfRangeException(nameof (offset));
      for (int index = 0; index < 16; index += 8)
        buffer[offset++] = (byte) ((int) i >> index & (int) byte.MaxValue);
    }

    public static void GetBytes(ushort i, byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0 || buffer.Length < offset + 2)
        throw new ArgumentOutOfRangeException(nameof (offset));
      for (int index = 0; index < 16; index += 8)
        buffer[offset++] = (byte) ((int) i >> index & (int) byte.MaxValue);
    }

    public static void GetBytes(int i, byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0 || buffer.Length < offset + 4)
        throw new ArgumentOutOfRangeException(nameof (offset));
      for (int index = 0; index < 32; index += 8)
        buffer[offset++] = (byte) (i >> index & (int) byte.MaxValue);
    }

    public static void GetBytes(uint i, byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0 || buffer.Length < offset + 4)
        throw new ArgumentOutOfRangeException(nameof (offset));
      for (int index = 0; index < 32; index += 8)
        buffer[offset++] = (byte) (i >> index & (uint) byte.MaxValue);
    }

    public static void GetBytes(long i, byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0 || buffer.Length < offset + 8)
        throw new ArgumentOutOfRangeException(nameof (offset));
      for (int index = 0; index < 64; index += 8)
        buffer[offset++] = (byte) ((ulong) (i >> index) & (ulong) byte.MaxValue);
    }

    public static void GetBytes(ulong i, byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0 || buffer.Length < offset + 8)
        throw new ArgumentOutOfRangeException(nameof (offset));
      for (int index = 0; index < 64; index += 8)
        buffer[offset++] = (byte) (i >> index & (ulong) byte.MaxValue);
    }

    public static Guid ToGuid(byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0 || buffer.Length < offset + 16)
        throw new ArgumentOutOfRangeException(nameof (offset));
      return buffer.Length == 16 ? new Guid(buffer) : new Guid(BitConverter.ToInt32(buffer, offset), BitConverter.ToInt16(buffer, offset + 4), BitConverter.ToInt16(buffer, offset + 6), buffer[offset + 8], buffer[offset + 9], buffer[offset + 10], buffer[offset + 11], buffer[offset + 12], buffer[offset + 13], buffer[offset + 14], buffer[offset + 15]);
    }

    public static void GetBytes(Guid guid, byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0 || buffer.Length < offset + 16)
        throw new ArgumentOutOfRangeException(nameof (offset));
      guid.ToByteArray().CopyTo((Array) buffer, offset);
    }

    public static string ToString(byte[] buffer) => buffer != null ? BandBitConverter.ToStringInternal(buffer, 0, buffer.Length) : throw new ArgumentNullException(nameof (buffer));

    public static string ToString(byte[] buffer, int offset, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0 || buffer.Length != 0 && offset >= buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0 || offset + count > buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (offset));
      return BandBitConverter.ToStringInternal(buffer, offset, count);
    }

    private static string ToStringInternal(byte[] buffer, int offset, int count)
    {
      StringBuilder stringBuilder = new StringBuilder(count * 2);
      while (count > 0)
      {
        byte num = buffer[offset];
        stringBuilder.Append(BandBitConverter.HexCharTable[(int) num >> 4 & 15]);
        stringBuilder.Append(BandBitConverter.HexCharTable[(int) num & 15]);
        --count;
        ++offset;
      }
      return stringBuilder.ToString();
    }
  }
}
