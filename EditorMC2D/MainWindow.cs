using EditorMC2D.Common;
using EditorMC2D.Events;
using EditorMC2D.Option;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EditorMC2D
{
    public partial class MainWindow : Form
    {
        #region デリゲートの宣言
        /// <summary>
        /// ソリューションを閉じたハンドラー
        /// </summary>
        public event ClosedSolutionHandler ClosedProjectEvent;
        public event D2StageFileOpenedHandler D2StageFileOpenedEvent;
        public event NewProjecCreatedHandler NewProjecCreatedEvent;
        public event AllSavedHandler AllSavedEvent;
        public event ProjectOpenedHandler ProjectOpenedEvent;
        #endregion

        /// <summary>
        /// MC2D実行ファイルがあるディレクトリーパス
        /// </summary>
        private string m_dirPathMC2D = "";
        CommonMC2D m_com;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            m_com = CommonMC2D.Instance;
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            m_com.MainWindow = this;

            // リスナー登録
            NewProjecCreatedEvent += new NewProjecCreatedHandler(this.NewProjecCreatedCall);
            ProjectOpenedEvent += new ProjectOpenedHandler(this.ProjectOpened);
            m_com.Process.ProcessMC2DEndEvent += new ProcessMC2DEndHandler(ProcessMC2DEnd);
            //SetSplashScreen();
            FirstDockingInit();

        }

        /// <summary>
        /// MC2D実行ファイルがあるディレクトリーパス
        /// </summary>
        public string DirPathMC2D { get { return m_dirPathMC2D; } }



        #region Event Handlers


        /// <summary>
        /// ファイルの保存メニューが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFile(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 新規プロジェクト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewProject(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.FileName = "MC2D.exe";
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "MC2D実行ファイル(*.exe)|MC2D.exe";
            ofd.Title = "MC2D.exeを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            var ret = ofd.ShowDialog();
            //ダイアログを表示する
            if (ret == DialogResult.OK)
            {
                if(ofd.SafeFileName != "MC2D.exe")
                {
                    MessageBox.Show("MC2D.exeファイルではありません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                return;
            }
            m_dirPathMC2D = System.IO.Path.GetDirectoryName(ofd.FileName);

            if (!m_com.CheckMediaDirs(m_dirPathMC2D))
            {
                MessageBox.Show("メディアディレクトリが不正です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                System.Windows.Shell.JumpList jumpList = new System.Windows.Shell.JumpList();
                // JumpTaskオブジェクトを生成し、JumpListオブジェクトに格納する
                var jumpTask = new System.Windows.Shell.JumpTask()
                {
                    CustomCategory = "最近使ったプロジェクト",
                    Title = ofd.FileName,
                    Arguments = "/project="+ ofd.FileName,
                };
                jumpList.JumpItems.Add(jumpTask);

                // Applyメソッドを呼び出して、Windowsに通知する
                jumpList.Apply();

                m_com.Config.AddRecentProject(ofd.FileName);
                m_com.Config.Save();
                //
                m_com.D2Stage.Clear();
                m_com.D2Stage.WriteFile(m_dirPathMC2D);

                // 初期パネル
                DockingPanelBeginningLayout();
                DockingPanelLayoutSave();

                NewProjecCreatedEvent(this, new NewProjectCreatedEventArgs(m_dirPathMC2D));
                D2StageFileOpenedEvent(this, new D2StageFileOpenedEventArgs(m_com.D2Stage));
                ProjectOpenedEvent(this, new ProjectOpenedEventArgs());
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// プロジェクトが作成された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProjecCreatedCall(object sender, NewProjectCreatedEventArgs e)
        {
        }
        /// <summary>
        /// プロジェクトが開かれた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectOpened(object sender, ProjectOpenedEventArgs e)
        {
            visibleMI.Enabled = true;
            closeProjectMI.Enabled = true;
            tsbAppRun.Enabled = true;
        }


        /// <summary>
        /// ファイルを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// アプリケーションを終了させる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitApplication(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemSolutionExplorer_Click(object sender, System.EventArgs e)
        {
            m_solutionExplorer.Show(dockPanel);
        }

        /// <summary>
        /// バージョン情報ダイアログ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void versionInfoMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 frmAbout = new AboutBox1();
            frmAbout.ShowDialog();
            frmAbout.Dispose();
            GC.Collect();
        }

        #endregion

        /// <summary>
        /// 最近使用したファイル群をメニューリストに挿入する。
        /// ただし、現在メニューにある物は初期化される
        /// </summary>
        private void InsertRecentlyUsedProjectFile()
        {
            projectListMI.DropDownItems.Clear();
            var v = m_com.Config.RecentProjects;
            for (int i = v.Count() - 1, j = 1; i >= 0; --i, ++j)
            {
                ToolStripMenuItem addProjectMI = new ToolStripMenuItem();
                addProjectMI.Name = "recentlyUsedProjectFile_" + j;
                addProjectMI.Text = j + " " + v[i].Title + " (" + j + ")";
                addProjectMI.Click += AddProjectMI_Click;
                projectListMI.DropDownItems.Add(addProjectMI);
            }
        }

        /// <summary>
        /// ウィンドウが読み込まれた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            CloseProject(this, new EventArgs());

            // JumpListオブジェクトを生成する
            var m_jumpList = new System.Windows.Shell.JumpList();


            if (m_com.Config.RecentProjects.Count() == 0)
            {
                projectListMI.Enabled = false;
            }
            else
            {
                projectListMI.Enabled = true;
                InsertRecentlyUsedProjectFile();
            }
        }
        /// <summary>
        /// プロジェクトファイルを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProjectMI_Click(object sender, EventArgs e)
        {
            if(sender.ToString() == "ToolStripMenuItem")
            {
                // 存在するか？
                string tmpPath = m_dirPathMC2D;
                try
                {
                    ToolStripMenuItem o = (ToolStripMenuItem)sender;
                    var m = Regex.Match(o.Text, @"^[0-9]\s");
                    while (m.Success)
                    {
                        Group g = m.Groups[1];
                        int i = int.Parse(g.Value);
                        var v = m_com.Config.RecentProjects[i];
                        if (!System.IO.File.Exists(v.FilePath))
                        {
                            MessageBox.Show("[" + v.Title, "]ファイルが存在しません。", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        m_dirPathMC2D = System.IO.Path.GetDirectoryName(v.FilePath);
                    }
                }
                catch(Exception ex)
                {
                    m_dirPathMC2D = tmpPath;
                    Console.WriteLine(ex.Message);
                    return;
                }


                //
                try
                {
                    m_com.D2Stage.Clear();
                    m_com.D2Stage.OpenFile(m_dirPathMC2D);

                    D2StageFileOpenedEvent(this, new D2StageFileOpenedEventArgs(m_com.D2Stage));
                    ProjectOpenedEvent(this, new ProjectOpenedEventArgs());
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        /// <summary>
        /// ウィンドウが閉じられている
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
        }        
        /// <summary>
        /// ウィンドウのサイズが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            //ResizeSplash();
        }
        /// <summary>
        /// オプションダイアログの表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShwoOptionForm(object sender, EventArgs e)
        {
            FormOption frmAbout = new FormOption(CommonMC2D.Instance);
            frmAbout.ShowDialog();
            frmAbout.Dispose();
            GC.Collect();
        }
        /// <summary>
        /// ドッキングパネルで、出力ウィンドウを表示させる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowOutputWindow(object sender, EventArgs e)
        {
            m_outputWindow.Show(dockPanel);
        }
        /// <summary>
        /// プロジェクトを閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseProject(object sender, EventArgs e)
        {
            ClosedProjectEvent(this, new ClosedProjectEventArgs());
            CloseAllContents();

            m_com.D2Stage.Clear();
            m_dirPathMC2D = "";
            visibleMI.Enabled = false;
            saveFileMenu.Enabled = false;
            allSaveMenuItem.Enabled = false;
            closeProjectMI.Enabled = false;
            tsbAppRun.Enabled = false;
        }
        /// <summary>
        /// 全てを保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllSave(object sender, EventArgs e)
        {
            try
            {
                AllSave(this, new AllSavedEventArgs());
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// MCD2実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppRun_Click(object sender, EventArgs e)
        {
            tsbAppRun.Enabled = false;
            m_com.Process.RunMC2dApp(m_com.DirPathMC2D);
        }

        /// <summary>
        /// MCD2実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessMC2DEnd(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() =>
            {
                tsbAppRun.Enabled = true;
            }));
        }
    }
}
