using System;
using System.Windows.Media.Imaging;
using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.ViewModels
{
    public class BaseVM : ViewModelBase
    {
        private string _pageTitle;
        private bool _isBusy = true;

        public string ApplicationName { get { return "Coachs Assistant"; } }

        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                _pageTitle = value;

                RaisePropertyChanged(() => PageTitle);
            }
        }

        public virtual BitmapImage BackgroundImage { get; private set; }
        
        public bool DataLoaded { get; set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public virtual BitmapImage GetBackgroundImage(int sportId)
        {
            var path = "";
            switch ((SportType)sportId)
            {
                case SportType.Baseball:
                    path = "../../Images/BaseballBackground_1024x768.jpg";
                    break;

                case SportType.Basketball:
                    path = "../../Images/BasketballBackground_1024x768.jpg";
                    break;

                case SportType.Soccer:
                    path = "../../Images/SoccerBackground_1024x768.jpg";
                    break;

                default:
                    path = "../../Images/SoccerBackground_1024x768.jpg";
                    break;
            }

            var bitmap = new BitmapImage
            {
                CreateOptions = BitmapCreateOptions.BackgroundCreation,
                UriSource = new Uri(path, UriKind.RelativeOrAbsolute),
            };

            return bitmap;
        }
    }
}