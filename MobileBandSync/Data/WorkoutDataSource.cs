// WorkoutDataSource.cs
// Type: MobileBandSync.Data.WorkoutDataSource
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using Microsoft.Data.Sqlite;
using Microsoft.Data.Sqlite.Internal;
using MobileBandSync.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Popups;

namespace MobileBandSync.Data
{
  public sealed class WorkoutDataSource
  {
    public const bool _offlineTest = false;
    public static CultureInfo AppCultureInfo = new CultureInfo(Language.CurrentInputMethodLanguageTag);
    public static string BandName = "MS Band 2";
    private const string WorkoutDbName = "workouts.db";
    private const string WorkoutFolderName = "Workouts";
    private const string WorkoutDbFolderName = "WorkoutDB";
    private static WorkoutDataSource _workoutDataSource = new WorkoutDataSource();
    public ulong TotalDistance;
    public ulong NumHRWorkouts;
    public ulong TotalHR;
    public ulong TotalAvgSpeed;
    private ObservableCollection<WorkoutItem> _workouts = new ObservableCollection<WorkoutItem>();
        private IUICommand yesCommand;
        private IUICommand uiCommand;

        public ObservableCollection<WorkoutItem> Workouts
    {
      get => this._workouts;
      set => this._workouts = value;
    }

    public StorageFolder WorkoutsFolder { get; private set; }

    public WorkoutFilterData CurrentFilter { get; set; }

    public static WorkoutDataSource DataSource => WorkoutDataSource._workoutDataSource;

    public StorageFolder DatabaseFolder { get; private set; }

    public SensorLog SensorLogEngine { get; private set; }

    public static bool DbInitialized { get; private set; }

    public StorageFolder WorkoutDbFolder { get; private set; }

    public string MapServiceToken { get; set; }

    public static string GetMapServiceToken() 
            => WorkoutDataSource._workoutDataSource.MapServiceToken;

    public static ObservableCollection<WorkoutItem> GetWorkouts() 
            => WorkoutDataSource._workoutDataSource.Workouts;

    public static void SetMapServiceToken(string strServiceToken) 
            => WorkoutDataSource._workoutDataSource.MapServiceToken = strServiceToken;

    public WorkoutDataSource() => SqliteEngine.UseWinSqlite3();

    public static async Task<IEnumerable<WorkoutItem>> GetWorkoutsAsync(
      bool bForceReload = false,
      WorkoutFilterData workoutFilter = null)
    {
      try
      {
        if (!WorkoutDataSource.DbInitialized)
        {
          if (WorkoutDataSource._workoutDataSource == null)
            WorkoutDataSource._workoutDataSource = new WorkoutDataSource();
          WorkoutDataSource.DbInitialized = await WorkoutDataSource._workoutDataSource.InitDatabase();
        }
        await WorkoutDataSource._workoutDataSource.GetWorkoutDataAsync(bForceReload, workoutFilter);
      }
      catch (Exception ex)
      {
         Debug.WriteLine(ex.Message);
      }

      return (IEnumerable<WorkoutItem>) WorkoutDataSource._workoutDataSource.Workouts;
    }

    public static async Task<string> GetWorkoutSummaryAsync() 
            => WorkoutDataSource._workoutDataSource.Summary;

    public static ObservableCollection<WorkoutItem> WorkoutList 
            => WorkoutDataSource._workoutDataSource.Workouts;

    public string Summary { get; set; }

    public static async Task<List<WorkoutItem>> ImportFromSensorlog(
      StorageFolder sensorLogFolder,
      Action<string> Status,
      Action<ulong, ulong> Progress)
    {
      WorkoutDataSource._workoutDataSource.SensorLogEngine.Sequences.Clear();

      List<Workout> Workouts 
                = await WorkoutDataSource._workoutDataSource.ReadWorkoutsFromSensorLogs(sensorLogFolder);

      if (Workouts == null || Workouts.Count <= 0)
        return new List<WorkoutItem>();
      int num = 0;
      foreach (Workout workout in Workouts)
      {
        if (workout.TrackPoints != null)
          num += workout.TrackPoints.Count;
      }
      ulong ulStepLength = WorkoutDataSource._workoutDataSource.SensorLogEngine.BufferSize / (ulong) num;
      WorkoutDataSource._workoutDataSource.SensorLogEngine.StepLength = ulStepLength;

      return await WorkoutDataSource._workoutDataSource.AddWorkouts(
          Workouts, Status: Status, Progress: Progress, ulStepLength: ulStepLength);
    }

    public static async Task<List<WorkoutItem>> ImportFromSensorlog(
      byte[] btSensorLog,
      Action<string> Status,
      Action<ulong, ulong> Progress)
    {
      if (!WorkoutDataSource.DbInitialized)
        WorkoutDataSource.DbInitialized = await WorkoutDataSource._workoutDataSource.InitDatabase();

      WorkoutDataSource._workoutDataSource.SensorLogEngine.Sequences.Clear();

      List<Workout> Workouts = 
                await WorkoutDataSource._workoutDataSource.ReadWorkoutsFromSensorLogBuffer(btSensorLog);

      if (Workouts == null || Workouts.Count <= 0)
        return new List<WorkoutItem>();
      int num = 0;
      foreach (Workout workout in Workouts)
      {
        if (workout.Type != EventType.Sleeping)
        {
          if (workout.TrackPoints != null)
            num += workout.TrackPoints.Count;
        }
        else if (workout.TrackPoints != null)
          num += workout.TrackPoints.Count;
      }

      ulong ulStepLength = WorkoutDataSource._workoutDataSource.SensorLogEngine.BufferSize / (ulong) num;

      WorkoutDataSource.DataSource.SensorLogEngine.StepLength = ulStepLength;

      return await WorkoutDataSource._workoutDataSource.AddWorkouts(
          Workouts, Status: Status, Progress: Progress, ulStepLength: ulStepLength);
    }

    public async Task<bool> InitDatabase(bool bDeleteOldDb = false)
    {
      this.WorkoutsFolder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("Workouts", 
          (CreationCollisionOption) 3);
      this.WorkoutDbFolder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("WorkoutDB",
          (CreationCollisionOption) 3);
      this.DatabaseFolder = ApplicationData.Current.LocalFolder;
      if (this.WorkoutsFolder == null || this.DatabaseFolder == null)
        return false;
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        //UICommand yesCommand = new UICommand("Yes", WorkoutDataSource.\u003C\u003Ec.\u003C\u003E9__59_0 
        //    ?? (WorkoutDataSource.\u003C\u003Ec.\u003C\u003E9__59_0 
        //    = new UICommandInvokedHandler((object) WorkoutDataSource.\u003C\u003Ec.\u003C\u003E9, 
        //    __methodptr(\u003CInitDatabase\u003Eb__59_0))));

        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        //UICommand uiCommand = new UICommand("No", 
        //    WorkoutDataSource.\u003C\u003Ec.\u003C\u003E9__59_1 
        //    ?? (WorkoutDataSource.\u003C\u003Ec.\u003C\u003E9__59_1 
        //    = new UICommandInvokedHandler((object) WorkoutDataSource.\u003C\u003Ec.\u003C\u003E9, 
        //    __methodptr(\u003CInitDatabase\u003Eb__59_1))));

        MessageDialog dialog = new MessageDialog(
            "Do you want to replace the DB with the newer one found in the WorkoutDb folder?", 
            "Copy new database");

        dialog.Options = ((MessageDialogOptions) 0);
        ((ICollection<IUICommand>) dialog.Commands).Add((IUICommand) yesCommand);
        ((ICollection<IUICommand>) dialog.Commands).Add((IUICommand) uiCommand);
        StorageFile oldDb = await this.DatabaseFolder.GetFileAsync("workouts.db");
        if (oldDb != null)
        {
          if (bDeleteOldDb)
          {
            await oldDb.DeleteAsync();
          }
          else
          {
            try
            {
              StorageFile databaseFile = await this.WorkoutDbFolder.GetFileAsync("workouts.db");
              if (databaseFile != null)
              {
                BasicProperties oldProp = await oldDb.GetBasicPropertiesAsync();
                if ((await databaseFile.GetBasicPropertiesAsync()).DateModified > oldProp.DateModified)
                {
                  if (await dialog.ShowAsync() == yesCommand)
                  {
                    await oldDb.DeleteAsync();
                    StorageFile storageFile = await databaseFile.CopyAsync(
                        (IStorageFolder) this.DatabaseFolder);
                  }
                }
                oldProp = (BasicProperties) null;
              }
              databaseFile = (StorageFile) null;
            }
            catch
            {
            }
          }
          return true;
        }
        yesCommand = (UICommand) null;
        dialog = (MessageDialog) null;
        oldDb = (StorageFile) null;
      }
      catch
      {
        try
        {
          StorageFile fileAsync = await this.WorkoutDbFolder.GetFileAsync("workouts.db");
          if (fileAsync != null)
          {
            if (!bDeleteOldDb)
            {
              StorageFile storageFile = await fileAsync.CopyAsync((IStorageFolder) this.DatabaseFolder);
              return true;
            }
          }
        }
        catch
        {
        }
      }

      StorageFile fileAsync1 = await this.DatabaseFolder.CreateFileAsync("workouts.db", (CreationCollisionOption) 3);
      using (SqliteConnection db = new SqliteConnection(string.Format("Filename={0}", (object) Path.Combine(this.DatabaseFolder.Path, "workouts.db"))))
      {
        try
        {
          await db.OpenAsync();
          SqliteDataReader sqliteDataReader1 = await new SqliteCommand(
              "CREATE TABLE IF NOT EXISTS Tracks (TrackId INTEGER PRIMARY KEY AUTOINCREMENT, WorkoutId INTEGER NOT NULL, SecFromStart INT, LongDelta INT, LatDelta INT, Elevation INT, Heartrate TINYINT, Barometer INT, Cadence TINYINT, SkinTemp TINYINT, GSR INT, UVExposure INT )", db).ExecuteReaderAsync();
          
          SqliteDataReader sqliteDataReader2 = await new SqliteCommand(
              "CREATE TABLE IF NOT EXISTS Sleep (SleepId INTEGER PRIMARY KEY AUTOINCREMENT, SleepActivityId INTEGER NOT NULL, SecFromStart INT, SegmentType TINYINT, SleepType TINYINT, Heartrate TINYINT )", db).ExecuteReaderAsync();
         
          SqliteDataReader sqliteDataReader3 = await new SqliteCommand(
              "CREATE TABLE IF NOT EXISTS Workouts (WorkoutId INTEGER PRIMARY KEY AUTOINCREMENT, WorkoutType TINYINT, Title NVARCHAR(128) NULL, Notes NVARCHAR(2048) NULL, Start DATETIME, End DATETIME, AvgHR TINYINT, MaxHR TINYINT, Calories INT, AvgSpeed INT, MaxSpeed INT, DurationSec INT, LongitudeStart INT8, LatitudeStart INT8, DistanceMeters INT8, LongDeltaRectSW INT, LatDeltaRectSW INT, LongDeltaRectNE INT, LatDeltaRectNE INT )", db).ExecuteReaderAsync();
        }
        catch (Exception ex)
        {
          IUICommand iuiCommand = await new MessageDialog(ex.Message, "Error").ShowAsync();
        }
      }
      return true;
    }

    public static async Task<List<long>> StoreWorkouts(
      List<WorkoutItem> workouts,
      Action<ulong, ulong> Progress = null,
      ulong ulStepLength = 0)
    {
      List<long> listResult = new List<long>();
      using (SqliteConnection db = new SqliteConnection(string.Format("Filename={0}", 
          (object) Path.Combine(ApplicationData.Current.LocalFolder.Path, "workouts.db"))))
      {
        await db.OpenAsync();
        foreach (WorkoutItem workout in workouts)
        {
          if (workout.Items.Count > 0)
          {
            List<long> longList = listResult;
            long num = await workout.StoreWorkout(db, Progress, ulStepLength);
            longList.Add(num);
            longList = (List<long>) null;
          }
        }
      }
      return listResult;
    }

    public static async Task<bool> BackupDatabase(StorageFolder targetFolder = null)
    {
      bool bResult = false;
      StorageFile database = await ApplicationData.Current.LocalFolder.GetFileAsync("workouts.db");
      if (database != null)
      {
        if (targetFolder == null)
          targetFolder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("WorkoutDB",
              (CreationCollisionOption) 3);

        bResult = await database.CopyAsync((IStorageFolder) targetFolder, database.Name, 
            (NameCollisionOption) 1) != null;
      }
      return bResult;
    }

    public async Task<List<Workout>> ReadWorkoutsFromSensorLogBuffer(
      byte[] btSensorLog)
    {
      if (this.SensorLogEngine == null || btSensorLog == null)
        return (List<Workout>) null;
      try
      {
        using (MemoryStream memStream = new MemoryStream(btSensorLog))
        {
          this.SensorLogEngine.BufferSize = (ulong) btSensorLog.Length;
          int num = await this.SensorLogEngine.Read((Stream) memStream) ? 1 : 0;
        }
      }
      catch (Exception ex)
      {
                Debug.WriteLine(ex.Message);
      }
      
      try
      {
        if (this.SensorLogEngine.Sequences.Count > 0)
          return await this.SensorLogEngine.CreateWorkouts(
              ExportType.HeartRate | ExportType.Cadence | ExportType.Temperature | 
              ExportType.GalvanicSkinResponse);
      }
      catch (Exception ex)
      {
                Debug.WriteLine(ex.Message);
      }
      return (List<Workout>) null;
    }

    public async Task<List<Workout>> ReadWorkoutsFromSensorLogs(
      StorageFolder SensorLogFolder)
    {
      if (this.SensorLogEngine == null || SensorLogFolder == null)
        return (List<Workout>) null;
      string strTempDir = SensorLogFolder.Path;
      this.SensorLogEngine.BufferSize = 0UL;
      try
      {
        Dictionary<DateTime, string> dictFiles = new Dictionary<DateTime, string>();
        foreach (StorageFile file in (IEnumerable<StorageFile>) await SensorLogFolder.GetFilesAsync())
        {
          this.SensorLogEngine.BufferSize += (await file.GetBasicPropertiesAsync()).Size;
          string str = file.Path.Substring(strTempDir.Length + 1);
          int.Parse(str.Substring(str.Length - 12, 3));
          int second = int.Parse(str.Substring(str.Length - 14, 2));
          int minute = int.Parse(str.Substring(str.Length - 16, 2));
          int hour = int.Parse(str.Substring(str.Length - 18, 2));
          int day = int.Parse(str.Substring(str.Length - 20, 2));
          int month = int.Parse(str.Substring(str.Length - 22, 2));

          dictFiles.Add(new DateTime(int.Parse(str.Substring(str.Length - 24, 2)), 
              month, day, hour, minute, second), str);
        }
        foreach (DateTime key in (IEnumerable<DateTime>) dictFiles.Keys.OrderBy<DateTime, DateTime>(
            (Func<DateTime, DateTime>) (d => d)))
        {
          using (Stream fileStream = 
                        await ((IStorageFolder) SensorLogFolder).OpenStreamForReadAsync(dictFiles[key]))
          {
            int num = await this.SensorLogEngine.Read(fileStream) ? 1 : 0;
          }
        }
        dictFiles = (Dictionary<DateTime, string>) null;
      }
      catch (Exception ex)
      {
                Debug.WriteLine(ex.Message);
      }

      try
      {
        if (this.SensorLogEngine.Sequences.Count > 0)
          return await this.SensorLogEngine.CreateWorkouts(ExportType.HeartRate | ExportType.Cadence 
              | ExportType.Temperature | ExportType.GalvanicSkinResponse);
      }
      catch (Exception ex)
      {
                Debug.WriteLine(ex.Message);
      }
      
      return (List<Workout>) null;
    }

    public async Task<List<WorkoutItem>> AddWorkouts(
      List<Workout> Workouts,
      bool bAddToDb = false,
      Action<string> Status = null,
      Action<ulong, ulong> Progress = null,
      ulong ulStepLength = 0)
    {
      List<WorkoutItem> listWorkouts = new List<WorkoutItem>();
      int count = Workouts.Count;

      ExportType type = ExportType.HeartRate | ExportType.Cadence | ExportType.Temperature
                | ExportType.GalvanicSkinResponse;
      try
      {
        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "workouts.db");
        ResourceLoader resourceLoader = ResourceLoader.GetForViewIndependentUse();
        int iIndex = 0;
        foreach (Workout workout in Workouts)
        {
          int val1_1 = 0;
          int val1_2 = 0;
          int val1_3 = 0;
          int val1_4 = 0;
          WorkoutItem workoutData = new WorkoutItem()
          {
            WorkoutType = (byte) workout.Type,
            LongDeltaRectSW = 0,
            LatDeltaRectSW = 0,
            LongDeltaRectNE = 0,
            LatDeltaRectNE = 0,
            Items = new ObservableCollection<TrackItem>(),
            DbPath = dbpath,
            Parent = WorkoutDataSource.WorkoutList,
            Index = iIndex++
          };
          if ((workout.Type == EventType.Running || workout.Type == EventType.Hike 
                        || workout.Type == EventType.Walking || workout.Type == EventType.Sleeping 
                        || workout.Type == EventType.Biking) && workout.TrackPoints.Count > 0)
          {
            ExportType exportType1 = type;
            ExportType exportType2;
            switch (workout.Type)
            {
              case EventType.Running:
              case EventType.Walking:
              case EventType.Hike:
                exportType2 = exportType1 & (ExportType.HeartRate | ExportType.Cadence
                                    | ExportType.Temperature | ExportType.GalvanicSkinResponse);
                break;
              case EventType.Biking:
                exportType2 = exportType1 & (ExportType.HeartRate | ExportType.Temperature 
                                    | ExportType.GalvanicSkinResponse);
                break;
              default:
                exportType2 = exportType1 & (ExportType.HeartRate | ExportType.Temperature);
                break;
            }
            workoutData.Start = workout.StartTime;
            workoutData.End = workout.EndTime;
            workoutData.Notes = workout.Notes;
            workoutData.WorkoutType = (byte) workout.Type;
            if (workout.Type == EventType.Sleeping)
            {
              TimeSpan timeSpan;
              if (workout.SleepSummary != null)
              {
                workoutData.Title = workout.StartTime.ToString((IFormatProvider) WorkoutDataSource.AppCultureInfo) + " " + resourceLoader.GetString("WorkoutSleep") + " " + workout.SleepSummary.Duration.ToString("hh\\:mm");
                workoutData.AvgHR = (byte) workout.SleepSummary.HFAverage;
                workoutData.MaxHR = (byte) workout.SleepSummary.HFMax;
                workoutData.Calories = workout.SleepSummary.CaloriesBurned;
                workoutData.FallAsleepDuration = workout.SleepSummary.FallAsleepTime;
                workoutData.AwakeDuration = workout.SleepSummary.Duration - workout.SleepSummary.TotalRestfulSleepDuration - workout.SleepSummary.TotalRestlessSleepDuration;
                workoutData.DurationSec = workout.SleepSummary.Duration.Milliseconds / 1000;
                workoutData.NumberOfWakeups = (int) workout.SleepSummary.TimesAwoke;
                workoutData.TotalRestfulSleepDuration = workout.SleepSummary.TotalRestfulSleepDuration;
                WorkoutItem workoutItem = workoutData;
                timeSpan = workout.SleepSummary.Duration;
                int num1 = timeSpan.Milliseconds * 100;
                timeSpan = workout.SleepSummary.TotalRestfulSleepDuration;
                int milliseconds = timeSpan.Milliseconds;
                int num2 = (int) Math.Ceiling((double) (num1 / milliseconds));
                workoutItem.SleepEfficiencyPercentage = num2;
                workoutData.TotalRestlessSleepDuration = workout.SleepSummary.TotalRestlessSleepDuration;
                workoutData.SleepDuration = workout.SleepSummary.Duration;
                workoutData.Feeling = workout.SleepSummary.Feeling;
              }
              else
                workoutData.Title = workout.StartTime.ToString((IFormatProvider) WorkoutDataSource.AppCultureInfo) + " " + resourceLoader.GetString("WorkoutUnknown");
              foreach (WorkoutPoint trackPoint in workout.TrackPoints)
              {
                TrackItem trackItem1 = new TrackItem()
                {
                  WorkoutId = workoutData.WorkoutId,
                  LatDelta = 0,
                  LongDelta = 0
                };
                TrackItem trackItem2 = trackItem1;
                timeSpan = trackPoint.Time - workoutData.Start;
                int totalSeconds = (int) timeSpan.TotalSeconds;
                trackItem2.SecFromStart = totalSeconds;
                trackItem1.Heartrate = (byte) trackPoint.HeartRateBpm;
                trackItem1.Elevation = (int) trackPoint.Elevation;
                trackItem1.Cadence = trackPoint.Cadence;
                trackItem1.GSR = (int) trackPoint.GalvanicSkinResponse;
                trackItem1.SkinTemp = trackPoint.SkinTemperature;
                trackItem1.Barometer = 0;
                workoutData.Items.Add(trackItem1);
              }
            }
            else
            {
              if (workout.Summary != null)
              {
                string str;
                if (workout.Type == EventType.Biking)
                  str = resourceLoader.GetString("WorkoutBiking");
                else if (workout.Type == EventType.Hike)
                  str = resourceLoader.GetString("WorkoutHiking");
                else if (workout.Summary.HFAverage <= 120)
                {
                  workoutData.WorkoutType = (byte) 16;
                  str = resourceLoader.GetString("WorkoutWalking");
                }
                else
                  str = workout.Summary.HFAverage >= 140 ? (workout.Summary.HFAverage >= 145 
                                        ? (workout.Summary.HFAverage >= 151 ? (workout.Summary.HFAverage >= 160 ? resourceLoader.GetString("WorkoutMaximum") : resourceLoader.GetString("WorkoutHard")) : resourceLoader.GetString("WorkoutModerate")) : resourceLoader.GetString("WorkoutLight")) : resourceLoader.GetString("WorkoutWarmup");
                double num3 = 1000.0 / (workout.Summary.Distance / workout.Summary.Duration) 
                                    / 60.0;
                double num4 = num3 % 1.0;
                double num5 = 0.6 * num4;
                double num6 = num3 - num4 + num5;
                workoutData.AvgSpeed = (int) Math.Ceiling(num6 * 1000.0);
                workoutData.MaxSpeed = (int) workout.Summary.MaximumSpeed;
                workoutData.Calories = workout.Summary.CaloriesBurned;
                workoutData.DurationSec = (int) workout.Summary.Duration;
                workoutData.AvgHR = (byte) workout.Summary.HFAverage;
                workoutData.MaxHR = (byte) workout.Summary.HFMax;
                workoutData.DistanceMeters = (long) workout.Summary.Distance;

                workoutData.Title = workout.StartTime.ToString(
                    (IFormatProvider) WorkoutDataSource.AppCultureInfo) + " " + str + " "
                    + ((double) workoutData.DistanceMeters / 1000.0).ToString("F2", 
                    (IFormatProvider) WorkoutDataSource.AppCultureInfo) + " km " 
                    + num6.ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo) 
                    + " min/km " + workoutData.AvgHR.ToString("F0") + " bpm";

                if (workoutData.Notes == null || workoutData.Notes.Length == 0)
                  workoutData.Notes = "Sensor log import " + DateTime.Now.ToString(
                      (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              }
              else
                workoutData.Title = workout.StartTime.ToString(
                    (IFormatProvider) WorkoutDataSource.AppCultureInfo) + " Workout";
              foreach (WorkoutPoint trackPoint in workout.TrackPoints)
              {
                TrackItem trackItem = new TrackItem()
                {
                  WorkoutId = workoutData.WorkoutId
                };
                trackItem.SecFromStart = (int) (trackPoint.Time - workoutData.Start).TotalSeconds;
                trackItem.Heartrate = (byte) trackPoint.HeartRateBpm;
                trackItem.Elevation = (int) trackPoint.Elevation;
                trackItem.Cadence = (uint) (byte) trackPoint.Cadence;
                trackItem.GSR = (int) trackPoint.GalvanicSkinResponse;
                trackItem.SkinTemp = trackPoint.SkinTemperature;
                trackItem.Barometer = 0;
                if (workoutData.LongitudeStart == 0L)
                {
                  workoutData.LongitudeStart = (long) (trackPoint.Position.LongitudeDegrees * 10000000.0);
                  trackItem.LongDelta = 0;
                }
                else
                  trackItem.LongDelta = (int) (trackPoint.Position.LongitudeDegrees * 10000000.0 
                                        - (double) workoutData.LongitudeStart);

                if (workoutData.LatitudeStart == 0L)
                {
                  workoutData.LatitudeStart = (long) (trackPoint.Position.LatitudeDegrees * 10000000.0);
                  trackItem.LatDelta = 0;
                }
                else
                  trackItem.LatDelta = (int) (trackPoint.Position.LatitudeDegrees * 10000000.0 
                                        - (double) workoutData.LatitudeStart);

                workoutData.Items.Add(trackItem);

                val1_1 = Math.Min(val1_1, trackItem.LatDelta);
                val1_2 = Math.Min(val1_2, trackItem.LongDelta);
                val1_3 = Math.Max(val1_3, trackItem.LatDelta);
                val1_4 = Math.Max(val1_4, trackItem.LongDelta);
              }
              workoutData.LatDeltaRectNE = val1_3;
              workoutData.LatDeltaRectSW = val1_1;
              workoutData.LongDeltaRectNE = val1_4;
              workoutData.LongDeltaRectSW = val1_2;
            }
          }
          if (workoutData != null & bAddToDb)
          {
            int num = await workoutData.StoreWorkout() ? 1 : 0;
          }
          listWorkouts.Add(workoutData);
          workoutData = (WorkoutItem) null;
        }
        dbpath = (string) null;
        resourceLoader = (ResourceLoader) null;
      }
      catch (Exception ex)
      {
        IUICommand iuiCommand = await new MessageDialog(ex.Message, "Error").ShowAsync();
      }
      return listWorkouts;
    }

    public static async Task<WorkoutItem> GetWorkoutAsync(int workoutId)
    {
      await WorkoutDataSource._workoutDataSource.GetWorkoutDataAsync();
      IEnumerable<WorkoutItem> source = WorkoutDataSource._workoutDataSource.Workouts
                .Where<WorkoutItem>((Func<WorkoutItem, bool>) (workout => workout.WorkoutId == workoutId));

      return source.Count<WorkoutItem>() <= 0 ? (WorkoutItem) null : source.First<WorkoutItem>();
    }

    public static async Task UpdateWorkoutAsync(
      int workoutId,
      string strTitle,
      string strNotes)
    {
      await WorkoutDataSource._workoutDataSource.GetWorkoutDataAsync();
      IEnumerable<WorkoutItem> source = WorkoutDataSource._workoutDataSource.Workouts
                .Where<WorkoutItem>((Func<WorkoutItem, bool>) (workout => workout.WorkoutId == workoutId));

      if (source.Count<WorkoutItem>() <= 0)
        return;
      WorkoutItem workoutItem = source.First<WorkoutItem>();
      workoutItem.Title = strTitle;
      workoutItem.Notes = strNotes;
    }

    public static async Task<TrackItem> GetItemAsync(string uniqueId)
    {
      await WorkoutDataSource._workoutDataSource.GetWorkoutDataAsync();
      IEnumerable<TrackItem> source = WorkoutDataSource._workoutDataSource.Workouts
                .SelectMany<WorkoutItem, TrackItem>(
          (Func<WorkoutItem, IEnumerable<TrackItem>>) (workout => (IEnumerable<TrackItem>) workout.Items))
                .Where<TrackItem>((Func<TrackItem, bool>) (item => item.UniqueId.Equals(uniqueId)));

      return source.Count<TrackItem>() != 1 ? (TrackItem) null : source.First<TrackItem>();
    }

    private async Task GetWorkoutDataAsync(bool bForceReload = false, WorkoutFilterData filterData = null)
    {
      if (!bForceReload && this.Workouts.Count != 0)
        return;
      if (bForceReload)
        this.Workouts.Clear();
      if (this.SensorLogEngine == null)
        this.SensorLogEngine = new SensorLog();
      this.Workouts = await WorkoutItem.ReadWorkouts(filterData);
    }
  }
}
