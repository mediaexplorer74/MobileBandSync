// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Tcx
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.OpenTcx.Entities;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace MobileBandSync.OpenTcx
{
  public class Tcx
  {
    public async Task<TrainingCenterDatabase_t> AnalyzeTcxFile(
      string tcxFile)
    {
      StorageFolder localFolder = ApplicationData.Current.LocalFolder;
      string str = await this.CopyLocally(tcxFile);
      return (TrainingCenterDatabase_t) null;
    }

    public async Task<string> CopyLocally(string tcxFile) => "";

    public TrainingCenterDatabase_t AnalyzeTcxStream(Stream fs) => new XmlSerializer(typeof (TrainingCenterDatabase_t)).Deserialize(fs) as TrainingCenterDatabase_t;

    public string GenerateTcx(TrainingCenterDatabase_t data)
    {
      string tcx = (string) null;
      try
      {
        using (StringWriter stringWriter = new StringWriter())
        {
          new XmlSerializer(typeof (TrainingCenterDatabase_t)).Serialize((TextWriter) stringWriter, (object) data);
          tcx = stringWriter.GetStringBuilder().ToString();
        }
      }
      catch (Exception ex)
      {
      }
      return tcx;
    }

    public bool ValidateTcx(string tckFile)
    {
      try
      {
        return new Validator().Validate(tckFile, false);
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}
