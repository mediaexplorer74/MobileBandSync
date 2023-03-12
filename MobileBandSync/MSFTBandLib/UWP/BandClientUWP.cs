// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.UWP.BandClientUWP
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;

namespace MobileBandSync.MSFTBandLib.UWP
{
  public class BandClientUWP : BandClientInterface
  {
    public async Task<List<BandInterface>> GetPairedBands()
    {
      List<BandInterface> bands = new List<BandInterface>();
      foreach (DeviceInformation deviceInformation in (IEnumerable<DeviceInformation>) await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.FromUuid(Guid.Parse("a502ca97-2ba5-413c-a4e0-13804e47b38f")))))
      {
        BluetoothDevice device = await BluetoothDevice.FromIdAsync(deviceInformation.Id);
        if (device != null)
          bands.Add((BandInterface) new Band<BandSocketUWP>(device));
      }
      return bands;
    }
  }
}
