// TracksLoadedEventArgs.cs
// Type: MobileBandSync.Data.TracksLoadedEventArgs
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;

namespace MobileBandSync.Data
{
  public class TracksLoadedEventArgs : EventArgs
  {
    public WorkoutItem Workout { get; private set; }

    public TracksLoadedEventArgs(WorkoutItem workout) => this.Workout = workout;
  }
}
