using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using System.IO;

namespace MapEdit
{
    public partial class FileSelectForm : Form
    {
        private bool m_bSelectFile = false;
        private string m_strReadFullPathFileName="";
        private string m_strReadFileName="";
        /// <summary>
        /// 
        /// </summary>
        public FileSelectForm()
        {
            InitializeComponent();
        }
        public bool GetSelectFileFlg()
        {
            return m_bSelectFile;
        }
        public string GetFullPathFileName()
        {
            return m_strReadFullPathFileName;
        }
        public string GetFileName()
        {
            return m_strReadFileName;
        }
        /// <summary>
        /// OKボタンを押した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBtnOK_MouseClick(object sender, MouseEventArgs e)
        {
            m_bSelectFile = true;
            this.Close();
        }
        /// <summary>
        /// キャンセルボタンを押した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBtnCancel_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// リストビューの内容が変わった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTmp;
            if (listView1.Items.Count == 0) return;
            if (listView1.SelectedItems.Count == 0) return;
            strTmp = listView1.SelectedItems[0].SubItems[0].Text;

            this.FileLoad(strTmp);
            objBtnOK.Enabled = true;
            objLBFile.Text = m_strReadFileName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName">ファイル名</param>
        /// <returns>読み込みエラーのときはfalseを返す。　成功時には、trueを返す</returns>
        bool FileLoad(string strFileName)
        {
            string strFullPas="";

            if (strFileName == "")
                return false;

            if (m_strReadFileName == strFileName)
                return true;

            m_strReadFileName = strFileName;
            strFullPas = System.IO.Path.GetDirectoryName(Application.ExecutablePath);//.Replace('\\', '/');
            strFullPas += "/data/stg/";
            strFullPas += strFileName;

            // フルパス取得
            m_strReadFullPathFileName = strFullPas;
            return true;
        }
        /// <summary>
        /// 指定されたディレクトリ内のファイルを列挙する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSelectForm_Shown(object sender, EventArgs e)
        {
            string strFileFolder;
            string strDate;
            int i;

            listView1.Items.Clear();

            //FolderPasを作成
            strFileFolder = "./data/stg/";

            //DirectoryInfoを作成
            DirectoryInfo di = new DirectoryInfo(strFileFolder); //パスを指定
            FileInfo[] fis = di.GetFiles("*.stg");	//パターンを指定
            for ( i = 0; i < fis.Length; i++)
            {
                if (fis[i].Name == "." || fis[i].Name == "..") continue;

                strDate = string.Format("{0:yyyy/MM/dd hh:mm}", fis[i].CreationTime);

                if ( (fis[i].Attributes & FileAttributes.Directory) == 0)
                {
                    try
                    {
                        string[] item1 = { fis[i].Name, strDate };
                        ListViewItem lvTmp = new ListViewItem(item1);

                        listView1.Items.Add(lvTmp);
                    }
                    catch
                    {

                    }

                }
            }
            objBtnOK.Enabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            m_bSelectFile = true;
            this.Close();
        }
    }
}
