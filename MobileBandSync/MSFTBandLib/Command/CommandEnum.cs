// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.Command.CommandEnum
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

namespace MobileBandSync.MSFTBandLib.Command
{
  public enum CommandEnum : ushort
  {
    SetDeviceTime = 29953, // 0x7501
    [CommandDataSize(16)] GetDeviceTime = 30082, // 0x7582
    GetSdkVersion = 30215, // 0x7607
    GetUniqueId = 30337, // 0x7681
    GetDeviceName = 30339, // 0x7683
    GetLogVersion = 30341, // 0x7685
    GetApiVersion = 30342, // 0x7686
    [CommandDataSize(12)] GetSerialNumber = 30856, // 0x7888
    FlushLog = 35853, // 0x8C0D
    ChunkRangeDelete = 35856, // 0x8C10
    ChunkCounts = 35977, // 0x8C89
    ChunkRangeGetMetadata = 35982, // 0x8C8E
    ChunkRangeGetData = 35983, // 0x8C8F
    Subscribe = 36608, // 0x8F00
    Unsubscribe = 36609, // 0x8F01
    SubscriptionSubscribeId = 36615, // 0x8F07
    SubscriptionUnsubscribeId = 36616, // 0x8F08
    SubscriptionGetDataLength = 36738, // 0x8F82
    SubscriptionGetData = 36739, // 0x8F83
    OobeSetStage = 44288, // 0xAD00
    OobeFinalise = 44290, // 0xAD02
    OobeGetStage = 44417, // 0xAD81
    UINavigateScreen = 49920, // 0xC300
    SetMeTileImage = 49937, // 0xC311
    GetMeTileImage = 50062, // 0xC38E
    ProfileSetDataApp = 50439, // 0xC507
    ProfileSetDataFw = 50441, // 0xC509
    ProfileGetDataApp = 50566, // 0xC586
    ProfileGetDataFw = 50568, // 0xC588
    OobeSetComplete = 51713, // 0xCA01
    GetMeTileImageId = 51858, // 0xCA92
    OobeGetComplete = 51859, // 0xCA93
    Notification = 52224, // 0xCC00
    GetStatisticsRun = 52866, // 0xCE82
    GetStatisticsWorkout = 52867, // 0xCE83
    [CommandDataSize(54)] GetStatisticsSleep = 52868, // 0xCE84
    GetStatisticsWorkoutGuided = 52869, // 0xCE85
    SetTiles = 54273, // 0xD401
    StartStripSyncStart = 54274, // 0xD402
    StartStripSyncEnd = 54275, // 0xD403
    SetTile = 54278, // 0xD406
    SetSettingsMask = 54286, // 0xD40E
    TilesEnableSetting = 54287, // 0xD40F
    TilesDisableSetting = 54288, // 0xD410
    GetTiles = 54400, // 0xD480
    GetTilesDefaults = 54404, // 0xD484
    GetTile = 54407, // 0xD487
    GetSettingsMask = 54413, // 0xD48D
    GetTilesNoImages = 54418, // 0xD492
    GetMaxTileCount = 54421, // 0xD495
    GetMaxTileCountAllocated = 54422, // 0xD496
    SetThemeColor = 55296, // 0xD800
  }
}
