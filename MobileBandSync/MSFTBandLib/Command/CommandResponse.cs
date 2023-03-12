// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Command.CommandResponse
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Includes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileBandSync.MSFTBandLib.Command
{
  public class CommandResponse
  {
    public byte[] Status = new byte[6];
    public List<byte[]> Data = new List<byte[]>();
    public const int RESPONSE_STATUS_LENGTH = 6;

    public void AddResponse(byte[] bytes)
    {
      if (CommandResponse.ResponseBytesStartWithStatus(bytes))
      {
        this.Status = CommandResponse.GetResponseStatusBytesStart(bytes);
        if (!CommandResponse.ResponseBytesContainData(bytes))
          return;
        this.AddResponseData(CommandResponse.GetResponseDataBytesStart(bytes));
      }
      else if (CommandResponse.ResponseBytesEndWithStatus(bytes))
      {
        this.Status = CommandResponse.GetResponseStatusBytesEnd(bytes);
        if (!CommandResponse.ResponseBytesContainData(bytes))
          return;
        this.AddResponseData(CommandResponse.GetResponseDataBytesEnd(bytes));
      }
      else
        this.AddResponseData(bytes);
    }

    protected void AddResponseData(byte[] bytes) => this.Data.Add(bytes);

    public byte[] GetData(int index = 0)
    {
      byte[] data = (byte[]) null;
      if (this.Data.Count > index && this.Data[index].Length != 0)
        data = this.Data[index];
      return data;
    }

    public static byte[] Combine(byte[] first, byte[] second)
    {
      byte[] dst = new byte[first.Length + second.Length];
      Buffer.BlockCopy((Array) first, 0, (Array) dst, 0, first.Length);
      Buffer.BlockCopy((Array) second, 0, (Array) dst, first.Length, second.Length);
      return dst;
    }

    public byte[] GetAllData()
    {
      int index = 0;
      byte[] first = (byte[]) null;
      for (; this.Data.Count > index && this.Data[index].Length != 0; ++index)
      {
        if (index == 0)
          first = this.Data[index];
        else if (first != null)
          first = CommandResponse.Combine(first, this.Data[index]);
      }
      return first;
    }

    public ByteStream GetByteStream(int index = 0)
    {
      byte[] data = this.GetData(index);
      return data == null ? (ByteStream) null : new ByteStream(data);
    }

    public bool StatusReceived() => !((IEnumerable<byte>) this.Status).All<byte>((Func<byte, bool>) (s => s == (byte) 0));

    public static byte[] GetResponseDataBytesStart(byte[] bytes) => ((IEnumerable<byte>) bytes).Skip<byte>(6).ToArray<byte>();

    public static byte[] GetResponseDataBytesEnd(byte[] bytes)
    {
      int count = bytes.Length - 6;
      return ((IEnumerable<byte>) bytes).Take<byte>(count).ToArray<byte>();
    }

    public static byte[] GetResponseStatusBytesStart(byte[] bytes) => ((IEnumerable<byte>) bytes).Take<byte>(6).ToArray<byte>();

    public static byte[] GetResponseStatusBytesEnd(byte[] bytes)
    {
      int count = bytes.Length - 6;
      return ((IEnumerable<byte>) bytes).Skip<byte>(count).ToArray<byte>();
    }

    public static bool ResponseBytesAreStatus(byte[] bytes)
    {
      int[] array = ((IEnumerable<byte>) bytes).Select<byte, int>((Func<byte, int>) (b => (int) b)).ToArray<int>();
      return array[0] == 254 && array[1] == 166;
    }

    public static bool ResponseBytesContainData(byte[] bytes) => bytes.Length > 6;

    public static bool ResponseBytesStartWithStatus(byte[] bytes) => CommandResponse.ResponseBytesAreStatus(CommandResponse.GetResponseStatusBytesStart(bytes));

    public static bool ResponseBytesEndWithStatus(byte[] bytes) => CommandResponse.ResponseBytesAreStatus(CommandResponse.GetResponseStatusBytesEnd(bytes));
  }
}
