// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.ProgressEventArgs
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;

namespace MobileBandSync.MSFTBandLib
{
  public class ProgressEventArgs : EventArgs
  {
    public ulong Completed { get; private set; }

    public ulong Total { get; private set; }

    public string StatusText { get; private set; }

    public ProgressEventArgs(ulong uiCompleted, ulong uiTotal, string strStatusText)
    {
      this.Completed = uiCompleted;
      this.Total = uiTotal;
      this.StatusText = strStatusText;
    }
  }
}
