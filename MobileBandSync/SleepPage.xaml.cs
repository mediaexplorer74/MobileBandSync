// SleepPage.cs
// Type: MobileBandSync.SleepPage
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.Common;
using MobileBandSync.Data;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace MobileBandSync
{
    public sealed partial class SleepPage : Page//, IComponentConnector
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private TimeSpan awakeTimeSpan;
        private TimeSpan sleepTimeSpan;
        private Color colAwakeBar = new Color()
        {
            A = byte.MaxValue,
            R = byte.MaxValue,
            G = 139,
            B = 2
        };
        private Color colLightBar = new Color()
        {
            A = byte.MaxValue,
            R = 0,
            G = 121,
            B = 214
        };
        private Color colRestfulBar = new Color()
        {
            A = byte.MaxValue,
            R = 0,
            G = 61,
            B = 110
        };
        private Color colHeaderBackground = new Color()
        {
            A = byte.MaxValue,
            R = 0,
            G = 90,
            B = 161
        };
        private Color colHeaderSummaryDate = new Color()
        {
            A = byte.MaxValue,
            R = 144,
            G = 206,
            B = byte.MaxValue
        };
        private Color colHeaderSummaryTime = new Color()
        {
            A = byte.MaxValue,
            R = 242,
            G = byte.MaxValue,
            B = byte.MaxValue
        };
        private Color colHeaderSummaryText = new Color()
        {
            A = byte.MaxValue,
            R = 242,
            G = byte.MaxValue,
            B = byte.MaxValue
        };
        private Color colDiagramHeader = new Color()
        {
            A = byte.MaxValue,
            R = 213,
            G = 213,
            B = 213
        };
        private Color colDiagramXAxisText = new Color()
        {
            A = byte.MaxValue,
            R = 213,
            G = 213,
            B = 213
        };
        private Color colDiagramYAxisText = new Color()
        {
            A = byte.MaxValue,
            R = 213,
            G = 213,
            B = 213
        };
        private Color colDiagramFooterTitle = new Color()
        {
            A = byte.MaxValue,
            R = 235,
            G = 235,
            B = 235
        };
        private Color colDiagramFooterSubtitle = new Color()
        {
            A = byte.MaxValue,
            R = 145,
            G = 145,
            B = 145
        };
        private Color colDiagramFooterDuration = new Color()
        {
            A = byte.MaxValue,
            R = 35,
            G = 104,
            B = 169
        };
        private Color colDiagramGrid = new Color()
        {
            A = byte.MaxValue,
            R = 239,
            G = 238,
            B = 236
        };

        private List<string> slCadence = new List<string>();
        private CultureInfo sleepPageCultureInfo = new CultureInfo("en-US");

        /*
        private Page pageRoot;
        
        private Grid DiagramGrid;
        
        private TextBlock Summary;
        
        private TextBlock Date;
        
        private TextBlock LightHours;
        
        private TextBlock LightMinutes;
        
        private TextBlock RestfulHours;
        
        private TextBlock RestfulMinutes;
        
        private TextBlock Hours;
        
        private TextBlock Minutes;
        
        private TextBlock AsleepTime;
        
        private TextBlock AwakeTime;
        
        private Grid XAxis;
        
        private Grid BarPanel;
        
        private Canvas SleepDiagrams;
        
        private Grid LineHour;
        
        private Grid HourText;
        
        //private bool _contentLoaded;
        */

        public SleepPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper((Page)this);
            this.navigationHelper.LoadState += new LoadStateEventHandler(this.NavigationHelper_LoadState);
            this.navigationHelper.SaveState += new SaveStateEventHandler(this.NavigationHelper_SaveState);
            this.CancelTokenSource = new CancellationTokenSource();
        }

        public NavigationHelper NavigationHelper => this.navigationHelper;

        public ObservableDictionary DefaultViewModel => this.defaultViewModel;

        public int currentWorkoutId { get; private set; }

        public CancellationTokenSource CancelTokenSource { get; private set; }

        public WorkoutItem CurrentWorkout { get; private set; }

        public Size CanvasSize { get; private set; }

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
            this.ShowWorkout(workoutAsync);
        }

        private async void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (this.CurrentWorkout == null || this.CurrentWorkout.Items == null || this.CurrentWorkout.Items.Count <= 0)
                return;
            this.CurrentWorkout.Items.Clear();
        }

        private async Task ShowWorkout(WorkoutItem workout)
        {
            if (workout == null)
                return;
            try
            {
                this.CurrentWorkout = workout;
                this.DefaultViewModel["Workout"] = (object)workout;
                if (this.CurrentWorkout == null)
                    return;
                this.CancelTokenSource.Dispose();
                this.CancelTokenSource = new CancellationTokenSource();
                if (this.CurrentWorkout.Items != null && this.CurrentWorkout.Items.Count != 0)
                    return;
                this.CurrentWorkout.TracksLoaded += new EventHandler<TracksLoadedEventArgs>(this.WorkoutTracks_Loaded);
                await this.CurrentWorkout.ReadSleepData(this.CancelTokenSource.Token);
                Size canvasSize = this.CanvasSize;
                double width = canvasSize.Width;
                canvasSize = this.CanvasSize;
                double height = canvasSize.Height;
                WorkoutItem currentWorkout = this.CurrentWorkout;
                await this.ShowChart(width, height, currentWorkout);
            }
            catch (Exception ex)
            {
            }
        }

        //protected virtual void OnNavigatedTo(NavigationEventArgs e) => this.navigationHelper.OnNavigatedTo(e);

        //protected virtual void OnNavigatedFrom(NavigationEventArgs e) => this.navigationHelper.OnNavigatedFrom(e);

        private async void Left_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.CurrentWorkout == null)
                return;
            if (this.CurrentWorkout.Items != null && this.CurrentWorkout.Items.Count > 0)
                this.CurrentWorkout.Items.Clear();
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
            if (this.CurrentWorkout.Items != null && this.CurrentWorkout.Items.Count > 0)
                this.CurrentWorkout.Items.Clear();
            WorkoutItem nextSibling = this.CurrentWorkout.GetNextSibling();
            if (nextSibling == null)
                return;
            this.CurrentWorkout = nextSibling;
            this.CancelTokenSource.Cancel();
            this.ShowWorkout(this.CurrentWorkout);
        }

        private async void Share_Tapped(object sender, TappedRoutedEventArgs e)
        {
            WorkoutItem currentWorkout = this.CurrentWorkout;
        }

        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(sender is Grid grid))
                return;
            e.GetPosition((UIElement)grid);
        }

        private bool TryGoBack()
        {
            Frame content = Window.Current.Content as Frame;
            if (!content.CanGoBack)
                return false;
            content.GoBack();
            return true;
        }

        private bool DrawSleepDiagram()
        {
            ((ICollection<UIElement>)((Panel)this.BarPanel).Children).Clear();
            ((ICollection<ColumnDefinition>)this.BarPanel.ColumnDefinitions).Clear();
            if (this.CurrentWorkout != null && this.CurrentWorkout.Items.Count > 0)
            {
                uint sleepType = (uint)this.CurrentWorkout.Items[0].SleepType;
                uint segmentType = (uint)this.CurrentWorkout.Items[0].SegmentType;
                DateTime start1 = this.CurrentWorkout.Start;
                DateTime start2 = this.CurrentWorkout.Start;
                this.AddXAxis(this.CurrentWorkout.Start, this.CurrentWorkout.End);
                this.Hours.put_Text(this.CurrentWorkout.SleepDuration.Hours.ToString());
                this.Minutes.put_Text(this.CurrentWorkout.SleepDuration.Minutes.ToString("00"));
                this.RestfulHours.put_Text(this.CurrentWorkout.TotalRestfulSleepDuration.Hours.ToString());
                this.RestfulMinutes.put_Text(this.CurrentWorkout.TotalRestfulSleepDuration.Minutes.ToString("00"));
                this.LightHours.put_Text(this.CurrentWorkout.TotalRestlessSleepDuration.Hours.ToString());
                this.LightMinutes.put_Text(this.CurrentWorkout.TotalRestlessSleepDuration.Minutes.ToString("00"));
                foreach (TrackItem trackItem in (Collection<TrackItem>)this.CurrentWorkout.Items)
                {
                    DateTime dateTime = this.CurrentWorkout.Start + new TimeSpan(0, 0, trackItem.SecFromStart);
                    this.AddSleepItem(trackItem, ref sleepType, ref segmentType, ref start1);
                }
                this.AddSleepItem((TrackItem)null, ref sleepType, ref segmentType, ref start1);
            }
            return true;
        }

        private void AddSleepItem(
          TrackItem item,
          ref uint lastSleepType,
          ref uint lastSegmentType,
          ref DateTime lastSegmentDate)
        {
            if (item != null)
            {
                DateTime dateTime = this.CurrentWorkout.Start + new TimeSpan(0, 0, item.SecFromStart);
                if ((int)item.SleepType == (int)lastSleepType && (int)item.SegmentType == (int)lastSegmentType && item != this.CurrentWorkout.Items[this.CurrentWorkout.Items.Count - 1])
                    return;
                TimeSpan dtLength = new TimeSpan(0, (int)(dateTime - lastSegmentDate).TotalMinutes, 0);
                this.slCadence.Add(Convert.ToString((long)item.Cadence, 2).PadLeft(32, '0'));
                if (lastSleepType == 0U && lastSegmentType == 0U)
                {
                    this.AddAwakeBar(dtLength);
                    this.awakeTimeSpan += dtLength;
                }
                else
                {
                    if (lastSleepType >= 20000U)
                        this.AddRestfulBar(dtLength);
                    else
                        this.AddLightBar(dtLength);
                    this.sleepTimeSpan += dtLength;
                }
                lastSleepType = (uint)item.SleepType;
                lastSegmentType = (uint)item.SegmentType;
                lastSegmentDate = dateTime;
            }
            else
            {
                if (!(lastSegmentDate < this.CurrentWorkout.End))
                    return;
                TimeSpan dtLength = new TimeSpan(0, (int)(this.CurrentWorkout.End - lastSegmentDate).TotalMinutes, 0);
                this.AddAwakeBar(dtLength);
                this.awakeTimeSpan += dtLength;
            }
        }

        private bool AddXAxis(DateTime dtStart, DateTime dtEnd)
        {
            bool flag = true;
            int num = 60 - dtStart.Minute;
            int minute = dtEnd.Minute;
            DateTime dateTime = dtStart.AddMinutes((double)num);
            this.Date.put_Text(dtStart.ToLocalTime().ToString("ddd M/d", (IFormatProvider)this.sleepPageCultureInfo) + "   Avg HR: " + this.CurrentWorkout.AvgHR.ToString() + "   Max HR: " + this.CurrentWorkout.MaxHR.ToString() + "   Cal: " + this.CurrentWorkout.Calories.ToString());
            string lower1 = dtStart.ToLocalTime().ToString("h:mmtt", (IFormatProvider)this.sleepPageCultureInfo).ToLower();
            this.AsleepTime.put_Text("Asleep " + lower1.Substring(0, lower1.Length - 1));
            string lower2 = dtEnd.ToLocalTime().ToString("h:mmtt", (IFormatProvider)this.sleepPageCultureInfo).ToLower();
            this.AwakeTime.put_Text("Woke up " + lower2.Substring(0, lower2.Length - 1));
            ((ICollection<UIElement>)((Panel)this.HourText).Children).Clear();
            ((ICollection<ColumnDefinition>)this.LineHour.ColumnDefinitions).Clear();
            ((ICollection<ColumnDefinition>)this.HourText.ColumnDefinitions).Clear();
            Color color;
            if (num > 0)
            {
                ColumnDefinitionCollection columnDefinitions1 = this.LineHour.ColumnDefinitions;
                ColumnDefinition columnDefinition1 = new ColumnDefinition();
                columnDefinition1.put_Width(new GridLength((double)num, GridUnitType.Star));
                ((ICollection<ColumnDefinition>)columnDefinitions1).Add(columnDefinition1);
                ColumnDefinitionCollection columnDefinitions2 = this.HourText.ColumnDefinitions;
                ColumnDefinition columnDefinition2 = new ColumnDefinition();
                columnDefinition2.put_Width(new GridLength((double)num, GridUnitType.Star));
                ((ICollection<ColumnDefinition>)columnDefinitions2).Add(columnDefinition2);
                if (num > 15)
                {
                    string str;
                    if (dtStart.ToLocalTime().Hour == 23 || dtStart.ToLocalTime().Hour == 0)
                    {
                        string lower3 = dtStart.ToLocalTime().ToString("htt", (IFormatProvider)this.sleepPageCultureInfo).ToLower();
                        str = lower3.Substring(0, lower3.Length - 1);
                    }
                    else
                        str = dtStart.ToLocalTime().ToString("hh", (IFormatProvider)this.sleepPageCultureInfo).TrimStart('0');
                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.put_Text(str);
                    ((FrameworkElement)textBlock1).put_VerticalAlignment((VerticalAlignment)2);
                    ((FrameworkElement)textBlock1).put_HorizontalAlignment((HorizontalAlignment)2);
                    textBlock1.put_FontSize(16.0);
                    textBlock1.put_FontWeight(FontWeights.Normal);
                    textBlock1.put_Foreground((Brush)new SolidColorBrush(new Color()
                    {
                        A = byte.MaxValue,
                        R = (byte)145,
                        G = (byte)145,
                        B = (byte)145
                    }));
                    ((FrameworkElement)textBlock1).Margin = (new Thickness(0.0, 0.0, -4.0, 0.0));
                    TextBlock textBlock2 = textBlock1;
                    Grid.SetColumn((FrameworkElement)textBlock2, 0);
                    ((ICollection<UIElement>)((Panel)this.HourText).Children).Add((UIElement)textBlock2);
                }
                Border border1 = new Border();
                border1.BorderThickness = (new Thickness(0.0, 0.0, 1.0, 0.0));
                Border border2 = border1;
                color = new Color();
                color.A = byte.MaxValue;
                color.R = (byte)237;
                color.G = (byte)236;
                color.B = (byte)234;
                SolidColorBrush solidColorBrush = new SolidColorBrush(color);
                border2.BorderBrush = ((Brush)solidColorBrush);
                ((FrameworkElement)border1).HorizontalAlignment = ((HorizontalAlignment)3);
                ((FrameworkElement)border1).VerticalAlignment = ((VerticalAlignment)3);
                ((FrameworkElement)border1).Margin = (new Thickness(0.0, 0.0, 0.0, 23.0));
                Grid.SetColumn((FrameworkElement)border1, ((ICollection<ColumnDefinition>)this.LineHour.ColumnDefinitions).Count - 1);
                Grid.SetRow((FrameworkElement)border1, 0);
                ((ICollection<UIElement>)((Panel)this.LineHour).Children).Add((UIElement)border1);
            }
            do
            {
                ColumnDefinitionCollection columnDefinitions3 = this.LineHour.ColumnDefinitions;
                ColumnDefinition columnDefinition3 = new ColumnDefinition();
                columnDefinition3.Width = (new GridLength(60.0, GridUnitType.Star));
                ((ICollection<ColumnDefinition>)columnDefinitions3).Add(columnDefinition3);
                ColumnDefinitionCollection columnDefinitions4 = this.HourText.ColumnDefinitions;
                ColumnDefinition columnDefinition4 = new ColumnDefinition();
                columnDefinition4.Width = (new GridLength(60.0, GridUnitType.Star));
                ((ICollection<ColumnDefinition>)columnDefinitions4).Add(columnDefinition4);
                DateTime localTime = dateTime.ToLocalTime();
                string str;
                if (localTime.Hour != 23)
                {
                    localTime = dateTime.ToLocalTime();
                    if (localTime.Hour != 0)
                    {
                        localTime = dateTime.ToLocalTime();
                        str = localTime.ToString("hh", (IFormatProvider)this.sleepPageCultureInfo)
                            .TrimStart('0');

                        goto label_11;
                    }
                }
                localTime = dateTime.ToLocalTime();
                string lower4 = localTime.ToString("htt", (IFormatProvider)this.sleepPageCultureInfo).ToLower();
                str = lower4.Substring(0, lower4.Length - 1);
            label_11:
                TextBlock textBlock3 = new TextBlock();
                textBlock3.Text = (str);
                ((FrameworkElement)textBlock3).VerticalAlignment = ((VerticalAlignment)2);
                ((FrameworkElement)textBlock3).HorizontalAlignment = ((HorizontalAlignment)2);
                textBlock3.FontSize = (16.0);
                textBlock3.FontWeight = (FontWeights.Normal);
                color = new Color();
                color.A = byte.MaxValue;
                color.R = (byte)145;
                color.G = (byte)145;
                color.B = (byte)145;
                textBlock3.Foreground = ((Brush)new SolidColorBrush(color));
                ((FrameworkElement)textBlock3).Margin = (new Thickness(0.0, 0.0, -4.0, 0.0));
                TextBlock textBlock4 = textBlock3;
                Grid.SetColumn((FrameworkElement)textBlock4, ((ICollection<ColumnDefinition>)this.HourText.ColumnDefinitions).Count - 1);
                ((ICollection<UIElement>)((Panel)this.HourText).Children).Add((UIElement)textBlock4);
                Border border3 = new Border();
                border3.BorderThickness = (new Thickness(0.0, 0.0, 1.0, 0.0));
                Border border4 = border3;
                color = new Color();
                color.A = byte.MaxValue;
                color.R = (byte)237;
                color.G = (byte)236;
                color.B = (byte)234;
                SolidColorBrush solidColorBrush = new SolidColorBrush(color);
                border4.BorderBrush = ((Brush)solidColorBrush);
                ((FrameworkElement)border3).HorizontalAlignment = ((HorizontalAlignment)3);
                ((FrameworkElement)border3).VerticalAlignment = ((VerticalAlignment)3);
                ((FrameworkElement)border3).Margin = (new Thickness(0.0, 0.0, 0.0, 23.0));
                Grid.SetColumn((FrameworkElement)border3, ((ICollection<ColumnDefinition>)this.LineHour.ColumnDefinitions).Count - 1);
                Grid.SetRow((FrameworkElement)border3, 0);
                ((ICollection<UIElement>)((Panel)this.LineHour).Children).Add((UIElement)border3);
                dateTime = dateTime.AddHours(1.0);
            }
            while (dateTime <= dtEnd);
            if (minute > 0)
            {
                ColumnDefinitionCollection columnDefinitions5 = this.LineHour.ColumnDefinitions;
                ColumnDefinition columnDefinition5 = new ColumnDefinition();
                columnDefinition5.put_Width(new GridLength((double)minute, GridUnitType.Star));
                ((ICollection<ColumnDefinition>)columnDefinitions5).Add(columnDefinition5);
                ColumnDefinitionCollection columnDefinitions6 = this.HourText.ColumnDefinitions;
                ColumnDefinition columnDefinition6 = new ColumnDefinition();
                columnDefinition6.put_Width(new GridLength((double)minute, GridUnitType.Star));
                ((ICollection<ColumnDefinition>)columnDefinitions6).Add(columnDefinition6);
            }
            return flag;
        }

        private bool AddBar(SleepPage.SleepType sleepType, TimeSpan tsLength)
        {
            bool flag = true;
            Rectangle rectangle = new Rectangle();
            ((FrameworkElement)rectangle).put_Margin(new Thickness(0.0, 0.0, 0.0, 0.0));
            ((FrameworkElement)rectangle).put_VerticalAlignment((VerticalAlignment)3);
            ((FrameworkElement)rectangle).put_HorizontalAlignment((HorizontalAlignment)3);
            switch (sleepType)
            {
                case SleepPage.SleepType.Awake:
                    ((Shape)rectangle).put_Fill((Brush)new SolidColorBrush(this.colAwakeBar));
                    ((Shape)rectangle).put_Stroke((Brush)new SolidColorBrush(this.colAwakeBar));
                    Grid.SetRow((FrameworkElement)rectangle, 0);
                    break;
                case SleepPage.SleepType.LightSleep:
                    ((Shape)rectangle).put_Fill((Brush)new SolidColorBrush(this.colLightBar));
                    ((Shape)rectangle).put_Stroke((Brush)new SolidColorBrush(this.colLightBar));
                    Grid.SetRow((FrameworkElement)rectangle, 1);
                    break;
                case SleepPage.SleepType.RestfulSleep:
                    ((Shape)rectangle).put_Fill((Brush)new SolidColorBrush(this.colRestfulBar));
                    ((Shape)rectangle).put_Stroke((Brush)new SolidColorBrush(this.colRestfulBar));
                    Grid.SetRow((FrameworkElement)rectangle, 1);
                    Grid.SetRowSpan((FrameworkElement)rectangle, 2);
                    break;
            }
            ColumnDefinitionCollection columnDefinitions = this.BarPanel.ColumnDefinitions;
            ColumnDefinition columnDefinition = new ColumnDefinition();
            columnDefinition.Width(new GridLength(tsLength.TotalMinutes, GridUnitType.Star));
            ((ICollection<ColumnDefinition>)columnDefinitions).Add(columnDefinition);
            Grid.SetColumn((FrameworkElement)rectangle,
                ((ICollection<ColumnDefinition>)this.BarPanel.ColumnDefinitions).Count - 1);

            ((ICollection<UIElement>)((Panel)this.BarPanel).Children).Add((UIElement)rectangle);
            return flag;
        }

        private bool AddAwakeBar(TimeSpan dtLength)
        { 
            this.AddBar(SleepPage.SleepType.Awake, dtLength); 
        }

        private bool AddLightBar(TimeSpan dtLength)
        { 
            this.AddBar(SleepPage.SleepType.LightSleep, dtLength); 
        }

        private bool AddRestfulBar(TimeSpan dtLength)
        { 
            this.AddBar(SleepPage.SleepType.RestfulSleep, dtLength); 
        }

        private async void WorkoutTracks_Loaded(object sender, TracksLoadedEventArgs e)
        {
            e.Workout.TracksLoaded -= new EventHandler<TracksLoadedEventArgs>(this.WorkoutTracks_Loaded);
            if (e.Workout != this.CurrentWorkout)
                return;
            try
            {
                // ISSUE: method pointer
                //await ((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)1,
                //    new DispatchedHandler((object)this, __methodptr(\u003CWorkoutTracks_Loaded\u003Eb__59_0)));
            }
            catch (Exception ex)
            {
                //
            }
        }

        private void RunIfSelected(UIElement element, Action action) => action();

        public async Task ShowChart(double width, double height, WorkoutItem workout)
        { 
            /*
            await ((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)0,
                new DispatchedHandler((object)new SleepPage.<> __DisplayClass61_0()
        {
          \u003C\u003E4__this = this,
              height = height,
              width = width,
              workout = workout
        }, __methodptr(\u003CShowChart\u003Eb__0)));
            */

        }

    private async void SleepDiagrams_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (e == null)
            return;
        this.CanvasSize = e.NewSize;
        if (!(e.PreviousSize != e.NewSize))
            return;
        WorkoutItem workout = sender is Canvas canvas ? 
                ((FrameworkElement)canvas).DataContext 
                as WorkoutItem : (WorkoutItem)null;
        if (workout == null)
            return;
        await this.ShowChart(e.NewSize.Width, e.NewSize.Height, workout);
    }
          

    public enum SleepType
    {
        Unknown,
        Awake,
        LightSleep,
        RestfulSleep,
    }
}
}
