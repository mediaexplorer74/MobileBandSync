// BandConnection.cs
// Type: MobileBandSync.MSFTBandLib.BandConnection
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Command;
using MobileBandSync.MSFTBandLib.Exceptions;
using System;
using System.Threading.Tasks;

namespace MobileBandSync.MSFTBandLib
{
  public class BandConnection<T> : BandConnectionInterface, IDisposable
    where T : class, BandSocketInterface
  {
    protected BandInterface Band;
    public readonly BandSocketInterface Cargo;
    public readonly BandSocketInterface Push;

    public bool Connected { get; protected set; }

    public bool Disposed { get; protected set; }

    public BandConnection()
    {
      this.Cargo = (BandSocketInterface) (Activator.CreateInstance(typeof (T), new object[0]) as T);
      this.Push = (BandSocketInterface) (Activator.CreateInstance(typeof (T), new object[0]) as T);
    }

    public BandConnection(BandInterface Band)
      : this()
    {
      this.Band = Band;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.Disposed)
        return;
      this.Cargo.Dispose();
      this.Push.Dispose();
      this.Disposed = true;
    }

    public async Task Connect(Action<ulong, ulong> Progress = null)
    {
      this.Connected = !this.Connected ? true : throw new BandConnectionConnected();
      await this.Cargo.Connect(this.Band.GetMac(), Guid.Parse("a502ca97-2ba5-413c-a4e0-13804e47b38f"), Progress);
    }

    public async Task Connect(BandInterface Band)
    {
      if (this.Connected)
        throw new BandConnectionConnected();
      this.Band = Band;
      await this.Connect();
    }

    public async Task Disconnect()
    {
      if (!this.Connected)
        throw new BandConnectionConnectedNot();
      await this.Cargo.Disconnect();
      await this.Push.Disconnect();
      this.Connected = false;
    }

    public async Task<CommandResponse> Command(
      CommandEnum command,
      Func<uint> BufferSize,
      byte[] args = null,
      uint buffer = 8192,
      Action<ulong, ulong> Progress = null)
    {
      if (!this.Connected)
        throw new BandConnectionConnectedNot();
      return await this.Cargo.Request(new CommandPacket(command, BufferSize, args), buffer, Progress);
    }

    public async Task CommandStore(
      CommandEnum command,
      Func<uint> BufferSize,
      byte[] args = null,
      uint buffer = 8192,
      Action<ulong, ulong> Progress = null)
    {
      if (!this.Connected)
        throw new BandConnectionConnectedNot();
      await this.Cargo.Send(new CommandPacket(command, BufferSize), args);
    }

    public async Task<CommandResponse> CommandStoreStatus(
      CommandEnum command,
      Func<uint> BufferSize,
      byte[] args = null,
      uint buffer = 8192,
      Action<ulong, ulong> Progress = null)
    {
      if (!this.Connected)
        throw new BandConnectionConnectedNot();
      return await this.Cargo.Request(new CommandPacket(command, BufferSize), buffer, Progress);
    }
  }
}
