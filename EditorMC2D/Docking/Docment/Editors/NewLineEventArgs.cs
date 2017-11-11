using ScintillaNET;
using System;

namespace EditorMC2D.Docking.Docment.Editors
{
    /// <summary>
    /// 新しい行が追加されたなどのイベント
    /// </summary>
    public class NewLineEventArgs : EventArgs
    {
        public class NewLine
        {
            public int  line;       /// <summary>行</summary>  
            public int  count;      /// <summary>カウント</summary>  
            public bool isEnable;   /// <summary>有効か</summary>  

            public void Init()
            {
                line = -1;
                count = -1;
                isEnable = false;
            }
            public NewLine() { Init(); }
            public void Set(ScintillaNET.Scintilla ed, ModificationEventArgs e)
            {
                Set(ed,
                    e.Position,
                    e.Text.Length
                );

            }
            public void Set(ScintillaNET.Scintilla ed, BeforeModificationEventArgs e)
            {
                int len = 0;
                if (e.Text != null) len = e.Text.Length;
                Set(ed,
                    e.Position,
                    len
                );
            }
            protected void Set(ScintillaNET.Scintilla ed, int position, int len)
            {
                int endPos = position + len;
                int lnStart = ed.LineFromPosition(position);
                int lnEnd = ed.LineFromPosition(endPos);

                line = ed.Lines.Count;

                count = lnEnd - lnStart;

                isEnable = false;

                if (count == 0)
                {
                    if (position == lnStart && endPos == lnEnd)
                    {
                        isEnable = true;
                        ++count;
                    }
                }
                else if (position == lnStart)
                {   // 行の開始位置か

                    if (endPos == lnEnd)
                    {
                        // 最終位置が終了行がスタート位置
                        --count;
                    }
                    else if(endPos == lnEnd)
                    {
                        // 最終位置が終了行が終了位置
                        //++count;
                    }
                }
                else 
                {
                    if (endPos == lnEnd)
                    {
                        line++;
                        count--;
                    }
                    else if( endPos == lnEnd)
                    {
                        ;
                    }
                    else
                    {
                        Init();
                        return;
                    }
                }

            }
        }

        public const int DELETE = 0;
        public const int INSERT = 1;
        private NewLine m_NL = new NewLine();
        private int kind;
        public int Kind { get { return kind; } }
        public int Line { get { return m_NL.line; } }
        public int Count { get { return m_NL.count; } }
        public NewLineEventArgs(ScintillaNET.Scintilla ed, ModificationEventArgs e, NewLine nl, int k)
        {
            kind = k;
            m_NL.Set(ed, e);
        }
        public NewLineEventArgs(ScintillaNET.Scintilla ed, BeforeModificationEventArgs e, NewLine nl, int k)
        {
            kind = k;
            m_NL.Set(ed, e);
        }
    }
}
