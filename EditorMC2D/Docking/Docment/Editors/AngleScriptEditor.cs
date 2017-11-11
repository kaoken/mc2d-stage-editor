using EditorMC2D.Common;
using EditorMC2D.Events;
using MC2DUtil;
using ScintillaNET;
using System;
using System.IO;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace EditorMC2D.Docking.Docment.Editors
{

    public partial class AngleScriptEditor : DockContent
    {
        #region メンバ変数
        private bool isFileLoading = false;
        private string m_fileFullPath;
        private string m_filePath;
        private Encoding m_encoding = Encoding.UTF8;
        //
        private CommonMC2D m_com;
        public Scintilla Editor { get { return scintilla; } }
        public Encoding Encoding { get { return m_encoding; } }
        public DocumentFocusHandler TextEditorGoFocus;
        /// <summary>
        /// scディレクトリ以降のファイルパス
        /// </summary>
        public string FilePath { get { return m_filePath; } }
        /// <summary>
        /// ファイルのフルパス
        /// </summary>
        public string FileFullPath { get { return m_fileFullPath; } }
        #endregion

        public event EventHandler<NewLineEventArgs> NewLineChanged;
        public event EventHandler SideMarginPaint;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath"></param>
        public AngleScriptEditor(string filePath)
        {
            m_com = CommonMC2D.Instance;
            InitializeComponent();
            new AngelScriptEditorSetting(this, scintilla, m_com.Config);



            m_filePath = filePath;
            m_fileFullPath = m_com.DirPathMC2D + @"\" + filePath;


            //scintilla.Text = File.ReadAllText(m_fileFullPath);
            ReadFile();
            //
            this.NewLineChanged += OnNewLineChanged;
            this.SideMarginPaint += OnSideMarginPaint;
            // テクスト変更イベント
        }

        #region テキスト変更イベント
        NewLineEventArgs.NewLine m_NLEvent = new NewLineEventArgs.NewLine();

        public void OnBeforeTextDelete(object sender, BeforeModificationEventArgs e)
        { 
            m_NLEvent.Set((Scintilla)sender, e);
            if (!m_NLEvent.isEnable) return;
            Scintilla ed = (Scintilla)sender;
            NewLineEventArgs nlE = new NewLineEventArgs(ed, e, m_NLEvent, NewLineEventArgs.DELETE);
            this.NewLineChanged(ed, nlE);
            UtilMarker.DeletRightMarginMarker(ed);
            this.SideMarginPaint(ed, new EventArgs());
        }

        public void OnTextDelete(object sender, ModificationEventArgs e)
        {
            if (!m_NLEvent.isEnable) return;
        }

        public void OnBeforeTextInsert(object sender, BeforeModificationEventArgs e)
        { if (isFileLoading)return; m_NLEvent.Set((Scintilla)sender, e); }

        public void OnTextInserted(object sender, ModificationEventArgs e)
        {
            Scintilla ed = (Scintilla)sender;
            if (isFileLoading || e.LinesAdded == 0 || !m_NLEvent.isEnable) return;
            NewLineEventArgs nlE = new NewLineEventArgs(ed, e, m_NLEvent,NewLineEventArgs.INSERT);
            this.NewLineChanged(ed, nlE);
            UtilMarker.DeletRightMarginMarker(ed);
            this.SideMarginPaint(ed, new EventArgs());
        }
        public void OnNewLineChanged(object sender, NewLineEventArgs e) {}
        public void OnSideMarginPaint(object sender, EventArgs e) { }
        #endregion

        /// <summary>
        /// フォームの読みこみ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSourceEditor_Load(object sender, EventArgs e)
        {
            objTT.ToolTipTitle = "完全パス";
            this.ToolTipText = FileFullPath;
        }
        /// <summary>
        /// コンボボックスのリサイズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void objMainSplitContainer_Panel1_Paint(object sender, PaintEventArgs e)
        //{
        //    int m = objMainSplitContainer.Panel1.Margin.Left + objMainSplitContainer.Panel1.Margin.Right;
        //    int w = (objMainSplitContainer.Panel1.Width - (m + 6)) / 2;

        //    objCBLeft.Width = w;
        //    objCBRight.Width = w;
        //    objCBRight.Location = new Point(objMainSplitContainer.Panel1.Margin.Left + w + 4, objCBLeft.Location.Y);

        //}

        /// <summary>
        /// 内容が変化したときタブタイトルに＊を追加削除する
        /// </summary>
        private void AddOrRemoveAsteric()
        {
            if (TabText == null) return;
            if (scintilla.Modified)
            {
                if (!TabText.EndsWith(" *"))
                    TabText += " *";
            }
            else
            {
                if (TabText.EndsWith(" *"))
                    TabText = Text.Substring(0, TabText.Length - 2);
            }
        }
        /// <summary>
        /// 変化があったか？
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModifiedChanged(object sender, EventArgs e)
        {
            AddOrRemoveAsteric();
        }

        public void FormSourceEditorGotFocus(Object sender, EventArgs e)
        {
            if( TextEditorGoFocus != null )
                TextEditorGoFocus(this, new DocumentFocusEventArgs(this));
        }
        /// <summary>
        /// ファイルの読みこみ
        /// </summary>
        public void ReadFile()
        {
            isFileLoading = true;
            //m_comCore.SetScintillaToCurrentOptions(this);
            try
            {
                // ファイルからバイナリで読み込む
                byte[] buff = File.ReadAllBytes(m_fileFullPath);
                // 文字コード判定
                m_encoding = CharCodeJudge.Encode(buff, buff.Length);

                scintilla.Text = m_encoding.GetString(buff);
                scintilla.EmptyUndoBuffer();


                //m_comCore.BookBreakMgr.ReDrawBreakPointAndBookMarks(m_objScintilla);
                isFileLoading = false;
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// ファイルの保存
        /// </summary>
        public bool SaveFile()
        {
            //FileStream fs = File.Create(m_fileFullPath);
            //BinaryWriter bw = new BinaryWriter(fs);
            //byte[] bs = System.IO.File.ReadAllBytes(scintilla.Text);
            //Encoding enc = GetCode(bs);

            //// shift-jisかチェック
            //if (enc != Encoding.UTF8)
            //{
            //    byte[] buffConv = Encoding.Convert(enc, Encoding.GetEncoding(932), bs);
            //    bw.Write(buffConv, 0, buffConv.Length-1); // 最後のヌルを省略
            //}
            //else
            //{
            //    bw.Write(bs, 0, bs.Length - 1); // 最後のヌルを省略
            //}

            //bw.Close();


            return true;
        }
        /// <summary>
        /// 文字コードを判別する
        /// </summary>
        /// <remarks>
        /// Jcode.pmのgetcodeメソッドを移植したものです。
        /// Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
        /// Jcode.pmの著作権情報
        /// Copyright 1999-2005 Dan Kogai <dankogai@dan.co.jp>
        /// This library is free software; you can redistribute it and/or modify it
        ///  under the same terms as Perl itself.
        /// </remarks>
        /// <param name="bytes">文字コードを調べるデータ</param>
        /// <returns>適当と思われるEncodingオブジェクト。
        /// 判断できなかった時はnull。</returns>
        public static System.Text.Encoding GetCode(byte[] bytes)
        {
            const byte bEscape = 0x1B;
            const byte bAt = 0x40;
            const byte bDollar = 0x24;
            const byte bAnd = 0x26;
            const byte bOpen = 0x28;    //'('
            const byte bB = 0x42;
            const byte bD = 0x44;
            const byte bJ = 0x4A;
            const byte bI = 0x49;

            int len = bytes.Length;
            byte b1, b2, b3, b4;

            //Encode::is_utf8 は無視

            bool isBinary = false;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 <= 0x06 || b1 == 0x7F || b1 == 0xFF)
                {
                    //'binary'
                    isBinary = true;
                    if (b1 == 0x00 && i < len - 1 && bytes[i + 1] <= 0x7F)
                    {
                        //smells like raw unicode
                        return System.Text.Encoding.Unicode;
                    }
                }
            }
            if (isBinary)
            {
                return null;
            }

            //not Japanese
            bool notJapanese = true;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 == bEscape || 0x80 <= b1)
                {
                    notJapanese = false;
                    break;
                }
            }
            if (notJapanese)
            {
                return System.Text.Encoding.ASCII;
            }

            for (int i = 0; i < len - 2; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                b3 = bytes[i + 2];

                if (b1 == bEscape)
                {
                    if (b2 == bDollar && b3 == bAt)
                    {
                        //JIS_0208 1978
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bDollar && b3 == bB)
                    {
                        //JIS_0208 1983
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && (b3 == bB || b3 == bJ))
                    {
                        //JIS_ASC
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && b3 == bI)
                    {
                        //JIS_KANA
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    if (i < len - 3)
                    {
                        b4 = bytes[i + 3];
                        if (b2 == bDollar && b3 == bOpen && b4 == bD)
                        {
                            //JIS_0212
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                        if (i < len - 5 &&
                            b2 == bAnd && b3 == bAt && b4 == bEscape &&
                            bytes[i + 4] == bDollar && bytes[i + 5] == bB)
                        {
                            //JIS_0208 1990
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                    }
                }
            }

            //should be euc|sjis|utf8
            //use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
            int sjis = 0;
            int euc = 0;
            int utf8 = 0;
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0x81 <= b1 && b1 <= 0x9F) || (0xE0 <= b1 && b1 <= 0xFC)) &&
                    ((0x40 <= b2 && b2 <= 0x7E) || (0x80 <= b2 && b2 <= 0xFC)))
                {
                    //SJIS_C
                    sjis += 2;
                    i++;
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0xA1 <= b1 && b1 <= 0xFE) && (0xA1 <= b2 && b2 <= 0xFE)) ||
                    (b1 == 0x8E && (0xA1 <= b2 && b2 <= 0xDF)))
                {
                    //EUC_C
                    //EUC_KANA
                    euc += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if (b1 == 0x8F && (0xA1 <= b2 && b2 <= 0xFE) &&
                        (0xA1 <= b3 && b3 <= 0xFE))
                    {
                        //EUC_0212
                        euc += 3;
                        i += 2;
                    }
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if ((0xC0 <= b1 && b1 <= 0xDF) && (0x80 <= b2 && b2 <= 0xBF))
                {
                    //UTF8
                    utf8 += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if ((0xE0 <= b1 && b1 <= 0xEF) && (0x80 <= b2 && b2 <= 0xBF) &&
                        (0x80 <= b3 && b3 <= 0xBF))
                    {
                        //UTF8
                        utf8 += 3;
                        i += 2;
                    }
                }
            }
            //M. Takahashi's suggestion
            //utf8 += utf8 / 2;

            System.Diagnostics.Debug.WriteLine(
                string.Format("sjis = {0}, euc = {1}, utf8 = {2}", sjis, euc, utf8));
            if (euc > sjis && euc > utf8)
            {
                //EUC
                return System.Text.Encoding.GetEncoding(51932);
            }
            else if (sjis > euc && sjis > utf8)
            {
                //SJIS
                return System.Text.Encoding.GetEncoding(932);
            }
            else if (utf8 > euc && utf8 > sjis)
            {
                //UTF8
                return System.Text.Encoding.UTF8;
            }

            return null;
        }

        #region メニュー
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTSMISave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTSMIClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 全て閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTSMIAllClose_Click(object sender, EventArgs e)
        {
            //m_comCore.AllSorceEditorClose();
        }
        /// <summary>
        /// このファイル以外全て閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTSMIAllCloseExceptMy_Click(object sender, EventArgs e)
        {
            //m_comCore.AllSorceEditorClose(FilePath);
        }
        /// <summary>
        /// このファイルの完全パスのコピー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTSMICopyPath_Click(object sender, EventArgs e)
        {
            System.Windows.Clipboard.SetText(FileFullPath);
        }
        /// <summary>
        /// このファイルのフォルダーを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTSMIOpenDir_Click(object sender, EventArgs e)
        {
            // 親ディレクトリ名 (フォルダ名) を取得する
            string stParentName = System.IO.Path.GetDirectoryName(FileFullPath);
            System.Diagnostics.Process.Start(stParentName);
        }
        #endregion


    }
}
