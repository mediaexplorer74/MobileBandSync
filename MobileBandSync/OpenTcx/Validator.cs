// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Validator
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.Xml;

namespace MobileBandSync.OpenTcx
{
  internal sealed class Validator
  {
    private string errMsg;

    public string validationErrMsg
    {
      get => this.errMsg;
      set => this.errMsg = value;
    }

    public bool Validate(string XMLFile, bool LocationDefined)
    {
      bool flag = true;
      try
      {
        XmlReaderSettings settings = new XmlReaderSettings();
        using (XmlReader xmlReader = XmlReader.Create(XMLFile, settings))
        {
          while (xmlReader.Read() & flag)
          {
            string name = xmlReader.Name;
          }
        }
      }
      catch (Exception ex)
      {
        this.validationErrMsg = this.validationErrMsg + "Exception occured when validating. " + ex.Message;
        flag = false;
      }
      return flag;
    }
  }
}
