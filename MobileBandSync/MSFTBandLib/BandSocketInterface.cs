// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.BandSocketInterface
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Command;
using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MobileBandSync.MSFTBandLib
{
  public interface BandSocketInterface : IDisposable
  {
    Task Connect(string mac, Guid uuid, Action<ulong, ulong> Progress = null);

    Task Disconnect();

    Task Send(CommandPacket packet);

    Task Send(CommandPacket packet, byte[] bytesToSend);

    Task<int> SendStatus(CommandPacket packet, byte[] bytesToSend);

    Task<CommandResponse> Receive(uint buffer, Action<ulong, ulong> Progress = null);

    Task<CommandResponse> Request(
      CommandPacket packet,
      uint buffer,
      Action<ulong, ulong> Progress = null);

    DataReader GetDataReader();

    DataWriter GetDataWriter();
  }
}
