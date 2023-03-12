// Decompiled with JetBrains decompiler
// Type: MobileBandSync.OpenTcx.Entities.AbstractStep_t
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MobileBandSync.OpenTcx.Entities
{
  [XmlInclude(typeof (Step_t))]
  [XmlInclude(typeof (Repeat_t))]
  [GeneratedCode("xsd", "4.0.30319.1")]
  [DebuggerStepThrough]
  [XmlType(Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
  public abstract class AbstractStep_t
  {
    private string stepIdField;

    [XmlElement(DataType = "positiveInteger")]
    public string StepId
    {
      get => this.stepIdField;
      set => this.stepIdField = value;
    }
  }
}
