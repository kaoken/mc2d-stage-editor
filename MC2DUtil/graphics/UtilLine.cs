using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MC2DUtil.graphics
{
    /// <summary>
    /// 線を単純に描画するためのもの
    /// </summary>
    public class UtilLine
    {
        public static void DrawTextWaveLine(Graphics g, Pen pen, string s, Font font, Brush brush, RectangleF r, StringFormat sf)
        {
            Size txtSize = TextRenderer.MeasureText(s, font);
            PointF start = new PointF();
            PointF end = new PointF();
            StringFormat sfCopy = (StringFormat)sf.Clone();
            CharacterRange[] characterRanges = { new CharacterRange(0, s.Length) };
            sfCopy.SetMeasurableCharacterRanges(characterRanges);

            Region[] rr = g.MeasureCharacterRanges(s, font, r, sfCopy);
            RectangleF txtR = rr[0].GetBounds(g);

            start.Y = end.Y = txtR.Y + txtR.Height;
            start.X = txtR.X;
            end.X = txtR.X + txtR.Width;
            g.DrawString(s, font, brush, r, sf);
            DrawWave(g, pen, start,end);
        }
        public static void DrawWave(Graphics g, Pen pen, PointF start, PointF end)  
        {
            SmoothingMode tmp = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.HighQuality;
            if ( (end.X-start.X) > 4 )  
            {  
                ArrayList pl = new ArrayList();  
                for (float i = start.X; i <= (end.X-2); i += 4)  
                {
                    pl.Add(new PointF(i, start.Y));
                    pl.Add(new PointF(i + 2, start.Y + 2));  
                }
                PointF[] p = (PointF[])pl.ToArray(typeof(PointF));  
                g.DrawLines(pen, p);  
            }  
            else   
            {  
                g.DrawLine(pen, start, end);  
            }
            g.SmoothingMode = tmp;
        } 
    }
}
