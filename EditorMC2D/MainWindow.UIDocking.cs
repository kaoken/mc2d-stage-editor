using EditorMC2D.Docking;
using EditorMC2D.Docking.Docment;
using EditorMC2D.Docking.Docment.Editors;
using EditorMC2D.Events;
using EditorMC2D.Option.Environment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace EditorMC2D
{
    /// <summary>
    /// 主にドックパネルの処理する
    /// </summary>
    public partial class MainWindow
    {
        public DockThemeChangeHandler DockThemeChange;

        private static Object ms_lockObj = new Object();
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();
        private bool m_isFirstDockingInit = false;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isSaveLayout = true;
        /// <summary>
        /// 
        /// </summary>
        private DeserializeDockContent m_deserializeDockContent = null;
        /// <summary>
        /// 選択中のドキュメントフォーム
        /// </summary>
        private Form m_fmDocCurrent = null;
        /// <summary>
        /// ドキュメントコンテナ
        /// </summary>
        private Dictionary<string, DockContent> m_dockContents = new Dictionary<string, DockContent>();
        /// <summary>
        /// ソリューションエクスプローラードキュメント
        /// </summary>
        private SolutionExplorerDoc m_solutionExplorer = null;
        /// <summary>
        /// 出力ウィンドウ
        /// </summary>
        private OutputWindow m_outputWindow;



        /// <summary>
        /// コンストラクタから1度だけ呼び出すこと
        /// </summary>
        private void FirstDockingInit()
        {
            if (m_isFirstDockingInit)
                throw new Exception("FirstDockingInit() 呼び出し済み");
            CreateStandardControls();

            m_solutionExplorer.RightToLeftLayout = RightToLeftLayout;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);



            vsToolStripExtender.DefaultRenderer = _toolStripProfessionalRenderer;
            m_isFirstDockingInit = true;
        }
        /// <summary>
        /// 初期のドッキングパネルのレイアウト
        /// </summary>
        private void DockingPanelBeginningLayout()
        {
            dockPanel.SuspendLayout(true);

            CloseAllContents();

            CreateStandardControls();

            Assembly assembly = Assembly.GetAssembly(typeof(MainWindow));
            Stream xmlStream = assembly.GetManifestResourceStream("EditorMC2D.Resources.BeginningDockPanel.xml");
            dockPanel.LoadFromXml(xmlStream, m_deserializeDockContent);
            xmlStream.Close();

            dockPanel.ResumeLayout(true, true);
        }
        /// <summary>
        /// 現行のドッキングパネルの状態を読み込む
        /// </summary>
        private bool DockingPanelLayoutLoad()
        {
            string configFile = Path.Combine(m_com.DirPathMC2D, "DockPanel.config");
            if (!File.Exists(configFile)) {
                DockingPanelBeginningLayout();
                return false;
            }
            

            dockPanel.SuspendLayout(true);

            CloseAllContents();

            CreateStandardControls();

            dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
            dockPanel.ResumeLayout(true, true);
            return true;
        }
        /// <summary>
        /// 現行のドッキングパネルの状態を保存する
        /// </summary>
        private void DockingPanelLayoutSave()
        {
            string configFile = Path.Combine(m_com.DirPathMC2D, "DockPanel.config");
            
            if (m_isSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }

        /// <summary>
        /// テーマを切り替える
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSchema(object sender, System.EventArgs e)
        {
            string configFile = Path.Combine(m_com.DirPathMC2D, "DockPanel.temp.config");
            if (File.Exists(configFile))
            {
                dockPanel.SaveAsXml(configFile);
                CloseAllContents();
            }
            if (sender == this.vs2015ThemeBlueMenuItem)
            {
                ThemeChange(Whole.ColorThemeBlue);
            }
            else if (sender == this.vs2015ThemeLightMenuItem)
            {
                ThemeChange(Whole.ColorThemeLightColor);
            }
            else if (sender == this.vs2015ThemeDarkMenuItem)
            {
                ThemeChange(Whole.ColorThemeDarkColor);
            }

            vs2015ThemeBlueMenuItem.Checked = (sender == vs2015ThemeBlueMenuItem);
            vs2015ThemeLightMenuItem.Checked = (sender == vs2015ThemeLightMenuItem);
            vs2015ThemeDarkMenuItem.Checked = (sender == vs2015ThemeDarkMenuItem);

            dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
        }
        /// <summary>
        /// テーマの変更
        /// </summary>
        /// <param name="theme"></param>
        private void ThemeChange(string theme)
        {
            if (theme == Whole.ColorThemeBlue)
            {
                this.dockPanel.Theme = this.vS2015BlueTheme;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015BlueTheme);
            }
            else if (theme == Whole.ColorThemeLightColor)
            {
                this.dockPanel.Theme = this.vS2015LightTheme;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015LightTheme);
            }
            else if (theme == Whole.ColorThemeDarkColor)
            {
                this.dockPanel.Theme = this.vS2015DarkTheme;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015DarkTheme);
            }

            if (dockPanel.Theme.ColorPalette != null)
            {
                statusBar.BackColor = dockPanel.Theme.ColorPalette.MainWindowStatusBarDefault.Background;
            }

        }


        /// <summary>
        /// VSレンダラを有効にする
        /// </summary>
        /// <param name="version"></param>
        /// <param name="theme"></param>
        private void EnableVSRenderer(VisualStudioToolStripExtender.VsVersion version, ThemeBase theme)
        {
            vsToolStripExtender.SetStyle(mainMenu, version, theme);
            vsToolStripExtender.SetStyle(mainToolBar, version, theme);
            vsToolStripExtender.SetStyle(statusBar, version, theme);


            try
            {
                DockThemeChange(this, new DockThemeChangeEventArgs(version, theme));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// 画像ドキュメントの追加
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public ImageViewerDoc AddImageViewerDoc(string filePath)
        {
            string fileName = "";
            ImageViewerDoc doc;
            lock (ms_lockObj)
            {
                if (m_dockContents.ContainsKey(filePath))
                    return null;
                doc = new ImageViewerDoc(filePath);

                m_dockContents.Add(filePath, doc);

                fileName = Path.GetFileName(filePath);

                doc.TabText = fileName;
                doc.Text = filePath;

                if (this.dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    doc.MdiParent = this;
                    doc.Show();
                }
                else
                {
                    doc.ShowHint = DockState.Document;
                    doc.Show(this.dockPanel);
                }
                doc.FormClosed += DocumentClosed;
                doc.DocumentFocusEvent += DocumentFocus;
            }
            return doc;
        }
        /// <summary>
        /// オーディオドキュメントの追加
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public AudioDoc AddAudioDocument(string filePath)
        {
            string fileName = "";
            AudioDoc doc;
            lock (ms_lockObj)
            {
                if (m_dockContents.ContainsKey(filePath))
                    return null;
                doc = new AudioDoc(filePath);

                m_dockContents.Add(filePath, doc);

                fileName = Path.GetFileName(filePath);

                doc.TabText = fileName;
                doc.Text = filePath;

                if (this.dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    doc.MdiParent = this;
                    doc.Show();
                }
                else
                {
                    doc.ShowHint = DockState.Document;
                    doc.Show(this.dockPanel);
                }
                doc.FormClosed += DocumentClosed;
                doc.DocumentFocusEvent += DocumentFocus;
            }
            return doc;
        }
        /// <summary>
        /// フォント・キュメントの追加
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public FontDoc AddFontDocument(string filePath)
        {
            string fileName = "";
            FontDoc doc;

            lock (ms_lockObj)
            {
                if (m_dockContents.ContainsKey(filePath))
                    return null;
                doc = new FontDoc(filePath);

                m_dockContents.Add(filePath, doc);

                fileName = Path.GetFileName(filePath);

                doc.TabText = fileName;
                doc.Text = filePath;

                if (this.dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    doc.MdiParent = this;
                    doc.Show();
                }
                else
                {
                    doc.ShowHint = DockState.Document;
                    doc.Show(this.dockPanel);
                }
                doc.FormClosed += DocumentClosed;
                doc.DocumentFocusEvent += DocumentFocus;
            }
            return doc;
        }

        /// <summary>
        /// エンジェルスクリプト・エディタを追加する
        /// </summary>
        /// <param name="filePath">相対ファイルパス</param>
        public AngleScriptEditor AddAngelScriptEditor(string filePath)
        {
            string fileName = "";
            AngleScriptEditor objSE;

            lock (ms_lockObj)
            {
                if (m_dockContents.ContainsKey(filePath))
                    return null;
                objSE = new AngleScriptEditor(filePath);
                objSE.ReadFile();
                m_dockContents.Add(filePath, objSE);

                fileName = Path.GetFileName(filePath);

                objSE.TabText = fileName;
                objSE.Text = filePath;

                if (this.dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    objSE.MdiParent = this;
                    objSE.Show();
                }
                else
                {
                    objSE.ShowHint = DockState.Document;
                    objSE.Show(this.dockPanel);
                }
                objSE.FormClosed += DocumentClosed;
                objSE.TextEditorGoFocus += DocumentFocus;
                objSE.Editor.Focus();
            }
            return objSE;
        }
        /// <summary>
        /// ドキュメントが閉じられた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DocumentClosed(object sender, EventArgs e)
        {
            string key = "";
            DockContent dc = (DockContent)sender;
            if (dc.GetType() == typeof(AngleScriptEditor))
            {
                AngleScriptEditor fmSE = (AngleScriptEditor)dc;
                key = fmSE.FilePath;
            }
            else if (dc.GetType() == typeof(FontDoc))
            {
                FontDoc fmSE = (FontDoc)dc;
                key = fmSE.FilePath;
            }
            else if (dc.GetType() == typeof(ImageViewerDoc))
            {
                ImageViewerDoc fmSE = (ImageViewerDoc)dc;
                key = fmSE.FilePath;
            }
            else if (dc.GetType() == typeof(AudioDoc))
            {
                AudioDoc fmSE = (AudioDoc)dc;
                key = fmSE.FilePath;
            }


            lock (ms_lockObj)
            {
                if (m_dockContents.ContainsKey(key))
                {
                    m_dockContents.Remove(key);
                }
                if (sender == m_fmDocCurrent)
                {
                    m_fmDocCurrent = null;
                }
                dc.Dispose();
            }


            if (m_dockContents.Count == 0)
            {
                //m_fmMain.BookMarkBreakPointToolbarEnable(false);
            }
        }
        /// <summary>
        /// ドキュメントフォーカスが移動した
        /// </summary>
        /// <param name="fmSE"></param>
        public void DocumentFocus(object sender, DocumentFocusEventArgs e)
        {
            m_fmDocCurrent = e.DockContent;
        }

        /// <summary>
        /// 標準コントロールを作成する
        /// </summary>
        private void CreateStandardControls()
        {
            // ソリューション
            m_solutionExplorer = new SolutionExplorerDoc();
            DockThemeChange += new DockThemeChangeHandler(m_solutionExplorer.EnableVSRenderer);
            D2StageFileOpenedEvent += new D2StageFileOpenedHandler(m_solutionExplorer.D2StageFileOpened);
            AllSavedEvent += new AllSavedHandler(m_solutionExplorer.AllSaved);
            ProjectOpenedEvent += new ProjectOpenedHandler(m_solutionExplorer.ProjectOpened);
            // 出力
            m_outputWindow = new OutputWindow();
            DockThemeChange += new DockThemeChangeHandler(m_outputWindow.EnableVSRenderer);
            m_com.OutputWindow = m_outputWindow;

            ThemeChange(m_com.Config.Whole.ColorTheme);
        }


        /// <summary>
        /// コンテンツ全てを閉じる
        /// </summary>
        private void CloseAllContents()
        {
            // ソリューション
            m_solutionExplorer.DockPanel = null;
            // 出力
            m_outputWindow.DockPanel = null;

            // Close all other document windows
            CloseAllDocuments();

            // 重要：すべてのフロートウィンドウを廃棄する
            foreach (var window in dockPanel.FloatWindows.ToList())
                window.Dispose();

            System.Diagnostics.Debug.Assert(dockPanel.Panes.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel.Contents.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel.FloatWindows.Count == 0);
            GC.Collect();
        }
        /// <summary>
        /// 全てのドキュメントを閉じる
        /// </summary>
        private void CloseAllDocuments()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    // 重要：すべてのペインを廃棄する
                    document.DockHandler.DockPanel = null;
                    document.DockHandler.Close();
                }
            }
            GC.Collect();
        }
        /// <summary>
        /// 持続文字列からコンテンツを取得する
        /// </summary>
        /// <param name="persistString"></param>
        /// <returns></returns>
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(SolutionExplorerDoc).ToString())
                return m_solutionExplorer;
            //else if (persistString == typeof(DummyPropertyWindow).ToString())
            //    return m_propertyWindow;
            //else if (persistString == typeof(DummyToolbox).ToString())
            //    return m_toolbox;
            else if (persistString == typeof(OutputWindow).ToString())
                return m_outputWindow;
            //else if (persistString == typeof(DummyTaskList).ToString())
            //    return m_taskList;
            else
            {
                // TextDocはGetPersistStringをオーバーライドして、余分な情報をpersistStringに追加
                // 任意のDockContentがこの値をオーバーライドして、逆シリアル化に必要な情報を追加できます。


                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(TextDoc).ToString())
                    return null;

                TextDoc dummyDoc = new TextDoc();
                if (parsedStrings[1] != string.Empty)
                    dummyDoc.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    dummyDoc.Text = parsedStrings[2];

                return dummyDoc;
            }
        }


        /// <summary>
        /// ドキュメントスタイルの設定をする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDocumentStyle(object sender, System.EventArgs e)
        {
            DocumentStyle oldStyle = dockPanel.DocumentStyle;
            DocumentStyle newStyle;
            //if (sender == menuItemDockingMdi)
            //    newStyle = DocumentStyle.DockingMdi;
            //else if (sender == menuItemDockingWindow)
            //    newStyle = DocumentStyle.DockingWindow;
            //else if (sender == menuItemDockingSdi)
            //    newStyle = DocumentStyle.DockingSdi;
            //else
                newStyle = DocumentStyle.SystemMdi;

            if (oldStyle == newStyle)
                return;

            if (oldStyle == DocumentStyle.SystemMdi || newStyle == DocumentStyle.SystemMdi)
                CloseAllDocuments();

            dockPanel.DocumentStyle = newStyle;
            //menuItemDockingMdi.Checked = (newStyle == DocumentStyle.DockingMdi);
            //menuItemDockingWindow.Checked = (newStyle == DocumentStyle.DockingWindow);
            //menuItemDockingSdi.Checked = (newStyle == DocumentStyle.DockingSdi);
            //menuItemSystemMdi.Checked = (newStyle == DocumentStyle.SystemMdi);
            //menuItemLayoutByCode.Enabled = (newStyle != DocumentStyle.SystemMdi);
            //menuItemLayoutByXml.Enabled = (newStyle != DocumentStyle.SystemMdi);
            //toolBarButtonLayoutByCode.Enabled = (newStyle != DocumentStyle.SystemMdi);
            //toolBarButtonLayoutByXml.Enabled = (newStyle != DocumentStyle.SystemMdi);
        }
        /// <summary>
        /// ドキュメントを探す
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private IDockContent FindDocument(string text)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    if (form.Text == text)
                        return form as IDockContent;

                return null;
            }
            else
            {
                foreach (IDockContent content in dockPanel.Documents)
                    if (content.DockHandler.TabText == text)
                        return content;

                return null;
            }
        }
        private TextDoc CreateNewDocument()
        {
            TextDoc dummyDoc = new TextDoc();

            int count = 1;
            string text = $"Document{count}";
            while (FindDocument(text) != null)
            {
                count++;
                text = $"Document{count}";
            }

            dummyDoc.Text = text;
            return dummyDoc;
        }
        private void AddDebugDocument(object sender, EventArgs e)
        {
            TextDoc dummyDoc = CreateNewDocument();
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                dummyDoc.MdiParent = this;
                dummyDoc.Show();
            }
            else
                dummyDoc.Show(dockPanel, DockState.Document);
            dockPanel.ResumeLayout(true, true);
        }
    }
}
