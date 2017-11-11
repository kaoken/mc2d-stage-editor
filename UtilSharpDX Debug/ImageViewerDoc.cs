using MC2DUtil;
using System;
using System.Drawing;
using System.Windows.Forms;
using UtilSharpDX.Controls;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.Math;

namespace UtilSharpDX_Debug
{
    public partial class ImageViewerDoc : Form
    {
        private static readonly string[] m_scaleStrList = new string[]
        {
            " 20 %",
            " 50 %",
            " 70 %",
            "100 %",
            "150 %",
            "200 %",
            "400 %",
        };
        private static readonly float[] m_scaleList = new float[]
        {
            0.2f,
            0.5f,
            0.7f,
            1.0f,
            1.5f,
            2.0f,
            4.0f,
        };


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageViewerDoc()
        {
            InitializeComponent();
            for(int i=0;i<m_scaleList.Length;++i)
                CBScaleList.Items.Add(m_scaleStrList[i]);
        }

        /// <summary>
        /// 画像読み込み
        /// </summary>
        /// <param name="path">ファイルパス、またはリソースパス</param>
        public void RaadImage(string path)
        {
            imageViwerContrl.ReadImage(path);

            LBImageTyep.Text = "画像の種類："+imageViwerContrl.ImageType.ToString();
            labelImageWH.Text = "  サイズ：" + imageViwerContrl.ImageWidth + "×" + imageViwerContrl.ImageHeight;
        }

        /// <summary>
        /// フォームをロード
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CBScaleList.SelectedIndex = 3;

            RaadImage(@"Media\Images\hero.dds");

            ImageViewerDoc_SizeChanged(null, new EventArgs());
            ScrollBar_ValueChanged(null, new EventArgs());
        }

        /// <summary>
        /// フォームのサイズが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageViewerDoc_SizeChanged(object sender, EventArgs e)
        {
            int w = ClientSize.Width - (vScrollBar.Width);
            int h = ClientSize.Height - (hScrollBar.Height+ topToolStrip.Height+ statusStrip.Height);
            imageViwerContrl.Location = new Point(0, topToolStrip.Bottom+1);
            imageViwerContrl.Size = new Size(w, h);


            vScrollBar.Height = h;
            vScrollBar.Location = new Point(ClientSize.Width - vScrollBar.Width, topToolStrip.Height);
            hScrollBar.Width = ClientSize.Width - vScrollBar.Width;
            hScrollBar.Location = new Point(0, h+ topToolStrip.Height);

            hScrollBar.Enabled = ClientSize.Width < imageViwerContrl.ImageScaleWidth;
            vScrollBar.Enabled = ClientSize.Height < imageViwerContrl.ImageScaleHeight;
        }

        /// <summary>
        /// スクロールバーの変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (imageViwerContrl == null) return;
            imageViwerContrl.CameraRangePosition = new MCVector2(
                hScrollBar.Value / (float)(1 + hScrollBar.Maximum - hScrollBar.LargeChange), 
                vScrollBar.Value / (float)(1 + vScrollBar.Maximum - vScrollBar.LargeChange));
#if DEBUG
            var p = imageViwerContrl.CameraPosition;
            LBScreenSize.Text = ClientSize.Width + "×" + ClientSize.Height + "--" + p.X + "×" + p.Y;

#endif
        }

        /// <summary>
        /// スケール値の変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBScaleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            imageViwerContrl.ImageScale = m_scaleList[CBScaleList.SelectedIndex];
            hScrollBar.Enabled = ClientSize.Width < imageViwerContrl.ImageScaleWidth;
            vScrollBar.Enabled = ClientSize.Height < imageViwerContrl.ImageScaleHeight;
        }

        /// <summary>
        /// 読み込み画像のα値
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlpha_Click(object sender, EventArgs e)
        {
            imageViwerContrl.IsAlpha = BtnAlpha.Checked;
            BtnAlpha.Checked = !BtnAlpha.Checked;
            if (BtnAlpha.Checked)
                BtnAlpha.BackColor = SystemColors.ActiveCaption;
            else
                BtnAlpha.BackColor = SystemColors.Control;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageViewerDoc_Resize(object sender, EventArgs e)
        {
#if DEBUG
            LBScreenSize.Text = ClientSize.Width + "×" + ClientSize.Height;
#endif
        }
    }
}
