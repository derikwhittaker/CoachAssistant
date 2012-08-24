using System;
using System.Linq;
using Dimesoft.CoachAssistant.Domain.Models;
using Microsoft.Phone.Shell;

namespace Dimesoft.CoachAssistant.Services
{
    public interface ITileService
    {
        void CreateEventTile( SportType sport, EventType eventType, int eventId, DateTime eventDateTime, string fieldName, string teamName, string opponentName = "" );
        void DeleteSecondaryTile(string tileUri);
    }

    public class TileService : ITileService
    {
        public const string BackImagePath = "/Images/Tiles/{0}SecondaryBackTile_173x173.png";
        public const string FrontImagePath = "/Images/Tiles/{0}SecondaryTile_173x173.png";

        public void CreateEventTile( SportType sport, EventType eventType, int eventId, DateTime eventDateTime, string fieldName, string teamName, string opponentName = "" )
        {
            var tileUri = string.Format("/EventLandingPage.xaml&EventId={0}", eventId);
            var tileImages = GetImages(sport);
            var frontTitle = GetTitle(eventType, teamName, opponentName);
            var backTitle = GetBackTitle(eventDateTime);

            DeleteSecondaryTile(tileUri);

            var newTileData = new StandardTileData
                                  {
                                      BackgroundImage = new Uri(tileImages.Item1, UriKind.RelativeOrAbsolute),
                                      Title = frontTitle,
                                      BackTitle = backTitle,
                                      BackContent = fieldName,
                                      BackBackgroundImage = new Uri(tileImages.Item2, UriKind.RelativeOrAbsolute)
                                  };

            ShellTile.Create(new Uri(tileUri, UriKind.RelativeOrAbsolute), newTileData);
        }

        public void DeleteSecondaryTile(string tileUri)
        {
            var secondaryTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(tileUri));

            if (secondaryTile != null)
            {
                secondaryTile.Delete();
            }
        }

        private string GetBackTitle(DateTime eventDateTime )
        {
            return string.Format("{0} @ {1}", eventDateTime.ToShortDateString(), eventDateTime.ToShortTimeString());
        }

        private Tuple<string, string> GetImages(SportType sport )
        {
            var frontTile = string.Format(FrontImagePath, sport.ToString());
            var backTile = string.Format(BackImagePath, sport.ToString());

            return Tuple.Create(frontTile, backTile);
        } 

        private string GetTitle(EventType eventType, string teamName, string opponentName )
        {
            switch (eventType)
            {
                case EventType.Practice:
                    return string.Format("{0} Practice", teamName);
                            
                case EventType.Game:
                    return string.Format("{0} vs {1}", teamName, opponentName);

                default:
                    return "";
            }
        }
    }
}
