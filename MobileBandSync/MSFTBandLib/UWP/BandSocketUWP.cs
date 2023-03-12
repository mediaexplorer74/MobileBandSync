// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.UWP.BandSocketUWP
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Command;
using MobileBandSync.MSFTBandLib.Exceptions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace MobileBandSync.MSFTBandLib.UWP
{
  public class BandSocketUWP : BandSocketInterface, IDisposable
  {
    protected StreamSocket Socket;
    protected DataReader DataReader;
    protected DataWriter DataWriter;

    public bool Connected { get; protected set; }

    public bool Disposed { get; protected set; }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.Disposed)
        return;
      this.Socket.Dispose();
      this.DataReader.Dispose();
      this.DataWriter.Dispose();
      this.Disposed = true;
    }

    public async Task Connect(string mac, Guid uuid, Action<ulong, ulong> Progress = null)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      //BandSocketUWP.\u003C\u003Ec__DisplayClass13_0 cDisplayClass130 
      //          = new BandSocketUWP.\u003C\u003Ec__DisplayClass13_0();
      // ISSUE: reference to a compiler-generated field
      //cDisplayClass130.Progress = Progress;
      if (this.Connected)
        return;
      try
      {
        // ISSUE: reference to a compiler-generated field
        //if (cDisplayClass130.Progress != null)
        //{
          // ISSUE: method pointer
          //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync((CoreDispatcherPriority) 
          //    0, new DispatchedHandler((object) cDisplayClass130, __methodptr(\u003CConnect\u003Eb__0)));
        //}
      }
      catch (Exception ex)
      {
      }
      HostName host = new HostName(mac);
      RfcommDeviceService serviceForHostFromUuid = 
                await BandSocketUWP.GetRfcommDeviceServiceForHostFromUuid(host, uuid);

      this.Socket = new StreamSocket();

      await this.Socket.ConnectAsync(host, serviceForHostFromUuid.ConnectionServiceName, 
          (SocketProtectionLevel) 3);
      DataReader dataReader = new DataReader(this.Socket.InputStream);
      dataReader.InputStreamOptions = ((InputStreamOptions) 1);
      this.DataReader = dataReader;
      this.DataWriter = new DataWriter(this.Socket.OutputStream);
      this.Connected = true;
    }

    public async Task Disconnect()
    {
      if (!this.Connected)
        throw new BandSocketConnectedNot();
      await Task.Run((Action) (() =>
      {
        this.DataReader.DetachStream();
        this.DataReader.Dispose();
        this.DataWriter.DetachStream();
        this.DataWriter.Dispose();
        this.Socket.Dispose();
        this.Connected = false;
      }));
    }

    public async Task Send(CommandPacket packet)
    {
      if (!this.Connected)
        throw new BandSocketConnectedNot();
      this.DataWriter.WriteBytes(packet.GetBytes());
      int num = (int) await (IAsyncOperation<uint>) this.DataWriter.StoreAsync();
    }

    public async Task Send(CommandPacket packet, byte[] bytesToSend)
    {
      if (!this.Connected)
        throw new BandSocketConnectedNot();
      this.DataWriter.WriteBytes(packet.GetBytes());
      int num1 = (int) await (IAsyncOperation<uint>) this.DataWriter.StoreAsync();
      if (bytesToSend == null)
        return;
      this.DataWriter.WriteBytes(bytesToSend);
      int num2 = (int) await (IAsyncOperation<uint>) this.DataWriter.StoreAsync();
    }

    public async Task<int> SendStatus(CommandPacket packet, byte[] bytesToSend)
    {
      if (!this.Connected)
        throw new BandSocketConnectedNot();
      this.DataWriter.WriteBytes(packet.GetBytes());
      int num1 = (int) await (IAsyncOperation<uint>) this.DataWriter.StoreAsync();
      if (bytesToSend != null)
      {
        this.DataWriter.WriteBytes(bytesToSend);
        int num2 = (int) await (IAsyncOperation<uint>) this.DataWriter.StoreAsync();
      }
      byte[] numArray = this.ReadBytes(await (IAsyncOperation<uint>) this.DataReader.LoadAsync(12U));
      int num3 = (int) numArray[0];
      int num4 = (int) numArray[1];
      return num3 == 254 || num4 == 166 ? 0 : 1;
    }

    public async Task<CommandResponse> Receive
    (
      uint buffer,
      Action<ulong, ulong> Progress = null
    )
    {
      // ISSUE: variable of a compiler-generated type
      //BandSocketUWP.\u003C\u003Ec__DisplayClass18_1 cDisplayClass181;
      return await Task.Run<CommandResponse>((Func<Task<CommandResponse>>) (async () =>
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        //BandSocketUWP.\u003C\u003Ec__DisplayClass18_2
        //var cDisplayClass182_1 = new BandSocketUWP.\u003C\u003Ec__DisplayClass18_2();
        // ISSUE: reference to a compiler-generated field
        //cDisplayClass182_1.CS\u0024\u003C\u003E8__locals1 = cDisplayClass181;
        // ISSUE: reference to a compiler-generated field
        //cDisplayClass182_1.response = new CommandResponse();
        if (!this.Connected)
          throw new BandSocketConnectedNot();
        while (true)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          //BandSocketUWP.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180 = new BandSocketUWP.\u003C\u003Ec__DisplayClass18_0();
          // ISSUE: reference to a compiler-generated field
          //cDisplayClass180.CS\u0024\u003C\u003E8__locals2 = cDisplayClass182_1;
          // ISSUE: reference to a compiler-generated field
          //cDisplayClass180.bytes = 0U;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          //BandSocketUWP.\u003C\u003Ec__DisplayClass18_2 cDisplayClass182_2 = cDisplayClass180.CS\u0024\u003C\u003E8__locals2;
          // ISSUE: reference to a compiler-generated field
          //CommandResponse response = cDisplayClass182_2.response;
          // ISSUE: reference to a compiler-generated method
          //CommandResponse commandResponse = await Task.Run<CommandResponse>(
          //    new Func<Task<CommandResponse>>(cDisplayClass180.\u003CReceive\u003Eb__1));
          // ISSUE: reference to a compiler-generated field
          //cDisplayClass182_2.response = commandResponse;
          //cDisplayClass182_2 = (BandSocketUWP.\u003C\u003Ec__DisplayClass18_2) null;
          try
          {
            if (Progress != null)
            {
              // ISSUE: method pointer
              //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync((CoreDispatcherPriority) 
              //    0, new DispatchedHandler((object) cDisplayClass180, __methodptr(\u003CReceive\u003Eb__2)));

            }
          }
          catch (Exception ex)
          {
                  Debug.WriteLine("[ex] Exception : " + ex.Message);
          }

          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          //if (!cDisplayClass180.CS\u0024\u003C\u003E8__locals2.response.StatusReceived() && cDisplayClass180.bytes != 0U)
          //  cDisplayClass180 = (BandSocketUWP.\u003C\u003Ec__DisplayClass18_0) null;
          //else
          //  break;
        }
          // ISSUE: reference to a compiler-generated field
          return default;//cDisplayClass182_1.response;
      }));
    }

    public async Task<CommandResponse> Request(
      CommandPacket packet,
      uint buffer,
      Action<ulong, ulong> Progress = null)
    {
      if (!this.Connected)
        throw new BandSocketConnectedNot();
      await this.Send(packet);
      return await this.Receive(buffer, Progress);
    }

    public async Task<CommandResponse> Put(CommandPacket packet, uint buffer)
    {
      if (!this.Connected)
        throw new BandSocketConnectedNot();
      await this.Send(packet);
      return await this.Receive(buffer, (Action<ulong, ulong>) null);
    }

    protected byte[] ReadBytes(uint count)
    {
      byte[] numArray = new byte[(int) count];
      this.DataReader.ReadBytes(numArray);
      return numArray;
    }

    public static async Task<RfcommDeviceService> GetRfcommDeviceServiceForHostFromUuid(
      HostName host,
      Guid uuid)
    {
      RfcommServiceId.FromUuid(uuid);
      return (await BluetoothDevice.FromHostNameAsync(host)).RfcommServices[0];
    }

    public DataReader GetDataReader() => this.DataReader;

    public DataWriter GetDataWriter() => this.DataWriter;
  }
}
