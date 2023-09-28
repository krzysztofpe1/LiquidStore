using System;
using System.Windows.Media;

namespace StoreClient.Utils
{
    public static class ColorScheme
    {
        public static SolidColorBrush PrimaryColor { get; set; } = GenerateBrushFromHex("f6dbc6");
        public static SolidColorBrush SecondaryColor { get; set; } = GenerateBrushFromHex("ac9c8b");
        public static SolidColorBrush PrimaryTextColor { get; set; } = GenerateBrushFromHex("fff9f3");
        public static SolidColorBrush ButtonStanbyColor { get; set; } = GenerateBrushFromHex("ac9c8b");
        public static SolidColorBrush ButtonStanbyTextColor { get; set; } = GenerateBrushFromHex("fff9f3");
        public static SolidColorBrush ButtonPressedColor { get; set; } = GenerateBrushFromHex("827567");
        public static SolidColorBrush ButtonPressedTextColor { get; set; } = GenerateBrushFromHex("fff9f3");
        public static SolidColorBrush ButtonHoverColor { get; set; } = GenerateBrushFromHex("918476");
        public static SolidColorBrush ButtonHoverTextColor { get; set; } = GenerateBrushFromHex("fff9f3");
        public static SolidColorBrush GenerateBrushFromHex(string hexColor)
        {
            if (hexColor.Length != 6) return null;
            byte r = Convert.ToByte(hexColor.Substring(0, 2), 16);
            byte g = Convert.ToByte(hexColor.Substring(2, 2), 16);
            byte b = Convert.ToByte(hexColor.Substring(4, 2), 16);
            Color color = Color.FromRgb(r, g, b);
            return new SolidColorBrush(color);
        }
    }
}
