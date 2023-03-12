// LoadStateEventArgs.cs
// Type: MobileBandSync.Common.LoadStateEventArgs
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.Collections.Generic;

namespace MobileBandSync.Common
{
  public class LoadStateEventArgs : EventArgs
  {
    public object NavigationParameter { get; private set; }

    public Dictionary<string, object> PageState { get; private set; }

    public LoadStateEventArgs(object navigationParameter, Dictionary<string, object> pageState)
    {
      this.NavigationParameter = navigationParameter;
      this.PageState = pageState;
    }
  }
}
