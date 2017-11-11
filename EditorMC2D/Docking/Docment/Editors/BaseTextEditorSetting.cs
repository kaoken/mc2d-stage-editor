using EditorMC2D.Option;
using EditorMC2D.Option.Environment;
using MC2DUtil;
using ScintillaNET;

namespace EditorMC2D.Docking.Docment.Editors
{
    public class BaseTextEditorSetting
    {
        /// <summary>
        /// 行番号を表示する余白を変更します。
        /// </summary>
        protected const int NUMBER_MARGIN = 1;

        /// <summary>
        /// ブックマーク/ブレークポイントに表示する余白を変更します。
        /// </summary>
        protected const int BOOKMARK_MARGIN = 2;
        protected const int BOOKMARK_MARKER = 2;

        /// <summary>
        /// これを、コードの折りたたみツリー (+/-) で表示する余白に変更します。
        /// </summary>
        protected const int FOLDING_MARGIN = 3;

        /// <summary>
        /// これをtrueに設定すると、コードの折り返し（[+]および[ - ]ボタンの丸いボタンが表示されます）
        /// </summary>
        protected const bool CODEFOLDING_CIRCULAR = false;

        protected Scintilla TextArea;
        protected APPConfig Config;
        protected SerializableDictionary<string, DisplayItem> Items;


        /// <summary>
        /// 色の初期化
        /// </summary>
        protected void InitColors()
        {
            TextArea.SetSelectionBackColor(true, Items["SelectedText"].BackgroundColor);
        }

        /// <summary>
        /// 行番号マージンの初期化
        /// </summary>
        protected void InitNumberMargin()
        {
            var tx = Config.GetTextEditorTheme("TextEditor");


            TextArea.Styles[Style.LineNumber].BackColor = Items["LineNumber"].BackgroundColor;
            TextArea.Styles[Style.LineNumber].ForeColor = Items["LineNumber"].FrontColor;
            TextArea.Styles[Style.IndentGuide].BackColor = Items["LineNumber"].BackgroundColor;
            TextArea.Styles[Style.IndentGuide].ForeColor = Items["LineNumber"].FrontColor;

            var nums = TextArea.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            //e.MarginClick += TextArea_MarginClick;
        }


        /// <summary>
        /// ブックマーク/ブレークポイント
        /// </summary>
        protected void InitBookmarkMargin(PrintSetting tx, DisplayItem item)
        {

            //TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
            var margin = TextArea.Margins[BOOKMARK_MARGIN];
            margin.Width = 20;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1 << BOOKMARK_MARKER);
            //margin.Cursor = MarginCursor.Arrow;

            var marker = TextArea.Markers[BOOKMARK_MARKER];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(item.BackgroundColor);
            marker.SetForeColor(item.FrontColor);
            marker.SetAlpha(100);

        }


        /// <summary>
        /// 折りたたみ
        /// </summary>
        protected void InitCodeFolding()
        {

            TextArea.SetFoldMarginColor(true, Items["TextFormat"].BackgroundColor);
            TextArea.SetFoldMarginHighlightColor(true, Items["TextFormat"].BackgroundColor);

            // コードの折り畳みを有効にする
            TextArea.SetProperty("fold", "1");
            TextArea.SetProperty("fold.compact", "1");

            // 折り返し記号を表示する余白を構成する
            TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
            TextArea.Margins[FOLDING_MARGIN].Width = 20;

            // すべての折りたたみマーカーの色を設定する
            for (int i = 25; i <= 31; i++)
            {
                TextArea.Markers[i].SetForeColor(Items["TextFormat"].BackgroundColor); // [+]と[-]のスタイル
                TextArea.Markers[i].SetBackColor(Items["TextFormat"].FrontColor); // [+]と[-]のスタイル
            }

            // 折りたたみマーカーにそれぞれの記号を設定する
            TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // 自動折り畳みを有効にする
            TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="st"></param>
        /// <param name="item"></param>
        protected void SetStyle(int idx, DisplayItem item)
        {
            var tx = Config.GetTextEditorTheme("TextEditor");

            var st = TextArea.Styles[idx];
            if (item.FrontColorState == DisplayItemColorState.Current)
                st.ForeColor = Items["TextFormat"].FrontColor;
            else
                st.ForeColor = item.FrontColor;

            if (item.BackgroundColorState == DisplayItemColorState.Current)
                st.BackColor = Items["TextFormat"].BackgroundColor;
            else
                st.BackColor = item.BackgroundColor;

            st.Bold = item.IsBoldface;
            st.Font = tx.FontName;
            st.Size = tx.FontSize;
        }
    }
}
