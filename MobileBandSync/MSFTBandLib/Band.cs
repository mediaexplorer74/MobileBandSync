// Band.cs
// Type: MobileBandSync.MSFTBandLib.Band
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Command;
using MobileBandSync.MSFTBandLib.Exceptions;
using MobileBandSync.MSFTBandLib.Helpers;
using MobileBandSync.MSFTBandLib.Metrics;
using MobileBandSync.MSFTBandLib.UWP;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Bluetooth;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace MobileBandSync.MSFTBandLib
{
  public class Band<T> : BandInterface where T : class, BandSocketInterface
  {
    internal BluetoothDevice _device;

    public string Mac { get; protected set; }

    public string Name { get; protected set; }

    public bool Connected
    {
      get => this.Connection.Connected;
      set => throw new Exception("Can't change connection directly!");
    }

    public BandConnection<T> Connection { get; protected set; }

    public BluetoothDevice GetDevice() => this._device;

    public Band(string mac, string name)
    {
      this.Mac = mac;
      this.Name = name;
      this.Connection = new BandConnection<T>((BandInterface) this);
      this._device = (BluetoothDevice) null;
    }

    public Band(BluetoothDevice device)
    {
      if (device == null)
        return;
      this.Mac = device.HostName.ToString();
      this.Name = device.Name;
      this.Connection = new BandConnection<T>((BandInterface) this);
      this._device = device;
    }

    public string GetMac() => this.Mac;

    public string GetName() => this.Name;

    public async Task Connect(Action<ulong, ulong> Progress = null)
    {
      if (this.Connected)
        throw new BandConnected();
      await this.Connection.Connect(Progress);
    }

    public async Task Disconnect()
    {
      if (!this.Connected)
        return;
      await this.Connection.Disconnect();
    }

    public async Task<CommandResponse> Command(
      CommandEnum Command,
      Func<uint> BufferSize,
      Action<ulong, ulong> Progress = null)
    {
      if (!this.Connected)
        throw new BandConnectedNot();
      return await this.Connection.Command(Command, BufferSize, (byte[]) null, 8192U, Progress);
    }

    public async Task<CommandResponse> Command(
      CommandEnum Command,
      Func<uint> BufferSize,
      byte[] btArgs,
      uint uiBufferSize = 8192,
      Action<ulong, ulong> Progress = null)
    {
      if (!this.Connected)
        throw new BandConnectedNot();
      return await this.Connection.Command(Command, BufferSize, btArgs, 8192U, Progress);
    }

    public async Task CommandStore(
      CommandEnum Command,
      Func<uint> BufferSize,
      byte[] btArgs = null,
      uint uiBufferSize = 8192,
      Action<ulong, ulong> Progress = null)
    {
      if (!this.Connected)
        throw new BandConnectedNot();
      await this.Connection.CommandStore(Command, BufferSize, btArgs, uiBufferSize, Progress);
    }

    public async Task<CommandResponse> CommandStoreStatus(
      CommandEnum Command,
      Func<uint> BufferSize,
      byte[] btArgs = null,
      uint uiBufferSize = 8192,
      Action<ulong, ulong> Progress = null)
    {
      if (!this.Connected)
        throw new BandConnectedNot();
      return await this.Connection.CommandStoreStatus(Command, BufferSize, btArgs, uiBufferSize, Progress);
    }

    public async Task<DateTime> GetDeviceTime() 
            => TimeHelper.DateTimeResponse(await this.Command(CommandEnum.GetDeviceTime, 
                (Func<uint>) null, (Action<ulong, ulong>) null));

    public async Task<Sleep> GetLastSleep() 
            => new Sleep(await this.Command(CommandEnum.GetStatisticsSleep, (Func<uint>) null, 
                (Action<ulong, ulong>) null));

    public async Task<string> GetSerialNumber() 
            => (await this.Command((CommandEnum) DeviceCommands.CargoGetProductSerialNumber, 
                (Func<uint>) null, (Action<ulong, ulong>) null)).GetByteStream().GetString();

    public static byte[] Combine(byte[] first, byte[] second)
    {
      byte[] dst = new byte[first.Length + second.Length];

      System.Buffer.BlockCopy((Array) first, 0, (Array) dst, 0, first.Length);
      System.Buffer.BlockCopy((Array) second, 0, (Array) dst, first.Length, second.Length);
      
      return dst;
    }

    public async Task<byte[]> GetSensorLog(
      Action<string> Report,
      Action<ulong, ulong> Progress,
      bool bCleanupSensorLog,
      bool bStoreSensorLog)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      //Band<T>.\u003C\u003Ec__DisplayClass31_0 cDisplayClass310 
      //          = new Band<T>.\u003C\u003Ec__DisplayClass31_0();
      // ISSUE: reference to a compiler-generated field
      //cDisplayClass310.Progress = Progress;
      byte[] btSensorLog = (byte[]) null;
      // ISSUE: reference to a compiler-generated field
      //cDisplayClass310.Progress(0UL, 0UL);
      try
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        //Band<T>.\u003C\u003Ec__DisplayClass31_1 cDisplayClass311 = new Band<T>.\u003C\u003Ec__DisplayClass31_1();
        // ISSUE: reference to a compiler-generated field
        //cDisplayClass311.CS\u0024\u003C\u003E8__locals1 = cDisplayClass310;
        // ISSUE: reference to a compiler-generated field
        //cDisplayClass311.metaData = (BandMetadataRange) null;
        await this.DeviceLogDataFlush();
        int num1;
        try
        {
          num1 = await this.RemainingDeviceLogDataChunks();
        }
        catch (Exception ex)
        {
          num1 = 0;
        }
        if (num1 > 0)
        {
          int chunkCount = num1;
          // ISSUE: reference to a compiler-generated field
          //BandMetadataRange metaData = cDisplayClass311.metaData;
          //BandMetadataRange chunkRangeMetadata = await this.GetChunkRangeMetadata(chunkCount);
          // ISSUE: reference to a compiler-generated field
          //cDisplayClass311.metaData = chunkRangeMetadata;
          // ISSUE: reference to a compiler-generated field
          if (true)//(cDisplayClass311.metaData == null)
          {
            Report("* error: failed to get chunk range metadata!");
          }
          else
          {
            try
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              //if (cDisplayClass311.CS\u0024\u003C\u003E8__locals1.Progress != null)
              //{
                // ISSUE: method pointer
              //  await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync((CoreDispatcherPriority) 0,
              //  new DispatchedHandler((object) cDisplayClass311, __methodptr(\u003CGetSensorLog\u003Eb__0)));
              //}
            }
            catch (Exception ex)
            {
                 Debug.WriteLine(ex.Message);
            }

            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            btSensorLog = null;//await this.GetChunkRangeData(cDisplayClass311.metaData, cDisplayClass311.CS\u0024\u003C\u003E8__locals1.Progress);
            if (btSensorLog != null && btSensorLog.Length != 0)
            {
              if (bStoreSensorLog)
              {
                string uploadId = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                StorageFolder folderAsync = await KnownFolders.DocumentsLibrary.CreateFolderAsync("SensorLog", (CreationCollisionOption) 3);
                if (folderAsync == null)
                  return (byte[]) null;
                await FileIO.WriteBytesAsync((IStorageFile) await folderAsync.CreateFileAsync("band-" + uploadId + "-Data.bin", (CreationCollisionOption) 1), btSensorLog);
                uploadId = (string) null;
              }
              if (bCleanupSensorLog)
              {
                // ISSUE: reference to a compiler-generated field
                //int num2 = await this.DeleteChunkRange(cDisplayClass311.metaData) ? 1 : 0;
              }
            }
            else
              Report("* error: failed to get chunk range data!");
          }
        }
        //cDisplayClass311 = (Band<T>.\u003C\u003Ec__DisplayClass31_1) null;
      }
      catch (Exception ex)
      {
        Report("Error downloading the logs: " + ex.Message);
        return (byte[]) null;
      }
      return btSensorLog;
    }

    public async Task<int> RemainingDeviceLogDataChunks()
    {
        return BitConverter.ToInt32((await this.Command((CommandEnum)DeviceCommands
            .CargoLoggerGetChunkCounts, (Func<uint>)null, (Action<ulong, ulong>)null))
            .GetByteStream().GetBytes(), 0);
    }

    public async Task DeviceLogDataFlush()
    {
      try
      {
        BandStatus status = (BandStatus) null;
        Func<uint> BufferSize = (Func<uint>) (() => 0U);
        int iTries = 0;
        do
        {
          byte[] status1 = (await this.Command((CommandEnum) DeviceCommands.CargoLoggerFlush, 
              BufferSize, (Action<ulong, ulong>) null)).Status;

          if (CommandResponse.ResponseBytesAreStatus(status1))
            status = BandStatus.DeserializeFromBytes(status1);

          CancellationToken.None.WaitAndThrowIfCancellationRequested(1000);
        }
        while (status == null || status.Status != 0U || iTries++ > 5);
        status = (BandStatus) null;
        BufferSize = (Func<uint>) null;
      }
      catch (Exception ex)
      {
      }
    }

    public async Task<BandMetadataRange> GetChunkRangeMetadata(int chunkCount)
    {
      BandMetadataRange metaResult = (BandMetadataRange) null;
      MemoryStream output = new MemoryStream(12);
      new BinaryWriter((Stream) output).Write(chunkCount);
      byte[] array = output.ToArray();
      try
      {
        Func<uint> BufferSize = (Func<uint>) (() => 12U);
        metaResult = BandMetadataRange.DeserializeFromBytes((await this.Command(
            (CommandEnum) DeviceCommands.CargoLoggerGetChunkRangeMetadata, BufferSize, array))
            .GetByteStream().GetBytes());
      }
      catch (Exception ex)
      {
                Debug.WriteLine(ex.Message);
      }
      return metaResult;
    }

    public async Task<byte[]> GetChunkRangeData(
      BandMetadataRange metaData,
      Action<ulong, ulong> Progress)
    {
      MemoryStream output = new MemoryStream(12);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write(metaData.StartingSeqNumber);
      binaryWriter.Write(metaData.EndingSeqNumber);
      binaryWriter.Write(metaData.ByteCount);
      byte[] array = output.ToArray();
      byte[] btResult = (byte[]) null;
      try
      {
        Func<uint> BufferSize = (Func<uint>) (() => metaData.ByteCount);
        btResult = (await this.Command((CommandEnum) DeviceCommands.CargoLoggerGetChunkRangeData, 
            BufferSize, array, Progress: Progress)).GetAllData();
      }
      catch (Exception ex)
      {
                Debug.WriteLine(ex.Message);
      }
      return btResult;
    }

    public async Task<bool> DeleteChunkRange(BandMetadataRange metaData)
    {
      MemoryStream output = new MemoryStream(12);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write(metaData.StartingSeqNumber);
      binaryWriter.Write(metaData.EndingSeqNumber);
      binaryWriter.Write(metaData.ByteCount);
      byte[] array = output.ToArray();
      try
      {
        Func<uint> BufferSize = (Func<uint>) (() => 12U);
        await this.CommandStore((CommandEnum) DeviceCommands.CargoLoggerDeleteChunkRange, 
            BufferSize, array, 8192U, (Action<ulong, ulong>) null);
      }
      catch (Exception ex)
      {
      }
      return true;
    }

        public async Task<BandVersion> GetVersion()
        {
            return new BandVersion(await this.Command((CommandEnum)DeviceCommands.CargoCoreModuleGetVersion,
                (Func<uint>)null, (Action<ulong, ulong>)null));
        }

        public DataReader GetDataReader()
        {
            return !(this.Connection is BandConnection<BandSocketUWP> connection)
                ? (DataReader)null
                : connection.Cargo.GetDataReader();
        }

        public DataWriter GetDataWriter()
        {
            return !(this.Connection is BandConnection<BandSocketUWP> connection)
                ? (DataWriter)null 
                : connection.Cargo.GetDataWriter();
        }
    }
}
