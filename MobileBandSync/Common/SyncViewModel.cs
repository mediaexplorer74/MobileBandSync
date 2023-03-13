// SyncViewModel.cs
// Type: MobileBandSync.Common.SyncViewModel
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.Data;
using MobileBandSync.MSFTBandLib;
using MobileBandSync.MSFTBandLib.UWP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Core;

namespace MobileBandSync.Common
{
  public class SyncViewModel : INotifyPropertyChanged
  {
    private bool _bEnabled = true;
    private bool _bCleanupSensorLog = true;
    private bool _bStoreSensorLogLocally;
    private double _dProgress;
    private string _strStatusText = "";
    private string _strConnectionText = "";
    private string _strDeviceText = "";
    private string _strConnectionLog = "";
    private BandClientUWP _bandClient = new BandClientUWP();

    public event PropertyChangedEventHandler PropertyChanged = (_param1, _param2) => { };

    public SyncViewModel()
    {
        // Сводка:
        //     Возвращает ResourceLoader для поддерева Resources основного объекта ResourceMap
        //     текущего выполняющегося приложения. Этот ResourceLoader использует контекст по
        //     умолчанию, не связанный ни с каким представлением.
        //
        // Возврат:
        //     Загрузчик ресурсов для поддерева Resources основного объекта ResourceMap текущего
        //     выполняющегося приложения. Этот ResourceLoader использует контекст по умолчанию,
        //     не связанный ни с каким представлением. Невозможно использовать этот ResourceLoader
        //     для извлечения любого ресурса, у которого имеются кандидаты ресурсов, пригодные
        //     для масштабирования.

        //this.ResourceLoader = ResourceLoader.GetForViewIndependentUse();

      this.Enabled = false;
      this.Connected = false;

        //FIXIT
        this.ConnectionText = "NotConnected";//this.ResourceLoader.GetString("NotConnected");

      this.DeviceText = "";
      this.StatusText = "";
      this.SyncProgress = 0.0;
    }

        //FIXIT
    //public ResourceLoader ResourceLoader { get; private set; }

    public bool Enabled
    {
      get => this._bEnabled;
      set
      {
        this._bEnabled = value;
        this.OnPropertyChanged(nameof (Enabled));
      }
    }

    public double SyncProgress
    {
      get => this._dProgress;
      set
      {
        if (this._dProgress == value)
          return;
        this._dProgress = value;
        this.OnPropertyChanged(nameof (SyncProgress));
      }
    }

    public string StatusText
    {
      get => this._strStatusText;
      set
      {
        this._strStatusText = value;
        this.OnPropertyChanged(nameof (StatusText));
      }
    }

    public string ConnectionText
    {
      get => this._strConnectionText;
      set
      {
        this._strConnectionText = value;
        this.OnPropertyChanged(nameof (ConnectionText));
      }
    }

    public string ConnectionLog
    {
      get => this._strConnectionLog;
      set
      {
        this._strConnectionLog = value;
        this.OnPropertyChanged(nameof (ConnectionLog));
      }
    }

    public string DeviceText
    {
      get => this._strDeviceText;
      set
      {
        this._strDeviceText = value;
        this.OnPropertyChanged(nameof (DeviceText));
      }
    }

    public bool CleanupSensorLog
    {
      get => this._bCleanupSensorLog;
      set
      {
        this._bCleanupSensorLog = value;
        this.OnPropertyChanged(nameof (CleanupSensorLog));
      }
    }

    public bool StoreSensorLogLocally
    {
      get => this._bStoreSensorLogLocally;
      set
      {
        this._bStoreSensorLogLocally = value;
        this.OnPropertyChanged(nameof (StoreSensorLogLocally));
      }
    }

    public BandClientUWP BandClient => this._bandClient;

    public void OnPropertyChanged(string propertyName = null)
    {
        this.PropertyChanged((object)this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task<bool> StartDeviceSearch()
    {
      this.Connected = false;

      //FIXIT
      this.ConnectionText = "NotConnected";//this.ResourceLoader.GetString("NotConnected");

      this.DeviceText = "";
      this.StatusText = "";
      this.SyncProgress = 0.0;
      this.Enabled = false;
      if (this.BandClient != null)
      {
        try
        {
          List<BandInterface> pairedBands = await this.BandClient.GetPairedBands();

            if (pairedBands.Count > 0)
            {
               //FIXIT
               this.ConnectionText = "SearchingDevice";//this.ResourceLoader.GetString("SearchingDevice");
            }
          
          this.CurrentBand = (BandInterface) null;
          foreach (BandInterface band in pairedBands)
          {
            try
            {
              await band.Connect(new Action<ulong, ulong>(this.Progress));
              this.CurrentBand = band;
              this.Connected = true;
              break;
            }
            catch (Exception ex)
            {
              this.CurrentBand = (BandInterface) null;
            }
          }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        if (this.CurrentBand != null)
        {
          // FIXIT
          string str = "Connected";//this.ResourceLoader.GetString("Connected");

          string serialNumber = await this.CurrentBand.GetSerialNumber();
          this.ConnectionText = str + ": #" + serialNumber;
          str = (string) null;
          WorkoutDataSource.BandName = this.DeviceText = this.CurrentBand.GetName();
          this.Connected = true;
          this.Enabled = true;
        }
        else
        {
          this.CurrentBand = (BandInterface) new Band<BandSocketUWP>("", "");

          this.ConnectionText = "NotConnected";//this.ResourceLoader.GetString("NotConnected");
        }
      }
      return this.Connected;
    }

    public void Report(string strReport)
    {
      if (strReport != null && strReport.Length > 0)
        this.ConnectionLog += strReport;
      this.ConnectionLog += Environment.NewLine;
    }

    public void Status(string strStatus)
    {
      if (strStatus == null || strStatus.Length <= 0)
        return;
      this.StatusText = strStatus;
    }

    public async Task<IEnumerable<WorkoutItem>> StartDeviceSync()
    {
      if (this.Connected && this.CurrentBand != null)
      {
        this.Enabled = false;
        //FIXIT
        this.StatusText = "Downloading";//this.ResourceLoader.GetString("Downloading");
        WorkoutDataSource.BandName = this.CurrentBand.GetName();

        byte[] sensorLog = await this.CurrentBand.GetSensorLog(new Action<string>(this.Report), 
            new Action<ulong, ulong>(this.Progress), this.CleanupSensorLog, this.StoreSensorLogLocally);

        if (sensorLog != null)
        {
          this.Report((string) null);

          //FIXIT
          this.StatusText = "Importing";//this.ResourceLoader.GetString("Importing");
          try
          {
            int length = sensorLog.Length;
            List<WorkoutItem> workouts = await WorkoutDataSource.ImportFromSensorlog(sensorLog, 
                new Action<string>(this.Status), new Action<ulong, ulong>(this.Progress));

            if (this.CurrentBand != null)
            {
              if (workouts.Count > 0)
              {
                                //FIXIT
                                this.StatusText = "Storing" + " " //this.ResourceLoader.GetString("Storing") + " " 
                                + (object)workouts.Count + " " + (workouts.Count == 1
                                ? "Workout"//(object) this.ResourceLoader.GetString("Workout")
                                : "Workouts");//(object) this.ResourceLoader.GetString("Workouts"));

                ulong stepLength = WorkoutDataSource.DataSource.SensorLogEngine.StepLength;

                List<long> longList = await WorkoutDataSource.StoreWorkouts(
                    workouts, new Action<ulong, ulong>(this.Progress), stepLength);

                IEnumerable<WorkoutItem> workoutsAsync = await WorkoutDataSource.GetWorkoutsAsync(true);
                this.Report((string) null);
                this.Progress(0UL, 0UL);
                this.StatusText = "";
                this.Enabled = true;
                return workoutsAsync;
              }
            }
          }
          catch (Exception ex)
          {
            Debug.WriteLine(ex.Message);
          }
        }
      }
      this.Report((string) null);
      this.Progress(0UL, 0UL);
      this.StatusText = "";
      this.Enabled = true;
      return (IEnumerable<WorkoutItem>) null;
    }

    public async Task<IEnumerable<WorkoutItem>> StartSyncFromLogs()
    {
      this.Enabled = false;
      WorkoutDataSource.BandName = "Virtual Test Band";
      List<WorkoutItem> listWorkouts = new List<WorkoutItem>();
      StorageFolder folderAsync = await KnownFolders.DocumentsLibrary.CreateFolderAsync("SensorLog", 
          (CreationCollisionOption) 3);
      if (folderAsync != null)
      {
        try
        {
          this.StatusText = "Reading from sensor log";
          listWorkouts = await WorkoutDataSource.ImportFromSensorlog(folderAsync, 
              new Action<string>(this.Status), new Action<ulong, ulong>(this.Progress));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
      }
      if (listWorkouts.Count > 0)
      {
        this.Progress(0UL, WorkoutDataSource.DataSource.SensorLogEngine.BufferSize);
        this.StatusText = "Storing " + (object) listWorkouts.Count + (listWorkouts.Count == 1 ? (object) " workout" : (object) " workouts");
        List<long> longList = await WorkoutDataSource.StoreWorkouts(listWorkouts, new Action<ulong, ulong>(this.Progress), WorkoutDataSource.DataSource.SensorLogEngine.StepLength);
      }
      this.Report((string) null);
      this.Progress(0UL, 0UL);
      this.StatusText = "";
      this.Enabled = true;
      IEnumerable<WorkoutItem> workoutItems;
      if (listWorkouts.Count > 0)
        workoutItems = await WorkoutDataSource.GetWorkoutsAsync(true);
      else
        workoutItems = (IEnumerable<WorkoutItem>) null;
      return workoutItems;
    }

    public async void Progress(ulong uiCompleted, ulong uiTotal)
    {
      if (uiTotal > 0UL && (long) this.TotalProgress != (long) uiTotal)
      {
        this.TotalProgress = uiTotal;
        this.CompletedProgress = 0UL;
        this.SyncProgress = 0.0;
      }
      if (this.TotalProgress == 0UL)
        return;
      if (uiCompleted == 0UL && uiTotal == 0UL)
      {
        this.SyncProgress = 0.0;
        this.TotalProgress = 0UL;
        this.CompletedProgress = 0UL;
      }
      else
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        //SyncViewModel.\u003C\u003Ec__DisplayClass40_0 cDisplayClass400 = new SyncViewModel.\u003C\u003Ec__DisplayClass40_0();
        // ISSUE: reference to a compiler-generated field
        //cDisplayClass400.\u003C\u003E4__this = this;
        this.CompletedProgress = Math.Min(this.TotalProgress, this.CompletedProgress + uiCompleted);
        // ISSUE: reference to a compiler-generated field
        //cDisplayClass400.uiVal = (float) (this.CompletedProgress * 100UL) / (float) this.TotalProgress;
        // ISSUE: reference to a compiler-generated field
        //if ((double) cDisplayClass400.uiVal < this._dProgress + 1.0)
        //  return;
        // ISSUE: method pointer
        //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync((CoreDispatcherPriority) 0, 
        //    new DispatchedHandler((object) cDisplayClass400, __methodptr(\u003CProgress\u003Eb__0)));
      }
    }

    public bool Connected { get; set; }

    public BandInterface CurrentBand { get; set; }

    public ulong TotalProgress { get; private set; }

    public ulong CompletedProgress { get; private set; }
  }
}
