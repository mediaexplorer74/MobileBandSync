// TrackItem.cs
// Type: MobileBandSync.Data.TrackItem
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

namespace MobileBandSync.Data
{
  public class TrackItem
  {
    public TrackItem(
      string uniqueId,
      string title,
      string subtitle,
      string imagePath,
      string description,
      string content)
    {
      this.Title = title;
      this.Subtitle = subtitle;
      this.Description = description;
      this.ImagePath = imagePath;
      this.Content = content;
    }

    public TrackItem()
    {
    }

    public string UniqueId => this.TrackId.ToString("B");

    public string Title { get; private set; }

    public string Subtitle { get; private set; }

    public string Description { get; private set; }

    public string ImagePath { get; private set; }

    public string Content { get; private set; }

    public int TrackId { get; set; }

    public int WorkoutId { get; set; }

    public int SecFromStart { get; set; }

    public int LongDelta { get; set; }

    public int LatDelta { get; set; }

    public int Elevation { get; set; }

    public byte Heartrate { get; set; }

    public int Barometer { get; set; }

    public uint Cadence { get; set; }

    public double SkinTemp { get; set; }

    public int GSR { get; set; }

    public int UV { get; set; }

    public double DistMeter { get; set; }

    public double SpeedMeterPerSecond { get; set; }

    public double TotalMeters { get; internal set; }

    public ushort SleepType => (ushort) (this.Cadence >> 16);

    public ushort SegmentType => (ushort) (this.Cadence & (uint) ushort.MaxValue);

    public override string ToString() => this.Title;
  }
}
