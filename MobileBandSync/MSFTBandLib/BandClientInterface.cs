// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.BandClientInterface
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileBandSync.MSFTBandLib
{
  public interface BandClientInterface
  {
    Task<List<BandInterface>> GetPairedBands();
  }
}
