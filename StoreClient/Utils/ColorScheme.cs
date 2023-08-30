using System.Windows.Media;

namespace StoreClient.Utils
{
    internal static class ColorScheme
    {
        public static Color PrimaryColor { get; set; } = Color.FromRgb(232, 245, 255); // #e8f5ff
        public static Color SecondaryColor { get; set; } = Color.FromRgb(255,255,255); // None
        public static Color ButtonStanbyColor { get; set; } = Color.FromRgb(66, 165, 245); // #42a5f5
        public static Color ButtonPressedColor { get; set; } = Color.FromRgb(255, 255, 255); // None
        public static Color ButtonHoverColor { get; set; } = Color.FromRgb(255, 255, 255); // None
    }
}
