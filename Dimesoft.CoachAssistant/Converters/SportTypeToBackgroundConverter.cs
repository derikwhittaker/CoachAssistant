using System;
using System.Globalization;
using System.Windows.Data;
using Dimesoft.CoachAssistant.Domain.Models;

namespace Dimesoft.CoachAssistant.Converters
{
    public class SportTypeToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var asType = (SportType)Enum.Parse(typeof(SportType), value.ToString(), true);


            switch (asType)
            {
                case SportType.Baseball:
                    return "../Images/BaseballBackground.jpg";

                case SportType.Basketball:
                    return "../Images/BasketballBackground.jpg";

                case SportType.Soccer:
                    return "../Images/SoccerBackground.jpg";

                default:
                    return "../Images/SoccerBackground.jpg";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}