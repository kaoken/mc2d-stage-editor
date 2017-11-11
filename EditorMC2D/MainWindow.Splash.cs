using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace EditorMC2D
{
    /// <summary>
    /// スプラッシュ関連の処理
    /// </summary>
    partial class MainWindow
    {
        /// <summary>
        /// スプラッシュの表示・非表示
        /// </summary>
        private bool m_showSplash;
        private SplashScreen m_splashScreen;


        /// <summary>
        /// 起動時のタイトル
        /// </summary>
        private void SetSplashScreen()
        {
            m_showSplash = true;
            m_splashScreen = new SplashScreen();

            ResizeSplash();
            m_splashScreen.Visible = true;
            m_splashScreen.TopMost = true;

            Timer tm = new Timer();
            tm.Tick += (sender, e) =>
            {
                m_splashScreen.Visible = false;
                tm.Enabled = false;
                m_showSplash = false;
            };
            tm.Interval = 1000;
            tm.Enabled = true;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ResizeSplash()
        {
            if (m_showSplash)
            {
                var centerXMain = (this.Location.X + this.Width) / 2.0;
                var LocationXSplash = Math.Max(0, centerXMain - (m_splashScreen.Width / 2.0));

                var centerYMain = (this.Location.Y + this.Height) / 2.0;
                var LocationYSplash = Math.Max(0, centerYMain - (m_splashScreen.Height / 2.0));

                m_splashScreen.Location = new System.Drawing.Point((int)Math.Round(LocationXSplash), (int)Math.Round(LocationYSplash));
            }
        }

    }
}
