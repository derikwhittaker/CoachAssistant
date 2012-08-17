using System;
using System.Globalization;
using System.Windows.Data;
using Dimesoft.CoachAssistant.Domain.Models;

namespace Dimesoft.CoachAssistant.Converters
{
    public class SportTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var asType = (SportType) Enum.Parse(typeof(SportType), value.ToString(), true);

            
            switch (asType)
            {
                case SportType.Baseball:
                    return "Images/BaseballGlyph.png";

                case SportType.Basketball:
                    return "Images/BasketballGlyph.png";

                case SportType.Soccer:
                    return "Images/SoccerGlyph.png";

                default:
                    return "Images/SoccerGlyph.png";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
