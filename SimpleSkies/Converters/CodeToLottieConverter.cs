using SkiaSharp.Extended.UI.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSkies.Converters
{
    public class CodeToLottieConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var code = (int)value;
            var lottieImageSource = new SKFileLottieImageSource();

            switch (code)
            {
                case 0:
                    lottieImageSource.File = "sunny.json";
                    return lottieImageSource;
                case 2:
                    lottieImageSource.File = "partlycloudy.json";
                    break;
                case 3:
                    lottieImageSource.File = "mist.json";
                    break;
                case 45:
                case 48:
                    lottieImageSource.File = "mist.json";
                    break;
                case 51:
                case 53:
                case 55:
                    lottieImageSource.File = "rain.json";
                    break;
                case 56:
                case 57:
                    lottieImageSource.File = "snowy.json";
                    break;
                case 61:
                case 63:
                case 65:
                    lottieImageSource.File = "rain.json";
                    break;
                case 66:
                case 67:
                    lottieImageSource.File = "snowy.json";
                    break;
                case 71:
                case 73:
                case 75:
                    lottieImageSource.File = "snowy.json";
                    break;
                case 77:
                    lottieImageSource.File = "snowy.json";
                    break;
                case 80:
                case 81:
                case 82:
                    lottieImageSource.File = "shower.json";
                    break;
                case 85:
                case 86:
                    lottieImageSource.File = "snowy.json";
                    break;
                case 95:
                    lottieImageSource.File = "thunder.json";
                    break;
                case 96:
                case 99:
                    lottieImageSource.File = "stormnight.json";
                    break;
                default:
                    lottieImageSource.File = "partlycloudy.json";
                    break;
            }

            return lottieImageSource;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
