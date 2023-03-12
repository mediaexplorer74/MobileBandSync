// ItemPage.cs
// Type: MobileBandSync.ItemPage
// Assembly: MobileBandSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6AE79C20-CD20-4792-8F76-8817DEB4DE21
// Assembly location: C:\Users\Admin\Desktop\re\mobile\MobileBandSync.exe

using MobileBandSync.Common;
using MobileBandSync.Data;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace MobileBandSync
{
    public sealed partial class ItemPage : Page//, IComponentConnector
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
       
        //private Page pageRoot;
       
        //private Grid LayoutRoot;
       
        //private Grid ContentRoot;
       
        //private bool _contentLoaded;

        public ItemPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper((Page)this);
            this.navigationHelper.LoadState += new LoadStateEventHandler(this.NavigationHelper_LoadState);
            this.navigationHelper.SaveState += new SaveStateEventHandler(this.NavigationHelper_SaveState);
        }

        public NavigationHelper NavigationHelper => this.navigationHelper;

        public ObservableDictionary DefaultViewModel => this.defaultViewModel;

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) 
            => this.DefaultViewModel["Item"] = (object)await 
            WorkoutDataSource.GetItemAsync((string)e.NavigationParameter);

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        //protected virtual void OnNavigatedTo(NavigationEventArgs e) => this.navigationHelper.OnNavigatedTo(e);

        //protected virtual void OnNavigatedFrom(NavigationEventArgs e) => this.navigationHelper.OnNavigatedFrom(e);

        /*
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("ms-appx:///ItemPage.xaml"), (ComponentResourceLocation)0);
            this.pageRoot = (Page)((FrameworkElement)this).FindName("pageRoot");
            this.LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
            this.ContentRoot = (Grid)((FrameworkElement)this).FindName("ContentRoot");
        }
        */

        /*
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        [DebuggerNonUserCode]
        public void Connect(int connectionId, object target) => this._contentLoaded = true;
        */
    }
}
