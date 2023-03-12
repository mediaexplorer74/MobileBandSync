// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Command.CommandPacket
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Includes;
using System;

namespace MobileBandSync.MSFTBandLib.Command
{
  public class CommandPacket
  {
    protected CommandEnum Command;
    protected byte[] args;
    protected Func<uint> CmdBufferSize;

    public CommandPacket(CommandEnum command, Func<uint> BufferSize, byte[] args = null)
    {
      this.Command = command;
      this.CmdBufferSize = BufferSize;
      if (args != null)
        this.args = args;
      else
        this.args = this.GetCommandDefaultArgumentsBytes();
    }

    public int GetCommandDataSize() => this.CmdBufferSize != null ? CommandHelper.GetCommandDataSize(this.CmdBufferSize) : CommandHelper.GetCommandDataSize(this.Command);

    public byte[] GetArgsSizeBytes() => new byte[1]
    {
      (byte) (8 + this.args.Length)
    };

    public byte[] GetCommandDefaultArgumentsBytes() => this.CmdBufferSize != null ? CommandHelper.GetCommandDefaultArgumentsBytes(this.CmdBufferSize) : CommandHelper.GetCommandDefaultArgumentsBytes(this.Command);

    public byte[] GetBytes()
    {
      ByteStream byteStream = new ByteStream();
      byteStream.BinaryWriter.Write(this.GetArgsSizeBytes());
      byteStream.BinaryWriter.Write((ushort) 12025);
      byteStream.BinaryWriter.Write((ushort) this.Command);
      byteStream.BinaryWriter.Write(this.GetCommandDataSize());
      byteStream.BinaryWriter.Write(this.args);
      return byteStream.GetBytes();
    }
  }
}
