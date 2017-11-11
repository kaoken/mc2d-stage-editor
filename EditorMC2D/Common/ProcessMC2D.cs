using EditorMC2D.Events;
using System;
using System.Diagnostics;
using System.Threading;

namespace EditorMC2D.Common
{
    public class ProcessMC2D
    {
        public ProcessMC2DEndHandler ProcessMC2DEndEvent;
        public ProcessMC2DStartHandler ProcessMC2DStartEvent;

        private string m_MC2DFilePath;
        private Process m_process = null;
        private const string m_processName = "MC2D.exe";
        private bool m_isRuning;
        private System.Object m_lockThis = new System.Object();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessMC2D()
        {
            m_isRuning = false;
        }
        /// <summary>
        /// アプリを強制停止する
        /// </summary>
        public void StopMC2dApp()
        {
            if (m_process != null)
            {
                if(!m_process.HasExited)
                    m_process.Kill();
            }
            m_process = null;
            while (IsRunMC2dApp())
            {
                Thread.Sleep(500);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessMC2DExit(object sender, EventArgs e)
        {
            if (ProcessMC2DEndEvent != null)
                ProcessMC2DEndEvent(this, new ProcessMC2DEndEventArgs());

            m_process = null;
            m_isRuning = false;
        }
        /// <summary>
        /// アプリの実行
        /// </summary>
        public void RunMC2dApp(string path)
        {
            try
            {
                StopMC2dApp();
                m_process = new Process();
                m_process.StartInfo.FileName = m_MC2DFilePath = path + "/MC2D.exe";
                m_process.StartInfo.WorkingDirectory = path;
                m_process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                m_process.EnableRaisingEvents = true;
                m_process.Exited += new EventHandler(ProcessMC2DExit);
                m_process.Start();
                m_isRuning = true;
                if (ProcessMC2DStartEvent != null && !m_process.HasExited)
                    ProcessMC2DStartEvent(this, new ProcessMC2DStartEventArgs());
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred trying to print \"{0}\":" + "\n" + ex.Message, m_MC2DFilePath);
                return;
            }
        }
        /// <summary>
        /// アプリが実行中か
        /// </summary>
        /// <returns></returns>
        public bool IsRunMC2dApp()
        {
            lock (m_lockThis)
            {
                return m_isRuning;
            }
        }
    }
}
