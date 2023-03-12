// WorkoutFilterData.cs
// Type: MobileBandSync.Common.WorkoutFilterData
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace MobileBandSync.Common
{
  public class WorkoutFilterData
  {
    public string strSearchText;
    public GeoboundingBox MapBoundingBox;
    public bool MapSelected;

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public bool? IsRunningWorkout { get; set; }

    public bool? IsBikingWorkout { get; set; }

    public bool? IsWalkingWorkout { get; set; }

    public bool? IsSleepingWorkout { get; set; }

    public GeoboundingBox SetBounds(MapControl map)
    {
      if (map == null)
      {
        this.MapBoundingBox = (GeoboundingBox) null;
      }
      else
      {
        Geopoint geopoint1 = (Geopoint) null;
        try
        {
          map.GetLocationFromOffset(new Point(0.0, 0.0), out geopoint1);
        }
        catch
        {
          Geopoint geopoint2 = new Geopoint(new BasicGeoposition()
          {
            Latitude = 85.0,
            Longitude = 0.0
          });
          Point point;
          map.GetOffsetFromLocation(geopoint2, out point);
          map.GetLocationFromOffset(new Point(0.0, point.Y), out geopoint1);
        }
        Geopoint geopoint3 = (Geopoint) null;
        try
        {
          map.GetLocationFromOffset(new Point(((FrameworkElement) map).ActualWidth, 
              ((FrameworkElement) map).ActualHeight), out geopoint3);
        }
        catch
        {
          Geopoint geopoint4 = new Geopoint(new BasicGeoposition()
          {
            Latitude = -85.0,
            Longitude = 0.0
          });
          Point point;
          map.GetOffsetFromLocation(geopoint4, out point);
          map.GetLocationFromOffset(new Point(0.0, point.Y), out geopoint3);
        }
        if (geopoint1 != null && geopoint3 != null)
        {
          this.MapBoundingBox = new GeoboundingBox(geopoint1.Position, geopoint3.Position);
          return this.MapBoundingBox;
        }
      }
      return (GeoboundingBox) null;
    }
  }
}
