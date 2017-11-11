using EditorMC2D.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorMC2D.Option.Environment
{
    public partial class FormWhole : Form
    {
        private CommonMC2D m_comCore;
        private APPConfig m_config;

        public FormWhole(CommonMC2D comCore, APPConfig config)
        {
            InitializeComponent();
            m_comCore = comCore;
            m_config = config;
        }

        private void FormWhole_Load(object sender, EventArgs e)
        {
            objTBFilePath.Text = m_config.Whole.FilePath;
            if (m_config.Whole.IsRunMCAS)
                objRBExeNet.Checked = true;
            else
                objRBNet.Checked = true;
            CheckedFilePath();
        }

        private void FormWhole_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_config.Whole.FilePath = objTBFilePath.Text;
            m_config.Whole.IsRunMCAS = objRBExeNet.Checked;
        }

        private void objRBExeNet_CheckedChanged(object sender, EventArgs e)
        {
            CheckedFilePath();
        }
        private void CheckedFilePath()
        {
            if (objRBExeNet.Checked)
            {
                objLFilePath.Enabled = true;
                objTBFilePath.Enabled = true;
                objBtnFilePath.Enabled = true;
            }
            else
            {
                objLFilePath.Enabled = false;
                objTBFilePath.Enabled = false;
                objBtnFilePath.Enabled = false;
            }
        }

        private void objBtnFilePath_Click(object sender, EventArgs e)
        {
            objOFDMain.FileName = Path.GetFileName(objTBFilePath.Text);

            if (Path.IsPathRooted(objTBFilePath.Text))
                objOFDMain.InitialDirectory = Path.GetDirectoryName(objTBFilePath.Text);
            else
                objOFDMain.InitialDirectory = Path.GetFullPath(Path.GetDirectoryName(objTBFilePath.Text));

            objOFDMain.Filter = "実行ファイル(*.exe)|*.exe";
            objOFDMain.Title = "開くファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            objOFDMain.RestoreDirectory = true;

            if (DialogResult.OK == objOFDMain.ShowDialog())
            {
                objTBFilePath.Text = objOFDMain.FileName;
            }
        }
    }
}
