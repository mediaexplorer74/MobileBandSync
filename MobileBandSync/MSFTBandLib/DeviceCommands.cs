// Decompiled with JetBrains decompiler
// Type: MobileBandSync.MSFTBandLib.DeviceCommands
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.MSFTBandLib.Facility;

namespace MobileBandSync.MSFTBandLib
{
  internal static class DeviceCommands
  {
    internal const ushort IndexShift = 0;
    internal const ushort IndexBits = 7;
    internal const ushort IndexMask = 127;
    internal const ushort TXShift = 7;
    internal const ushort TXBits = 1;
    internal const ushort TXMask = 128;
    internal const ushort CategoryShift = 8;
    internal const ushort CategoryBits = 8;
    internal const ushort CategoryMask = 65280;
    internal static ushort CargoCoreModuleGetVersion = DeviceCommands.MakeCommand(FacilityEnum.LibraryJutil, TX.True, (byte) 1);
    internal static ushort CargoCoreModuleGetUniqueID = DeviceCommands.MakeCommand(FacilityEnum.LibraryJutil, TX.True, (byte) 2);
    internal static ushort CargoCoreModuleWhoAmI = DeviceCommands.MakeCommand(FacilityEnum.LibraryJutil, TX.True, (byte) 3);
    internal static ushort CargoCoreModuleGetLogVersion = DeviceCommands.MakeCommand(FacilityEnum.LibraryJutil, TX.True, (byte) 5);
    internal static ushort CargoCoreModuleGetApiVersion = DeviceCommands.MakeCommand(FacilityEnum.LibraryJutil, TX.True, (byte) 6);
    internal static ushort CargoCoreModuleSdkCheck = DeviceCommands.MakeCommand(FacilityEnum.LibraryJutil, TX.False, (byte) 7);
    internal static ushort CargoTimeGetUtcTime = DeviceCommands.MakeCommand(FacilityEnum.LibraryTime, TX.True, (byte) 0);
    internal static ushort CargoTimeSetUtcTime = DeviceCommands.MakeCommand(FacilityEnum.LibraryTime, TX.False, (byte) 1);
    internal static ushort CargoTimeGetLocalTime = DeviceCommands.MakeCommand(FacilityEnum.LibraryTime, TX.True, (byte) 2);
    internal static ushort CargoTimeSetTimeZoneFile = DeviceCommands.MakeCommand(FacilityEnum.LibraryTime, TX.False, (byte) 4);
    internal static ushort CargoTimeZoneFileGetVersion = DeviceCommands.MakeCommand(FacilityEnum.LibraryTime, TX.True, (byte) 6);
    internal static ushort CargoLoggerGetChunkData = DeviceCommands.MakeCommand(FacilityEnum.LibraryLogger, TX.True, (byte) 1);
    internal static ushort CargoLoggerEnableLogging = DeviceCommands.MakeCommand(FacilityEnum.LibraryLogger, TX.False, (byte) 3);
    internal static ushort CargoLoggerDisableLogging = DeviceCommands.MakeCommand(FacilityEnum.LibraryLogger, TX.False, (byte) 4);
    internal static ushort CargoLoggerGetChunkCounts = DeviceCommands.MakeCommand(FacilityEnum.LibraryLogger, TX.True, (byte) 9);
    internal static ushort CargoLoggerFlush = DeviceCommands.MakeCommand(FacilityEnum.LibraryLogger, TX.False, (byte) 13);
    internal static ushort CargoLoggerGetChunkRangeMetadata = DeviceCommands.MakeCommand(FacilityEnum.LibraryLogger, TX.True, (byte) 14);
    internal static ushort CargoLoggerGetChunkRangeData = DeviceCommands.MakeCommand(FacilityEnum.LibraryLogger, TX.True, (byte) 15);
    internal static ushort CargoLoggerDeleteChunkRange = DeviceCommands.MakeCommand(FacilityEnum.LibraryLogger, TX.False, (byte) 16);
    internal static ushort CargoProfileGetDataApp = DeviceCommands.MakeCommand(FacilityEnum.ModuleProfile, TX.True, (byte) 6);
    internal static ushort CargoProfileSetDataApp = DeviceCommands.MakeCommand(FacilityEnum.ModuleProfile, TX.False, (byte) 7);
    internal static ushort CargoProfileGetDataFW = DeviceCommands.MakeCommand(FacilityEnum.ModuleProfile, TX.True, (byte) 8);
    internal static ushort CargoProfileSetDataFW = DeviceCommands.MakeCommand(FacilityEnum.ModuleProfile, TX.False, (byte) 9);
    internal static ushort CargoRemoteSubscriptionSubscribe = DeviceCommands.MakeCommand(FacilityEnum.LibraryRemoteSubscription, TX.False, (byte) 0);
    internal static ushort CargoRemoteSubscriptionUnsubscribe = DeviceCommands.MakeCommand(FacilityEnum.LibraryRemoteSubscription, TX.False, (byte) 1);
    internal static ushort CargoRemoteSubscriptionGetDataLength = DeviceCommands.MakeCommand(FacilityEnum.LibraryRemoteSubscription, TX.True, (byte) 2);
    internal static ushort CargoRemoteSubscriptionGetData = DeviceCommands.MakeCommand(FacilityEnum.LibraryRemoteSubscription, TX.True, (byte) 3);
    internal static ushort CargoRemoteSubscriptionSubscribeId = DeviceCommands.MakeCommand(FacilityEnum.LibraryRemoteSubscription, TX.False, (byte) 7);
    internal static ushort CargoRemoteSubscriptionUnsubscribeId = DeviceCommands.MakeCommand(FacilityEnum.LibraryRemoteSubscription, TX.False, (byte) 8);
    internal static ushort CargoNotification = DeviceCommands.MakeCommand(FacilityEnum.ModuleNotification, TX.False, (byte) 0);
    internal static ushort CargoNotificationProtoBuf = DeviceCommands.MakeCommand(FacilityEnum.ModuleNotification, TX.False, (byte) 5);
    internal static ushort CargoDynamicAppRegisterApp = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballAppsManagement, TX.False, (byte) 0);
    internal static ushort CargoDynamicAppRemoveApp = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballAppsManagement, TX.False, (byte) 1);
    internal static ushort CargoDynamicAppRegisterAppIcons = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballAppsManagement, TX.False, (byte) 2);
    internal static ushort CargoDynamicAppSetAppTileIndex = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballAppsManagement, TX.False, (byte) 3);
    internal static ushort CargoDynamicAppSetAppBadgeTileIndex = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballAppsManagement, TX.False, (byte) 5);
    internal static ushort CargoDynamicAppSetAppNotificationTileIndex = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballAppsManagement, TX.False, (byte) 11);
    internal static ushort CargoDynamicPageLayoutSet = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballPageManagement, TX.False, (byte) 0);
    internal static ushort CargoDynamicPageLayoutRemove = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballPageManagement, TX.False, (byte) 1);
    internal static ushort CargoDynamicPageLayoutGet = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballPageManagement, TX.True, (byte) 2);
    internal static ushort CargoInstalledAppListGet = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.True, (byte) 0);
    internal static ushort CargoInstalledAppListSet = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.False, (byte) 1);
    internal static ushort CargoInstalledAppListStartStripSyncStart = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.False, (byte) 2);
    internal static ushort CargoInstalledAppListStartStripSyncEnd = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.False, (byte) 3);
    internal static ushort CargoInstalledAppListGetDefaults = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.True, (byte) 4);
    internal static ushort CargoInstalledAppListSetTile = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.False, (byte) 6);
    internal static ushort CargoInstalledAppListGetTile = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.True, (byte) 7);
    internal static ushort CargoInstalledAppListGetSettingsMask = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.True, (byte) 13);
    internal static ushort CargoInstalledAppListSetSettingsMask = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.False, (byte) 14);
    internal static ushort CargoInstalledAppListEnableSetting = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.False, (byte) 15);
    internal static ushort CargoInstalledAppListDisableSetting = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.False, (byte) 16);
    internal static ushort CargoInstalledAppListGetNoImages = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.True, (byte) 18);
    internal static ushort CargoInstalledAppListGetDefaultsNoImages = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.True, (byte) 19);
    internal static ushort CargoInstalledAppListGetMaxTileCount = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.True, (byte) 21);
    internal static ushort CargoInstalledAppListGetMaxTileAllocatedCount = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstalledAppList, TX.True, (byte) 22);
    internal static ushort CargoSystemSettingsOobeCompleteClear = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.False, (byte) 0);
    internal static ushort CargoSystemSettingsOobeCompleteSet = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.False, (byte) 1);
    internal static ushort CargoSystemSettingsFactoryReset = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.True, (byte) 7);
    internal static ushort CargoSystemSettingsGetTimeZone = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.True, (byte) 10);
    internal static ushort CargoSystemSettingsSetTimeZone = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.False, (byte) 11);
    internal static ushort CargoSystemSettingsSetEphemerisFile = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.False, (byte) 15);
    internal static ushort CargoSystemSettingsGetMeTileImageID = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.True, (byte) 18);
    internal static ushort CargoSystemSettingsOobeCompleteGet = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.True, (byte) 19);
    internal static ushort CargoSystemSettingsEnableDemoMode = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.False, (byte) 25);
    internal static ushort CargoSystemSettingsDisableDemoMode = DeviceCommands.MakeCommand(FacilityEnum.ModuleSystemSettings, TX.False, (byte) 26);
    internal static ushort CargoSRAMFWUpdateLoadData = DeviceCommands.MakeCommand(FacilityEnum.LibrarySRAMFWUpdate, TX.False, (byte) 0);
    internal static ushort CargoSRAMFWUpdateBootIntoUpdateMode = DeviceCommands.MakeCommand(FacilityEnum.LibrarySRAMFWUpdate, TX.False, (byte) 1);
    internal static ushort CargoSRAMFWUpdateValidateAssets = DeviceCommands.MakeCommand(FacilityEnum.LibrarySRAMFWUpdate, TX.True, (byte) 2);
    internal static ushort CargoEFlashRead = DeviceCommands.MakeCommand(FacilityEnum.DriverEFlash, TX.True, (byte) 1);
    internal static ushort CargoGpsIsEnabled = DeviceCommands.MakeCommand(FacilityEnum.LibraryGps, TX.True, (byte) 6);
    internal static ushort CargoGpsEphemerisCoverageDates = DeviceCommands.MakeCommand(FacilityEnum.LibraryGps, TX.True, (byte) 13);
    internal static ushort CargoFireballUINavigateToScreen = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballUI, TX.False, (byte) 0);
    internal static ushort CargoFireballUIClearMeTileImage = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballUI, TX.False, (byte) 6);
    internal static ushort CargoFireballUISetSmsResponse = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballUI, TX.False, (byte) 7);
    internal static ushort CargoFireballUIGetAllSmsResponse = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballUI, TX.True, (byte) 11);
    internal static ushort CargoFireballUIReadMeTileImage = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballUI, TX.True, (byte) 14);
    internal static ushort CargoFireballUIWriteMeTileImageWithID = DeviceCommands.MakeCommand(FacilityEnum.ModuleFireballUI, TX.False, (byte) 17);
    internal static ushort CargoThemeColorSetFirstPartyTheme = DeviceCommands.MakeCommand(FacilityEnum.ModuleThemeColor, TX.False, (byte) 0);
    internal static ushort CargoThemeColorGetFirstPartyTheme = DeviceCommands.MakeCommand(FacilityEnum.ModuleThemeColor, TX.True, (byte) 1);
    internal static ushort CargoThemeColorSetCustomTheme = DeviceCommands.MakeCommand(FacilityEnum.ModuleThemeColor, TX.False, (byte) 2);
    internal static ushort CargoThemeColorReset = DeviceCommands.MakeCommand(FacilityEnum.ModuleThemeColor, TX.False, (byte) 4);
    internal static ushort CargoHapticPlayVibrationStream = DeviceCommands.MakeCommand(FacilityEnum.LibraryHaptic, TX.False, (byte) 0);
    internal static ushort CargoGoalTrackerSet = DeviceCommands.MakeCommand(FacilityEnum.ModuleGoalTracker, TX.False, (byte) 0);
    internal static ushort CargoFitnessPlansWriteFile = DeviceCommands.MakeCommand(FacilityEnum.LibraryFitnessPlans, TX.False, (byte) 4);
    internal static ushort CargoGolfCourseFileWrite = DeviceCommands.MakeCommand(FacilityEnum.LibraryGolf, TX.False, (byte) 0);
    internal static ushort CargoGolfCourseFileGetMaxSize = DeviceCommands.MakeCommand(FacilityEnum.LibraryGolf, TX.True, (byte) 1);
    internal static ushort CargoOobeSetStage = DeviceCommands.MakeCommand(FacilityEnum.ModuleOobe, TX.False, (byte) 0);
    internal static ushort CargoOobeGetStage = DeviceCommands.MakeCommand(FacilityEnum.ModuleOobe, TX.True, (byte) 1);
    internal static ushort CargoOobeFinalize = DeviceCommands.MakeCommand(FacilityEnum.ModuleOobe, TX.False, (byte) 2);
    internal static ushort CargoCortanaNotification = DeviceCommands.MakeCommand(FacilityEnum.ModuleCortana, TX.False, (byte) 0);
    internal static ushort CargoCortanaStart = DeviceCommands.MakeCommand(FacilityEnum.ModuleCortana, TX.False, (byte) 1);
    internal static ushort CargoCortanaStop = DeviceCommands.MakeCommand(FacilityEnum.ModuleCortana, TX.False, (byte) 2);
    internal static ushort CargoCortanaCancel = DeviceCommands.MakeCommand(FacilityEnum.ModuleCortana, TX.False, (byte) 3);
    internal static ushort CargoPersistedAppDataSetRunMetrics = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.False, (byte) 0);
    internal static ushort CargoPersistedAppDataGetRunMetrics = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.True, (byte) 1);
    internal static ushort CargoPersistedAppDataSetBikeMetrics = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.False, (byte) 2);
    internal static ushort CargoPersistedAppDataGetBikeMetrics = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.True, (byte) 3);
    internal static ushort CargoPersistedAppDataSetBikeSplitMult = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.False, (byte) 4);
    internal static ushort CargoPersistedAppDataGetBikeSplitMult = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.True, (byte) 5);
    internal static ushort CargoPersistedAppDataSetWorkoutActivities = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.False, (byte) 9);
    internal static ushort CargoPersistedAppDataGetWorkoutActivities = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.True, (byte) 16);
    internal static ushort CargoPersistedAppDataSetSleepNotification = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.False, (byte) 17);
    internal static ushort CargoPersistedAppDataGetSleepNotification = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.True, (byte) 18);
    internal static ushort CargoPersistedAppDataDisableSleepNotification = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.False, (byte) 19);
    internal static ushort CargoPersistedAppDataSetLightExposureNotification = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.False, (byte) 21);
    internal static ushort CargoPersistedAppDataGetLightExposureNotification = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.True, (byte) 22);
    internal static ushort CargoPersistedAppDataDisableLightExposureNotification = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedApplicationData, TX.False, (byte) 23);
    internal static ushort CargoGetProductSerialNumber = DeviceCommands.MakeCommand(FacilityEnum.LibraryConfiguration, TX.True, (byte) 8);
    internal static ushort CargoKeyboardCmd = DeviceCommands.MakeCommand(FacilityEnum.LibraryKeyboard, TX.False, (byte) 0);
    internal static ushort CargoSubscriptionLoggerSubscribe = DeviceCommands.MakeCommand(FacilityEnum.ModuleLoggerSubscriptions, TX.False, (byte) 0);
    internal static ushort CargoSubscriptionLoggerUnsubscribe = DeviceCommands.MakeCommand(FacilityEnum.ModuleLoggerSubscriptions, TX.False, (byte) 1);
    internal static ushort CargoCrashDumpGetFileSize = DeviceCommands.MakeCommand(FacilityEnum.DriverCrashDump, TX.True, (byte) 1);
    internal static ushort CargoCrashDumpGetAndDeleteFile = DeviceCommands.MakeCommand(FacilityEnum.DriverCrashDump, TX.True, (byte) 2);
    internal static ushort CargoInstrumentationGetFileSize = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstrumentation, TX.True, (byte) 4);
    internal static ushort CargoInstrumentationGetFile = DeviceCommands.MakeCommand(FacilityEnum.ModuleInstrumentation, TX.True, (byte) 5);
    internal static ushort CargoPersistedStatisticsRunGet = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedStatistics, TX.True, (byte) 2);
    internal static ushort CargoPersistedStatisticsWorkoutGet = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedStatistics, TX.True, (byte) 3);
    internal static ushort CargoPersistedStatisticsSleepGet = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedStatistics, TX.True, (byte) 4);
    internal static ushort CargoPersistedStatisticsGuidedWorkoutGet = DeviceCommands.MakeCommand(FacilityEnum.ModulePersistedStatistics, TX.True, (byte) 5);

    internal static ushort MakeCommand(FacilityEnum category, TX isTXCommand, byte index) => (ushort) ((uint) (ushort) ((uint) (ushort) category << 8) | (uint) (ushort) ((uint) isTXCommand << 7) | (uint) index);

    internal static void LookupCommand(
      ushort commandId,
      out FacilityEnum category,
      out TX isTXCommand,
      out byte index)
    {
      category = (FacilityEnum) (((int) commandId & 65280) >> 8);
      isTXCommand = (TX) (((int) commandId & 128) >> 7);
      index = (byte) ((uint) commandId & (uint) sbyte.MaxValue);
    }
  }
}
