// Decompiled with JetBrains decompiler
// Type: MobileBandSync.Common.SensorLog
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.Data;
using MobileBandSync.OpenTcx;
using MobileBandSync.OpenTcx.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Data.Xml.Dom;

namespace MobileBandSync.Common
{
  public class SensorLog
  {
    private Stream DataStream;
    public Dictionary<uint, Dictionary<uint, uint>> IdOccurencies = new Dictionary<uint, Dictionary<uint, uint>>();

    public SensorLog() => this.Sequences = new List<SensorLogSequence>();

    public static bool IsSensorLog(Stream stream, out DateTime dtStartDate)
    {
      Stream stream1 = stream;
      bool flag = false;
      dtStartDate = DateTime.MinValue;
      if (stream1.CanRead && stream1.CanSeek)
      {
        stream1.Seek(0L, SeekOrigin.Begin);
        if (stream1.ReadByte() == 0)
        {
          int count = stream1.ReadByte();
          if (count == 8)
          {
            byte[] buffer = new byte[count];
            stream1.Read(buffer, 0, count);
            long int64 = BitConverter.ToInt64(buffer, 0);
            if (int64 > 0L)
            {
              dtStartDate = DateTime.FromFileTime(int64);
              flag = true;
            }
          }
        }
      }
      return flag;
    }

    public async Task<bool> Read(Stream stream)
    {
      this.DataStream = stream;
      DateTime LastTimeStamp = DateTime.MinValue;
      ushort CurrentSleepType = 9999;
      ushort CurrentSegmentType = 9999;
      if (this.DataStream.CanSeek)
        await Task.Run((Action) (() =>
        {
          SensorLogSequence sensorLogSequence1 = (SensorLogSequence) null;
          DateTime dateTime1 = DateTime.Now;
          DateTime dateTime2 = DateTime.Now;
          TimeSpan timeSpan = TimeSpan.MinValue;
          this.DataStream.Seek(0L, SeekOrigin.Begin);
          int num1;
          do
          {
            try
            {
              num1 = this.DataStream.ReadByte();
              if (num1 == -1)
                break;
              int count = this.DataStream.ReadByte();
              SensorLogType sensorLogType = (SensorLogType) num1;
              byte[] buffer = new byte[count];
              this.DataStream.Read(buffer, 0, count);
              switch (sensorLogType)
              {
                case SensorLogType.Timestamp:
                  long int64_1 = BitConverter.ToInt64(buffer, 0);
                  if (int64_1 > 0L)
                  {
                    if (sensorLogSequence1 == null)
                    {
                      sensorLogSequence1 = new SensorLogSequence(int64_1);
                      dateTime1 = DateTime.FromFileTime(int64_1);
                      dateTime2 = DateTime.FromFileTimeUtc(int64_1);
                      if (this.Sequences.Count > 0)
                      {
                        SensorLogSequence sequence = this.Sequences[this.Sequences.Count - 1];
                        if (sequence != null)
                        {
                          SensorLogSequence sensorLogSequence2 = sequence;
                          DateTime dateTime3 = dateTime1;
                          DateTime? timeStamp = sequence.TimeStamp;
                          TimeSpan? nullable = timeStamp.HasValue ? new TimeSpan?(dateTime3 - timeStamp.GetValueOrDefault()) : new TimeSpan?();
                          sensorLogSequence2.Duration = nullable;
                          break;
                        }
                        break;
                      }
                      break;
                    }
                    dateTime1 = LastTimeStamp = DateTime.FromFileTime(int64_1);
                    dateTime2 = DateTime.FromFileTimeUtc(int64_1);
                    break;
                  }
                  break;
                case SensorLogType.Timestamp3:
                case SensorLogType.Timestamp2:
                case SensorLogType.Timestamp4:
                  long int64_2 = BitConverter.ToInt64(buffer, 1);
                  if (int64_2 > 0L)
                  {
                    dateTime1 = DateTime.FromFileTime(int64_2);
                    dateTime2 = DateTime.FromFileTimeUtc(int64_2);
                    break;
                  }
                  break;
                case SensorLogType.SequenceID:
                  if (sensorLogSequence1 != null)
                  {
                    sensorLogSequence1.ID = BitConverter.ToInt32(buffer, 0);
                    this.Sequences.Add(sensorLogSequence1);
                    sensorLogSequence1 = (SensorLogSequence) null;
                    break;
                  }
                  break;
                case SensorLogType.UtcOffset:
                  if (sensorLogSequence1 != null)
                  {
                    sensorLogSequence1.UtcOffset = (int) BitConverter.ToInt16(buffer, 0);
                    timeSpan = new TimeSpan(0, sensorLogSequence1.UtcOffset, 0);
                    break;
                  }
                  break;
                case SensorLogType.SkinTemperature:
                  if (sensorLogSequence1 != null)
                  {
                    double int16 = (double) BitConverter.ToInt16(buffer, 0);
                    sensorLogSequence1.Temperatures.Add(new SkinTemperature()
                    {
                      DegreeCelsius = int16 > 0.0 ? int16 / 100.0 : 0.0,
                      TimeStamp = dateTime1
                    });
                    break;
                  }
                  break;
                case SensorLogType.Waypoint:
                  if (sensorLogSequence1 != null)
                  {
                    double int32_1 = (double) BitConverter.ToInt32(buffer, 2);
                    double int32_2 = (double) BitConverter.ToInt32(buffer, 6);
                    double int32_3 = (double) BitConverter.ToInt32(buffer, 10);
                    double int16 = (double) BitConverter.ToInt16(buffer, 0);
                    double int32_4 = (double) BitConverter.ToInt32(buffer, 14);
                    double int32_5 = (double) BitConverter.ToInt32(buffer, 18);
                    sensorLogSequence1.Waypoints.Add(new Waypoint()
                    {
                      SpeedOverGround = int16 > 0.0 ? int16 / 100.0 : 0.0,
                      Latitude = int32_1 / 10000000.0,
                      Longitude = int32_2 / 10000000.0,
                      ElevationFromMeanSeaLevel = int32_3 / 100.0,
                      EstimatedHorizontalError = int32_4 > 0.0 ? int32_4 / 100.0 : 0.0,
                      EstimatedVerticalError = int32_5 > 0.0 ? int32_5 / 100.0 : 0.0,
                      TimeStamp = dateTime1
                    });
                    break;
                  }
                  break;
                case SensorLogType.Sensor:
                  if (sensorLogSequence1 != null)
                  {
                    sensorLogSequence1.SensorList.Add(new Sensor()
                    {
                      Value1 = BitConverter.ToUInt32(buffer, 0),
                      GalvanicSkinResponse = BitConverter.ToUInt32(buffer, 4),
                      Value2 = BitConverter.ToUInt32(buffer, 8),
                      Value3 = BitConverter.ToUInt32(buffer, 12),
                      TimeStamp = dateTime1
                    });
                    break;
                  }
                  break;
                case SensorLogType.HeartRate:
                  if (sensorLogSequence1 != null)
                  {
                    sensorLogSequence1.HeartRates.Add(new HeartRate()
                    {
                      Bpm = (int) buffer[0],
                      Accuracy = (int) buffer[1],
                      TimeStamp = dateTime1
                    });
                    if (CurrentSleepType != (ushort) 9999)
                    {
                      if (CurrentSegmentType != (ushort) 9999)
                      {
                        sensorLogSequence1.StepSnapshots.Add(new Steps()
                        {
                          Counter = 0U,
                          SleepType = CurrentSleepType,
                          SegmentType = CurrentSegmentType,
                          TimeStamp = dateTime1
                        });
                        break;
                      }
                      break;
                    }
                    break;
                  }
                  break;
                case SensorLogType.Steps:
                  if (sensorLogSequence1 != null)
                  {
                    if (CurrentSleepType == (ushort) 9999)
                    {
                      if (CurrentSegmentType == (ushort) 9999)
                      {
                        sensorLogSequence1.StepSnapshots.Add(new Steps()
                        {
                          Counter = BitConverter.ToUInt32(buffer, 0),
                          TimeStamp = dateTime1
                        });
                        break;
                      }
                      break;
                    }
                    break;
                  }
                  break;
                case SensorLogType.Sleep:
                  if (sensorLogSequence1 != null)
                  {
                    CurrentSleepType = BitConverter.ToUInt16(buffer, 0);
                    CurrentSegmentType = BitConverter.ToUInt16(buffer, 2);
                    break;
                  }
                  break;
                case SensorLogType.WorkoutSummary:
                  if (sensorLogSequence1 != null)
                  {
                    long int64_3 = BitConverter.ToInt64(buffer, 0);
                    long int64_4 = BitConverter.ToInt64(buffer, 38);
                    DateTime dateTime4 = int64_3 > 0L ? DateTime.FromFileTime(int64_3) : DateTime.MinValue;
                    DateTime dateTime5 = int64_4 > 0L ? DateTime.FromFileTime(int64_4) : DateTime.MinValue;
                    double int32_6 = (double) BitConverter.ToInt32(buffer, 10);
                    double int32_7 = (double) BitConverter.ToInt32(buffer, 14);
                    double int32_8 = (double) BitConverter.ToInt32(buffer, 18);
                    double int32_9 = (double) BitConverter.ToInt32(buffer, 22);
                    int int32_10 = BitConverter.ToInt32(buffer, 26);
                    int int32_11 = BitConverter.ToInt32(buffer, 30);
                    int int32_12 = BitConverter.ToInt32(buffer, 34);
                    int int32_13 = BitConverter.ToInt32(buffer, 46);
                    double int32_14 = (double) BitConverter.ToInt32(buffer, 50);
                    sensorLogSequence1.WorkoutSummaries.Add(new WorkoutSummary()
                    {
                      StartDate = dateTime4,
                      IntermediateDate = dateTime5,
                      Duration = int32_6 / 1000.0,
                      Distance = int32_7 / 100.0,
                      AverageSpeed = int32_8 / 100.0,
                      MaximumSpeed = int32_9 / 100.0,
                      CaloriesBurned = int32_10,
                      HFAverage = int32_11,
                      HFMax = int32_12,
                      UtcDiffHrs = int32_13,
                      TotalElevation = int32_14 / 100.0,
                      UnknownValue1 = (int) BitConverter.ToInt16(buffer, 8),
                      UnknownValue2 = BitConverter.ToInt32(buffer, 54),
                      UnknownValue3 = BitConverter.ToInt32(buffer, 58),
                      UnknownValue4 = BitConverter.ToInt32(buffer, 62),
                      UnknownValue5 = BitConverter.ToInt32(buffer, 66),
                      UnknownValue6 = BitConverter.ToInt32(buffer, 70),
                      UnknownValue7 = BitConverter.ToInt32(buffer, 74)
                    });
                    break;
                  }
                  break;
                case SensorLogType.Counter:
                  if (sensorLogSequence1 != null)
                  {
                    sensorLogSequence1.Counters.Add(new Counter()
                    {
                      Value1 = (double) BitConverter.ToInt32(buffer, 0),
                      Value2 = (double) BitConverter.ToInt32(buffer, 4)
                    });
                    dateTime1 = dateTime1.AddSeconds(1.0);
                    dateTime2 = dateTime2.AddSeconds(1.0);
                    break;
                  }
                  break;
                case SensorLogType.WorkoutMarker:
                  if (sensorLogSequence1 != null)
                  {
                    int num2 = (int) buffer[1];
                    switch (num2)
                    {
                      case 4:
                      case 6:
                      case 21:
                      case 32:
                        DateTime dateTime6 = num2 != 21 ? dateTime2 + timeSpan : LastTimeStamp;
                        if (num2 == 21)
                        {
                          CurrentSleepType = (ushort) 0;
                          CurrentSegmentType = (ushort) 0;
                        }
                        else
                        {
                          CurrentSleepType = (ushort) 9999;
                          CurrentSegmentType = (ushort) 9999;
                        }
                        sensorLogSequence1.WorkoutMarkers.Add(new WorkoutMarker()
                        {
                          Action = (DistanceAnnotationType) buffer[0],
                          WorkoutType = (EventType) num2,
                          Value2 = (int) buffer[2],
                          TimeStamp = dateTime6
                        });
                        break;
                      default:
                        num2 = 99;
                        goto case 4;
                    }
                  }
                  else
                    break;
                  break;
                case SensorLogType.WorkoutMarker2:
                  if (sensorLogSequence1 != null)
                  {
                    sensorLogSequence1.WorkoutMarkers2.Add(new WorkoutMarker2()
                    {
                      Value1 = (int) BitConverter.ToInt16(buffer, 0),
                      Value2 = BitConverter.ToInt32(buffer, 2),
                      TimeStamp = LastTimeStamp
                    });
                    break;
                  }
                  break;
                case SensorLogType.SleepSummary:
                  if (sensorLogSequence1 != null)
                  {
                    long int64_5 = BitConverter.ToInt64(buffer, 0);
                    long int64_6 = BitConverter.ToInt64(buffer, 38);
                    DateTime dateTime7 = int64_5 > 0L ? DateTime.FromFileTime(int64_5) : DateTime.MinValue;
                    DateTime dateTime8 = int64_6 > 0L ? DateTime.FromFileTime(int64_6) : DateTime.MinValue;
                    sensorLogSequence1.SleepSummaries.Add(new SleepSummary()
                    {
                      StartDate = dateTime7,
                      IntermediateDate = dateTime8,
                      Duration = new TimeSpan(0, 0, 0, 0, BitConverter.ToInt32(buffer, 10)),
                      Version = (double) BitConverter.ToInt16(buffer, 8),
                      TimesAwoke = (double) BitConverter.ToInt32(buffer, 14),
                      TotalRestlessSleepDuration = new TimeSpan(0, 0, 0, 0, BitConverter.ToInt32(buffer, 18)),
                      TotalRestfulSleepDuration = new TimeSpan(0, 0, 0, 0, BitConverter.ToInt32(buffer, 22)),
                      CaloriesBurned = BitConverter.ToInt32(buffer, 26),
                      HFAverage = BitConverter.ToInt32(buffer, 30),
                      HFMax = BitConverter.ToInt32(buffer, 34),
                      FallAsleepTime = new TimeSpan(0, 0, 0, 0, BitConverter.ToInt32(buffer, 46)),
                      Feeling = BitConverter.ToUInt32(buffer, 50)
                    });
                    break;
                  }
                  break;
                case SensorLogType.DailySummary:
                  if (sensorLogSequence1 != null)
                  {
                    long int64_7 = BitConverter.ToInt64(buffer, 1);
                    sensorLogSequence1.DailySummaries.Add(new DailySummary()
                    {
                      Flag = (uint) buffer[0],
                      Date = int64_7 > 0L ? DateTime.FromFileTime(int64_7) : DateTime.MinValue
                    });
                    break;
                  }
                  break;
                default:
                  if (num1 != 134 && num1 != 213 && num1 != 17 && num1 != 20 && num1 != 2 && num1 != 3 && num1 != 4)
                  {
                    if (num1 != 14)
                      break;
                  }
                  break;
              }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                break;
            }
          }
          while (num1 >= 0);
        }));
      if (this.Sequences.Count > 0)
      {
        SensorLogSequence sequence = this.Sequences[this.Sequences.Count - 1];
        if (sequence != null && LastTimeStamp != DateTime.MinValue)
        {
          DateTime dateTime9 = LastTimeStamp;
          DateTime? timeStamp = sequence.TimeStamp;
          if ((timeStamp.HasValue ? (dateTime9 > timeStamp.GetValueOrDefault() ? 1 : 0) : 0) != 0)
          {
            SensorLogSequence sensorLogSequence = sequence;
            DateTime dateTime10 = LastTimeStamp;
            timeStamp = sequence.TimeStamp;
            TimeSpan? nullable = timeStamp.HasValue ? new TimeSpan?(dateTime10 - timeStamp.GetValueOrDefault()) : new TimeSpan?();
            sensorLogSequence.Duration = nullable;
          }
        }
      }
      return true;
    }

    public async Task<List<Workout>> CreateWorkouts(ExportType type = ExportType.TCX | ExportType.HeartRate | ExportType.Cadence)
    {
      List<Workout> Workouts = new List<Workout>();
      await Task.Run((Action) (() =>
      {
        Workout currenWorkout = (Workout) null;
        ResourceLoader viewIndependentUse = ResourceLoader.GetForViewIndependentUse();
        Dictionary<DateTime, int> heartRateList1 = new Dictionary<DateTime, int>();
        Dictionary<DateTime, double> elevationList = new Dictionary<DateTime, double>();
        Dictionary<DateTime, GpsPosition> positionList = new Dictionary<DateTime, GpsPosition>();
        Dictionary<DateTime, uint> galvanicList = new Dictionary<DateTime, uint>();
        Dictionary<DateTime, double> temperatureList = new Dictionary<DateTime, double>();
        Dictionary<DateTime, uint> stepsList = new Dictionary<DateTime, uint>();
        DateTime dateTime1 = DateTime.Now;
        double num1 = 48.6721393;
        double num2 = 9.24037;
        double num3 = 5E-05;
        double num4 = num1;
        double num5 = num2;
        foreach (SensorLogSequence sequence in this.Sequences)
        {
          if (sequence.WorkoutSummaries.Count > 0 && Workouts.Count > 0)
          {
            (currenWorkout == null ? Workouts[Workouts.Count - 1] : currenWorkout).Summary = sequence.WorkoutSummaries[sequence.WorkoutSummaries.Count - 1];
            num4 = num1;
            num5 = num2;
          }
          if (sequence.SleepSummaries.Count > 0 && Workouts.Count > 0)
            (currenWorkout == null ? Workouts[Workouts.Count - 1] : currenWorkout).SleepSummary = sequence.SleepSummaries[sequence.SleepSummaries.Count - 1];
          if (sequence.WorkoutMarkers.Count > 0)
          {
            foreach (WorkoutMarker workoutMarker in sequence.WorkoutMarkers)
            {
              if (workoutMarker.Action == DistanceAnnotationType.Start)
              {
                if (currenWorkout != null)
                {
                  currenWorkout.EndTime = dateTime1;
                  this.AddWorkoutData(ref currenWorkout, heartRateList1, elevationList, positionList, galvanicList, temperatureList, stepsList);
                  if (!Workouts.Contains(currenWorkout))
                    Workouts.Add(currenWorkout);
                  currenWorkout = (Workout) null;
                  heartRateList1.Clear();
                  elevationList.Clear();
                  positionList.Clear();
                  temperatureList.Clear();
                  galvanicList.Clear();
                  stepsList.Clear();
                }
                currenWorkout = new Workout();
                currenWorkout.LastSplitTime = currenWorkout.StartTime = workoutMarker.TimeStamp;
                currenWorkout.Type = workoutMarker.WorkoutType;
                if (!Workouts.Contains(currenWorkout))
                  Workouts.Add(currenWorkout);
                Workout workout1 = currenWorkout;
                string[] strArray = new string[9];
                strArray[0] = currenWorkout.Type.ToString();
                strArray[1] = "-";
                DateTime startTime = currenWorkout.StartTime;
                strArray[2] = startTime.Year.ToString("D4", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
                startTime = currenWorkout.StartTime;
                strArray[3] = startTime.Month.ToString("D2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
                startTime = currenWorkout.StartTime;
                int num6 = startTime.Day;
                strArray[4] = num6.ToString("D2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
                startTime = currenWorkout.StartTime;
                num6 = startTime.Hour;
                strArray[5] = num6.ToString("D2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
                startTime = currenWorkout.StartTime;
                num6 = startTime.Minute;
                strArray[6] = num6.ToString("D2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
                startTime = currenWorkout.StartTime;
                num6 = startTime.Second;
                strArray[7] = num6.ToString("D2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
                strArray[8] = ".tcx";
                string str1 = string.Concat(strArray);
                workout1.Filename = str1;
                Workout workout2 = currenWorkout;
                string format = viewIndependentUse.GetString("GeneratedString");
                object[] objArray = new object[2]
                {
                  (object) WorkoutDataSource.BandName,
                  null
                };
                startTime = currenWorkout.StartTime;
                objArray[1] = (object) startTime.ToString((IFormatProvider) WorkoutDataSource.AppCultureInfo);
                string str2 = string.Format(format, objArray);
                workout2.Notes = str2;
              }
              else if (workoutMarker.Action == DistanceAnnotationType.Split)
              {
                if (currenWorkout == null && Workouts.Count > 0)
                {
                  currenWorkout = Workouts[Workouts.Count - 1];
                  Workouts.RemoveAt(Workouts.Count - 1);
                }
                if (currenWorkout != null)
                  currenWorkout.LastSplitTime = workoutMarker.TimeStamp;
              }
              else if (workoutMarker.Action == DistanceAnnotationType.Pause)
              {
                if (currenWorkout != null)
                {
                  dateTime1 = currenWorkout.EndTime = workoutMarker.TimeStamp;
                  this.AddWorkoutData(ref currenWorkout, heartRateList1, elevationList, positionList, galvanicList, temperatureList, stepsList);
                  if (!Workouts.Contains(currenWorkout))
                    Workouts.Add(currenWorkout);
                  if (currenWorkout.Type == EventType.Sleeping)
                  {
                    num4 = num1;
                    num5 = num2;
                  }
                  currenWorkout = (Workout) null;
                  heartRateList1.Clear();
                  elevationList.Clear();
                  positionList.Clear();
                  temperatureList.Clear();
                  galvanicList.Clear();
                  stepsList.Clear();
                }
              }
              else
              {
                dateTime1 = workoutMarker.TimeStamp;
                if (currenWorkout != null && currenWorkout.EndTime == DateTime.MinValue)
                  currenWorkout.EndTime = dateTime1;
              }
            }
          }
          if (currenWorkout != null)
          {
            if ((type & ExportType.HeartRate) == ExportType.HeartRate && sequence.HeartRates.Count > 0)
            {
              if (currenWorkout.Type == EventType.Sleeping)
              {
                List<HeartRate> heartRateList2 = new List<HeartRate>();
                foreach (HeartRate heartRate in sequence.HeartRates)
                {
                  if (heartRate.Accuracy >= 9)
                    heartRateList2.Add(heartRate);
                }
                double num7 = 0.0;
                TimeSpan? duration = sequence.Duration;
                TimeSpan timeSpan1 = duration.Value;
                duration = sequence.Duration;
                TimeSpan timeSpan2 = TimeSpan.FromSeconds(1.0);
                if ((duration.HasValue ? (duration.GetValueOrDefault() > timeSpan2 ? 1 : 0) : 0) != 0)
                {
                  duration = sequence.Duration;
                  num7 = duration.Value.TotalSeconds / (double) heartRateList2.Count;
                }
                double num8 = 0.0;
                DateTime? timeStamp = sequence.TimeStamp;
                DateTime dateTime2 = timeStamp.Value;
                foreach (HeartRate heartRate in heartRateList2)
                {
                  timeStamp = sequence.TimeStamp;
                  DateTime key = timeStamp.Value + TimeSpan.FromSeconds(num8);
                  if (num8 == 0.0 || key >= dateTime2 + TimeSpan.FromSeconds(60.0))
                  {
                    heartRateList1[key] = heartRate.Bpm;
                    positionList[key] = new GpsPosition()
                    {
                      LatitudeDegrees = num4,
                      LongitudeDegrees = num5
                    };
                    elevationList[key] = 360.0;
                    num4 += num3;
                    num5 += num3;
                    dateTime2 = key;
                  }
                  num8 += num7;
                }
              }
              else
              {
                foreach (HeartRate heartRate in sequence.HeartRates)
                {
                  if (heartRate.TimeStamp >= currenWorkout.LastSplitTime - TimeSpan.FromSeconds(10.0) && (heartRate.Accuracy >= 8 || heartRate.Accuracy >= 5 && heartRate.TimeStamp <= currenWorkout.LastSplitTime + TimeSpan.FromSeconds(120.0)))
                    heartRateList1[heartRate.TimeStamp] = heartRate.Bpm;
                }
              }
            }
            if (sequence.SensorList.Count > 0)
            {
              foreach (Sensor sensor in sequence.SensorList)
                galvanicList[sensor.TimeStamp] = sensor.GalvanicSkinResponse;
            }
            if (sequence.StepSnapshots.Count > 0)
            {
              foreach (Steps stepSnapshot in sequence.StepSnapshots)
              {
                if (currenWorkout.Type == EventType.Sleeping)
                {
                  DateTime key = default;
                  ref DateTime local = ref key;
                  int year = stepSnapshot.TimeStamp.Year;
                  DateTime timeStamp = stepSnapshot.TimeStamp;
                  int month = timeStamp.Month;
                  timeStamp = stepSnapshot.TimeStamp;
                  int day = timeStamp.Day;
                  timeStamp = stepSnapshot.TimeStamp;
                  int hour = timeStamp.Hour;
                  timeStamp = stepSnapshot.TimeStamp;
                  int minute = timeStamp.Minute;
                  timeStamp = stepSnapshot.TimeStamp;
                  int second = timeStamp.Second;
                  timeStamp = stepSnapshot.TimeStamp;
                  int kind = (int) timeStamp.Kind;
                  local = new DateTime(year, month, day, hour, minute, second, (DateTimeKind) kind);
                  if (!stepsList.ContainsKey(key))
                    stepsList[key] = (uint) stepSnapshot.SleepType << 16 | (uint) stepSnapshot.SegmentType;
                }
                else
                  stepsList[stepSnapshot.TimeStamp] = stepSnapshot.Counter;
              }
            }
            if (sequence.Temperatures.Count > 0)
            {
              foreach (SkinTemperature temperature in sequence.Temperatures)
                temperatureList[temperature.TimeStamp] = temperature.DegreeCelsius;
            }
            if (sequence.Waypoints.Count > 0)
            {
              DateTime dateTime3 = currenWorkout.LastSplitTime - TimeSpan.FromSeconds(20.0);
              DateTime dateTime4 = dateTime3;
              int num9 = currenWorkout.Type == EventType.Running || currenWorkout.Type == EventType.Biking ? 20 : 150;
              int num10 = currenWorkout.Type == EventType.Running || currenWorkout.Type == EventType.Biking ? 135 : 300;
              foreach (Waypoint waypoint in sequence.Waypoints)
              {
                if (waypoint.TimeStamp >= currenWorkout.LastSplitTime - TimeSpan.FromSeconds(10.0))
                {
                  if (waypoint.EstimatedHorizontalError <= (double) num9 || waypoint.EstimatedVerticalError <= (double) num9)
                  {
                    if (waypoint.TimeStamp - dateTime3 >= TimeSpan.FromSeconds(3.0))
                    {
                      dateTime4 = dateTime3 = waypoint.TimeStamp;
                      positionList[waypoint.TimeStamp] = new GpsPosition()
                      {
                        LatitudeDegrees = waypoint.Latitude,
                        LongitudeDegrees = waypoint.Longitude
                      };
                      elevationList[waypoint.TimeStamp] = waypoint.ElevationFromMeanSeaLevel;
                    }
                  }
                  else if ((waypoint.EstimatedHorizontalError <= (double) num10 || waypoint.EstimatedVerticalError <= (double) num10) && waypoint.TimeStamp - dateTime4 >= TimeSpan.FromSeconds(3.0))
                  {
                    dateTime4 = waypoint.TimeStamp;
                    positionList[waypoint.TimeStamp] = new GpsPosition()
                    {
                      LatitudeDegrees = waypoint.Latitude,
                      LongitudeDegrees = waypoint.Longitude
                    };
                    elevationList[waypoint.TimeStamp] = waypoint.ElevationFromMeanSeaLevel;
                  }
                }
              }
            }
          }
        }
        if ((type & ExportType.TCX) != ExportType.TCX)
          return;
        this.AddTcxExport(ref Workouts, type);
      }));
      return Workouts;
    }

    public bool AddWorkoutData(
      ref Workout currenWorkout,
      Dictionary<DateTime, int> heartRateList,
      Dictionary<DateTime, double> elevationList,
      Dictionary<DateTime, GpsPosition> positionList,
      Dictionary<DateTime, uint> galvanicList,
      Dictionary<DateTime, double> temperatureList,
      Dictionary<DateTime, uint> stepsList)
    {
      uint num1 = 0;
      uint num2 = 0;
      uint num3 = 0;
      DateTime dateTime = DateTime.MinValue;
      uint num4 = 0;
      double num5 = 0.0;
      if (currenWorkout.Type == EventType.Sleeping)
      {
        foreach (DateTime key1 in heartRateList.Keys)
        {
          DateTime minValue = DateTime.MinValue;
          if (!stepsList.ContainsKey(key1))
          {
            DateTime key2 = key1;
            while (key2 >= key1 - TimeSpan.FromSeconds(120.0))
            {
              key2 = new DateTime(key2.Year, key2.Month, key2.Day, key2.Hour, key2.Minute, key2.Second, key1.Kind);
              if (stepsList.ContainsKey(key2))
              {
                num2 = stepsList[key2];
                break;
              }
              key2 -= TimeSpan.FromSeconds(1.0);
            }
          }
          else
            num2 = stepsList[key1];
          if (!galvanicList.ContainsKey(key1))
          {
            DateTime key3 = key1;
            while (key3 >= key1 - TimeSpan.FromSeconds(10.0))
            {
              if (galvanicList.ContainsKey(key3))
              {
                num1 = galvanicList[key3];
                break;
              }
              key3 -= TimeSpan.FromSeconds(1.0);
            }
          }
          else
            num1 = galvanicList[key1];
          if (!temperatureList.ContainsKey(key1))
          {
            DateTime key4 = key1;
            while (key4 >= key1 - TimeSpan.FromSeconds(10.0))
            {
              if (temperatureList.ContainsKey(key4))
              {
                num5 = temperatureList[key4];
                break;
              }
              key4 -= TimeSpan.FromSeconds(1.0);
            }
          }
          else
            num5 = temperatureList[key1];
          currenWorkout.TrackPoints.Add(new WorkoutPoint()
          {
            Time = key1,
            Position = new GpsPosition()
            {
              LatitudeDegrees = 0.0,
              LongitudeDegrees = 0.0
            },
            Elevation = 0.0,
            HeartRateBpm = heartRateList[key1],
            GalvanicSkinResponse = num1,
            SkinTemperature = num5,
            Cadence = num2
          });
        }
      }
      else
      {
        foreach (DateTime key5 in positionList.Keys)
        {
          if (positionList.ContainsKey(key5) && elevationList.ContainsKey(key5))
          {
            DateTime key6 = DateTime.MinValue;
            if (!heartRateList.ContainsKey(key5))
            {
              DateTime key7 = key5;
              while (key7 >= key5 - TimeSpan.FromSeconds(10.0))
              {
                if (heartRateList.ContainsKey(key7))
                {
                  key6 = key7;
                  break;
                }
                key7 -= TimeSpan.FromSeconds(1.0);
              }
            }
            else
              key6 = key5;
            if (!galvanicList.ContainsKey(key5))
            {
              DateTime key8 = key5;
              while (key8 >= key5 - TimeSpan.FromSeconds(10.0))
              {
                if (galvanicList.ContainsKey(key8))
                {
                  num1 = galvanicList[key8];
                  break;
                }
                key8 -= TimeSpan.FromSeconds(1.0);
              }
            }
            else
              num1 = galvanicList[key5];
            if (!stepsList.ContainsKey(key5))
            {
              DateTime key9 = key5;
              while (key9 >= key5 - TimeSpan.FromSeconds(50.0))
              {
                if (stepsList.ContainsKey(key9))
                {
                  num2 = stepsList[key9];
                  break;
                }
                key9 -= TimeSpan.FromSeconds(1.0);
              }
            }
            else
              num2 = stepsList[key5];
            if (!temperatureList.ContainsKey(key5))
            {
              DateTime key10 = key5;
              while (key10 >= key5 - TimeSpan.FromSeconds(10.0))
              {
                if (temperatureList.ContainsKey(key10))
                {
                  num5 = temperatureList[key10];
                  break;
                }
                key10 -= TimeSpan.FromSeconds(1.0);
              }
            }
            else
              num5 = temperatureList[key5];
            if (num2 > num3)
            {
              if (num3 > 0U)
                num4 = (uint) ((double) (num2 - num3) / ((key5 - dateTime).TotalSeconds / 60.0));
              num3 = num2;
              dateTime = key5;
            }
            if (heartRateList.Count > 0 && heartRateList.ContainsKey(key6))
              currenWorkout.LastHR = heartRateList[key6];
            currenWorkout.TrackPoints.Add(new WorkoutPoint()
            {
              Time = key5,
              Position = positionList[key5],
              Elevation = elevationList[key5],
              HeartRateBpm = currenWorkout.LastHR,
              GalvanicSkinResponse = num1,
              SkinTemperature = num5,
              Cadence = num4
            });
          }
        }
      }
      return true;
    }

    public bool AddTcxExport(ref List<Workout> Workouts, ExportType type)
    {
      Tcx tcx1 = new Tcx();
      XmlDocument xmlDocument = new XmlDocument();
      bool flag = Workouts.Count > 0;
      try
      {
        foreach (Workout workout1 in Workouts)
        {
          if ((workout1.Type == EventType.Running || workout1.Type == EventType.Hike || workout1.Type == EventType.Sleeping || workout1.Type == EventType.Biking) && workout1.TrackPoints.Count > 0)
          {
            ExportType exportType1 = type;
            ExportType exportType2;
            switch (workout1.Type)
            {
              case EventType.Running:
              case EventType.Hike:
                exportType2 = exportType1 & (ExportType.HeartRate | ExportType.Cadence | ExportType.Temperature | ExportType.GalvanicSkinResponse);
                break;
              case EventType.Biking:
                exportType2 = exportType1 & (ExportType.HeartRate | ExportType.Temperature | ExportType.GalvanicSkinResponse);
                break;
              default:
                exportType2 = exportType1 & ExportType.HeartRate;
                break;
            }
            TrainingCenterDatabase_t data = new TrainingCenterDatabase_t()
            {
              Activities = new ActivityList_t()
            };
            data.Activities.Activity = new Activity_t[1];
            data.Activities.Activity[0] = new Activity_t();
            data.Activities.Activity[0].Id = workout1.StartTime;
            data.Activities.Activity[0].Notes = workout1.Notes;
            data.Activities.Activity[0].Sport = workout1.Type == EventType.Biking ? Sport_t.Biking : Sport_t.Running;
            data.Activities.Activity[0].Lap = new ActivityLap_t[1];
            data.Activities.Activity[0].Lap[0] = new ActivityLap_t();
            if (workout1.Summary != null)
            {
              data.Activities.Activity[0].Sport = Sport_t.Running;
              ResourceLoader viewIndependentUse = ResourceLoader.GetForViewIndependentUse();
              string str1;
              if (workout1.Type == EventType.Biking)
              {
                str1 = viewIndependentUse.GetString("WorkoutBiking");
                data.Activities.Activity[0].Sport = Sport_t.Biking;
              }
              else
                str1 = workout1.Type != EventType.Hike ? (workout1.Summary.HFAverage > 120 ? (workout1.Summary.HFAverage >= 140 ? (workout1.Summary.HFAverage >= 145 ? (workout1.Summary.HFAverage >= 151 ? (workout1.Summary.HFAverage >= 160 ? viewIndependentUse.GetString("WorkoutMaximum") : viewIndependentUse.GetString("WorkoutHard")) : viewIndependentUse.GetString("WorkoutModerate")) : viewIndependentUse.GetString("WorkoutLight")) : viewIndependentUse.GetString("WorkoutWarmup")) : viewIndependentUse.GetString("WorkoutWalking")) : viewIndependentUse.GetString("WorkoutHiking");
              if ((type & ExportType.HeartRate) == ExportType.HeartRate)
              {
                data.Activities.Activity[0].Lap[0].AverageHeartRateBpm = new HeartRateInBeatsPerMinute_t();
                data.Activities.Activity[0].Lap[0].AverageHeartRateBpm.Value = (byte) workout1.Summary.HFAverage;
                data.Activities.Activity[0].Lap[0].MaximumHeartRateBpm = new HeartRateInBeatsPerMinute_t();
                data.Activities.Activity[0].Lap[0].MaximumHeartRateBpm.Value = (byte) workout1.Summary.HFMax;
              }
              data.Activities.Activity[0].Lap[0].MaximumSpeed = workout1.Summary.MaximumSpeed;
              data.Activities.Activity[0].Lap[0].MaximumSpeedSpecified = true;
              data.Activities.Activity[0].Lap[0].TotalTimeSeconds = workout1.Summary.Duration;
              data.Activities.Activity[0].Lap[0].Calories = (ushort) workout1.Summary.CaloriesBurned;
              data.Activities.Activity[0].Lap[0].DistanceMeters = workout1.Summary.Distance;
              data.Activities.Activity[0].Lap[0].Intensity = Intensity_t.Active;
              double num1 = 1000.0 / (workout1.Summary.Distance / workout1.Summary.Duration) / 60.0;
              double num2 = num1 % 1.0;
              double num3 = 0.6 * num2;
              double num4 = num1 - num2 + num3;
              Workout workout2 = workout1;
              string[] strArray = new string[15];
              DateTime startTime = workout1.StartTime;
              strArray[0] = startTime.Year.ToString("D4", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              startTime = workout1.StartTime;
              strArray[1] = startTime.Month.ToString("D2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              startTime = workout1.StartTime;
              strArray[2] = startTime.Day.ToString("D2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              strArray[3] = "_";
              startTime = workout1.StartTime;
              strArray[4] = startTime.Hour.ToString("D2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              startTime = workout1.StartTime;
              int num5 = startTime.Minute;
              strArray[5] = num5.ToString("D2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              strArray[6] = "_";
              strArray[7] = str1;
              strArray[8] = "_";
              strArray[9] = (workout1.Summary.Distance / 1000.0).ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              strArray[10] = "_";
              strArray[11] = num4.ToString("F2", (IFormatProvider) WorkoutDataSource.AppCultureInfo);
              strArray[12] = "_";
              num5 = workout1.Summary.HFAverage;
              strArray[13] = num5.ToString("F0");
              strArray[14] = ".tcx";
              string str2 = string.Concat(strArray);
              workout2.Filename = str2;
            }
            else
              data.Activities.Activity[0].Sport = workout1.Type == EventType.Biking ? Sport_t.Biking : Sport_t.Running;
            data.Activities.Activity[0].Lap[0].StartTime = workout1.StartTime;
            data.Activities.Activity[0].Lap[0].TriggerMethod = TriggerMethod_t.Manual;
            data.Activities.Activity[0].Lap[0].Track = new Trackpoint_t[workout1.TrackPoints.Count];
            int index = 0;
            foreach (WorkoutPoint trackPoint in workout1.TrackPoints)
            {
              data.Activities.Activity[0].Lap[0].Track[index] = new Trackpoint_t();
              data.Activities.Activity[0].Lap[0].Track[index].Time = trackPoint.Time;
              data.Activities.Activity[0].Lap[0].Track[index].SensorState = SensorState_t.Present;
              data.Activities.Activity[0].Lap[0].Track[index].SensorStateSpecified = true;
              if ((type & ExportType.HeartRate) == ExportType.HeartRate)
              {
                data.Activities.Activity[0].Lap[0].Track[index].HeartRateBpm = new HeartRateInBeatsPerMinute_t();
                data.Activities.Activity[0].Lap[0].Track[index].HeartRateBpm.Value = (byte) trackPoint.HeartRateBpm;
              }
              if ((type & ExportType.Cadence) == ExportType.Cadence && workout1.Type != EventType.Biking)
              {
                data.Activities.Activity[0].Lap[0].Track[index].Cadence = (byte) trackPoint.Cadence;
                data.Activities.Activity[0].Lap[0].Track[index].CadenceSpecified = true;
              }
              data.Activities.Activity[0].Lap[0].Track[index].AltitudeMeters = trackPoint.Elevation;
              data.Activities.Activity[0].Lap[0].Track[index].AltitudeMetersSpecified = true;
              data.Activities.Activity[0].Lap[0].Track[index].Position = new Position_t();
              data.Activities.Activity[0].Lap[0].Track[index].Position.LatitudeDegrees = trackPoint.Position.LatitudeDegrees;
              data.Activities.Activity[0].Lap[0].Track[index].Position.LongitudeDegrees = trackPoint.Position.LongitudeDegrees;
              ++index;
            }
            string tcx2 = tcx1.GenerateTcx(data);
            if (tcx2 != null && tcx2.Length > 0)
              workout1.TCXBuffer = tcx2.Replace("\"utf-16\"", "\"UTF-8\"");
          }
        }
      }
      catch (Exception ex)
      {
        flag = false;
      }
      return flag;
    }

    public static string ToBinaryString(uint num) => Convert.ToString((long) num, 2).PadLeft(16, '0');

    public List<SensorLogSequence> Sequences { get; }

    public ulong BufferSize { get; set; }

    public ulong StepLength { get; internal set; }

    public string BandName { get; set; }
  }
}
