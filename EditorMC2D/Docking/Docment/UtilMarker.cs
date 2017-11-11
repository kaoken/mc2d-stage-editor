using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorMC2D.Docking.Docment
{
    /// <summary>
    /// テキストエディタで使用するマーカー類
    /// </summary>
    public class UtilMarker
    {
        public const int BREKE_POINT     = 0;   /// 
        public const int BREKE_POINT_INV = 3;   /// 
        public const int STEP_IN         = 4;
        public const int STEP_IN_MARKER  = 5;
        public const int BOOK_MARK       = 10;
        public const int BOOK_MARK_INV   = 11;

        /// <summary>
        /// マーカーの初期化
        /// </summary>
        /// <param name="e"></param>
        public static void InitMarker(Scintilla e)
        {
            // ブレークポイント
            Marker marker = e.Markers[BREKE_POINT];
            marker.Symbol = MarkerSymbol.RgbaImage;
            marker.DefineRgbaImage(Properties.Resources.BreakpointEnable_16x);

            // 無効ブレークポイント
            marker = e.Markers[BREKE_POINT_INV];
            marker.Symbol = MarkerSymbol.RgbaImage;
            marker.DefineRgbaImage(Properties.Resources.BreakpointDisable_16x);
            // ステップイン
            marker = e.Markers[STEP_IN];
            marker.Symbol = MarkerSymbol.RgbaImage;
            marker.DefineRgbaImage(Properties.Resources.CurrentInstructionPointer_16x);
            // ステップイン・マーカー
            marker = e.Markers[STEP_IN_MARKER];
            marker.Symbol = MarkerSymbol.Background;
            marker.SetBackColor(System.Drawing.Color.Yellow);
            // ブックマーク
            marker = e.Markers[BOOK_MARK];
            marker.Symbol = MarkerSymbol.RgbaImage;
            marker.DefineRgbaImage(Properties.Resources.bookmark_003_16xLG);
            // ブックマーク無効
            marker = e.Markers[BOOK_MARK_INV];
            marker.Symbol = MarkerSymbol.RgbaImage;
            marker.DefineRgbaImage(Properties.Resources.bookmark_006_16xLG);
        }

        #region ブックマーク　＆ ブックマーク
        /// <summary>
        /// ブレークポイントマーカ取得
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Marker GetBrekePoint(Scintilla e) { return e.Markers[BREKE_POINT]; }
        /// <summary>
        /// 無効ブレークポイントマーカ取得
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Marker GetBrekePointInv(Scintilla e) { return e.Markers[BREKE_POINT_INV]; }
        /// <summary>
        /// ブックマーク取得
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Marker GetBookMark(Scintilla e) { return e.Markers[BOOK_MARK]; }
        /// <summary>
        /// 無効ブックマーク取得
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Marker GetBookMarkInv(Scintilla e) { return e.Markers[BOOK_MARK_INV]; }
        /// <summary>
        /// ブレークポイントマーカ描画
        /// </summary>
        /// <param name="e">エディター</param>
        /// <param name="line">行数</param>
        /// <returns></returns>
        public static void DrawBrekePoint(Scintilla e, int line) { if (e.Lines.Count <= line)return; e.Lines[line].MarkerAdd(BREKE_POINT); }
        /// <summary>
        /// 無効ブレークポイントマーカ描画
        /// </summary>
        /// <param name="e">エディター</param>
        /// <param name="line">行数</param>
        /// <returns></returns>
        public static void DrawBrekePointInv(Scintilla e, int line) { if (e.Lines.Count <= line)return; e.Lines[line].MarkerAdd(BREKE_POINT_INV); }
        /// <summary>
        /// ブックマーク描画
        /// </summary>
        /// <param name="e">エディター</param>
        /// <param name="line">行数</param>
        /// <returns></returns>
        public static void DrawBookMark(Scintilla e, int line) { if (e.Lines.Count <= line)return; e.Lines[line].MarkerAdd(BOOK_MARK); }
        /// <summary>
        /// 無効ブックマーク描画
        /// </summary>
        /// <param name="e">エディター</param>
        /// <param name="line">行数</param>
        /// <returns></returns>
        public static void DrawBookMarkInv(Scintilla e, int line) { if (e.Lines.Count <= line)return; e.Lines[line].MarkerAdd(BOOK_MARK_INV); }
        /// <summary>
        /// 指定行のブレークポイントマーカー削除
        /// </summary>
        /// <param name="e">エディタ</param>
        /// <param name="line">行番号</param>
        public static void DeletBrekePoint(Scintilla e, int line) { if (e.Lines.Count <= line)return; e.Lines[line].MarkerDelete(BREKE_POINT); }
        /// <summary>
        /// 指定行の無効ブレークポイントマーカー削除
        /// </summary>
        /// <param name="e">エディタ</param>
        /// <param name="line">行番号</param>
        public static void DeletBrekePointInv(Scintilla e, int line) { if (e.Lines.Count <= line)return; e.Lines[line].MarkerDelete(BREKE_POINT_INV); }
        /// <summary>
        /// 指定行のブックマーク削除
        /// </summary>
        /// <param name="e">エディタ</param>
        /// <param name="line">行番号</param>
        public static void DeletBookMark(Scintilla e, int line) { if (e.Lines.Count <= line)return; e.Lines[line].MarkerDelete(BOOK_MARK); }
        /// <summary>
        /// 指定行の無効ブックマーク削除
        /// </summary>
        /// <param name="e">エディタ</param>
        /// <param name="line">行番号</param>
        public static void DeletBookMarkInv(Scintilla e, int line) { if (e.Lines.Count <= line)return; e.Lines[line].MarkerDelete(BOOK_MARK_INV); }
        /// <summary>
        /// 指定した行の左側のマージンマーカーを削除
        /// </summary>
        /// <param name="e"></param>
        /// <param name="line"></param>
        public static void DeletRightMarginMarker(Scintilla e, int line)
        {
            DeletBrekePoint(e, line);
            DeletBrekePointInv(e, line);
            DeletBookMark(e, line);
            DeletBookMarkInv(e, line);
        }
        /// <summary>
        ///  左側のマージンマーカーを全て削除
        /// </summary>
        /// <param name="e"></param>
        public static void DeletRightMarginMarker(Scintilla e)
        {
            for (int i = 0; i < e.Lines.Count; ++i)
            {
                UtilMarker.DeletRightMarginMarker(e, i);
            }
        }
        #endregion

        #region 警告波線

        #endregion
    }
}
