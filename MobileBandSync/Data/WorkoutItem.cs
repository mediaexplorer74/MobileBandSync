// WorkoutItem.cs
// Type: MobileBandSync.Data.WorkoutItem
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using Microsoft.Data.Sqlite;
using MobileBandSync.Common;
using MobileBandSync.OpenTcx;
using MobileBandSync.OpenTcx.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace MobileBandSync.Data
{
  public class WorkoutItem
  {
    private EventRegistrationTokenTable<EventHandler<TracksLoadedEventArgs>> m_currentWorkout;
    private string _title;

    public WorkoutItem(
      string uniqueId,
      string title,
      string subtitle,
      string imagePath,
      string description)
    {
      this.Title = title;
      this.Description = description;
      this.ImagePath = imagePath;
      this.Items = new ObservableCollection<TrackItem>();
      this.HeartRateChart = new ObservableCollection<DiagramData>();
      this.ElevationChart = new ObservableCollection<DiagramData>();
      this.CadenceNormChart = new ObservableCollection<DiagramData>();
      this.SpeedChart = new ObservableCollection<DiagramData>();
    }

    public WorkoutItem()
    {
      this.HeartRateChart = new ObservableCollection<DiagramData>();
      this.ElevationChart = new ObservableCollection<DiagramData>();
      this.CadenceNormChart = new ObservableCollection<DiagramData>();
      this.SpeedChart = new ObservableCollection<DiagramData>();
    }

    public string UniqueId => Guid.NewGuid().ToString("B");

    public string Subtitle
    {
      get => this.Notes;
      set
      {
        this.Modified = this.Notes != value;
        this.Notes = value;
      }
    }

    public string Description { get; set; }

    public string ImagePath { get; private set; }

    public ObservableCollection<TrackItem> Items { get; set; }

    public ObservableCollection<SleepItem> SleepItems { get; set; }

    public int WorkoutId { get; set; }

    public byte WorkoutType { get; set; }

    public string Title
    {
      get => this._title;
      set
      {
        this.Modified = this._title != value;
        this._title = value;
      }
    }

    public string Notes { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public byte AvgHR { get; set; }

    public byte MaxHR { get; set; }

    public int Calories { get; set; }

    public int AvgSpeed { get; set; }

    public int MaxSpeed { get; set; }

    public int DurationSec { get; set; }

    public long DistanceMeters { get; set; }

    public long LongitudeStart { get; set; }

    public long LatitudeStart { get; set; }

    public int LongDeltaRectSW { get; set; }

    public int LatDeltaRectSW { get; set; }

    public int LongDeltaRectNE { get; set; }

    public int LatDeltaRectNE { get; set; }

    public string DbPath { get; set; }

    public string FilenameTCX { get; set; }

    public string TCXBuffer { get; set; }

    public int Index { get; set; }

    public TimeSpan AwakeDuration
    {
      get => new TimeSpan(0, 0, 0, this.AvgSpeed);
      set => this.AvgSpeed = (int) value.TotalSeconds;
    }

    public TimeSpan SleepDuration
    {
      get => new TimeSpan(0, 0, 0, this.MaxSpeed);
      set => this.MaxSpeed = (int) value.TotalSeconds;
    }

    public int NumberOfWakeups
    {
      get => this.DurationSec;
      set => this.DurationSec = value;
    }

    public TimeSpan FallAsleepDuration
    {
      get => new TimeSpan(0, 0, 0, (int) this.DistanceMeters);
      set => this.DistanceMeters = (long) value.TotalSeconds;
    }

    public int SleepEfficiencyPercentage
    {
      get => this.LongDeltaRectSW;
      set => this.LongDeltaRectSW = value;
    }

    public TimeSpan TotalRestlessSleepDuration
    {
      get => new TimeSpan(0, 0, 0, this.LatDeltaRectSW);
      set => this.LatDeltaRectSW = (int) value.TotalSeconds;
    }

    public TimeSpan TotalRestfulSleepDuration
    {
      get => new TimeSpan(0, 0, 0, this.LongDeltaRectNE);
      set => this.LongDeltaRectNE = (int) value.TotalSeconds;
    }

    public uint Feeling
    {
      get => (uint) this.LatDeltaRectNE;
      set => this.LatDeltaRectNE = (int) value;
    }

    public string WorkoutImageSource
    {
      get
      {
        switch ((EventType) this.WorkoutType)
        {
          case EventType.Running:
            return "Resources/running.png";
          case EventType.Biking:
            return "Resources/biking.png";
          case EventType.Walking:
            return "Resources/walking.png";
          case EventType.Sleeping:
            return "Resources/sleep.png";
          default:
            return "Resources/walking.png";
        }
      }
    }

    public Visibility DownVisibility
    {
      get
      {
        if (this.Parent != null && this.Parent.Count > 0)
        {
          for (int index = this.Index; index > 0; --index)
          {
            if (this.WorkoutType == (byte) 21 && this.Parent[index - 1].WorkoutType == (byte) 21 
                            || this.WorkoutType != (byte) 21 
                            && this.Parent[index - 1].WorkoutType != (byte) 21)
              return (Visibility) 0;
          }
        }
        return (Visibility) 1;
      }
    }

    public Visibility UpVisibility
    {
      get
      {
        if (this.Parent != null && this.Parent.Count > 0)
        {
          for (int index = this.Index; index < this.Parent.Count - 1; ++index)
          {
            if (this.WorkoutType == (byte) 21 && this.Parent[index + 1].WorkoutType == (byte) 21 
                            || this.WorkoutType != (byte) 21 
                            && this.Parent[index + 1].WorkoutType != (byte) 21)
              return (Visibility) 0;
          }
        }
        return (Visibility) 1;
      }
    }

    public event EventHandler<TracksLoadedEventArgs> TracksLoaded
    {
      add
      {
        if (this.m_currentWorkout != null)
          return;
        EventRegistrationTokenTable<EventHandler<TracksLoadedEventArgs>>
                    .GetOrCreateEventRegistrationTokenTable(ref this.m_currentWorkout)
                    .AddEventHandler(value);
      }
      remove => EventRegistrationTokenTable<EventHandler<TracksLoadedEventArgs>>
                .GetOrCreateEventRegistrationTokenTable(ref this.m_currentWorkout)
                .RemoveEventHandler(value);
    }

    internal void OnTracksLoaded(WorkoutItem workout)
    {
      EventHandler<TracksLoadedEventArgs> invocationList
                = EventRegistrationTokenTable<EventHandler<TracksLoadedEventArgs>>
                .GetOrCreateEventRegistrationTokenTable(ref this.m_currentWorkout).InvocationList;

      if (invocationList == null)
        return;
      invocationList((object) this, new TracksLoadedEventArgs(workout));
    }

    public WorkoutItem GetPrevSibling()
    {
      if (this.Parent != null && this.Parent.Count > 0)
      {
        for (int index = this.Index; index > 0; --index)
        {
          if (this.WorkoutType == (byte) 21 && this.Parent[index - 1].WorkoutType == (byte) 21
                        || this.WorkoutType != (byte) 21 
                        && this.Parent[index - 1].WorkoutType != (byte) 21)
            return this.Parent[index - 1];
        }
      }
      return (WorkoutItem) null;
    }

    public WorkoutItem GetNextSibling()
    {
      if (this.Parent != null && this.Parent.Count > 0)
      {
        for (int index = this.Index; index < this.Parent.Count - 1; ++index)
        {
          if (this.WorkoutType == (byte) 21 && this.Parent[index + 1].WorkoutType == (byte) 21 
                        || this.WorkoutType != (byte) 21 
                        && this.Parent[index + 1].WorkoutType != (byte) 21)
            return this.Parent[index + 1];
        }
      }
      return (WorkoutItem) null;
    }

    public ObservableCollection<WorkoutItem> Parent { get; set; }

    public ObservableCollection<DiagramData> HeartRateChart { get; private set; }

    public ObservableCollection<DiagramData> ElevationChart { get; private set; }

    public ObservableCollection<DiagramData> CadenceNormChart { get; private set; }

    public ObservableCollection<DiagramData> SpeedChart { get; private set; }

    public bool Modified { get; set; }

    public override string ToString() => this.Title;

    public async Task<bool> StoreWorkout()
    {
      bool flag = false;
      using (SqliteConnection db = new SqliteConnection(string.Format("Filename={0}", 
          (object) Path.Combine(ApplicationData.Current.LocalFolder.Path, "workouts.db"))))
      {
        await db.OpenAsync();
        flag = await this.StoreWorkout(db) != -1L;
      }
      return flag;
    }

    public async Task CopyToExternal(string tcxFile)
    {
      try
      {
        string targetFile = tcxFile.Substring(tcxFile.LastIndexOf('\\') + 1);

        StorageFolder targetPath = await StorageFolder.GetFolderFromPathAsync(
            tcxFile.Remove(tcxFile.LastIndexOf('\\')));

        StorageFile storageFile = await (
                    await ApplicationData.Current.LocalFolder.GetFileAsync(targetFile))
                    .CopyAsync((IStorageFolder) targetPath, targetFile, (NameCollisionOption) 1);

        targetFile = (string) null;
        targetPath = (StorageFolder) null;
      }
      catch (Exception ex)
      {
      }
    }

    public async Task<bool> ExportWorkout(StorageFile tcxFile)
    {
      bool bResult = false;
      if (tcxFile != null)
      {
        try
        {
          StorageFile createFile = 
                        await ApplicationData.Current.LocalFolder.CreateFileAsync(
              tcxFile.Name, (CreationCollisionOption) 1);

          this.TCXBuffer = this.GenerateTcxBuffer();
          await FileIO.WriteTextAsync((IStorageFile) createFile, this.TCXBuffer);
          bResult = this.TCXBuffer.Length > 0;
          await this.CopyToExternal(tcxFile.Path);
          await createFile.DeleteAsync();
          createFile = (StorageFile) null;
        }
        catch (Exception ex)
        {
          IUICommand iuiCommand = await new MessageDialog(ex.Message, "Error").ShowAsync();
        }
      }
      return bResult;
    }

    public async Task UpdateWorkout()
    {
      using (SqliteConnection db = new SqliteConnection(string.Format("Filename={0}", 
          (object) Path.Combine(ApplicationData.Current.LocalFolder.Path, "workouts.db"))))
      {
        await db.OpenAsync();
        SqliteCommand sqliteCommand = new SqliteCommand();

        sqliteCommand.Connection = db;
        sqliteCommand.CommandText = 
                    "UPDATE Workouts SET Title=@Title, Notes=@Notes WHERE WorkoutId=@WorkoutId";

        sqliteCommand.Parameters.AddWithValue("@WorkoutId", (object) this.WorkoutId);
        sqliteCommand.Parameters.AddWithValue("@Title", (object) this.Title);
        sqliteCommand.Parameters.AddWithValue("@Notes", (object) this.Notes);
        SqliteDataReader sqliteDataReader = await sqliteCommand.ExecuteReaderAsync();
      }
    }

    public async Task<long> StoreWorkout
    (
      SqliteConnection dbParam,
      Action<ulong, ulong> Progress = null,
      ulong ulStepLength = 0
    )
    {
      long lResult = -1;
      if (dbParam != null)
      {
        long lastId = 0;
        await Task.Run((Action) (() =>
        {
          SqliteCommand sqliteCommand = new SqliteCommand();
         
          sqliteCommand.Connection = dbParam;
          sqliteCommand.CommandText = 
            "INSERT INTO Workouts VALUES (NULL, @WorkoutType, @Title, @Notes, @Start, @End, @AvgHR, @MaxHR, @Calories, @AvgSpeed, @MaxSpeed, @DurationSec, @LongitudeStart, @LatitudeStart, @DistanceMeters, @LongDeltaRectSW, @LatDeltaRectSW, @LongDeltaRectNE, @LatDeltaRectNE);";
         
          sqliteCommand.Parameters.AddWithValue("@WorkoutType", (object) this.WorkoutType);
          sqliteCommand.Parameters.AddWithValue("@Title", (object) this.Title);
          sqliteCommand.Parameters.AddWithValue("@Notes", (object) this.Notes);
          sqliteCommand.Parameters.AddWithValue("@Start", (object) this.Start);
          sqliteCommand.Parameters.AddWithValue("@End", (object) this.End);
          sqliteCommand.Parameters.AddWithValue("@AvgHR", (object) this.AvgHR);
          sqliteCommand.Parameters.AddWithValue("@MaxHR", (object) this.MaxHR);
          sqliteCommand.Parameters.AddWithValue("@Calories", (object) this.Calories);
          sqliteCommand.Parameters.AddWithValue("@AvgSpeed", (object) this.AvgSpeed);
          sqliteCommand.Parameters.AddWithValue("@MaxSpeed", (object) this.MaxSpeed);
          sqliteCommand.Parameters.AddWithValue("@DurationSec", (object) this.DurationSec);
          sqliteCommand.Parameters.AddWithValue("@LongitudeStart", (object) this.LongitudeStart);
          sqliteCommand.Parameters.AddWithValue("@LatitudeStart", (object) this.LatitudeStart);
          sqliteCommand.Parameters.AddWithValue("@DistanceMeters", (object) this.DistanceMeters);
          sqliteCommand.Parameters.AddWithValue("@LongDeltaRectSW", (object) this.LongDeltaRectSW);
          sqliteCommand.Parameters.AddWithValue("@LatDeltaRectSW", (object) this.LatDeltaRectSW);
          sqliteCommand.Parameters.AddWithValue("@LongDeltaRectNE", (object) this.LongDeltaRectNE);
          sqliteCommand.Parameters.AddWithValue("@LatDeltaRectNE", (object) this.LatDeltaRectNE);
          sqliteCommand.ExecuteReader();
          lastId = (long) new SqliteCommand()
          {
            Connection = dbParam,
            CommandText = "select last_insert_rowid()"
          }.ExecuteScalar();
          lResult = (long) (this.WorkoutId = (int) lastId);
        }));
        SqliteCommand insertTrackCommand = new SqliteCommand();
        insertTrackCommand.Connection = dbParam;
        if (this.WorkoutType == (byte) 21)
        {
          insertTrackCommand.CommandText = "INSERT INTO Sleep VALUES (NULL, @SleepActivityId, @SecFromStart, @SegmentType, @SleepType, @Heartrate);";
          foreach (TrackItem trackItem in (Collection<TrackItem>) this.Items)
          {
            TrackItem sleep = trackItem;
            byte skinTempRaw = sleep.SkinTemp > 0.0 ? (byte) (sleep.SkinTemp * 10.0 - 200.0) : (byte) 0;
            await Task.Run((Action) (() =>
            {
              insertTrackCommand.Parameters.AddWithValue("@SleepActivityId", (object) lastId);
              insertTrackCommand.Parameters.AddWithValue("@SecFromStart", (object) sleep.SecFromStart);
              insertTrackCommand.Parameters.AddWithValue("@SegmentType", (object) skinTempRaw);
              insertTrackCommand.Parameters.AddWithValue("@SleepType", (object) sleep.Cadence);
              insertTrackCommand.Parameters.AddWithValue("@Heartrate", (object) sleep.Heartrate);
              insertTrackCommand.ExecuteReader();
              insertTrackCommand.Parameters.Clear();
            }));
            if (Progress != null)
              Progress(ulStepLength, 0UL);
          }
        }
        else
        {
          insertTrackCommand.CommandText = "INSERT INTO Tracks VALUES (NULL, @WorkoutId, @SecFromStart, @LongDelta, @LatDelta, @Elevation, @Heartrate, @Barometer, @Cadence, @SkinTemp, @GSR, @UVExposure);";
          foreach (TrackItem trackItem in (Collection<TrackItem>) this.Items)
          {
            TrackItem track = trackItem;
            byte skinTempRaw = track.SkinTemp > 0.0 ? (byte) (track.SkinTemp * 10.0 - 200.0) : (byte) 0;
            await Task.Run((Action) (() =>
            {
              insertTrackCommand.Parameters.AddWithValue("@WorkoutId", (object) lastId);
              insertTrackCommand.Parameters.AddWithValue("@SecFromStart", (object) track.SecFromStart);
              insertTrackCommand.Parameters.AddWithValue("@LongDelta", (object) track.LongDelta);
              insertTrackCommand.Parameters.AddWithValue("@LatDelta", (object) track.LatDelta);
              insertTrackCommand.Parameters.AddWithValue("@Elevation", (object) track.Elevation);
              insertTrackCommand.Parameters.AddWithValue("@Heartrate", (object) track.Heartrate);
              insertTrackCommand.Parameters.AddWithValue("@Barometer", (object) track.Barometer);
              insertTrackCommand.Parameters.AddWithValue("@Cadence", (object) track.Cadence);
              insertTrackCommand.Parameters.AddWithValue("@SkinTemp", (object) skinTempRaw);
              insertTrackCommand.Parameters.AddWithValue("@GSR", (object) track.GSR);
              insertTrackCommand.Parameters.AddWithValue("@UVExposure", (object) track.UV);
              insertTrackCommand.ExecuteReader();
              insertTrackCommand.Parameters.Clear();
            }));
            if (Progress != null)
              Progress(ulStepLength, 0UL);
          }
        }
      }
      return lResult;
    }

    public static async Task<ObservableCollection<WorkoutItem>> ReadWorkouts(
      WorkoutFilterData filterData = null)
    {
      ObservableCollection<WorkoutItem> workouts = new ObservableCollection<WorkoutItem>();
      using (SqliteConnection db = new SqliteConnection(string.Format("Filename={0}", (object) Path.Combine(ApplicationData.Current.LocalFolder.Path, "workouts.db"))))
      {
        await db.OpenAsync();
        SqliteCommand sqliteCommand = new SqliteCommand();
        sqliteCommand.Connection = db;
        if (filterData == null && WorkoutDataSource.DataSource.CurrentFilter == null)
        {
          sqliteCommand.CommandText = "SELECT * FROM Workouts ORDER BY Start DESC";
        }
        else
        {
          if (filterData == null)
            filterData = WorkoutDataSource.DataSource.CurrentFilter;
          sqliteCommand.CommandText = "SELECT * FROM Workouts WHERE Start >= @StartDate AND End <= @EndDate";
          bool? nullable = filterData.IsWalkingWorkout;
          bool flag1 = true;
          if ((nullable.GetValueOrDefault() == flag1 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
          {
            nullable = filterData.IsSleepingWorkout;
            bool flag2 = true;
            if ((nullable.GetValueOrDefault() == flag2 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
            {
              nullable = filterData.IsRunningWorkout;
              bool flag3 = true;
              if ((nullable.GetValueOrDefault() == flag3 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
              {
                nullable = filterData.IsBikingWorkout;
                bool flag4 = true;
                if ((nullable.GetValueOrDefault() == flag4 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                  goto label_19;
              }
            }
          }
          sqliteCommand.CommandText += " AND ( ";
          nullable = filterData.IsWalkingWorkout;
          bool flag5 = true;
          if ((nullable.GetValueOrDefault() == flag5 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
            sqliteCommand.CommandText += "WorkoutType = 16 OR ";
          nullable = filterData.IsBikingWorkout;
          bool flag6 = true;
          if ((nullable.GetValueOrDefault() == flag6 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
            sqliteCommand.CommandText += "WorkoutType = 6 OR ";
          nullable = filterData.IsRunningWorkout;
          bool flag7 = true;
          if ((nullable.GetValueOrDefault() == flag7 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
            sqliteCommand.CommandText += "WorkoutType = 4 OR ";
          nullable = filterData.IsSleepingWorkout;
          bool flag8 = true;
          if ((nullable.GetValueOrDefault() == flag8 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
            sqliteCommand.CommandText += "WorkoutType = 21 ";
          sqliteCommand.CommandText = sqliteCommand.CommandText.TrimEnd(' ', 'O', 'R');
          sqliteCommand.CommandText += " ) ";
label_19:
          if (filterData.MapBoundingBox != null)
          {
            double num1 = Math.Min(filterData.MapBoundingBox.NorthwestCorner.Latitude, filterData.MapBoundingBox.SoutheastCorner.Latitude);
            double num2 = Math.Max(filterData.MapBoundingBox.NorthwestCorner.Latitude, filterData.MapBoundingBox.SoutheastCorner.Latitude);
            double num3 = Math.Min(filterData.MapBoundingBox.NorthwestCorner.Longitude, filterData.MapBoundingBox.SoutheastCorner.Longitude);
            double num4 = Math.Max(filterData.MapBoundingBox.NorthwestCorner.Longitude, filterData.MapBoundingBox.SoutheastCorner.Longitude);
            sqliteCommand.CommandText += " AND LongitudeStart >= @Long1 AND LongitudeStart <= @Long2";
            sqliteCommand.CommandText += " AND LatitudeStart >= @Lat1 AND LatitudeStart <= @Lat2";
            sqliteCommand.Parameters.AddWithValue("@Long1", (object) (num3 * 10000000.0));
            sqliteCommand.Parameters.AddWithValue("@Long2", (object) (num4 * 10000000.0));
            sqliteCommand.Parameters.AddWithValue("@Lat1", (object) (num1 * 10000000.0));
            sqliteCommand.Parameters.AddWithValue("@Lat2", (object) (num2 * 10000000.0));
          }
          sqliteCommand.CommandText += " ORDER BY Start DESC";
          sqliteCommand.Parameters.AddWithValue("@StartDate", (object) filterData.Start);
          sqliteCommand.Parameters.AddWithValue("@EndDate", (object) filterData.End);
        }
        int iIndex = 0;
        using (SqliteDataReader reader = await sqliteCommand.ExecuteReaderAsync())
        {
          WorkoutDataSource.DataSource.TotalDistance = 0UL;
          WorkoutDataSource.DataSource.TotalHR = 0UL;
          WorkoutDataSource.DataSource.NumHRWorkouts = 0UL;
          WorkoutDataSource.DataSource.TotalAvgSpeed = 0UL;
          while (true)
          {
            if (await reader.ReadAsync())
            {
              WorkoutItem workoutItem1 = new WorkoutItem();
              workoutItem1.WorkoutId = reader.GetInt32(0);
              workoutItem1.WorkoutType = reader.GetByte(1);
              workoutItem1.Title = reader.GetString(2);
              workoutItem1.Notes = reader.GetString(3);
              DateTime dateTime = reader.GetDateTime(4);
              workoutItem1.Start = dateTime.ToUniversalTime();
              dateTime = reader.GetDateTime(5);
              workoutItem1.End = dateTime.ToUniversalTime();
              workoutItem1.AvgHR = reader.GetByte(6);
              workoutItem1.MaxHR = reader.GetByte(7);
              workoutItem1.Calories = reader.GetInt32(8);
              workoutItem1.AvgSpeed = reader.GetInt32(9);
              workoutItem1.MaxSpeed = reader.GetInt32(10);
              workoutItem1.DurationSec = reader.GetInt32(11);
              workoutItem1.LongitudeStart = reader.GetInt64(12);
              workoutItem1.LatitudeStart = reader.GetInt64(13);
              workoutItem1.DistanceMeters = reader.GetInt64(14);
              workoutItem1.LongDeltaRectSW = reader.GetInt32(15);
              workoutItem1.LatDeltaRectSW = reader.GetInt32(16);
              workoutItem1.LongDeltaRectNE = reader.GetInt32(17);
              workoutItem1.LatDeltaRectNE = reader.GetInt32(18);
              workoutItem1.Items = new ObservableCollection<TrackItem>();
              workoutItem1.SleepItems = new ObservableCollection<SleepItem>();
              workoutItem1.Parent = workouts;
              int num5 = iIndex++;
              workoutItem1.Index = num5;
              WorkoutItem workoutItem2 = workoutItem1;
              if (workoutItem2.WorkoutType != (byte) 21)
                WorkoutDataSource.DataSource.TotalDistance += (ulong) workoutItem2.DistanceMeters;
              string str1 = workoutItem2.WorkoutType == (byte) 4 ? "Running" : (workoutItem2.WorkoutType == (byte) 6 ? "Biking" : (workoutItem2.WorkoutType == (byte) 21 ? "Sleeping" : "Walking"));
              if (workoutItem2.WorkoutType != (byte) 21 && workoutItem2.AvgHR > (byte) 0)
                str1 = workoutItem2.AvgHR > (byte) 120 ? (workoutItem2.AvgHR >= (byte) 140 ? (workoutItem2.AvgHR >= (byte) 145 ? (workoutItem2.AvgHR >= (byte) 151 ? (workoutItem2.AvgHR >= (byte) 158 ? "Maximum" : "Hard") : "Moderate") : "Light") : "WarmUp") : "Walking";
              if (workoutItem2.AvgHR > (byte) 0)
              {
                WorkoutDataSource.DataSource.TotalHR += (ulong) workoutItem2.AvgHR;
                ++WorkoutDataSource.DataSource.NumHRWorkouts;
              }
              double num6 = (double) workoutItem2.AvgSpeed / 1000.0;
              WorkoutItem workoutItem3 = workoutItem2;
              string[] strArray = new string[15];
              dateTime = workoutItem2.Start;
              num5 = dateTime.Year;
              strArray[0] = num5.ToString("D4");
              dateTime = workoutItem2.Start;
              num5 = dateTime.Month;
              strArray[1] = num5.ToString("D2");
              dateTime = workoutItem2.Start;
              num5 = dateTime.Day;
              strArray[2] = num5.ToString("D2");
              strArray[3] = "_";
              dateTime = workoutItem2.Start;
              num5 = dateTime.Hour;
              strArray[4] = num5.ToString("D2");
              dateTime = workoutItem2.Start;
              num5 = dateTime.Minute;
              strArray[5] = num5.ToString("D2");
              strArray[6] = "_";
              strArray[7] = str1;
              strArray[8] = "_";
              strArray[9] = ((double) workoutItem2.DistanceMeters / 1000.0).ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              strArray[10] = "_";
              strArray[11] = num6.ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              strArray[12] = "_";
              strArray[13] = workoutItem2.AvgHR.ToString("F0");
              strArray[14] = ".tcx";
              string str2 = string.Concat(strArray);
              workoutItem3.FilenameTCX = str2;
              workouts.Add(workoutItem2);
            }
            else
              break;
          }
        }
      }
      WorkoutDataSource.DataSource.Summary = (WorkoutDataSource.DataSource.TotalDistance > 0UL ? ((double) WorkoutDataSource.DataSource.TotalDistance / 1000.0).ToString("0,0.00", (IFormatProvider) WorkoutDataSource.AppCultureInfo) : "0") + " km, Ø " + (WorkoutDataSource.DataSource.TotalHR / WorkoutDataSource.DataSource.NumHRWorkouts).ToString() + " bpm";
      return workouts;
    }

    public static async Task<WorkoutItem> ReadWorkoutFromTcx(string strTcxPath)
    {
      WorkoutItem workout = (WorkoutItem) null;
      StorageFile fileTcx = (StorageFile) null;
      try
      {
        fileTcx = await StorageFile.GetFileFromPathAsync(strTcxPath);
      }
      catch (Exception ex)
      {
      }
      if (fileTcx != null)
      {
        try
        {
          TrainingCenterDatabase_t trainingCenterDatabaseT = await new Tcx().AnalyzeTcxFile(strTcxPath);
          if (trainingCenterDatabaseT != null)
          {
            if (trainingCenterDatabaseT.Activities != null)
            {
              if (trainingCenterDatabaseT.Activities.Activity[0] != null)
              {
                if (trainingCenterDatabaseT.Activities.Activity[0].Lap[0] != null)
                {
                  DateTime id = trainingCenterDatabaseT.Activities.Activity[0].Id;
                  int num1 = 0;
                  long num2 = 0;
                  int num3 = 0;
                  int val1_1 = 0;
                  int num4 = 0;
                  int num5 = 1;
                  foreach (ActivityLap_t activityLapT in trainingCenterDatabaseT.Activities.Activity[0].Lap)
                  {
                    num1 += (int) activityLapT.TotalTimeSeconds;
                    num2 += (long) activityLapT.DistanceMeters;
                    num3 += (int) activityLapT.Calories;
                    if (activityLapT.MaximumHeartRateBpm != null)
                      val1_1 = Math.Max(val1_1, (int) activityLapT.MaximumHeartRateBpm.Value);
                    if (activityLapT.AverageHeartRateBpm != null)
                    {
                      num4 += (int) activityLapT.AverageHeartRateBpm.Value;
                      ++num5;
                    }
                  }
                  workout = new WorkoutItem()
                  {
                    WorkoutType = trainingCenterDatabaseT.Activities.Activity[0].Sport == Sport_t.Running ? (byte) 4 : (trainingCenterDatabaseT.Activities.Activity[0].Sport == Sport_t.Biking ? (byte) 6 : (byte) 32),
                    Notes = trainingCenterDatabaseT.Activities.Activity[0].Notes,
                    Start = id.ToUniversalTime(),
                    End = id.AddSeconds((double) num1).ToUniversalTime(),
                    MaxHR = (byte) val1_1,
                    Calories = num3,
                    DurationSec = num1,
                    DistanceMeters = num2,
                    Items = new ObservableCollection<TrackItem>(),
                    SleepItems = new ObservableCollection<SleepItem>()
                  };
                  if (num4 > 0 && num5 > 0)
                    workout.AvgHR = (byte) (num4 / num5);
                  double num6 = 1000.0 / ((double) workout.DistanceMeters / (double) workout.DurationSec) / 60.0;
                  double num7 = num6 % 1.0;
                  double num8 = 0.6 * num7;
                  double num9 = num6 - num7 + num8;
                  workout.AvgSpeed = (int) Math.Ceiling(num9 * 1000.0);
                  string str1 = trainingCenterDatabaseT.Activities.Activity[0].Sport == Sport_t.Running ? "Running" : (trainingCenterDatabaseT.Activities.Activity[0].Sport == Sport_t.Biking ? "Biking" : "Walking");
                  if (workout.AvgHR > (byte) 0)
                    str1 = workout.AvgHR > (byte) 120 ? (workout.AvgHR >= (byte) 140 ? (workout.AvgHR >= (byte) 145 ? (workout.AvgHR >= (byte) 151 ? (workout.AvgHR >= (byte) 158 ? "Maximum" : "Hard") : "Moderate") : "Light") : "WarmUp") : "Walking";
                  if (trainingCenterDatabaseT.Activities.Activity[0].Lap[0].MaximumSpeedSpecified)
                    workout.MaxSpeed = (int) trainingCenterDatabaseT.Activities.Activity[0].Lap[0].MaximumSpeed;
                  WorkoutItem workoutItem1 = workout;
                  string[] strArray1 = new string[15]
                  {
                    id.Year.ToString("D4"),
                    id.Month.ToString("D2"),
                    id.Day.ToString("D2"),
                    "_",
                    id.Hour.ToString("D2"),
                    id.Minute.ToString("D2"),
                    "_",
                    str1,
                    "_",
                    ((double) (workout.DistanceMeters / 1000L)).ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo),
                    "_",
                    num9.ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo),
                    "_",
                    null,
                    null
                  };
                  byte avgHr = workout.AvgHR;
                  strArray1[13] = avgHr.ToString("F0");
                  strArray1[14] = ".tcx";
                  string str2 = string.Concat(strArray1);
                  workoutItem1.FilenameTCX = str2;
                  WorkoutItem workoutItem2 = workout;
                  string[] strArray2 = new string[10]
                  {
                    id.ToString((IFormatProvider) WorkoutDataSource.AppCultureInfo),
                    " ",
                    str1,
                    " ",
                    ((double) workout.DistanceMeters / 1000.0).ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo),
                    " km ",
                    num9.ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo),
                    " min/km ",
                    null,
                    null
                  };
                  avgHr = workout.AvgHR;
                  strArray2[8] = avgHr.ToString("F0");
                  strArray2[9] = " bpm";
                  string str3 = string.Concat(strArray2);
                  workoutItem2.Title = str3;
                  if (workout.Notes == null || workout.Notes.Length == 0)
                    workout.Notes = "TCX import " + DateTime.Now.ToString((IFormatProvider) WorkoutDataSource.AppCultureInfo);
                  int val1_2 = 0;
                  int val1_3 = 0;
                  int val1_4 = 0;
                  int val1_5 = 0;
                  foreach (ActivityLap_t activityLapT in trainingCenterDatabaseT.Activities.Activity[0].Lap)
                  {
                    foreach (Trackpoint_t trackpointT in activityLapT.Track)
                    {
                      if (trackpointT.Position != null)
                      {
                        TrackItem trackItem1 = new TrackItem();
                        workout.Items.Add(trackItem1);
                        if (workout.LongitudeStart == 0L)
                        {
                          workout.LongitudeStart = (long) (trackpointT.Position.LongitudeDegrees * 10000000.0);
                          workout.LatitudeStart = (long) (trackpointT.Position.LatitudeDegrees * 10000000.0);
                          trackItem1.LongDelta = 0;
                          trackItem1.LatDelta = 0;
                        }
                        else
                        {
                          trackItem1.LongDelta = (int) ((long) (trackpointT.Position.LongitudeDegrees * 10000000.0) - workout.LongitudeStart);
                          trackItem1.LatDelta = (int) ((long) (trackpointT.Position.LatitudeDegrees * 10000000.0) - workout.LatitudeStart);
                        }
                        val1_2 = Math.Min(val1_2, trackItem1.LatDelta);
                        val1_3 = Math.Min(val1_3, trackItem1.LongDelta);
                        val1_4 = Math.Max(val1_4, trackItem1.LatDelta);
                        val1_5 = Math.Max(val1_5, trackItem1.LongDelta);
                        if (trackpointT.AltitudeMetersSpecified)
                          trackItem1.Elevation = (int) trackpointT.AltitudeMeters;
                        if (trackpointT.CadenceSpecified)
                          trackItem1.Cadence = (uint) trackpointT.Cadence;
                        if (trackpointT.HeartRateBpm != null)
                          trackItem1.Heartrate = trackpointT.HeartRateBpm.Value;
                        DateTime time;
                        if (workout.Start == DateTime.MinValue)
                        {
                          WorkoutItem workoutItem3 = workout;
                          time = trackpointT.Time;
                          DateTime universalTime = time.ToUniversalTime();
                          workoutItem3.Start = universalTime;
                        }
                        TrackItem trackItem2 = trackItem1;
                        time = trackpointT.Time;
                        int totalSeconds = (int) (time.ToUniversalTime() - workout.Start).TotalSeconds;
                        trackItem2.SecFromStart = totalSeconds;
                      }
                    }
                  }
                  workout.LatDeltaRectNE = val1_4;
                  workout.LatDeltaRectSW = val1_2;
                  workout.LongDeltaRectNE = val1_5;
                  workout.LongDeltaRectSW = val1_3;
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
      return workout;
    }

    public async Task ReadTrackData(CancellationToken token)
    {
      string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "workouts.db");
      await Task.Run((Action) (() =>
      {
        using (SqliteConnection sqliteConnection = new SqliteConnection(string.Format("Filename={0}", (object) dbpath)))
        {
          try
          {
            sqliteConnection.Open();
            SqliteCommand sqliteCommand = new SqliteCommand();
            sqliteCommand.Connection = sqliteConnection;
            if (this.Items != null && this.Items.Count > 0)
            {
              this.OnTracksLoaded(this);
            }
            else
            {
              token.ThrowIfCancellationRequested();
              sqliteCommand.CommandText = "SELECT * FROM Tracks WHERE WorkoutId = $id";
              sqliteCommand.Parameters.AddWithValue("$id", (object) this.WorkoutId);
              this.Items = new ObservableCollection<TrackItem>();
              using (SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
              {
                double totalSeconds = (this.End - this.Start).TotalSeconds;
                double num1 = -1.0;
                double num2 = num1;
                double num3 = (totalSeconds - num2) / 150.0;
                uint val1 = 0;
                double lat1 = (double) this.LatitudeStart / 10000000.0;
                double long1 = (double) this.LongitudeStart / 10000000.0;
                int num4 = 0;
                List<DiagramData> diagramDataList = new List<DiagramData>();
                this.HeartRateChart.Clear();
                this.ElevationChart.Clear();
                this.CadenceNormChart.Clear();
                this.SpeedChart.Clear();
                int val2_1 = -999;
                double val2_2 = -999.0;
                double num5 = 0.0;
                TrackItem trackItem1 = (TrackItem) null;
                int num6 = 0;
                while (sqliteDataReader.Read())
                {
                  TrackItem trackItem2 = new TrackItem()
                  {
                    TrackId = sqliteDataReader.GetInt32(0),
                    WorkoutId = sqliteDataReader.GetInt32(1),
                    SecFromStart = sqliteDataReader.GetInt32(2),
                    LongDelta = sqliteDataReader.GetInt32(3),
                    LatDelta = sqliteDataReader.GetInt32(4),
                    Elevation = sqliteDataReader.GetInt32(5),
                    Heartrate = sqliteDataReader.GetByte(6),
                    Barometer = sqliteDataReader.GetInt32(7),
                    Cadence = (uint) sqliteDataReader.GetByte(8),
                    SkinTemp = (double) sqliteDataReader.GetByte(9),
                    GSR = sqliteDataReader.GetInt32(10),
                    UV = sqliteDataReader.GetInt32(11)
                  };
                  trackItem2.SkinTemp = trackItem2.SkinTemp > 0.0 ? (trackItem2.SkinTemp + 200.0) / 10.0 : 0.0;
                  double lat2 = (double) (this.LatitudeStart + (long) trackItem2.LatDelta) / 10000000.0;
                  double long2 = (double) (this.LongitudeStart + (long) trackItem2.LongDelta) / 10000000.0;
                  int secFromStart = trackItem2.SecFromStart;
                  trackItem2.DistMeter = this.GetDistMeter(lat1, long1, lat2, long2);
                  if (trackItem1 != null && num6 <= 40 && trackItem2.DistMeter > (this.WorkoutType == (byte) 6 ? 120.0 : 50.0))
                  {
                    this.Items.Clear();
                    trackItem2.DistMeter = 0.0;
                  }
                  if (num6 >= this.Items.Count - 40 && trackItem2.DistMeter > (this.WorkoutType == (byte) 6 ? 200.0 : 150.0))
                  {
                    ++num6;
                  }
                  else
                  {
                    ++num6;
                    int num7 = secFromStart - num4;
                    trackItem2.SpeedMeterPerSecond = num7 > 1 ? trackItem2.DistMeter / (double) num7 : 0.0;
                    num5 += trackItem2.DistMeter;
                    trackItem2.TotalMeters = num5;
                    trackItem1 = trackItem2;
                    this.Items.Add(trackItem2);
                    lat1 = lat2;
                    long1 = long2;
                    num4 = secFromStart;
                    val2_1 = val2_1 != -999 ? Math.Min(trackItem2.Elevation, val2_1) : trackItem2.Elevation;
                    val2_2 = val2_2 != -999.0 ? Math.Max(trackItem2.SpeedMeterPerSecond, val2_2) : trackItem2.SpeedMeterPerSecond;
                  }
                }
                if (this.Items.Count > 0)
                {
                  int num8 = 0;
                  int num9 = -1;
                  double num10 = 150.0 / val2_2;
                  foreach (TrackItem trackItem3 in (Collection<TrackItem>) this.Items)
                  {
                    if (num9 < 0 || trackItem3.SecFromStart - num9 >= 10)
                    {
                      num9 = trackItem3.SecFromStart;
                      if (num1 < 0.0)
                        num1 = (double) trackItem3.SecFromStart;
                      if ((double) trackItem3.SecFromStart >= num1)
                      {
                        double num11 = (double) trackItem3.SecFromStart / 60.0;
                        this.HeartRateChart.Add(new DiagramData()
                        {
                          Min = num11,
                          Value = (double) trackItem3.Heartrate,
                          Index = num8
                        });
                        this.ElevationChart.Add(new DiagramData()
                        {
                          Min = num11,
                          Value = (double) Math.Max(-10, trackItem3.Elevation - val2_1)
                        });
                        diagramDataList.Add(new DiagramData()
                        {
                          Min = num11,
                          Value = (double) trackItem3.Cadence
                        });
                        val1 = Math.Max(val1, trackItem3.Cadence);
                        this.SpeedChart.Add(new DiagramData()
                        {
                          Min = num11,
                          Value = trackItem3.SpeedMeterPerSecond * num10
                        });
                        num1 += num3;
                      }
                    }
                    ++num8;
                  }
                }
                if (val1 > 0U)
                {
                  double num12 = val1 > 0U ? (double) this.MaxHR / (double) (2U * val1) : 1.0;
                  foreach (DiagramData diagramData in diagramDataList)
                  {
                    token.ThrowIfCancellationRequested();
                    this.CadenceNormChart.Add(new DiagramData()
                    {
                      Min = diagramData.Min,
                      Value = diagramData.Value * num12
                    });
                  }
                }
                this.OnTracksLoaded(this);
              }
              sqliteCommand.Parameters.Clear();
            }
          }
          catch (Exception ex)
          {
          }
        }
      }));
    }

    public async Task ReadSleepData(CancellationToken token)
    {
      string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "workouts.db");
      await Task.Run((Action) (() =>
      {
        using (SqliteConnection sqliteConnection = new SqliteConnection(string.Format("Filename={0}", (object) dbpath)))
        {
          try
          {
            sqliteConnection.Open();
            SqliteCommand sqliteCommand = new SqliteCommand();
            sqliteCommand.Connection = sqliteConnection;
            if (this.Items != null && this.Items.Count > 0)
            {
              this.OnTracksLoaded(this);
            }
            else
            {
              token.ThrowIfCancellationRequested();
              sqliteCommand.CommandText = "SELECT * FROM Sleep WHERE SleepActivityId = $id";
              sqliteCommand.Parameters.AddWithValue("$id", (object) this.WorkoutId);
              this.Items = new ObservableCollection<TrackItem>();
              using (SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
              {
                double num = ((this.End - this.Start).TotalSeconds - -1.0) / 150.0;
                while (sqliteDataReader.Read())
                {
                  TrackItem trackItem = new TrackItem()
                  {
                    TrackId = sqliteDataReader.GetInt32(0),
                    WorkoutId = sqliteDataReader.GetInt32(1),
                    SecFromStart = sqliteDataReader.GetInt32(2),
                    SkinTemp = (double) sqliteDataReader.GetInt32(3),
                    LongDelta = 0,
                    LatDelta = 0,
                    Elevation = 0,
                    Barometer = 0,
                    Cadence = (uint) sqliteDataReader.GetInt32(4),
                    Heartrate = sqliteDataReader.GetByte(5),
                    GSR = 0,
                    UV = 0
                  };
                  trackItem.SkinTemp = trackItem.SkinTemp > 0.0 ? (trackItem.SkinTemp + 200.0) / 10.0 : 0.0;
                  int secFromStart = trackItem.SecFromStart;
                  this.Items.Add(trackItem);
                }
                this.OnTracksLoaded(this);
              }
              sqliteCommand.Parameters.Clear();
            }
          }
          catch (Exception ex)
          {
          }
        }
      }));
    }

    public double GetDistMeter(double lat1, double long1, double lat2, double long2)
    {
      double num1 = 111.3 * Math.Cos((lat1 + lat2) / 2.0 * 0.01745) * (long1 - long2);
      double num2 = 111.3 * (lat1 - lat2);
      return Math.Sqrt(num1 * num1 + num2 * num2) * 1000.0;
    }

    public string GenerateTcxBuffer()
    {
      string tcxBuffer = "";
      if (this.Items.Count <= 0)
        return tcxBuffer;
      ExportType exportType1 = ExportType.HeartRate | ExportType.Cadence | ExportType.Temperature | ExportType.GalvanicSkinResponse;
      try
      {
        Tcx tcx1 = new Tcx();
        XmlDocument xmlDocument = new XmlDocument();
        if (this.WorkoutType != (byte) 4 && this.WorkoutType != (byte) 32 && this.WorkoutType != (byte) 6)
        {
          if (this.WorkoutType != (byte) 16)
            goto label_32;
        }
        if (this.Items.Count > 0)
        {
          ExportType exportType2 = exportType1;
          ExportType exportType3;
          switch ((EventType) this.WorkoutType)
          {
            case EventType.Running:
            case EventType.Hike:
              exportType3 = exportType2 & (ExportType.HeartRate | ExportType.Cadence | ExportType.Temperature | ExportType.GalvanicSkinResponse);
              break;
            case EventType.Biking:
              exportType3 = exportType2 & (ExportType.HeartRate | ExportType.Temperature | ExportType.GalvanicSkinResponse);
              break;
            default:
              exportType3 = exportType2 & ExportType.HeartRate;
              break;
          }
          TrainingCenterDatabase_t data = new TrainingCenterDatabase_t()
          {
            Activities = new ActivityList_t()
          };
          data.Activities.Activity = new Activity_t[1];
          data.Activities.Activity[0] = new Activity_t();
          data.Activities.Activity[0].Id = this.Start;
          data.Activities.Activity[0].Notes = this.Notes;
          data.Activities.Activity[0].Sport = this.WorkoutType == (byte) 6 ? Sport_t.Biking : Sport_t.Running;
          data.Activities.Activity[0].Lap = new ActivityLap_t[1];
          data.Activities.Activity[0].Lap[0] = new ActivityLap_t();
          data.Activities.Activity[0].Sport = this.WorkoutType == (byte) 6 ? Sport_t.Biking : Sport_t.Running;
          string str;
          if (this.AvgHR != (byte) 0)
          {
            if (this.AvgHR <= (byte) 120)
            {
              data.Activities.Activity[0].Sport = Sport_t.Other;
              str = "Walking";
            }
            else
              str = this.AvgHR >= (byte) 140 ? (this.AvgHR >= (byte) 145 ? (this.AvgHR >= (byte) 151 ? (this.AvgHR >= (byte) 158 ? "Maximum" : "Hard") : "Moderate") : "Light") : "WarmUp";
            if ((exportType1 & ExportType.HeartRate) == ExportType.HeartRate)
            {
              data.Activities.Activity[0].Lap[0].AverageHeartRateBpm = new HeartRateInBeatsPerMinute_t();
              data.Activities.Activity[0].Lap[0].AverageHeartRateBpm.Value = this.AvgHR;
              data.Activities.Activity[0].Lap[0].MaximumHeartRateBpm = new HeartRateInBeatsPerMinute_t();
              data.Activities.Activity[0].Lap[0].MaximumHeartRateBpm.Value = this.MaxHR;
            }
          }
          else
            str = this.WorkoutType == (byte) 6 ? "Biking" : (this.WorkoutType == (byte) 4 ? "Running" : "Walking");
          data.Activities.Activity[0].Lap[0].MaximumSpeed = (double) this.MaxSpeed;
          data.Activities.Activity[0].Lap[0].MaximumSpeedSpecified = true;
          data.Activities.Activity[0].Lap[0].TotalTimeSeconds = (double) this.DurationSec;
          data.Activities.Activity[0].Lap[0].Calories = (ushort) this.Calories;
          data.Activities.Activity[0].Lap[0].DistanceMeters = (double) this.DistanceMeters;
          data.Activities.Activity[0].Lap[0].Intensity = Intensity_t.Active;
          double num1 = 1000.0 / ((double) this.DistanceMeters / (double) this.DurationSec) / 60.0;
          double num2 = num1 % 1.0;
          double num3 = 0.6 * num2;
          double num4 = num1 - num2 + num3;
          string[] strArray = new string[15];
          strArray[0] = this.Start.Year.ToString("D4");
          strArray[1] = this.Start.Month.ToString("D2");
          DateTime start = this.Start;
          strArray[2] = start.Day.ToString("D2");
          strArray[3] = "_";
          start = this.Start;
          strArray[4] = start.Hour.ToString("D2");
          start = this.Start;
          strArray[5] = start.Minute.ToString("D2");
          strArray[6] = "_";
          strArray[7] = str;
          strArray[8] = "_";
          strArray[9] = ((double) this.DistanceMeters / 1000.0).ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
          strArray[10] = "_";
          strArray[11] = num4.ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
          strArray[12] = "_";
          strArray[13] = this.AvgHR.ToString("F0");
          strArray[14] = ".tcx";
          this.FilenameTCX = string.Concat(strArray);
          data.Activities.Activity[0].Lap[0].StartTime = this.Start;
          data.Activities.Activity[0].Lap[0].TriggerMethod = TriggerMethod_t.Manual;
          data.Activities.Activity[0].Lap[0].Track = new Trackpoint_t[this.Items.Count];
          int index = 0;
          foreach (TrackItem trackItem in (Collection<TrackItem>) this.Items)
          {
            data.Activities.Activity[0].Lap[0].Track[index] = new Trackpoint_t();
            Trackpoint_t trackpointT = data.Activities.Activity[0].Lap[0].Track[index];
            start = this.Start;
            DateTime dateTime = start.AddSeconds((double) trackItem.SecFromStart);
            trackpointT.Time = dateTime;
            data.Activities.Activity[0].Lap[0].Track[index].SensorState = SensorState_t.Present;
            data.Activities.Activity[0].Lap[0].Track[index].SensorStateSpecified = true;
            if ((exportType1 & ExportType.HeartRate) == ExportType.HeartRate)
            {
              data.Activities.Activity[0].Lap[0].Track[index].HeartRateBpm = new HeartRateInBeatsPerMinute_t();
              data.Activities.Activity[0].Lap[0].Track[index].HeartRateBpm.Value = trackItem.Heartrate;
            }
            if ((exportType1 & ExportType.Cadence) == ExportType.Cadence && this.WorkoutType != (byte) 6)
            {
              data.Activities.Activity[0].Lap[0].Track[index].Cadence = (byte) trackItem.Cadence;
              data.Activities.Activity[0].Lap[0].Track[index].CadenceSpecified = true;
            }
            data.Activities.Activity[0].Lap[0].Track[index].AltitudeMeters = (double) trackItem.Elevation;
            data.Activities.Activity[0].Lap[0].Track[index].AltitudeMetersSpecified = true;
            data.Activities.Activity[0].Lap[0].Track[index].Position = new Position_t();
            double num5 = (double) (this.LatitudeStart + (long) trackItem.LatDelta) / 10000000.0;
            double num6 = (double) (this.LongitudeStart + (long) trackItem.LongDelta) / 10000000.0;
            data.Activities.Activity[0].Lap[0].Track[index].Position.LatitudeDegrees = num5;
            data.Activities.Activity[0].Lap[0].Track[index].Position.LongitudeDegrees = num6;
            ++index;
          }
          string tcx2 = tcx1.GenerateTcx(data);
          if (tcx2 != null)
          {
            if (tcx2.Length > 0)
              tcxBuffer = tcx2.Replace("\"utf-16\"", "\"UTF-8\"");
          }
        }
      }
      catch (Exception ex)
      {
        tcxBuffer = "";
      }
label_32:
      return tcxBuffer;
    }
  }
}
