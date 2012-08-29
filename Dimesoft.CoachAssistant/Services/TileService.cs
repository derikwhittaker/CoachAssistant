using System;
using System.Linq;
using Dimesoft.CoachAssistant.Domain.Models;
using Microsoft.Phone.Shell;

namespace Dimesoft.CoachAssistant.Services
{
    public interface ITileService
    {
        void CreateEventTile( SportType sport, EventType eventType, int eventId, DateTime eventDateTime, string fieldName, string teamName, string opponentName = "" );
        void DeleteSecondaryTile(SportType sport, int eventId);
        bool SecondaryEvenTileExists(SportType sport, int eventId);
    }

    public class TileService : ITileService
    {
        public const string BackImagePath = "/Images/Tiles/{0}SecondaryBackTile_173x173.png";
        public const string FrontImagePath = "/Images/Tiles/{0}SecondaryTile_173x173.png";
        public const string EventSecondaryTileQuery = "FromTile=True&EventId={0}&SportTypeId={1}";

        public void CreateEventTile( SportType sport, EventType eventType, int eventId, DateTime eventDateTime, string fieldName, string teamName, string opponentName = "" )
        {
            var tileParms = string.Format(EventSecondaryTileQuery, eventId, (int)sport);
            var tileUri = string.Format("/Views/Events/PracticeEventLandingPage.xaml?{0}", tileParms);
            var tileImages = GetImages(sport);
            var frontTitle = GetTitle(eventType, teamName, opponentName);
            var backTitle = GetBackTitle(eventDateTime);

            DeleteSecondaryTile(sport, eventId);

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

        public void DeleteSecondaryTile(SportType sport, int eventId)
        {
            var tileParms = string.Format(EventSecondaryTileQuery, eventId, (int)sport);
            var secondaryTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(tileParms));

            if (secondaryTile != null)
            {
                secondaryTile.Delete();
            }
        }

        public bool SecondaryEvenTileExists(SportType sport, int eventId)
        {
            var tileParms = string.Format(EventSecondaryTileQuery, eventId, (int)sport);
            var secondaryTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(tileParms));

            return secondaryTile != null;
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
