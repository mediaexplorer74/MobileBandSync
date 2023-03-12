// SectionPage.cs
// Type: MobileBandSync.SectionPage
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using MobileBandSync.Common;
using MobileBandSync.Data;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;


namespace MobileBandSync
{
    public sealed partial class SectionPage : Page//, IComponentConnector
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        public static List<MapIcon> DistanceMarkers = new List<MapIcon>();
        
        //private Page pageRoot;
        
        //private Grid DiagramGrid;
        
        //private Grid StatusGrid;
        
        //private TextBlock StatusText;
        
        //private Chart lineChart; // !
        
        //private LineSeries heartLine; // !
        
        //private MapControl WorkoutMap;
        
        
        public SectionPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper((Page)this);

            this.navigationHelper.LoadState += new LoadStateEventHandler(this.NavigationHelper_LoadState);
            this.navigationHelper.SaveState += new SaveStateEventHandler(this.NavigationHelper_SaveState);

            MapControl workoutMap = this.WorkoutMap;

            // ISSUE: method pointer
            //WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<MapControl, object>>(
            //    new Func<TypedEventHandler<MapControl, object>, EventRegistrationToken>(
            //        workoutMap.add_LoadingStatusChanged), new Action<EventRegistrationToken>(
            //            workoutMap.remove_LoadingStatusChanged), 
            //    new TypedEventHandler<MapControl, object>((object)this, 
            //    __methodptr(Map_LoadingStatusChanged)));

            this.CancelTokenSource = new CancellationTokenSource();
            this.Viewport = (GeoboundingBox)null;
            this.ViewInitialized = false;
            //this.WorkoutMap.MapServiceToken(WorkoutDataSource.GetMapServiceToken());
        }

        public NavigationHelper NavigationHelper => this.navigationHelper;

        public ObservableDictionary DefaultViewModel => this.defaultViewModel;

        public int currentWorkoutId { get; private set; }

        public GeoboundingBox Viewport { get; private set; }

        public bool MapInitialized { get; private set; }

        public CancellationTokenSource CancelTokenSource { get; private set; }

        public WorkoutItem CurrentWorkout { get; private set; }

        public bool ViewInitialized { get; private set; }

        public Line chartLine { get; private set; }

        public MapIcon PosNeedleIcon { get; private set; }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.currentWorkoutId = (int)e.NavigationParameter;
            WorkoutItem workoutAsync = await WorkoutDataSource.GetWorkoutAsync(this.currentWorkoutId);
            if (workoutAsync.Items.Count == 0)
            {
                this.CancelTokenSource.Dispose();
                this.CancelTokenSource = new CancellationTokenSource();
            }
            workoutAsync.Modified = false;
            await this.ShowWorkout(workoutAsync);
        }

        private async Task ShowWorkout(WorkoutItem workout)
        {
            if (workout == null)
                return;
            try
            {
                this.MapInitialized = false;
                this.ViewInitialized = false;

                //this.StatusText.Text("");
                //((UIElement)this.StatusGrid).Visibility((Visibility)1);

                this.CurrentWorkout = workout;
                this.DefaultViewModel["Workout"] = (object)workout;
                this.Viewport = new GeoboundingBox(new BasicGeoposition()
                {
                    Latitude = (double)(workout.LatitudeStart + (long)workout.LatDeltaRectNE)/10000000.0,
                    Longitude = (double)(workout.LongitudeStart + (long)workout.LongDeltaRectSW)/10000000.0
                }, new BasicGeoposition()
                {
                    Latitude = (double)(workout.LatitudeStart + (long)workout.LatDeltaRectSW)/10000000.0,
                    Longitude = (double)(workout.LongitudeStart + (long)workout.LongDeltaRectNE)/10000000.0
                }, (AltitudeReferenceSystem)1);
                if (this.CurrentWorkout == null)
                    return;
                this.CancelTokenSource.Dispose();
                this.CancelTokenSource = new CancellationTokenSource();
                if (this.CurrentWorkout.Items == null || this.CurrentWorkout.Items.Count == 0)
                {
                    this.CurrentWorkout.TracksLoaded += 
                        new EventHandler<TracksLoadedEventArgs>(this.WorkoutTracks_Loaded);

                    this.PosNeedleIcon = (MapIcon)null;

                    ((ICollection<MapElement>)this.WorkoutMap.MapElements).Clear();

                    this.CurrentWorkout.ReadTrackData(this.CancelTokenSource.Token);
                }
                else
                {
                    this.LoadChartContents(this.CurrentWorkout);

                    await this.AddTracks(this.CurrentWorkout);
                }
            }
            catch (Exception ex)
            {
                this.PosNeedleIcon = (MapIcon)null;
                ((ICollection<MapElement>)this.WorkoutMap.MapElements).Clear();
            }
        }

        private async void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            this.CleanupChart();
            if (this.CurrentWorkout != null && this.CurrentWorkout.Items != null 
                && this.CurrentWorkout.Items.Count > 0)
            {
                this.CurrentWorkout.Items.Clear();
                this.CurrentWorkout.ElevationChart.Clear();
                this.CurrentWorkout.HeartRateChart.Clear();
                this.CurrentWorkout.CadenceNormChart.Clear();
                this.CurrentWorkout.SpeedChart.Clear();
            }
            if (!this.CurrentWorkout.Modified)
                return;
            await WorkoutDataSource.UpdateWorkoutAsync(this.CurrentWorkout.WorkoutId, 
                this.CurrentWorkout.Title, this.CurrentWorkout.Notes);
            await this.CurrentWorkout.UpdateWorkout();
        }

        public async void CreateDistancePoint(WorkoutItem item, TrackItem trackpoint, int iDistance)
        {
            if (item == null)
                return;
            try
            {
                DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
                float num1 = (float)(DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel 
                    * 2.0 / 6.0);

                int num2 = (int)((iDistance > 0 ? 50.0 : 30.0) * (double)num1);
                string str = iDistance.ToString();
                CanvasRenderTarget distanceMarker = new CanvasRenderTarget(
                    (ICanvasResourceCreator)CanvasDevice.GetSharedDevice(), (float)num2, (float)num2, 96f);

                using (CanvasDrawingSession drawingSession = distanceMarker.CreateDrawingSession())
                {
                    drawingSession.FillRectangle(0.0f, 0.0f, (float)num2, (float)num2, iDistance > 0 
                        ? Colors.DarkRed 
                        : Colors.Green);

                    drawingSession.DrawRectangle(2f * num1, 2f * num1, (float)num2 - 3f * num1,
                        (float)num2 - 3f * num1, Colors.White, 5f * num1);

                    if (iDistance > 0)
                        drawingSession.DrawText(str, (float)((str.Length > 1 ? 6.0 : 15.0) 
                            * (double)num1), (float)(1.0 * (double)num1),
                            Colors.White, new CanvasTextFormat()
                        {
                            FontSize = (float)(int)((double)num2 / 1.5),
                            FontWeight = FontWeights.Bold
                        });
                }
                MapIcon mapIcon = new MapIcon();
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    BitmapEncoder async = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, (IRandomAccessStream)stream);
                    Size size = distanceMarker.Size;
                    int width = (int)(uint)size.Width;
                    size = distanceMarker.Size;
                    int height = (int)(uint)size.Height;
                    double logicalDpi1 = (double)displayInformation.LogicalDpi;
                    double logicalDpi2 = (double)displayInformation.LogicalDpi;
                    byte[] pixelBytes = distanceMarker.GetPixelBytes();
                    async.SetPixelData((BitmapPixelFormat)87, (BitmapAlphaMode)2, (uint)width,
                        (uint)height, logicalDpi1, logicalDpi2, pixelBytes);
                    await async.FlushAsync();

                    mapIcon.Image = ((IRandomAccessStreamReference)
                        RandomAccessStreamReference.CreateFromStream((IRandomAccessStream)stream));

                    ((MapElement)mapIcon).ZIndex = (iDistance + 2);
                    mapIcon.Title = ("");
                    ((MapElement)mapIcon).Visible = (true);
                    mapIcon.NormalizedAnchorPoint = (new Point(0.5, 0.5));
                    mapIcon.Location = (new Geopoint(new BasicGeoposition()
                    {
                        Latitude = (double)(item.LatitudeStart + (trackpoint == null 
                        ? 0L
                        : (long)trackpoint.LatDelta)) / 10000000.0,
                        Longitude = (double)(item.LongitudeStart + (trackpoint == null 
                        ? 0L : (long)trackpoint.LongDelta)) / 10000000.0,
                        Altitude = 0.0
                    }));

                    ((ICollection<MapElement>)this.WorkoutMap.MapElements).Add((MapElement)mapIcon);
                    SectionPage.DistanceMarkers.Add(mapIcon);
                }
                displayInformation = (DisplayInformation)null;
                distanceMarker = (CanvasRenderTarget)null;
                mapIcon = (MapIcon)null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] Exception: " + ex.Message);
            }
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!this.Frame.Navigate(typeof(ItemPage), (object)((TrackItem)e.ClickedItem).UniqueId))
                throw new Exception(ResourceLoader.GetForCurrentView("Resources")
                    .GetString("NavigationFailedExceptionMessage"));
        }

        //protected virtual void OnNavigatedTo(NavigationEventArgs e) => this.navigationHelper.OnNavigatedTo(e);

        //protected virtual void OnNavigatedFrom(NavigationEventArgs e) => this.navigationHelper.OnNavigatedFrom(e);

        private async void Left_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.CurrentWorkout == null)
                return;
            if (this.CurrentWorkout.Modified)
            {
                await WorkoutDataSource.UpdateWorkoutAsync(this.CurrentWorkout.WorkoutId, 
                    this.CurrentWorkout.Title, this.CurrentWorkout.Notes);

                await this.CurrentWorkout.UpdateWorkout();
                this.CurrentWorkout.Modified = false;
            }
            if (this.CurrentWorkout.Items != null && this.CurrentWorkout.Items.Count > 0)
            {
                this.PosNeedleIcon = (MapIcon)null;
                ((ICollection<MapElement>)this.WorkoutMap.MapElements).Clear();
                this.CurrentWorkout.Items.Clear();
                this.CurrentWorkout.ElevationChart.Clear();
                this.CurrentWorkout.HeartRateChart.Clear();
                this.CurrentWorkout.CadenceNormChart.Clear();
                this.CurrentWorkout.SpeedChart.Clear();
            }

            if (this.chartLine != null)
                ((ICollection<UIElement>)(
                    (Panel)this.DiagramGrid).Children).Remove((UIElement)this.chartLine);

            this.CleanupChart();

            WorkoutItem prevSibling = this.CurrentWorkout.GetPrevSibling();
            if (prevSibling == null)
                return;
            this.CurrentWorkout = prevSibling;
            this.CancelTokenSource.Cancel();
            this.ShowWorkout(prevSibling);
        }

        private async void Right_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.CurrentWorkout == null)
                return;
            
            if (this.CurrentWorkout.Modified)
            {
                await WorkoutDataSource.UpdateWorkoutAsync(
                    this.CurrentWorkout.WorkoutId, this.CurrentWorkout.Title, this.CurrentWorkout.Notes);

                await this.CurrentWorkout.UpdateWorkout();

                this.CurrentWorkout.Modified = false;
            }

            if (this.CurrentWorkout.Items != null && this.CurrentWorkout.Items.Count > 0)
            {
                this.PosNeedleIcon = (MapIcon)null;
                ((ICollection<MapElement>)this.WorkoutMap.MapElements).Clear();
                this.CurrentWorkout.Items.Clear();
                this.CurrentWorkout.ElevationChart.Clear();
                this.CurrentWorkout.HeartRateChart.Clear();
                this.CurrentWorkout.CadenceNormChart.Clear();
                this.CurrentWorkout.SpeedChart.Clear();
            }

            if (this.chartLine != null)
                ((ICollection<UIElement>)((Panel)this.DiagramGrid).Children).Remove((UIElement)this.chartLine);
           
            this.CleanupChart();
            WorkoutItem nextSibling = this.CurrentWorkout.GetNextSibling();
            
            if (nextSibling == null)
                return;
            
            this.CurrentWorkout = nextSibling;
            this.CancelTokenSource.Cancel();
            this.ShowWorkout(this.CurrentWorkout);
        }

        private async void Share_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.CurrentWorkout == null)
                return;

            FileSavePicker fileSavePicker = new FileSavePicker();
            bool bResult = false;

            fileSavePicker.SuggestedStartLocation = ((PickerLocationId)0);

            fileSavePicker.FileTypeChoices.Add("Garmin Training Center Database", 
            (IList<string>)new List<string>()
            {
                ".tcx"
            });

            fileSavePicker.SuggestedFileName = (this.CurrentWorkout.FilenameTCX);
            StorageFile tcxFile = await fileSavePicker.PickSaveFileAsync();
            if (tcxFile != null)
                bResult = await this.CurrentWorkout.ExportWorkout(tcxFile);
            if (!bResult)
                return;
            this.ViewInitialized = true;
            this.MapInitialized = true;
        }

        private async void Map_LoadingStatusChanged(MapControl sender, object args)
        {
            if ((int)sender.LoadingStatus != 1)
                return;
            try
            {
                if (this.MapInitialized || this.Viewport == null)
                    return;
                this.MapInitialized = true;
                this.WorkoutMap.DesiredPitch = (0.0);
                int iRetry = 0;
                Thickness margin = new Thickness(70.0, 70.0, 70.0, 70.0);
                bool flag;
                do
                {
                    flag = await this.WorkoutMap.TrySetViewBoundsAsync(this.Viewport, 
                        new Thickness?(margin), (MapAnimationKind)1);
                    if (!flag)
                        this.CancelTokenSource.Token.ThrowIfCancellationRequested();
                }
                while (!flag && iRetry++ < 10);
                this.Viewport = (GeoboundingBox)null;

                if (NetworkInterface.GetIsNetworkAvailable())
                    this.WorkoutMap.Style = ((MapStyle)3);

                margin = new Thickness();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] Exception : " + ex.Message);
            }
        }

    private async Task AddTracks(WorkoutItem workout)
    {
            /*
        await 
        ((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)1, 
            new DispatchedHandler((object)new SectionPage.C51_0()
        {
        <>__this = this,
            workout = workout
        }, __methodptr(\u003CAddTracks\u003Eb__0))
        );
            */
    }

    private async void WorkoutTracks_Loaded(object sender, TracksLoadedEventArgs e)
    {
        if (e.Workout != this.CurrentWorkout)
            return;

        this.CurrentWorkout.TracksLoaded -= 
                new EventHandler<TracksLoadedEventArgs>(this.WorkoutTracks_Loaded);
        try
        {
            SectionPage.DistanceMarkers.Clear();
            this.LoadChartContents(this.CurrentWorkout);

            // ISSUE: method pointer
            //await ((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)1, 
            //    new DispatchedHandler((object)this, __methodptr(\u003CWorkoutTracks_Loaded\u003Eb__52_0)));
            this.AddTracks(this.CurrentWorkout);
        }
        catch (Exception ex)
        {
            this.PosNeedleIcon = (MapIcon)null;
            ((ICollection<MapElement>)this.WorkoutMap.MapElements).Clear();
        }
    }

    private void RunIfSelected(UIElement element, Action action)
    {
        action();
    }

    private async Task LoadChartContents(WorkoutItem workout)
    {
        //await ((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)1, new DispatchedHandler((object)new SectionPage.\u003C\u003Ec__DisplayClass54_0()
        //{
        //  \u003C\u003E4__this = this,
        //  workout = workout
        //}, __methodptr(\u003CLoadChartContents\u003Eb__0)));
    }

        private async void CleanupChart()
        {
            //await ((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)1, 
            //    new DispatchedHandler((object)this, __methodptr(\u003CCleanupChart\u003Eb__55_0)));
        }

        private async void WorkoutMap_LayoutUpdated(object sender, object e)
    {
        try
        {
            if (this.ViewInitialized || this.CurrentWorkout == null || this.Viewport == null)
                return;
            this.ViewInitialized = true;
            int iRetry = 0;
            Thickness margin = new Thickness(70.0, 70.0, 70.0, 70.0);
            bool flag;
            do
            {
                flag = await this.WorkoutMap.TrySetViewBoundsAsync(this.Viewport, 
                    new Thickness?(margin), (MapAnimationKind)1);
                if (!flag)
                    this.CancelTokenSource.Token.ThrowIfCancellationRequested();
            }
            while (!flag && iRetry++ < 10);
            margin = new Thickness();
        }
        catch (Exception ex)
        {
        }
    }

    private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
    {
        if (!(sender is Grid grid))
            return;
        Point position = e.GetPosition((UIElement)grid);
        bool flag1 = false;
        if (this.chartLine != null)
            ((ICollection<UIElement>)((Panel)grid).Children).Remove((UIElement)this.chartLine);
        if (this.PosNeedleIcon != null)
            ((ICollection<MapElement>)this.WorkoutMap.MapElements).Remove((MapElement)this.PosNeedleIcon);

        if (!(((FrameworkElement)grid).FindName("heartLine") is LineSeries name))
            return;
        WorkoutItem dataContext = ((FrameworkElement)name).DataContext 
                as WorkoutItem;

        ObservableCollection<DiagramData> itemsSource = name.ItemsSource 
                as ObservableCollection<DiagramData>;

        if (dataContext == null || itemsSource == null)
            return;
        GeneralTransform visual = ((UIElement)name).TransformToVisual((UIElement)grid);
        Point point1 = visual.TransformPoint(new Point(((FrameworkElement)name).ActualWidth, 0.0));
        Point point2 = visual.TransformPoint(new Point(0.0, 0.0));
        if (position.X >= point2.X && position.X <= point1.X)
        {
            double num1 = ((FrameworkElement)name).ActualWidth / (double)itemsSource.Count;
            int index1 = (int)((position.X - point2.X) / num1);
            int index2 = itemsSource[index1].Index;
            TrackItem trackItem = index2 < dataContext.Items.Count ? dataContext.Items[index2] : (TrackItem)null;
            if (trackItem != null)
            {
                this.chartLine = new Line();
                ((Shape)this.chartLine).Stroke = ((Brush)new SolidColorBrush(Colors.White));
                Line chartLine = this.chartLine;
                double x;
                this.chartLine.X2=(x = position.X);
                double num2 = x;
                chartLine.X1 = (num2);
                this.chartLine.Y1 = (0.0);
                this.chartLine.Y2 = (((FrameworkElement)grid).ActualHeight);
                ((ICollection<UIElement>)((Panel)grid).Children).Add((UIElement)this.chartLine);
                double num3 = 1.0 / trackItem.SpeedMeterPerSecond / 60.0 * 1000.0;
                double num4 = num3 % 1.0;
                double num5 = num3 - num4;
                double num6 = Math.Round(num4 * 60.0);
                double num7 = trackItem.SpeedMeterPerSecond * 3.6;

                if (trackItem.SpeedMeterPerSecond <= 0.0)
                    num5 = num6 = 0.0;

                this.StatusText.Text = ((trackItem.TotalMeters / 1000.0).ToString("0.000") + " km, " 
                    + trackItem.Elevation.ToString() + " m, " + num5.ToString() + ":" + 
                    num6.ToString("00") + "/km, " + num7.ToString("0.00") + " km/h, HR: "
                    + trackItem.Heartrate.ToString() + ", GSR: " + trackItem.GSR.ToString() 
                    + ", Temp: " + trackItem.SkinTemp.ToString());

                ((UIElement)this.StatusGrid).Visibility = ((Visibility)0);
                
                    Geopoint point3 = new Geopoint(new BasicGeoposition()
                {
                    Latitude = (double)(dataContext.LatitudeStart + (long)trackItem.LatDelta) 
                    / 10000000.0,
                    Longitude = (double)(dataContext.LongitudeStart + (long)trackItem.LongDelta) 
                    / 10000000.0,
                    Altitude = 0.0
                });

                MapIcon mapIcon = new MapIcon();
                mapIcon.Location = (point3);
                mapIcon.NormalizedAnchorPoint = (new Point(0.5, 1.0));
                ((MapElement)mapIcon).ZIndex = (80);
                mapIcon.Image = ((IRandomAccessStreamReference)RandomAccessStreamReference.CreateFromUri
                    (new Uri("ms-appx:///Assets/DetailPos.png")));
                mapIcon.Title = ("");
                ((MapElement)mapIcon).Visible = (true);
                this.PosNeedleIcon = mapIcon;
                ((ICollection<MapElement>)this.WorkoutMap.MapElements).Add((MapElement)this.PosNeedleIcon);
                if (!this.IsGeopointInGeoboundingBox(this.GetBounds(this.WorkoutMap), point3))
                    this.WorkoutMap.Center = (point3);
                flag1 = true;
            }
        }
        if (flag1)
            return;
        
        this.StatusText.Text = ("");

        ((UIElement)this.StatusGrid).Visibility = ((Visibility)1);
        int iRetry = 0;
        Thickness margin = new Thickness(70.0, 70.0, 70.0, 70.0);
        GeoboundingBox viewport = new GeoboundingBox(new BasicGeoposition()
        {
            Latitude = (double)(dataContext.LatitudeStart + (long)dataContext.LatDeltaRectNE) / 10000000.0,
            Longitude = (double)(dataContext.LongitudeStart + (long)dataContext.LongDeltaRectSW) / 10000000.0
        }, new BasicGeoposition()
        {
            Latitude = (double)(dataContext.LatitudeStart + (long)dataContext.LatDeltaRectSW) / 10000000.0,
            Longitude = (double)(dataContext.LongitudeStart + (long)dataContext.LongDeltaRectNE) / 10000000.0
        }, (AltitudeReferenceSystem)1);
        bool flag2;
        do
        {
            flag2 = await this.WorkoutMap.TrySetViewBoundsAsync(viewport, new Thickness?(margin), (MapAnimationKind)1);
            if (!flag2)
                this.CancelTokenSource.Token.ThrowIfCancellationRequested();
        }
        while (!flag2 && iRetry++ < 10);
        margin = new Thickness();
        viewport = (GeoboundingBox)null;
    }

    public GeoboundingBox GetBounds(MapControl map)
    {
        if (map.Center.Position.Latitude == 0.0)
            return (GeoboundingBox)null;

        double num1 = 156543.04 * Math.Cos(map.Center.Position.Latitude * 3.1415926535897931 
            / 180.0) / (111325.0 * Math.Pow(2.0, map.ZoomLevel));

        double num2 = ((FrameworkElement)map).ActualWidth * num1 / 0.9;
        double num3 = ((FrameworkElement)map).ActualHeight * num1 / 1.7;
        double num4 = map.Center.Position.Latitude + num3;
        double num5 = map.Center.Position.Longitude - num2;
        double num6 = map.Center.Position.Latitude - num3;
        double num7 = map.Center.Position.Longitude + num2;
        return new GeoboundingBox(new BasicGeoposition()
        {
            Latitude = num4,
            Longitude = num5
        }, new BasicGeoposition()
        {
            Latitude = num6,
            Longitude = num7
        });
    }

    public bool IsGeopointInGeoboundingBox(GeoboundingBox bounds, Geopoint point)
    {
        return  (  point.Position.Latitude < bounds.NorthwestCorner.Latitude
                && point.Position.Longitude > bounds.NorthwestCorner.Longitude 
                && point.Position.Latitude > bounds.SoutheastCorner.Latitude
                && point.Position.Longitude < bounds.SoutheastCorner.Longitude );
    }

    private bool TryGoBack()
    {
        Frame content = Window.Current.Content as Frame;
        if (!content.CanGoBack)
            return false;
        content.GoBack();
        return true;
    }

    /*
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
        if (this._contentLoaded)
            return;
        this._contentLoaded = true;
        Application.LoadComponent((object)this, new Uri("ms-appx:///SectionPage.xaml"), (ComponentResourceLocation)0);
        this.pageRoot = (Page)((FrameworkElement)this).FindName("pageRoot");
        this.DiagramGrid = (Grid)((FrameworkElement)this).FindName("DiagramGrid");
        this.StatusGrid = (Grid)((FrameworkElement)this).FindName("StatusGrid");
        this.StatusText = (TextBlock)((FrameworkElement)this).FindName("StatusText");
        this.lineChart = (Chart)((FrameworkElement)this).FindName("lineChart");
        this.heartLine = (LineSeries)((FrameworkElement)this).FindName("heartLine");
        this.WorkoutMap = (MapControl)((FrameworkElement)this).FindName("WorkoutMap");
    }
    */

    /*
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
        switch (connectionId)
        {
            case 1:
                UIElement uiElement1 = (UIElement)target;
                // ISSUE: method pointer
                WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler((object)this, __methodptr(Grid_Tapped)));
                break;
            case 2:
                UIElement uiElement2 = (UIElement)target;
                // ISSUE: method pointer
                WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler((object)this, __methodptr(Share_Tapped)));
                break;
            case 3:
                UIElement uiElement3 = (UIElement)target;
                // ISSUE: method pointer
                WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler((object)this, __methodptr(Left_Tapped)));
                break;
            case 4:
                UIElement uiElement4 = (UIElement)target;
                // ISSUE: method pointer
                WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler((object)this, __methodptr(Right_Tapped)));
                break;
            case 5:
                FrameworkElement frameworkElement = (FrameworkElement)target;
                WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(frameworkElement.add_LayoutUpdated), new Action<EventRegistrationToken>(frameworkElement.remove_LayoutUpdated), new EventHandler<object>(this.WorkoutMap_LayoutUpdated));
                break;
        }
        this._contentLoaded = true;
    }
    */
  }
}
