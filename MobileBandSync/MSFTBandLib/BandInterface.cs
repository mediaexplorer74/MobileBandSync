// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.BandInterface
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Command;
using MobileBandSync.MSFTBandLib.Metrics;
using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Storage.Streams;

namespace MobileBandSync.MSFTBandLib
{
  public interface BandInterface
  {
    string GetMac();

    string GetName();

    BluetoothDevice GetDevice();

    Task Connect(Action<ulong, ulong> Progress = null);

    Task Disconnect();

    Task<CommandResponse> Command(
      CommandEnum Command,
      Func<uint> BufferSize,
      Action<ulong, ulong> Progress = null);

    Task CommandStore(
      CommandEnum Command,
      Func<uint> BufferSize,
      byte[] btArgs,
      uint uiBufferSize = 8192,
      Action<ulong, ulong> Progress = null);

    Task<DateTime> GetDeviceTime();

    Task<Sleep> GetLastSleep();

    Task<string> GetSerialNumber();

    Task<BandVersion> GetVersion();

    Task<byte[]> GetSensorLog(
      Action<string> Report,
      Action<ulong, ulong> Progress,
      bool bCleanupSensorLog,
      bool bStoreSensorLog);

    Task<bool> DeleteChunkRange(BandMetadataRange metaData);

    DataReader GetDataReader();

    DataWriter GetDataWriter();
  }
}
