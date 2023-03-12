// HubPage.cs
// Type: MobileBandSync.HubPage
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MobileBandSync.Common;
using MobileBandSync.Data;

using System.CodeDom.Compiler;

using System.Diagnostics;

using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Maps;

using Windows.UI.Xaml.Markup;

namespace MobileBandSync
{
  public sealed partial class HubPage : Page
  {
    private readonly NavigationHelper navigationHelper;
    private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
    private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");
    public WorkoutData PageWorkoutData = new WorkoutData();
   
    //private DatePicker startDatePicker;
   
    //private DatePicker endDatePicker;
   
    //private MapControl MapPicker;
   
    //private CheckBox chkMap;
   
    //private CheckBox chkWalk;
    
    //private CheckBox chkSleep;
    
    //private CheckBox chkRun;
    
    //private CheckBox chkBike;
    
    //private Grid LayoutRoot;
    
    //private Hub Hub;

    public HubPage()
    {
      this.InitializeComponent();
      //DisplayInformation.AutoRotationPreferences((DisplayOrientations) 2);
      //this.NavigationCacheMode((NavigationCacheMode) 1);
      this.navigationHelper = new NavigationHelper((Page) this);
      this.navigationHelper.LoadState += new LoadStateEventHandler(this.NavigationHelper_LoadState);
      this.navigationHelper.SaveState += new SaveStateEventHandler(this.NavigationHelper_SaveState);
      this.SyncView = new SyncViewModel();
      this.MapServiceToken = "AobMbD2yKlST1QB_mh1mPfpnJGDtpm0lefHMTVPqU0NQR58-xEVO3KhAaOaqJL6y";
      WorkoutDataSource.SetMapServiceToken(this.MapServiceToken);
    }

    public NavigationHelper NavigationHelper => this.navigationHelper;

    public ObservableDictionary DefaultViewModel => this.defaultViewModel;

    public SyncViewModel SyncView { get; set; }

    public DispatcherTimer DeviceTimer { get; private set; }

    public string MapServiceToken { get; private set; }

    public bool FilterAccepted { get; private set; }

    public bool MapPickerInitialized { get; private set; }

    public ToggleButton ToggleFilter { get; private set; }

    private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
    {
      if (this.DefaultViewModel.ContainsKey("WorkoutData"))
        this.DefaultViewModel.Remove("WorkoutData");
      WorkoutData workoutData = this.PageWorkoutData;
      IEnumerable<WorkoutItem> workoutsAsync = await WorkoutDataSource.GetWorkoutsAsync(true);
      workoutData.Workouts = workoutsAsync;
      workoutData = (WorkoutData) null;
      workoutData = this.PageWorkoutData;
      string workoutSummaryAsync = await WorkoutDataSource.GetWorkoutSummaryAsync();
      workoutData.WorkoutTitle = workoutSummaryAsync;
      workoutData = (WorkoutData) null;
      this.DefaultViewModel["WorkoutData"] = (object) this.PageWorkoutData;
      if (this.DefaultViewModel.ContainsKey("SyncView"))
        return;
      this.SyncView.Enabled = false;
      this.SyncView.Connected = false;
      this.DefaultViewModel["SyncView"] = (object) this.SyncView;
      this.SyncView.ConnectionText = "Disconnected";
      this.SyncView.DeviceText = "";
      this.SyncView.StatusText = "";
      this.SyncView.ConnectionLog = "";
      if (await this.SyncView.StartDeviceSearch())
        return;
      this.DeviceTimer = new DispatcherTimer();

      this.DeviceTimer.Interval = (new TimeSpan(0, 0, 10));

      DispatcherTimer deviceTimer = this.DeviceTimer;
      
      //WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>
      //          (new Func<EventHandler<object>, EventRegistrationToken>(deviceTimer.add_Tick), 
      //          new Action<EventRegistrationToken>(deviceTimer.remove_Tick), 
      //          new EventHandler<object>(this.OnDeviceTimer));
      
      this.DeviceTimer.Start();
    }

    private async void OnDeviceTimer(object sender, object e)
    {
      this.DeviceTimer.Stop();
      if (await this.SyncView.StartDeviceSearch())
        return;
      this.DeviceTimer.Start();
    }

    private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
    {
    }

    private void WorkoutItem_ItemClick(object sender, ItemClickEventArgs e)
    {
      try
      {
        if (e.ClickedItem == null)
          return;
        int workoutId = ((WorkoutItem) e.ClickedItem).WorkoutId;
        if (!(e.ClickedItem is WorkoutItem clickedItem))
          return;
        Type type = typeof (SectionPage);
        if (clickedItem.WorkoutType == (byte) 21)
          type = typeof (SleepPage);
        this.Frame.Navigate(type, (object) workoutId);
      }
      catch
      {
      }
    }

    private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (!this.Frame.Navigate(typeof (ItemPage), (object) ((TrackItem) e.ClickedItem).UniqueId))
        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
    }

    public async void btnSync_Click(object sender, RoutedEventArgs e)
    {
      IEnumerable<WorkoutItem> workoutItems = await this.SyncView.StartDeviceSync();
      this.PageWorkoutData.Workouts = (IEnumerable<WorkoutItem>) WorkoutDataSource.GetWorkouts();
      WorkoutData workoutData = this.PageWorkoutData;
      string workoutSummaryAsync = await WorkoutDataSource.GetWorkoutSummaryAsync();
      workoutData.WorkoutTitle = workoutSummaryAsync;
      workoutData = (WorkoutData) null;
      if (this.PageWorkoutData.Workouts == null)
        return;
      if (this.DefaultViewModel.ContainsKey("WorkoutData"))
        this.DefaultViewModel.Remove("WorkoutData");
      this.DefaultViewModel["WorkoutData"] = (object) this.PageWorkoutData;
    }

    //protected virtual void OnNavigatedTo(NavigationEventArgs e) => this.navigationHelper.OnNavigatedTo(e);

    //protected virtual void OnNavigatedFrom(NavigationEventArgs e)  => this.navigationHelper.OnNavigatedFrom(e);

    private async void BackupDatabase_Tapped(object sender, TappedRoutedEventArgs e)
    {
      FolderPicker folderPicker = new FolderPicker();
      //folderPicker.SuggestedStartLocation((PickerLocationId) 0);
      folderPicker.FileTypeFilter.Add("*");
      StorageFolder targetFolder = await folderPicker.PickSingleFolderAsync();
      if (targetFolder == null)
        return;
      int num = await WorkoutDataSource.BackupDatabase(targetFolder) ? 1 : 0;
    }

    private async void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
      Flyout resource = ((IDictionary<object, object>) 
                ((FrameworkElement) this).Resources)[(object) "MyFlyout"] as Flyout;
      if (!(sender is ToggleButton toggleButton))
        return;
      this.ToggleFilter = toggleButton;
      if ((DateTimeOffset) DateTime.Now - this.startDatePicker.Date < new TimeSpan(1, 0, 0))
        this.startDatePicker.Date = ((DateTimeOffset) (DateTime.Now - new TimeSpan(5475, 0, 0, 0, 0)));

      if ((DateTimeOffset) DateTime.Now - this.endDatePicker.Date < new TimeSpan(1, 0, 0))
        this.endDatePicker.Date = ((DateTimeOffset) DateTime.Now);
      ((FlyoutBase) resource).ShowAt((FrameworkElement) toggleButton);
      if (this.MapPickerInitialized)
        return;
      this.MapPickerInitialized = true;
      try
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        //HubPage.\u003C\u003Ec__DisplayClass42_0 cDisplayClass420 = 
        //            new HubPage.\u003C\u003Ec__DisplayClass42_0();
        
        // ISSUE: reference to a compiler-generated field
        //cDisplayClass420.\u003C\u003E4__this = this;
        
        Geolocator geolocator = new Geolocator();
        //geolocator.DesiredAccuracyInMeters(new uint?(500U));

        // ISSUE: reference to a compiler-generated field
        //Geoposition pos = cDisplayClass420.pos;
        Geoposition geopositionAsync = await geolocator.GetGeopositionAsync();

        // ISSUE: reference to a compiler-generated field
        //cDisplayClass420.pos = geopositionAsync;

        // ISSUE: method pointer
        //((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 1, 
        //    new DispatchedHandler((object) cDisplayClass420, 
        //    __methodptr(\u003CToggleButton_Checked\u003Eb__0)));

        //cDisplayClass420 = (HubPage.\u003C\u003Ec__DisplayClass42_0) null;
      }
      catch
      {
                //
      }
    }

    private async void ButtonOK_Tapped(object sender, TappedRoutedEventArgs e)
    {
      this.FilterAccepted = true;

      ((FlyoutBase) (((IDictionary<object, object>) ((FrameworkElement) this)
                .Resources)[(object) "MyFlyout"] as Flyout)).Hide();

      WorkoutDataSource dataSource = WorkoutDataSource.DataSource;
      WorkoutFilterData workoutFilterData = new WorkoutFilterData();
      DateTimeOffset date1 = this.startDatePicker.Date;
      workoutFilterData.Start = this.startDatePicker.Date.DateTime;
      DateTimeOffset date2 = this.endDatePicker.Date;
      workoutFilterData.End = this.endDatePicker.Date.DateTime;
      workoutFilterData.IsBikingWorkout = ((ToggleButton) this.chkBike).IsChecked;
      workoutFilterData.IsRunningWorkout = ((ToggleButton) this.chkRun).IsChecked;
      workoutFilterData.IsWalkingWorkout = ((ToggleButton) this.chkWalk).IsChecked;
      workoutFilterData.IsSleepingWorkout = ((ToggleButton) this.chkSleep).IsChecked;
      dataSource.CurrentFilter = workoutFilterData;
      bool? isChecked = ((ToggleButton) this.chkMap).IsChecked;
      bool flag = true;
      if ((isChecked.GetValueOrDefault() == flag ? (isChecked.HasValue ? 1 : 0) : 0) != 0)
      {
        WorkoutDataSource.DataSource.CurrentFilter.SetBounds(this.MapPicker);
        WorkoutDataSource.DataSource.CurrentFilter.MapSelected = true;
      }
      else
        WorkoutDataSource.DataSource.CurrentFilter.SetBounds((MapControl) null);
      WorkoutData workoutData = this.PageWorkoutData;
      IEnumerable<WorkoutItem> workoutsAsync = 
                await WorkoutDataSource.GetWorkoutsAsync(true, WorkoutDataSource.DataSource.CurrentFilter);
      workoutData.Workouts = workoutsAsync;
      workoutData = (WorkoutData) null;
      workoutData = this.PageWorkoutData;
      string workoutSummaryAsync = await WorkoutDataSource.GetWorkoutSummaryAsync();
      workoutData.WorkoutTitle = workoutSummaryAsync;
      workoutData = (WorkoutData) null;
      if (this.DefaultViewModel.ContainsKey("WorkoutData"))
        this.DefaultViewModel.Remove("WorkoutData");
      this.DefaultViewModel["WorkoutData"] = (object) this.PageWorkoutData;
    }

    private void ButtonCancel_Tapped(object sender, TappedRoutedEventArgs e) 
            => ((FlyoutBase) (((IDictionary<object, object>) 
            ((FrameworkElement) this).Resources)[(object) "MyFlyout"] as Flyout)).Hide();

    private async void ToggleFilter_Unchecked(object sender, RoutedEventArgs e)
    {
      WorkoutDataSource.DataSource.CurrentFilter = (WorkoutFilterData) null;
      WorkoutData workoutData = this.PageWorkoutData;
      IEnumerable<WorkoutItem> workoutsAsync = await WorkoutDataSource.GetWorkoutsAsync(true);
      workoutData.Workouts = workoutsAsync;
      workoutData = (WorkoutData) null;
      workoutData = this.PageWorkoutData;
      string workoutSummaryAsync = await WorkoutDataSource.GetWorkoutSummaryAsync();
      workoutData.WorkoutTitle = workoutSummaryAsync;
      workoutData = (WorkoutData) null;
      if (this.DefaultViewModel.ContainsKey("WorkoutData"))
        this.DefaultViewModel.Remove("WorkoutData");
      this.DefaultViewModel["WorkoutData"] = (object) this.PageWorkoutData;
    }

    private async void Flyout_Closed(object sender, object e)
    {
      if (!this.FilterAccepted)
      {
        if (this.ToggleFilter != null)
          this.ToggleFilter.IsChecked = (new bool?(false));

        WorkoutDataSource.DataSource.CurrentFilter = (WorkoutFilterData) null;
        WorkoutData workoutData = this.PageWorkoutData;
        IEnumerable<WorkoutItem> workoutsAsync = await WorkoutDataSource.GetWorkoutsAsync(true);
        workoutData.Workouts = workoutsAsync;
        workoutData = (WorkoutData) null;
        workoutData = this.PageWorkoutData;
        string workoutSummaryAsync = await WorkoutDataSource.GetWorkoutSummaryAsync();
        workoutData.WorkoutTitle = workoutSummaryAsync;
        workoutData = (WorkoutData) null;
        if (this.DefaultViewModel.ContainsKey("WorkoutData"))
          this.DefaultViewModel.Remove("WorkoutData");
        this.DefaultViewModel["WorkoutData"] = (object) this.PageWorkoutData;
      }
      this.FilterAccepted = false;
    }

    private async void PlusButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      FileOpenPicker fileOpenPicker = new FileOpenPicker();
      //fileOpenPicker.SuggestedStartLocation((PickerLocationId) 0);
      fileOpenPicker.FileTypeFilter.Add(".tcx");
      StorageFile storageFile = await fileOpenPicker.PickSingleFileAsync();
      if (storageFile == null)
        return;
      List<WorkoutItem> listWorkouts = new List<WorkoutItem>();
      WorkoutItem workoutItem = await WorkoutItem.ReadWorkoutFromTcx(storageFile.Path);
      if (workoutItem != null)
      {
        listWorkouts.Add(workoutItem);
        List<long> longList = await WorkoutDataSource.StoreWorkouts(listWorkouts);
        WorkoutData workoutData = this.PageWorkoutData;
        IEnumerable<WorkoutItem> workoutsAsync = await WorkoutDataSource.GetWorkoutsAsync(true);
        workoutData.Workouts = workoutsAsync;
        workoutData = (WorkoutData) null;
        workoutData = this.PageWorkoutData;
        string workoutSummaryAsync = await WorkoutDataSource.GetWorkoutSummaryAsync();
        workoutData.WorkoutTitle = workoutSummaryAsync;
        workoutData = (WorkoutData) null;
        if (this.DefaultViewModel.ContainsKey("WorkoutData"))
          this.DefaultViewModel.Remove("WorkoutData");
        this.DefaultViewModel["WorkoutData"] = (object) this.PageWorkoutData;
      }
      listWorkouts = (List<WorkoutItem>) null;
    }

    
  }
}

