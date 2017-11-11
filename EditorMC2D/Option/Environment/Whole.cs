using System;


namespace EditorMC2D.Option.Environment
{
    [Serializable]
    public class Whole
    {
        public const string ColorThemeBlue = "blue";
        public const string ColorThemeLightColor = "lightColor";
        public const string ColorThemeDarkColor = "darkColor";

        public string ColorTheme = ColorThemeLightColor;
        public string FilePath = "../MC2D.exe";
        public bool IsRunMCAS = true;
    }
}
