using MC2DUtil;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace EditorMC2D.Option.Environment
{
    public enum DisplayItemColorState
    {
        /// <summary>
        /// 通常
        /// </summary>
        Normal,
        /// <summary>
        /// 現行カラー
        /// </summary>
        Current,
        /// <summary>
        /// カラーが決まっていない
        /// </summary>
        None,
        /// <summary>
        /// 参照
        /// </summary>
        Reference
    }
    /// <summary>
    /// 各表示項目情報
    /// </summary>
    [Serializable]
    public class DisplayItem
    {
        /// <summary>
        /// デフォルト前景色
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        internal Color DefaultFrontColor = ColorTranslator.FromHtml("#dcdcdc");
        /// <summary>
        /// デフォルト背景色
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        internal Color DefaultBackgroundColor = ColorTranslator.FromHtml("#1e1e1e");
        /// <summary>
        /// 前景色
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Color FrontColor = ColorTranslator.FromHtml("#dcdcdc");
        /// <summary>
        /// 前景色の状態
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public DisplayItemColorState FrontColorState = DisplayItemColorState.Normal;
        [System.Xml.Serialization.XmlIgnore]
        public string FrontColorRef = "";
        /// <summary>
        /// 背景色
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Color BackgroundColor = ColorTranslator.FromHtml("#1e1e1e");
        /// <summary>
        /// 背景色の状態
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public DisplayItemColorState BackgroundColorState = DisplayItemColorState.Normal;
        [System.Xml.Serialization.XmlIgnore]
        public string BackgroundColorRef = "";
        /// <summary>
        /// 太文字が有効か？
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public bool boldfaceValid = true;
        /// <summary>
        /// 太字か？
        /// </summary>
        public bool IsBoldface = false;


        [XmlElement("FrontColor")]
        public string XML_FrontColor
        {
            get { return ColorTranslator.ToHtml(FrontColor); }
            set { FrontColor = ColorTranslator.FromHtml(value); }
        }

        [XmlElement("BackgroundColor")]
        public string XML_BackgroundColor
        {
            get { return ColorTranslator.ToHtml(BackgroundColor); }
            set { BackgroundColor = ColorTranslator.FromHtml(value); }
        }
    };

    /// <summary>
    /// 設定の表示
    /// </summary>
    [Serializable]
    public class PrintSetting
    {
        public class SimpleData
        {
            public string name = "";
            public string bgColor = "";
            public string frColor = "";
            public bool boldfaceValid = true;
            public SimpleData(string a, string b, string c, bool d=true) { name = a;bgColor = b;frColor = c;boldfaceValid = d; }
        }
        /// <summary>
        /// 設定の表示名
        /// </summary>
        public string DisplaySettings = "";
        /// <summary>
        /// フォントの種類名
        /// </summary>
        public string FontName = "MS Gothic";
        /// <summary>
        /// フォントサイズ
        /// </summary>
        public int FontSize = 10;
        public SerializableDictionary<string, DisplayItem> DisplayItems;

        /// <summary>
        /// 設定の表示名
        /// </summary>
        public string Name { get { return DisplaySettings; } }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="displaySettings">表示の設定名</param>
        public PrintSetting(string displaySettings)
        {
            DisplayItems = new SerializableDictionary<string, DisplayItem>();
            DisplaySettings = displaySettings;
        }
        public PrintSetting()
        {
            DisplayItems = new SerializableDictionary<string, DisplayItem>();
        }

        /// <summary>
        /// 背景色の初期化
        /// </summary>
        /// <param name="name">表示項目名</param>
        /// <param name="bgColor">背景カラー</param>
        /// <param name="frColor">フロントカラー</param>
        /// <param name="boldfaceValid">太文字が有効か？</param>
        private void AddCreateInitDisplayItem(string name, string bgColor, string frColor, bool boldfaceValid = true)
        {
            DisplayItems.Add(name, new DisplayItem());
            var item = DisplayItems[name];
            if (bgColor == "Current")
            {
                item.BackgroundColorState = DisplayItemColorState.Current;
                item.DefaultBackgroundColor = Color.FromArgb(0, 0, 0, 0);
                item.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            }
            else if (bgColor == "None")
            {
                item.BackgroundColorState = DisplayItemColorState.None;
                item.DefaultBackgroundColor = Color.FromArgb(0, 0, 0, 0);
                item.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            }
            else if (DisplayItems.ContainsKey(bgColor))
            {
                var c = DisplayItems[bgColor];
                item.BackgroundColorState = DisplayItemColorState.Reference;
                item.BackgroundColorRef = bgColor;
                item.DefaultBackgroundColor = c.DefaultBackgroundColor;
                item.BackgroundColor = c.BackgroundColor;
            }
            else
            {
                item.DefaultBackgroundColor = ColorTranslator.FromHtml(bgColor);
                item.BackgroundColor = ColorTranslator.FromHtml(bgColor);
            }
            //-----------------------------
            if (frColor == "Current")
            {
                item.FrontColorState = DisplayItemColorState.Current;
                item.DefaultFrontColor = Color.FromArgb(0, 0, 0, 0);
                item.FrontColor = Color.FromArgb(0, 0, 0, 0);
            }
            else if (frColor == "None")
            {
                item.FrontColorState = DisplayItemColorState.None;
                item.DefaultFrontColor = Color.FromArgb(0, 0, 0, 0);
                item.FrontColor = Color.FromArgb(0, 0, 0, 0);
            }
            else if (DisplayItems.ContainsKey(frColor))
            {
                var c = DisplayItems[frColor];
                item.FrontColorState = DisplayItemColorState.Reference;
                item.FrontColorRef = frColor;
                item.DefaultFrontColor = c.DefaultFrontColor;
                item.FrontColor = c.FrontColor;
            }
            else
            {
                item.DefaultFrontColor = ColorTranslator.FromHtml(frColor);
                item.FrontColor = ColorTranslator.FromHtml(frColor);
            }
            item.boldfaceValid = boldfaceValid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">対象</param>
        /// <param name="item">対象</param>
        /// <param name="currentData">現行</param>
        /// <param name="currentItem">現行</param>
        protected void ReadXmlAfterCommit(SimpleData v, DisplayItem item, SimpleData currentData, DisplayItem currentItem)
        {
            if (v.bgColor == "Current")
            {
                item.BackgroundColorState = DisplayItemColorState.Current;
                item.DefaultBackgroundColor = ColorTranslator.FromHtml(currentData.bgColor);
                item.BackgroundColor = currentItem.BackgroundColor;
            }
            else if (v.bgColor == "None")
            {
                item.BackgroundColorState = DisplayItemColorState.None;
                item.DefaultBackgroundColor = Color.FromArgb(0, 0, 0, 0);
                item.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            }
            else if (DisplayItems.ContainsKey(v.bgColor))
            {
                var r = DisplayItems[v.bgColor];
                item.BackgroundColorState = DisplayItemColorState.Reference;
                item.BackgroundColorRef = v.bgColor;
                item.DefaultBackgroundColor = r.DefaultBackgroundColor;
                item.BackgroundColor = r.BackgroundColor;
            }
            else
            {
                item.DefaultBackgroundColor = ColorTranslator.FromHtml(v.bgColor);
            }
            //-----------------------------
            if (v.frColor == "Current")
            {
                item.FrontColorState = DisplayItemColorState.Current;
                item.DefaultFrontColor = ColorTranslator.FromHtml(currentData.frColor);
                item.FrontColor = currentItem.FrontColor;
            }
            else if (v.frColor == "None")
            {
                item.FrontColorState = DisplayItemColorState.None;
                item.DefaultFrontColor = Color.FromArgb(0, 0, 0, 0);
                item.FrontColor = Color.FromArgb(0, 0, 0, 0);
            }
            else if (DisplayItems.ContainsKey(v.frColor))
            {
                var r = DisplayItems[v.frColor];
                item.FrontColorState = DisplayItemColorState.Reference;
                item.FrontColorRef = v.frColor;
                item.DefaultFrontColor = r.DefaultFrontColor;
                item.FrontColor = r.FrontColor;
            }
            else
            {
                item.DefaultFrontColor = ColorTranslator.FromHtml(v.frColor);
            }
        }
        #region テキストエディタ
        /// <summary>
        /// テキストエディターダーク
        /// </summary>
        /// <returns></returns>
        protected static SimpleData[] CreateTextEditorDarkSimpleDatas()
        {
            return new SimpleData[]{
                new SimpleData("TextFormat", "#1e1e1e", "#dcdcdc"),
                new SimpleData("SelectedText", "#3399ff", "Current"),
                new SimpleData("TextNotSelectedActive", "#bfcddb", "Current"),
                new SimpleData("IndicatorMargin", "#333333", "Current"),
                new SimpleData("LineNumber", "#1e1e1e", "#2b91af"),
                new SimpleData("DisplaySpace", "Current", "#2b91af"),
                new SimpleData("AS_UserKeyword", "#1e1e1e", "#569cd6"),
                new SimpleData("Comment", "#1e1e1e", "#57a64a"),
                new SimpleData("CompileError", "Current", "#0000ff"),
                new SimpleData("BreakpointEnabled", "#8c2f2f", "#ffffff"),
                new SimpleData("SyntaxError", "Current", "#ff0000", false),
                new SimpleData("Number", "#1e1e1e", "#b5cea8"),
                new SimpleData("String", "#1e1e1e", "#d69d85"),
                new SimpleData("PreprocessorKeyword", "#1e1e1e", "#9b9b9b"),
                new SimpleData("PreprocessorText", "#1e1e1e", "#ffffff"),
                new SimpleData("Operator", "#1e1e1e", "#dcdcdc"),
                new SimpleData("Identifier", "#1e1e1e", "#dcdcdc"),
                new SimpleData("Regex", "#1e1e1e", "#a31515"),
                new SimpleData("Doxygen", "Current", "#608b4e"),
                new SimpleData("DoxygenCommand", "Current", "#4169e1"),
                new SimpleData("DoxygenErrKey", "Current", "#800000"),
                new SimpleData("DoxygenFirstArg", "Current", "#008080"),
                new SimpleData("DoxygenSecondArg", "Current", "#808080"),
                new SimpleData("DoxygenThirdArg", "Current", "#008080"),
                new SimpleData("KeySelectedRowCodeAnalysis", "#ffff00", "#ff0000"),
                new SimpleData("HighlightingMatchingItems", "#f4a721", "#dcdcdc"),
                new SimpleData("HighlightedReference", "#dbe0cc", "#dcdcdc")
            };
        }
        /// <summary>
        /// テキストエディター ライト
        /// </summary>
        /// <returns></returns>
        protected static SimpleData[] CreateTextEditorLightSimpleDatas()
        {
            return new SimpleData[]{
                new SimpleData("TextFormat", "#ffffff", "#000000"),
                new SimpleData("SelectedText", "#3399ff", "Current"),
                new SimpleData("TextNotSelectedActive", "#bfcddb", "Current"),
                new SimpleData("IndicatorMargin", "#e6e7e8", "Current"),
                new SimpleData("LineNumber", "#ffffff", "#2b91af"),
                new SimpleData("DisplaySpace", "Current", "#2b91af"),
                new SimpleData("AS_UserKeyword", "#ffffff", "#0000ff"),
                new SimpleData("Comment", "#ffffff", "#008000"),
                new SimpleData("CompileError", "Current", "#0000ff"),
                new SimpleData("BreakpointEnabled", "#963a46", "#ffffff"),
                new SimpleData("SyntaxError", "Current", "#ff0000", false),
                new SimpleData("Number", "#ffffff", "#000000"),
                new SimpleData("String", "#ffffff", "#a31515"),
                new SimpleData("PreprocessorKeyword", "#ffffff", "#a0a0a0"),
                new SimpleData("PreprocessorText", "#ffffff", "#000000"),
                new SimpleData("Operator", "#ffffff", "#000000"),
                new SimpleData("Identifier", "#ffffff", "#000000"),
                new SimpleData("Regex", "#ffffff", "#800000"),
                new SimpleData("Doxygen", "Current", "#008000"),
                new SimpleData("DoxygenCommand", "Current", "#4169e1"),
                new SimpleData("DoxygenErrKey", "Current", "#800000"),
                new SimpleData("DoxygenFirstArg", "Current", "#008080"),
                new SimpleData("DoxygenSecondArg", "Current", "#808080"),
                new SimpleData("DoxygenThirdArg", "Current", "#008080"),
                new SimpleData("KeySelectedRowCodeAnalysis", "#ffff00", "#ff0000"),
                new SimpleData("HighlightingMatchingItems", "#f4a721", "Current"),
                new SimpleData("HighlightedReference", "#dbe0cc", "#000000")
            };
        }
        public void ReadXmlTextEditorAfterCommit(string theme = APPConfig.ColorThemeLightColor)
        {
            // TextEditor
            SimpleData txtf=null;
            var t = theme == APPConfig.ColorThemeLightColor ? CreateTextEditorLightSimpleDatas() : CreateTextEditorDarkSimpleDatas();
            foreach (var s in t)
            {
                if (s.name == "TextFormat")
                {
                    txtf = s;
                    break;
                }
            }
            var c = DisplayItems["TextFormat"];

            foreach (var v in t)
            {
                ReadXmlAfterCommit(v, DisplayItems[v.name], txtf, c);
            }
        }
        /// <summary>
        /// テキストエディターのインスタンスを作成する
        /// </summary>
        /// <param name="theme">配色テーマ名</param>
        /// <returns></returns>
        public static PrintSetting CreateTextEditor(string theme = APPConfig.ColorThemeLightColor)
        {
            PrintSetting ins = new PrintSetting("TextEditor");
            if( theme == APPConfig.ColorThemeDarkColor)
            {
                foreach(var v in CreateTextEditorDarkSimpleDatas())
                {
                    ins.AddCreateInitDisplayItem(v.name, v.bgColor, v.frColor, v.boldfaceValid);
                }
            }
            else
            {
                foreach (var v in CreateTextEditorLightSimpleDatas())
                {
                    ins.AddCreateInitDisplayItem(v.name, v.bgColor, v.frColor, v.boldfaceValid);
                }
            }
            return ins;
        }
        #endregion




        #region 出力ウィンドウ
        /// <summary>
        /// テキストエディターダーク
        /// </summary>
        /// <returns></returns>
        protected static SimpleData[] CreateOutputWindowDarkSimpleDatas()
        {
            return new SimpleData[]{
                new SimpleData("TextFormat", "#e6e7e8", "#1e1e1e"),
                new SimpleData("SelectedText", "#0a246a", "#000000"),
                new SimpleData("TextNotSelectedActive", "#bbbdbf", "#000000"),
                new SimpleData("CurrentLocationList", "#3399ff", "#ffffff"),
                new SimpleData("UrlHyperLink", "None", "#0000ff", false),
                new SimpleData("HeadingOutput", "None", "#3399ff", false),
                new SimpleData("ErrorOutput", "None", "#ff0000", false),
                new SimpleData("OutputDetails", "None", "#939393", false)
            };
        }
        /// <summary>
        /// テキストエディター ライト
        /// </summary>
        /// <returns></returns>
        protected static SimpleData[] CreateOutputWindowLightSimpleDatas()
        {
            return new SimpleData[]{
                new SimpleData("TextFormat", "#252526", "#f1f1f1"),
                new SimpleData("SelectedText", "#2b537d", "#f1f1f1"),
                new SimpleData("TextNotSelectedActive", "#2f2f33", "#f1f1f1"),
                new SimpleData("CurrentLocationList", "#ff0000", "#f1f1f1"),
                new SimpleData("UrlHyperLink", "None", "#569cd6", false),
                new SimpleData("HeadingOutput", "None", "#007acc", false),
                new SimpleData("ErrorOutput", "None", "#ff0000", false),
                new SimpleData("OutputDetails", "None", "#939393", false)
            };
        }
        public void ReadXmlOutputWindowAfterCommit(string theme = APPConfig.ColorThemeLightColor)
        {
            // Output Window
            SimpleData txtf = null;
            var t = theme == APPConfig.ColorThemeLightColor ? CreateOutputWindowLightSimpleDatas() : CreateOutputWindowDarkSimpleDatas();
            foreach (var s in t)
            {
                if (s.name == "TextFormat")
                {
                    txtf = s;
                    break;
                }
            }
            var c = DisplayItems["TextFormat"];

            foreach (var v in t)
            {
                ReadXmlAfterCommit(v, DisplayItems[v.name], txtf, c);
            }
        }

        /// <summary>
        /// 出力ウィンドウのインスタンスを作成する
        /// </summary>
        /// <param name="theme">配色テーマ名</param>
        /// <returns></returns>
        public static PrintSetting CreateOutputWindow(string theme = APPConfig.ColorThemeLightColor)
        {
            PrintSetting ins = new PrintSetting("TextEditor");
            if (theme == APPConfig.ColorThemeDarkColor)
            {
                foreach (var v in CreateOutputWindowDarkSimpleDatas())
                {
                    ins.AddCreateInitDisplayItem(v.name, v.bgColor, v.frColor, v.boldfaceValid);
                }
            }
            else
            {
                foreach (var v in CreateOutputWindowLightSimpleDatas())
                {
                    ins.AddCreateInitDisplayItem(v.name, v.bgColor, v.frColor, v.boldfaceValid);
                }
            }
            return ins;
        }
        #endregion





        #region 検索のインスタンス
        /// <summary>
        /// 検索のインスタンスダーク
        /// </summary>
        /// <returns></returns>
        protected static SimpleData[] CreateSearchResultWindowDarkSimpleDatas()
        {
            return new SimpleData[]{
                new SimpleData("TextFormat", "#252526", "#f1f1f1"),
                new SimpleData("SelectedText", "#2b537d", "TextFormat"),
                new SimpleData("TextNotSelectedActive", "#2f2f33", "TextFormat"),
                new SimpleData("CurrentLocationList", "#3399ff", "#f1f1f1"),
            };
        }
        /// <summary>
        /// 検索のインスタンス ライト
        /// </summary>
        /// <returns></returns>
        protected static SimpleData[] CreateSearchResultWindowLightSimpleDatas()
        {
            return new SimpleData[]{
                new SimpleData("TextFormat", "#e6e7e8", "#1e1e1e"),
                new SimpleData("SelectedText", "#0a246a", "TextFormat"),
                new SimpleData("TextNotSelectedActive", "#bbbdbf", "TextFormat"),
                new SimpleData("CurrentLocationList", "#3399ff", "#ffffff"),
            };
        }
        public void ReadXmlSearchResultWindowAfterCommit(string theme = APPConfig.ColorThemeLightColor)
        {
            // Output Window
            SimpleData txtf = null;
            var t = theme == APPConfig.ColorThemeLightColor ? CreateSearchResultWindowLightSimpleDatas() : CreateSearchResultWindowDarkSimpleDatas();
            foreach (var s in t)
            {
                if (s.name == "TextFormat")
                {
                    txtf = s;
                    break;
                }
            }
            var c = DisplayItems["TextFormat"];

            foreach (var v in t)
            {
                ReadXmlAfterCommit(v, DisplayItems[v.name], txtf, c);
            }
        }
        /// <summary>
        /// 検索のインスタンスを作成する
        /// </summary>
        /// <param name="theme">配色テーマ名</param>
        /// <returns></returns>
        public static PrintSetting CreateSearchResultWindow(string theme = APPConfig.ColorThemeLightColor)
        {
            PrintSetting ins = new PrintSetting("SearchResultWindow");
            if (theme == APPConfig.ColorThemeDarkColor) {
                foreach (var v in CreateSearchResultWindowDarkSimpleDatas())
                {
                    ins.AddCreateInitDisplayItem(v.name, v.bgColor, v.frColor, v.boldfaceValid);
                }
            }
            else
            {
                foreach (var v in CreateSearchResultWindowLightSimpleDatas())
                {
                    ins.AddCreateInitDisplayItem(v.name, v.bgColor, v.frColor, v.boldfaceValid);
                }
            }
            return ins;
        }
        #endregion

    };

    /// <summary>
    /// フォントと色
    /// </summary>
    [Serializable]
    public class FontAndColor
    {
        /// <summary>
        /// 設定の表示名
        /// </summary>
        public string ColorSchemeTheme = "";
        /// <summary>
        /// 設定の表示群
        /// </summary>
        public SerializableDictionary<string, PrintSetting> PrintSettings;

        /// <summary>
        /// 設定の表示名
        /// </summary>
        public string Theme { get { return ColorSchemeTheme; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FontAndColor(string colorSchemeTheme = APPConfig.ColorThemeLightColor)
        {
            PrintSettings = new SerializableDictionary<string, PrintSetting>();
            ColorSchemeTheme = colorSchemeTheme;

            PrintSettings.Add("TextEditor", PrintSetting.CreateTextEditor(colorSchemeTheme));
            PrintSettings.Add("OutputWindow", PrintSetting.CreateOutputWindow(colorSchemeTheme));
            PrintSettings.Add("SearchResultWindow", PrintSetting.CreateSearchResultWindow(colorSchemeTheme));

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FontAndColor() : this(APPConfig.ColorThemeLightColor) { }
    }
}
