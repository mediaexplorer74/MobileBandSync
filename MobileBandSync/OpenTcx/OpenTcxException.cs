// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.OpenTcxException
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;

namespace MobileBandSync.OpenTcx
{
  public class OpenTcxException : Exception
  {
    public OpenTcxException(string message)
      : base(message)
    {
    }

    public OpenTcxException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
