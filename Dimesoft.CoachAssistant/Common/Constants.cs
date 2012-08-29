namespace Dimesoft.CoachAssistant.Common
{
    public class QueryStringConstants
    {
        public const string FromTile = "FromTile";
        public const string CallingPageName = "CallingPageName";
        public const string SportTypeId = "SportTypeId";
        public const string EventId = "EventId";
        public const string SportId = "SportId";
        public const string TeamId = "TeamId";
        public const string FieldId = "FieldId";
        public const string SelectedDrills = "SelectedDrills";
    }

    public class SessoinStateConstants
    {
        public const string SelectedDrills = "SelectedDrills";
        public const string NewDrill = "NewDrill";
    }

    public class RoutingPageConstants
    {
        public const string PracticeEventLandingPage = "PracticeEventLandingPage.xaml";
    }

    public enum DashboardViewState
    {
        ShowAll = 1,
        ShowActive = 2,
        ShowCompleted = 3
    }
}
