using EditorMC2D.Common;
using EditorMC2D.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EditorMC2D.Docking.Docment
{
    public partial class FontDoc : DockContent
    {
        private string m_filePath = "";
        private string m_fileFullPath = "";
        public DocumentFocusHandler DocumentFocusEvent;
        FontFamily m_fontFamiliy = null;

        /// <summary>
        /// scディレクトリ以降のファイルパス
        /// </summary>
        public string FilePath { get { return m_filePath; } }
        /// <summary>
        /// ファイルのフルパス
        /// </summary>
        public string FileFullPath { get { return m_fileFullPath; } }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        public FontDoc(string path)
        {
            InitializeComponent();
            m_filePath = path;
            m_fileFullPath = CommonMC2D.Instance.DirPathMC2D + @"\" + path;
            this.Icon = Icon.FromHandle(Properties.Resources.Font_16x.GetHicon());

            PrivateFontCollection privateFontCollection = new PrivateFontCollection();

            privateFontCollection.AddFontFile(m_fileFullPath);

            m_fontFamiliy = privateFontCollection.Families[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DocumentFocus(Object sender, EventArgs e)
        {
            if (DocumentFocusEvent != null)
                DocumentFocusEvent(this, new DocumentFocusEventArgs(this));
        }

        private void FontDoc_Load(object sender, EventArgs e)
        {

        }

        private void FontDoc_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            SolidBrush solidBrush = new SolidBrush(Color.Black);
            string sample = "Windows　でコンピューターの世界が広がります。 0123456789";


            // Is the regular style available?
            if (m_fontFamiliy.IsStyleAvailable(FontStyle.Regular))
            {
                int y = 0;
                Font font, fontDef = new Font(
                   System.Drawing.SystemFonts.DefaultFont.FontFamily,
                   13,
                   FontStyle.Regular,
                   GraphicsUnit.Pixel);

                g.DrawString(
                   "フォント名：" + m_fontFamiliy.Name,
                   fontDef, solidBrush, 0, 2);
                y = fontDef.Height+2;
                g.DrawLine(new Pen(Color.Black), 0, y, Width, y);

                font = FontSize(24);
                y += 3;
                g.DrawString(
                   "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ ",
                    font, solidBrush, 0, y);
                y += font.Height;
                g.DrawString(
                   "1234567890.:,; ' \" (!?) +-*/=",
                    FontSize(24), solidBrush, 0, y);
                y += font.Height + 3;
                g.DrawLine(new Pen(Color.Black), 0, y, Width, y);


                g.DrawString(sample, (font = FontSize(18)), solidBrush, 26, (y += 3));
                g.DrawString("18", fontDef, solidBrush, 0, y + BaseLine(font) - fontDef.Height);
                g.DrawString(sample, (font = FontSize(24)), solidBrush, 26, (y += font.Height + 1));
                g.DrawString("24", fontDef, solidBrush, 0, y + BaseLine(font) - fontDef.Height);
                g.DrawString(sample, (font = FontSize(36)), solidBrush, 26, (y += font.Height + 1));
                g.DrawString("36", fontDef, solidBrush, 0, y + BaseLine(font) - fontDef.Height);
                g.DrawString(sample, (font = FontSize(48)), solidBrush, 26, (y += font.Height + 1));
                g.DrawString("48", fontDef, solidBrush, 0, y + BaseLine(font) - fontDef.Height);
                g.DrawString(sample, (font = FontSize(60)), solidBrush, 26, (y += font.Height + 1));
                g.DrawString("60", fontDef, solidBrush, 0, y + BaseLine(font) - fontDef.Height);
                g.DrawString(sample, (font = FontSize(72)), solidBrush, 26, (y += font.Height + 1));
                g.DrawString("72", fontDef, solidBrush, 0, y + BaseLine(font) - fontDef.Height);
            }

        }
        private int BaseLine(Font font)
        {
            return (int)((font.SizeInPoints * (96.0f / 72)) * (font.FontFamily.GetCellAscent(FontStyle.Regular) / (float)font.FontFamily.GetEmHeight(FontStyle.Regular)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private Font FontSize(int size)
        {
            return new Font(
                    m_fontFamiliy,
                    size,
                    FontStyle.Regular,
                    GraphicsUnit.Pixel);
        }
    }
}
