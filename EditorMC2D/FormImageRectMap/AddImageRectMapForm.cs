using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EditorMC2D.FormImageRectMap
{
    public partial class AddImageRectMapForm : Form
    {
        private string m_Name;
        private bool m_isOK;
        public AddImageRectMapForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 名を返す
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return m_Name;
        }
        public bool IsOK()
        {
            return m_isOK;
        }
        /// <summary>
        /// OKボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (textName.Text.Length == 0)
            {
                MessageBox.Show("名前が入力されていません。", "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button3
                );
                textName.Focus();
                return;
            }
            m_Name = textName.Text;
            m_isOK = true;
            this.Close();
        }
        /// <summary>
        /// キャンセルボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddImageRectMapForm_Load(object sender, EventArgs e)
        {
            m_isOK = false;
        }
    }
}
