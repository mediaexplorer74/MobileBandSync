// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Command.CommandHelper
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Facility;
using MobileBandSync.MSFTBandLib.Includes;
using System;
using System.Reflection;

namespace MobileBandSync.MSFTBandLib.Command
{
  public static class CommandHelper
  {
    public static ushort Create(FacilityEnum facility, bool tx, int index) => (ushort) ((int) facility << 8 | (tx ? 1 : 0) << 7 | index);

    public static int GetCommandDataSize(CommandEnum command)
    {
      Attribute customAttribute = typeof (CommandEnum).GetRuntimeField(command.ToString()).GetCustomAttribute(typeof (CommandDataSize));
      return customAttribute == null ? 8192 : ((CommandDataSize) customAttribute).DataSize;
    }

    public static int GetCommandDataSize(Func<uint> BufferSize) => BufferSize == null ? 8192 : (int) BufferSize();

    public static byte[] GetCommandDefaultArgumentsBytes(CommandEnum command)
    {
      ByteStream byteStream = new ByteStream();
      byteStream.BinaryWriter.Write(CommandHelper.GetCommandDataSize(command));
      return byteStream.GetBytes();
    }

    public static byte[] GetCommandDefaultArgumentsBytes(Func<uint> BufferSize)
    {
      byte[] defaultArgumentsBytes = (byte[]) null;
      if (BufferSize != null)
      {
        ByteStream byteStream = new ByteStream();
        byteStream.BinaryWriter.Write(BufferSize());
        defaultArgumentsBytes = byteStream.GetBytes();
      }
      return defaultArgumentsBytes;
    }
  }
}
