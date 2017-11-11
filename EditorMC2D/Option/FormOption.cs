using EditorMC2D.Common;
using EditorMC2D.Option.Environment;
using EditorMC2D.Option.TextEditer;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EditorMC2D.Option
{
    public partial class FormOption : Form
    {
        private CommonMC2D m_comCore;
        private APPConfig m_configTmp;
        private Form m_currentForm = null;

        [DllImport("USER32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point pt);

        public FormOption(CommonMC2D comCore)
        {
            m_comCore = comCore;
            m_configTmp = comCore.Config;//.Cloen();
            InitializeComponent();
        }

        private void FormOption_Load(object sender, EventArgs e)
        {
            CurrentFormClose(new FormWhole(m_comCore, m_configTmp));
        }

        private void FormOption_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_currentForm != null)
            {
                m_currentForm.Close();
                m_currentForm.Dispose();
            }
        }
        /// <summary>
        /// 現在、選択中のフォームを設定する
        /// </summary>
        /// <param name="fm">Form</param>
        private void CurrentFormClose(Form fm)
        {
            if (fm == m_currentForm) return;

            if (m_currentForm != null )
            {
                m_currentForm.Close();
                if (objPanel.Controls.Count != 0)
                    objPanel.Controls.Remove(fm);
                m_currentForm.Dispose();
                GC.Collect();
            }
            m_currentForm = fm;
            fm.TopLevel = false;
            objPanel.Controls.Add(fm);
            fm.Show();
        }
        private bool IsCurrentFormSame(string name)
        {
            if (m_currentForm == null) return false;
            return m_currentForm.Name == name;
        }
        /// <summary>
        /// ツリービュー内でマウスが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTree_MouseDown(object sender, MouseEventArgs e)
        {
            objTree.SelectedNode = objTree.GetNodeAt(e.X, e.Y);　//マウス座標から選択位置のノードを取得
            if (objTree.SelectedNode != null)  //ノード上でクリックされたか？
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point point = new Point(e.X, e.Y);
                    ClientToScreen(objTree.Handle, ref point);
                    if (!IsCurrentFormSame(objTree.SelectedNode.Name))
                    {
                        if (objTree.SelectedNode.Name == "FormWhole")
                        {
                            CurrentFormClose(new FormWhole(m_comCore, m_configTmp));
                        }
                        else if (objTree.SelectedNode.Name == "FormFontAndColor")
                        {
                            CurrentFormClose(new FormFontAndColor(m_comCore, m_configTmp));
                        }
                        else if (objTree.SelectedNode.Name == "FormASTextEditorConfig")
                        {
                            CurrentFormClose(new FormASTextEditorConfig(m_comCore, m_configTmp));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// OKボタンをクリックした
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objOK_Click(object sender, EventArgs e)
        {
            // 設定の反映

            this.Close();
        }
        /// <summary>
        /// キャンセルボタンをクリックした
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// ノードを展開した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            node.ImageIndex = 1;
            node.SelectedImageIndex = 1;
        }
        /// <summary>
        /// ノードを縮小した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
        }
    }
}
