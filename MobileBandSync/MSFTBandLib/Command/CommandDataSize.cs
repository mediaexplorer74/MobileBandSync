// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Command.CommandDataSize
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;

namespace MobileBandSync.MSFTBandLib.Command
{
  public class CommandDataSize : Attribute
  {
    public int DataSize { get; protected set; }

    public CommandDataSize(int DataSize) => this.DataSize = DataSize;
  }
}
