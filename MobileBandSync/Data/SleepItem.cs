// SleepItem.cs
// Type: MobileBandSync.Data.SleepItem
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

namespace MobileBandSync.Data
{
  public class SleepItem
  {
    public SleepItem(
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

    public SleepItem()
    {
    }

    public string UniqueId => this.SleepId.ToString("B");

    public string Title { get; private set; }

    public string Subtitle { get; private set; }

    public string Description { get; private set; }

    public string ImagePath { get; private set; }

    public string Content { get; private set; }

    public int SleepId { get; set; }

    public int SleepActivityId { get; set; }

    public int SecFromStart { get; set; }

    public byte SegmentType { get; set; }

    public byte SleepType { get; set; }

    public byte Heartrate { get; set; }

    public override string ToString() => this.Title;
  }
}
