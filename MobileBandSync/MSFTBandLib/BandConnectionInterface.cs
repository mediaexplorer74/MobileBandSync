// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.BandConnectionInterface
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Command;
using System;
using System.Threading.Tasks;

namespace MobileBandSync.MSFTBandLib
{
  public interface BandConnectionInterface : IDisposable
  {
    Task Connect(BandInterface Band);

    Task Disconnect();

    Task<CommandResponse> Command(
      CommandEnum command,
      Func<uint> BufferSize,
      byte[] args = null,
      uint buffer = 8192,
      Action<ulong, ulong> Progress = null);

    Task CommandStore(
      CommandEnum command,
      Func<uint> BufferSize,
      byte[] args = null,
      uint buffer = 8192,
      Action<ulong, ulong> Progress = null);
  }
}
