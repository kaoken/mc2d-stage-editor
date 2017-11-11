using EditorMC2D.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MC2DUtil.graphics;

namespace EditorMC2D.Option.Environment
{
    public partial class FormFontAndColor : Form
    {
        #region メンバ変数
        private Dictionary<string, Dictionary<string, string>> m_outputItems = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, string> m_currentOptItem = null;
        private string m_currentOptName = null;
        private PrintSetting m_currentPS;

        private class DisplayItemItem
        {
            public string Text="";
            public string Key="";
            public DisplayItemItem(string key, string txt) { Text = txt; Key = key; }
            public override string ToString(){ return this.Text; }
        };
        List<string> m_constPitch = new List<string>();
        private CommonMC2D m_comCore;
        private APPConfig m_config;
        private FontAndColor m_FontAndColor;
        private DisplayItem m_correntDItem;
        #endregion


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="comCore"></param>
        /// <param name="config"></param>
        public FormFontAndColor(CommonMC2D comCore,APPConfig config)
        {
            InitializeComponent();
            m_comCore = comCore;
            m_config = config;
            m_FontAndColor = config.FontAndColors[config.Whole.ColorTheme];
        }

        ~FormFontAndColor()
        {
            m_comCore = null;
        }

        private void FormFontAndColor_Load(object sender, EventArgs e)
        {
            //-------------------------------------
            // 出力の表示
            //-------------------------------------
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            m_outputItems.Add("TextEditor", tmp);
            {
                objCBPrintSetting.Items.Add(new DisplayItemItem("TextEditor", "テキストエディター"));
                tmp.Add("TextFormat", "テキスト形式");
                tmp.Add("SelectedText", "選択されたテキスト");
                tmp.Add("TextNotSelectedActive", "選択されたアクティブでないテキスト");
                tmp.Add("IndicatorMargin", "インジケーターマージン");
                tmp.Add("LineNumber", "行番号");
                tmp.Add("DisplaySpace", "スペースの表示");
                tmp.Add("AS_UserKeyword", "ASユーザーキーワード");
                tmp.Add("KeySelectedRowCodeAnalysis", "コード分析の選択されたキー行");
                tmp.Add("Comment", "コメント");
                tmp.Add("CompileError", "コンパイルエラー");
                tmp.Add("PreprocessorKeyword", "プリプロセッサ キーワード");
                tmp.Add("PreprocessorText", "プリプロセッサ テキスト");
                tmp.Add("BreakpointEnabled", "ブレークポイント(有効)");
                tmp.Add("HighlightingMatchingItems", "一致項目の強調表示");
                tmp.Add("HighlightedReference", "強調表示された参照");
                tmp.Add("Operator", "演算子");
                tmp.Add("SyntaxError", "構文エラー");
                tmp.Add("Identifier", "識別子");
                tmp.Add("Number", "数字");
                tmp.Add("Regex", "正規表現");
                tmp.Add("Doxygen", "Doxygenコメント");
                tmp.Add("DoxygenCommand", "Doxygen コマンド");
                tmp.Add("DoxygenErrKey", "Doxygen エラーキー");
                tmp.Add("DoxygenFirstArg", "Doxygen 第一引数");
                tmp.Add("DoxygenSecondArg", "Doxygen 第二引数");
                tmp.Add("DoxygenThirdArg", "Doxygen 第三引数");
                tmp.Add("String", "文字列");
            }
            tmp = new Dictionary<string, string>();
            m_outputItems.Add("OutputWindow", tmp);
            {
                objCBPrintSetting.Items.Add(new DisplayItemItem("OutputWindow", "出力ウィンドウ"));
                tmp.Add("TextFormat", "テキスト形式");
                tmp.Add("SelectedText", "選択されたテキスト");
                tmp.Add("TextNotSelectedActive", "選択されたアクティブでないテキスト");
                tmp.Add("CurrentLocationList", "現在の一覧の場所");
                tmp.Add("UrlHyperLink", "URL ハイパーリンク");
                tmp.Add("HeadingOutput", "見出しの出力");
                tmp.Add("ErrorOutput", "エラーの出力");
                tmp.Add("OutputDetails", "詳細の出力");
            }
            tmp = new Dictionary<string, string>();
            m_outputItems.Add("SearchResultWindow", tmp);
            {
                objCBPrintSetting.Items.Add(new DisplayItemItem("SearchResultWindow", "検索結果ウィンドウ"));
                tmp.Add("TextFormat", "テキスト形式");
                tmp.Add("SelectedText", "選択されたテキスト");
                tmp.Add("TextNotSelectedActive", "選択されたアクティブでないテキスト");
                tmp.Add("CurrentLocationList", "現在の一覧の場所");
            }

            //-------------------------------------
            // フォントの種類作成
            //-------------------------------------
            foreach (FontFamily item in FontFamily.Families)
            {
                if (item.IsStyleAvailable(FontStyle.Regular))
                {
                    // 固定ピッチか判定
                    float diff;
                    using (Font font = new Font(item, 16))
                    {
                        diff = TextRenderer.MeasureText("WWW", font).Width - TextRenderer.MeasureText("...", font).Width;
                    }
                    if (Math.Abs(diff) < float.Epsilon * 2)
                    {
                        m_constPitch.Add(item.Name);
                    }
                    objCBFontKind.Items.Add(item.Name);
                }
            }
            //-------------------------------------
            // フォントのサイズ
            //-------------------------------------
            for(int i=6;i<25;++i)
                objCBFontSize.Items.Add(i.ToString());
            //-------------------------------------
            // 
            //-------------------------------------
            objCBFront.SelectedIndex = 1;
            objCBBackground.SelectedIndex = 1;
            objCBPrintSetting.SelectedIndex = 0;
        }
        private void objLBItemAdd(string txt, string key)
        {
            DisplayItemItem item = new DisplayItemItem(txt, key);
            objLBItem.Items.Add(item);
        }

        
        #region 出力の表示コンボボックス
        /// <summary>
        /// 出力の表示コンボボックスの項目変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCBPrintSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayItemItem dii = (DisplayItemItem)objCBPrintSetting.SelectedItem;
            m_currentOptItem = m_outputItems[dii.Key];
            m_currentOptName = dii.Key;
            m_currentPS = m_FontAndColor.PrintSettings[dii.Key];

            // リストの作り直し
            objLBItem.Items.Clear();
            foreach (var v in m_currentOptItem)
            {
                objLBItem.Items.Add(new DisplayItemItem(v.Key, v.Value));
            }

            // フォント名のセット
            bool isFind = false;
            int idxMeiryoUI = 0, i = 0;
            foreach (string fnt in objCBFontKind.Items)
            {
                if (fnt == "Meiryo UI")
                    idxMeiryoUI = i;
                if (fnt == m_currentPS.FontName)
                {
                    isFind = true;
                    objCBFontKind.SelectedIndex = i;
                    break;
                }
                else if (fnt == "ＭＳ ゴシック")
                {
                    isFind = true;
                    objCBFontKind.SelectedIndex = i;
                    break;
                }

                ++i;
            }
            if (!isFind)
            {
                objCBFontKind.SelectedIndex = idxMeiryoUI;
            }
            // フォントサイズのセット
            objCBFontSize.Text = m_currentPS.FontSize + "";
            //
            objLBItem.SelectedIndex = 0;
        }
        #endregion


        #region フォント関係
        /// <summary>
        /// フォントの種類を変更した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCBFontKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            if (cmb.SelectedIndex == -1) return;
            m_currentPS.FontName = cmb.Items[cmb.SelectedIndex].ToString();
            objPBSampleText.Refresh();
        }
        /// <summary>
        /// フォントの種類項目を描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCBFontKind_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
            //背景を描画する
            //項目が選択されている時は強調表示される
            e.DrawBackground();
            ComboBox cmb = (ComboBox)sender;

            FontStyle fStyle = FontStyle.Regular;
            if (m_constPitch.Contains(cmb.Items[e.Index].ToString()))
                fStyle = FontStyle.Bold;

            //文字列を描画する
            e.Graphics.DrawString(cmb.Items[e.Index].ToString(),
                                  new Font("Meiryo UI", 9, fStyle),
                                  new SolidBrush(Color.Black),
                                  e.Bounds.X,
                                  e.Bounds.Y);

            //フォーカスを示す四角形を描画
            e.DrawFocusRectangle();

        }
        /// <summary>
        /// フォントサイズのコンボボックスのフォーカスを失った
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCBFontSize_Check(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(objCBFontSize.Text, out i))
            {
                m_currentPS.FontSize = i;
            }
            else
            {
                m_currentPS.FontSize = 10;
            }
            if (m_currentPS.FontSize > 128)
                m_currentPS.FontSize = 128;
            if (m_currentPS.FontSize <= 5)
                m_currentPS.FontSize = 10;

            objCBFontSize.Text = m_currentPS.FontSize+"";
            objPBSampleText.Refresh();
        }
        #endregion


        /// <summary>
        /// コンボボックスのカラー選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomColorCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
            Brush[] aColors = {
                 Brushes.Black, Brushes.Black, Brushes.Black, Brushes.White, Brushes.Brown,
                 Brushes.Green, Brushes.Olive, Brushes.DarkBlue, Brushes.Purple, Brushes.DarkCyan,
                 Brushes.Gray, Brushes.Silver, Brushes.Red, Brushes.Lime, Brushes.Yellow,
                 Brushes.Blue, Brushes.Magenta, Brushes.Aqua
            };
            if (m_correntDItem != null)
            {
                if (((ComboBox)sender).Name == "objCBFront")
                {
                    aColors[0] = new SolidBrush(m_correntDItem.DefaultFrontColor);
                    aColors[1] = new SolidBrush(m_correntDItem.FrontColor);
                }
                else
                {
                    aColors[0] = new SolidBrush(m_correntDItem.DefaultBackgroundColor);
                    aColors[1] = new SolidBrush(m_correntDItem.BackgroundColor);
                }
            }
            Brush selectBrush = aColors[e.Index];
            //背景を描画する
            ComboBox cmb = (ComboBox)sender;
            if (!cmb.Enabled)
            {
                e.Graphics.FillRectangle(Brushes.Silver, e.Bounds);
                e.DrawFocusRectangle();
                return;
            }
            //項目が選択されている時は強調表示される
            e.DrawBackground();

            // 枠
            Pen pen = new Pen(Color.Black, 1);
            e.Graphics.FillRectangle(selectBrush, new Rectangle(5, e.Bounds.Y + 2, 13, 13));
            e.Graphics.DrawRectangle(pen, new Rectangle(5, e.Bounds.Y + 2, 13, 13));

            FontStyle fStyle = FontStyle.Regular;
            if (m_constPitch.Contains(cmb.Items[e.Index].ToString()))
                fStyle = FontStyle.Bold;

            //文字列を描画する
            e.Graphics.DrawString(cmb.Items[e.Index].ToString(),
                                  new Font("Meiryo UI", 9, fStyle),
                                  new SolidBrush(Color.Black),
                                  e.Bounds.X+20,
                                  e.Bounds.Y);

            //フォーカスを示す四角形を描画
            e.DrawFocusRectangle();

        }
        /// <summary>
        /// カラーカスタマイズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorCustomize_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            DialogResult ret;
            ret = objColorDlg.ShowDialog();

            if (ret == DialogResult.OK)
            {

            }
            else if (ret == DialogResult.Cancel)
            {
                return;
            }

            if (btn.Name == "objBtnFCustomize")
            {

            }
            else if (btn.Name == "objBtnBCustomize")
            {

            }
        }

        /// <summary>
        /// サンプルテキストの表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objPBSampleText_Paint(object sender, PaintEventArgs e)
        {
            Brush bgTxtColor = Brushes.White;
            Brush frTxtColor = Brushes.Black;
            int type=0;
            int idx = objLBItem.SelectedIndex;
            if (idx < 0) return;
            DisplayItemItem strKind = (DisplayItemItem)objLBItem.Items[idx];
            string sampleTxt = "ij = I::oO(0xB81l)";
            var g = e.Graphics;
            Rectangle r = objPBSampleText.ClientRectangle;
            r.Width -= 1;
            r.Height -= 1;
            //----------------------
            // 枠の色
            //----------------------
            Pen pen = new Pen(Color.Gray, 1);
            switch (strKind.Key)
            {
                // 前景のみ
                case "CompileError":
                case "SyntaxError":
                    type = 1;
                    frTxtColor = new SolidBrush(m_correntDItem.FrontColor);
                    break;
                // 背景のみ
                case "SelectedText":
                case "TextNotSelectedActive":
                case "IndicatorMargin":
                    bgTxtColor = new SolidBrush(m_correntDItem.BackgroundColor);
                    break;
                default:
                    type = 0;
                    bgTxtColor = new SolidBrush(m_correntDItem.BackgroundColor);
                    frTxtColor = new SolidBrush(m_correntDItem.FrontColor);
                    break;
            }

            //----------------------
            // 背景色
            //----------------------
            g.FillRectangle(bgTxtColor, objPBSampleText.ClientRectangle);
            g.DrawRectangle(pen, r);
            //----------------------
            // 文字
            //----------------------
            //Fontを作成
            Font fnt = new Font(m_currentPS.FontName, m_currentPS.FontSize);
            //StringFormatを作成
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            r.X = -100;
            r.Y = 0;
            r.Width = 431;
            r.Height = 54;
            //文字を書く
            switch (type)
            {
                case 0:
                    g.DrawString(sampleTxt, fnt, frTxtColor, r, sf);
                    break;
                case 1:
                    UtilLine.DrawTextWaveLine(g, new Pen(frTxtColor, 1), sampleTxt, fnt, Brushes.Black, r, sf);
                    break;
                case 2:
                    UtilLine.DrawTextWaveLine(g, new Pen(frTxtColor, 1), sampleTxt, fnt, Brushes.Black, r, sf);
                    break;
            }

            //g.MeasureString
            //リソースを解放する
            fnt.Dispose();
        }
        /// <summary>
        /// 表示項目の変更をされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objLBItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = objLBItem.SelectedIndex;
            DisplayItemItem strKind = (DisplayItemItem)objLBItem.Items[idx];
            m_correntDItem = m_currentPS.DisplayItems[strKind.Key];

            if( m_correntDItem.BackgroundColorState == DisplayItemColorState.Normal &&
                m_correntDItem.FrontColorState == DisplayItemColorState.Normal)
            {
                ColorCB_BackFrontEnabl(m_correntDItem.boldfaceValid);
            }
            else if (m_correntDItem.BackgroundColorState != DisplayItemColorState.Normal &&
                m_correntDItem.FrontColorState == DisplayItemColorState.Normal)
            {
                ColorCB_FrontEnablOnly(m_correntDItem.boldfaceValid);
            }
            else if ( m_correntDItem.BackgroundColorState == DisplayItemColorState.Normal &&
                m_correntDItem.FrontColorState != DisplayItemColorState.Normal)
            {
                ColorCB_BackEnablOnly(m_correntDItem.boldfaceValid);
            }

            objCBFront.Refresh();
            objCBBackground.Refresh();
            objPBSampleText.Refresh();
        }
        /// <summary>
        /// 背景色＆前景色の有効
        /// </summary>
        /// <param name="isBold"></param>
        private void ColorCB_BackFrontEnabl(bool isBold = false)
        {
            objCBFront.Enabled = true;
            objBtnFCustomize.Enabled = true;
            objCBBackground.Enabled = true;
            objBtnBCustomize.Enabled = true;
            objCKBoldface.Enabled = isBold;
        }
        /// <summary>
        /// 前景色のみ有効
        /// </summary>
        /// <param name="isBold"></param>
        private void ColorCB_FrontEnablOnly(bool isBold=false)
        {
            objCBFront.Enabled = true;
            objBtnFCustomize.Enabled = true;
            objCBBackground.Enabled = false;
            objBtnBCustomize.Enabled = false;
            objCKBoldface.Enabled = isBold;
        }
        /// <summary>
        /// 背景色のみ有効
        /// </summary>
        /// <param name="isBold"></param>
        private void ColorCB_BackEnablOnly(bool isBold = false)
        {
            objCBFront.Enabled = false;
            objBtnFCustomize.Enabled = false;
            objCBBackground.Enabled = true;
            objBtnBCustomize.Enabled = true;
            objCKBoldface.Enabled = isBold;
        }
    }
}
