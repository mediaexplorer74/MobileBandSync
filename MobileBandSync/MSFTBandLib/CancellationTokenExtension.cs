// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.CancellationTokenExtension
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.Threading;

namespace MobileBandSync.MSFTBandLib
{
  internal static class CancellationTokenExtension
  {
    public static void WaitAndThrowIfCancellationRequested(
      this CancellationToken token,
      TimeSpan timeout)
    {
      token.WaitHandle.WaitOne(timeout);
      token.ThrowIfCancellationRequested();
    }

    public static void WaitAndThrowIfCancellationRequested(
      this CancellationToken token,
      int timeout)
    {
      token.WaitHandle.WaitOne(timeout);
      token.ThrowIfCancellationRequested();
    }
  }
}
