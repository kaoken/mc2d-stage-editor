using System;
using System.Windows.Forms;

namespace EditorMC2D.Forms
{
    public partial class AddSquareImage : Form
    {

        public string FilePath{ get; private set;}
        public bool IsOK { get; private set; }
        public AddSquareImage()
        {
            InitializeComponent();
            IsOK = false;
        }

        /// <summary>
        /// 読み込み時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddSquareImage_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 一辺の長さが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EdgeLenghtt_ValueChanged(object sender, EventArgs e)
        {
            if((edgeLenght.Value % 2)== 1)
            {
                // 偶数に修正
                edgeLenght.Value += 1;
            }
        }

        /// <summary>
        /// メディアディレクトリ内の画像を選ぶダイアログを表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgSelect_Click(object sender, EventArgs e)
        {
            var dlg = new ImageList();
            dlg.ShowDialog();
            if(dlg.GetReadFullPathFileName() != "")
            {
                FilePath = dlg.GetReadFullPathFileName();
                tbImgFilePath.Text = FilePath;
                btnOK.Enabled = true;
            }
        }

        /// <summary>
        /// OKボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, EventArgs e)
        {
            IsOK = true;
            Close();
        }

        /// <summary>
        /// キャンセルボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            IsOK = false;
            Close();
        }
    }
}
