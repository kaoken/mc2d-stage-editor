namespace EditorMC2D
{
    partial class MainWindow
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.allSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.projectListMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.closeProjectMI = new System.Windows.Forms.ToolStripMenuItem();
            this.endProjectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.visibleMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.themeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vs2015ThemeBlueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vs2015ThemeLightMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vs2015ThemeDarkMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solutionExplorerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.outputMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAppRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.debugDocTSB = new System.Windows.Forms.ToolStripButton();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.vS2015BlueTheme = new WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme();
            this.vS2015DarkTheme = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.vS2015LightTheme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.mainMenu.SuspendLayout();
            this.mainToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 578);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(800, 22);
            this.statusBar.TabIndex = 4;
            this.statusBar.Text = "statusStrip1";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.visibleMI,
            this.toolToolStripMenuItem,
            this.helpMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(800, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "mainMenu";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectMenu,
            this.openFileMenu,
            this.toolStripSeparator1,
            this.saveFileMenu,
            this.allSaveMenuItem,
            this.toolStripSeparator2,
            this.projectListMI,
            this.toolStripSeparator8,
            this.closeProjectMI,
            this.endProjectMenu});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(67, 20);
            this.fileMenuItem.Text = "ファイル(&F)";
            // 
            // newProjectMenu
            // 
            this.newProjectMenu.Name = "newProjectMenu";
            this.newProjectMenu.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.newProjectMenu.Size = new System.Drawing.Size(232, 22);
            this.newProjectMenu.Text = "新規作成(&N)";
            this.newProjectMenu.Click += new System.EventHandler(this.CreateNewProject);
            // 
            // openFileMenu
            // 
            this.openFileMenu.Name = "openFileMenu";
            this.openFileMenu.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.openFileMenu.Size = new System.Drawing.Size(232, 22);
            this.openFileMenu.Text = "開く(&O)";
            this.openFileMenu.Click += new System.EventHandler(this.OpenFile);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(229, 6);
            // 
            // saveFileMenu
            // 
            this.saveFileMenu.Image = global::EditorMC2D.Properties.Resources.save_16xLG;
            this.saveFileMenu.Name = "saveFileMenu";
            this.saveFileMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveFileMenu.Size = new System.Drawing.Size(232, 22);
            this.saveFileMenu.Text = "保存(&S)";
            this.saveFileMenu.Click += new System.EventHandler(this.SaveFile);
            // 
            // allSaveMenuItem
            // 
            this.allSaveMenuItem.Image = global::EditorMC2D.Properties.Resources.SaveAll_16x;
            this.allSaveMenuItem.Name = "allSaveMenuItem";
            this.allSaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.allSaveMenuItem.Size = new System.Drawing.Size(232, 22);
            this.allSaveMenuItem.Text = "全てを保存する(&L)";
            this.allSaveMenuItem.Click += new System.EventHandler(this.AllSave);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(229, 6);
            // 
            // projectListMI
            // 
            this.projectListMI.Name = "projectListMI";
            this.projectListMI.Size = new System.Drawing.Size(232, 22);
            this.projectListMI.Text = "最近使ったプロジェクト(&J)";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(229, 6);
            // 
            // closeProjectMI
            // 
            this.closeProjectMI.Image = global::EditorMC2D.Properties.Resources.Close_16xLG;
            this.closeProjectMI.Name = "closeProjectMI";
            this.closeProjectMI.Size = new System.Drawing.Size(232, 22);
            this.closeProjectMI.Text = "プロジェクトを閉じる(&T)";
            this.closeProjectMI.Click += new System.EventHandler(this.CloseProject);
            // 
            // endProjectMenu
            // 
            this.endProjectMenu.Image = global::EditorMC2D.Properties.Resources.Close_16xLG;
            this.endProjectMenu.Name = "endProjectMenu";
            this.endProjectMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.endProjectMenu.Size = new System.Drawing.Size(232, 22);
            this.endProjectMenu.Text = "終了(&X)";
            this.endProjectMenu.Click += new System.EventHandler(this.ExitApplication);
            // 
            // visibleMI
            // 
            this.visibleMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator4,
            this.themeMenuItem,
            this.solutionExplorerMenuItem,
            this.toolStripSeparator6,
            this.outputMenuItem,
            this.toolStripSeparator7});
            this.visibleMI.Enabled = false;
            this.visibleMI.Name = "visibleMI";
            this.visibleMI.Size = new System.Drawing.Size(58, 20);
            this.visibleMI.Text = "表示(&V)";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(277, 6);
            // 
            // themeMenuItem
            // 
            this.themeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vs2015ThemeBlueMenuItem,
            this.vs2015ThemeLightMenuItem,
            this.vs2015ThemeDarkMenuItem});
            this.themeMenuItem.Name = "themeMenuItem";
            this.themeMenuItem.Size = new System.Drawing.Size(280, 22);
            this.themeMenuItem.Text = "テーマ(&T)";
            // 
            // vs2015ThemeBlueMenuItem
            // 
            this.vs2015ThemeBlueMenuItem.Name = "vs2015ThemeBlueMenuItem";
            this.vs2015ThemeBlueMenuItem.Size = new System.Drawing.Size(116, 22);
            this.vs2015ThemeBlueMenuItem.Text = "ブルー(&B)";
            this.vs2015ThemeBlueMenuItem.Click += new System.EventHandler(this.SetSchema);
            // 
            // vs2015ThemeLightMenuItem
            // 
            this.vs2015ThemeLightMenuItem.Name = "vs2015ThemeLightMenuItem";
            this.vs2015ThemeLightMenuItem.Size = new System.Drawing.Size(116, 22);
            this.vs2015ThemeLightMenuItem.Text = "ライト(&L)";
            this.vs2015ThemeLightMenuItem.Click += new System.EventHandler(this.SetSchema);
            // 
            // vs2015ThemeDarkMenuItem
            // 
            this.vs2015ThemeDarkMenuItem.Name = "vs2015ThemeDarkMenuItem";
            this.vs2015ThemeDarkMenuItem.Size = new System.Drawing.Size(116, 22);
            this.vs2015ThemeDarkMenuItem.Text = "ダーク(&D)";
            this.vs2015ThemeDarkMenuItem.Click += new System.EventHandler(this.SetSchema);
            // 
            // solutionExplorerMenuItem
            // 
            this.solutionExplorerMenuItem.Checked = true;
            this.solutionExplorerMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.solutionExplorerMenuItem.Image = global::EditorMC2D.Properties.Resources.solution_16x;
            this.solutionExplorerMenuItem.Name = "solutionExplorerMenuItem";
            this.solutionExplorerMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.L)));
            this.solutionExplorerMenuItem.Size = new System.Drawing.Size(280, 22);
            this.solutionExplorerMenuItem.Text = "ソリューションエクスプローラー(&A)";
            this.solutionExplorerMenuItem.Click += new System.EventHandler(this.menuItemSolutionExplorer_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(277, 6);
            // 
            // outputMenuItem
            // 
            this.outputMenuItem.Image = global::EditorMC2D.Properties.Resources.Output_16x;
            this.outputMenuItem.Name = "outputMenuItem";
            this.outputMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.O)));
            this.outputMenuItem.Size = new System.Drawing.Size(280, 22);
            this.outputMenuItem.Text = "出力(&O)";
            this.outputMenuItem.Click += new System.EventHandler(this.ShowOutputWindow);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(277, 6);
            // 
            // toolToolStripMenuItem
            // 
            this.toolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionMenuItem});
            this.toolToolStripMenuItem.Name = "toolToolStripMenuItem";
            this.toolToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.toolToolStripMenuItem.Text = "ツール(&T)";
            // 
            // optionMenuItem
            // 
            this.optionMenuItem.Image = global::EditorMC2D.Properties.Resources.Settings_16x;
            this.optionMenuItem.Name = "optionMenuItem";
            this.optionMenuItem.Size = new System.Drawing.Size(144, 22);
            this.optionMenuItem.Text = "オプション(&O)...";
            this.optionMenuItem.Click += new System.EventHandler(this.ShwoOptionForm);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionInfoMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(65, 20);
            this.helpMenuItem.Text = "ヘルプ(&H)";
            // 
            // versionInfoMenuItem
            // 
            this.versionInfoMenuItem.Name = "versionInfoMenuItem";
            this.versionInfoMenuItem.Size = new System.Drawing.Size(158, 22);
            this.versionInfoMenuItem.Text = "バージョン情報(&A)";
            this.versionInfoMenuItem.Click += new System.EventHandler(this.versionInfoMenuItem_Click);
            // 
            // mainToolBar
            // 
            this.mainToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButton1,
            this.toolStripSeparator3,
            this.tsbAppRun,
            this.toolStripSeparator5,
            this.debugDocTSB});
            this.mainToolBar.Location = new System.Drawing.Point(0, 24);
            this.mainToolBar.Name = "mainToolBar";
            this.mainToolBar.Size = new System.Drawing.Size(800, 25);
            this.mainToolBar.TabIndex = 2;
            this.mainToolBar.Text = "mainToolBar";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::EditorMC2D.Properties.Resources.OpenFolder_16x;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "ファイルを開く";
            this.toolStripButton2.Click += new System.EventHandler(this.OpenFile);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::EditorMC2D.Properties.Resources.save_16xLG;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "保存";
            this.toolStripButton1.Click += new System.EventHandler(this.SaveFile);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbAppRun
            // 
            this.tsbAppRun.Image = global::EditorMC2D.Properties.Resources.Run_16x;
            this.tsbAppRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAppRun.Name = "tsbAppRun";
            this.tsbAppRun.Size = new System.Drawing.Size(51, 22);
            this.tsbAppRun.Text = "実行";
            this.tsbAppRun.Click += new System.EventHandler(this.AppRun_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // debugDocTSB
            // 
            this.debugDocTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.debugDocTSB.Image = global::EditorMC2D.Properties.Resources.DirectX3D_16x;
            this.debugDocTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.debugDocTSB.Name = "debugDocTSB";
            this.debugDocTSB.Size = new System.Drawing.Size(23, 22);
            this.debugDocTSB.Text = "デバッグテキスト";
            this.debugDocTSB.Click += new System.EventHandler(this.AddDebugDocument);
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dockPanel.Location = new System.Drawing.Point(0, 49);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(800, 529);
            this.dockPanel.TabIndex = 0;
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.mainToolBar);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainWindow";
            this.Text = "MC2D ステージエディタ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainWindowClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.SizeChanged += new System.EventHandler(this.MainWindow_SizeChanged);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainToolBar.ResumeLayout(false);
            this.mainToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStrip mainToolBar;
        private System.Windows.Forms.ToolStripMenuItem newProjectMenu;
        private System.Windows.Forms.ToolStripMenuItem openFileMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveFileMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem endProjectMenu;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem visibleMI;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionInfoMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme vS2015BlueTheme;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private System.Windows.Forms.ToolStripMenuItem themeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vs2015ThemeBlueMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vs2015ThemeLightMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vs2015ThemeDarkMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem solutionExplorerMenuItem;
        private System.Windows.Forms.ToolStripButton tsbAppRun;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem outputMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton debugDocTSB;
        private System.Windows.Forms.ToolStripMenuItem toolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeProjectMI;
        private System.Windows.Forms.ToolStripMenuItem allSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectListMI;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    }
}

