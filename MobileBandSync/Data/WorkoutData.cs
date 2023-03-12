// WorkoutData.cs
// Type: MobileBandSync.Data.WorkoutData
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobileBandSync.Data
{
  public class WorkoutData
  {
    private IEnumerable<WorkoutItem> _workouts;
    private string _workoutTitle;

    public string WorkoutTitle
    {
      get => this._workoutTitle;
      set
      {
        this._workoutTitle = value;
        this.OnPropertyChanged(nameof (WorkoutTitle));
      }
    }

    public IEnumerable<WorkoutItem> Workouts
    {
      get => this._workouts;
      set
      {
        this._workouts = value;
        this.OnPropertyChanged(nameof (Workouts));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
