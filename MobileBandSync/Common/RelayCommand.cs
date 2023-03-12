// RelayCommand.cs
// Type: MobileBandSync.Common.RelayCommand
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using System;
using System.Windows.Input;

namespace MobileBandSync.Common
{
  public class RelayCommand : ICommand
  {
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public event EventHandler CanExecuteChanged;

    public RelayCommand(Action execute)
      : this(execute, (Func<bool>) null)
    {
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
      this._execute = execute != null ? execute : throw new ArgumentNullException(nameof (execute));
      this._canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => this._canExecute == null || this._canExecute();

    public void Execute(object parameter) => this._execute();

    public void RaiseCanExecuteChanged()
    {
      EventHandler canExecuteChanged = this.CanExecuteChanged;
      if (canExecuteChanged == null)
        return;
      canExecuteChanged((object) this, EventArgs.Empty);
    }
  }
}
