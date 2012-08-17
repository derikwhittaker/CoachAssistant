using System.Threading;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Dimesoft.CoachAssistant.Views
{
    public class PageBase : PhoneApplicationPage
    {
        public PageBase()
        {
            this.Loaded += (sender, args) =>
                               {
                                   var t = new Thread(x => InitDelayedLoad(sender, args));
                                   t.Start();
                               };
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {
            
        }

        private void InitDelayedLoad(object sender, RoutedEventArgs args)
        {
            Thread.Sleep(500);

            Deployment.Current.Dispatcher.BeginInvoke(() => OnLoaded(sender, args));
            
        }
    }
}
