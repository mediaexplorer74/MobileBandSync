// Decompiled with JetBrains decompiler
// Type: MobileBandSync.Common.UnknownData
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;

namespace MobileBandSync.Common
{
  public class UnknownData
  {
    public int[] ValueInt16;
    public int[] ValueInt32;

    public UnknownData(int iID, byte[] buffer, int iLength)
    {
      this.ID = iID;
      this.Content = new byte[iLength];
      buffer.CopyTo((Array) this.Content, 0);
      this.ValueInt16 = new int[100];
      this.ValueInt32 = new int[100];
      int index = 0;
      for (int startIndex = 0; startIndex < iLength; startIndex += 2)
      {
        this.ValueInt16[index] = startIndex + 1 < iLength ? (int) BitConverter.ToInt16(buffer, startIndex) : (int) buffer[startIndex];
        if (startIndex + 3 < iLength)
          this.ValueInt32[index] = BitConverter.ToInt32(buffer, startIndex);
        ++index;
      }
    }

    public byte[] Content { get; }

    public int ID { get; set; }
  }
}
