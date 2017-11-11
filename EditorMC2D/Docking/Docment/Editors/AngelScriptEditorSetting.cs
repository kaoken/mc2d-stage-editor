using EditorMC2D.Option;
using EditorMC2D.Option.Environment;
using MC2DUtil;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorMC2D.Docking.Docment.Editors
{
    public class AngelScriptEditorSetting : BaseTextEditorSetting
    {
        private AngleScriptEditor Form;

        /// <summary>
        /// ScintillNETエディタの初期化と設定
        /// </summary>
        /// <param name="TextArea"></param>
        /// <param name="Config"></param>
        public AngelScriptEditorSetting(AngleScriptEditor form, ScintillaNET.Scintilla e, APPConfig c)
        {
            this.Form = form;
            this.TextArea = e;
            this.Config = c;
            //ScintillaNET.X
            var tx = Config.GetTextEditorTheme("TextEditor");
            Items = tx.DisplayItems;



            // ホワイトスペースをオンにします？
            if (Config.TextConfig.IsWhiteSpace)
                TextArea.ViewWhitespace = WhitespaceMode.VisibleAlways;
            else
                TextArea.ViewWhitespace = WhitespaceMode.Invisible;


            // ワードラップをオンにします？
            if (Config.TextConfig.IsWordWrap)
                TextArea.WrapMode = WrapMode.Word;
            else
                TextArea.WrapMode = WrapMode.None;
            // INITIAL VIEW CONFIG
            TextArea.IndentationGuides = IndentView.LookBoth;

            TextArea.Lexer = Lexer.Cpp;
            TextArea.Font = new Font(tx.FontName, tx.FontSize);

            InitColors();

            //--------------------------
            // フォントの色をセット
            //--------------------------
            // Configure the default style
            TextArea.StyleResetDefault();
            SetStyle(Style.Cpp.Default, Items["TextFormat"]);
            TextArea.StyleClearAll();



            //SetStyle(c, GetStyle(e, "BRACEBAD"), aItem["IndicatorMargin"]);//★
            //SetStyle(c, GetStyle(e, "BRACELIGHT"), aItem["IndicatorMargin"]);//★
            //SetStyle(c, GetStyle(e, "CALLTIP"), aItem["IndicatorMargin"]);//★
            //SetStyle(c, GetStyle(e, "CONTROLCHAR"), aItem["IndicatorMargin"]);//★

            //SetStyle(c, GetStyle(e, "SIDESYMBOL"), aItem["IndicatorMargin"]);

            SetStyle(Style.Cpp.Comment, Items["Comment"]);
            SetStyle(Style.Cpp.CommentLine, Items["Comment"]);
            SetStyle(Style.Cpp.CommentDoc, Items["Comment"]);
            SetStyle(Style.Cpp.Word, Items["AS_UserKeyword"]);
            SetStyle(Style.Cpp.Number, Items["Number"]);
            SetStyle(Style.Cpp.String, Items["String"]);
            SetStyle(Style.Cpp.Character, Items["String"]);
            SetStyle(Style.Cpp.Verbatim, Items["String"]);
            //SetStyle(c, GetStyle(e, "UUID"), aItem["TextFormat"]);
            SetStyle(Style.Cpp.Preprocessor, Items["PreprocessorKeyword"]);
            SetStyle(Style.Cpp.Operator, Items["Operator"]);
            SetStyle(Style.Cpp.Identifier, Items["Identifier"]);
            SetStyle(Style.Cpp.StringEol, Items["LineNumber"]);
            SetStyle(Style.Cpp.Regex, Items["Regex"]);
            SetStyle(Style.Cpp.CommentLineDoc, Items["Doxygen"]);
            SetStyle(Style.Cpp.Word2, Items["AS_UserKeyword"]);
            SetStyle(Style.Cpp.GlobalClass, Items["AS_UserKeyword"]);

            SetStyle(Style.Cpp.CommentDocKeyword, Items["DoxygenCommand"]);
            SetStyle(Style.Cpp.CommentDocKeywordError, Items["DoxygenErrKey"]);
            //SetStyle(c, GetStyle(e, "DOXY_ARG_0"), aItem["DoxygenFirstArg"]);
            //SetStyle(c, GetStyle(e, "DOXY_ARG_1"), aItem["DoxygenSecondArg"]);
            //SetStyle(c, GetStyle(e, "DOXY_ARG_2"), aItem["DoxygenThirdArg"]);

            SetStyle(Style.Cpp.GlobalClass, Items["AS_UserKeyword"]);//★
            //SetStyle(c, GetStyle(e, "STRINGRAW"), aItem["String"]);//★
            //SetStyle(c, GetStyle(e, "TRIPLEVERBATIM"), aItem["AS_UserKeyword"]);//★
            //SetStyle(c, GetStyle(e, "HASHQUOTEDSTRING"), aItem["AS_UserKeyword"]);//★
            //SetStyle(c, GetStyle(e, "PREPROCESSORCOMMENT"), aItem["Doxygen"]);//★
            //SetStyle(c, GetStyle(e, "PREPROCESSORCOMMENTDOC"), aItem["Doxygen"]);//★
            //SetStyle(c, GetStyle(e, "USERLITERAL"), aItem["AS_UserKeyword"]);//★
            //SetStyle(c, GetStyle(e, "TASKMARKER"), aItem["AS_UserKeyword"]);//★
            //SetStyle(c, GetStyle(e, "ESCAPESEQUENCE"), aItem["AS_UserKeyword"]);//★

            TextArea.SetKeywords(0, "class interface new case do while else if for in switch throw get set function var while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated example exception haxe internal link mtasc mxmlc param private return since throws usage version langversion productversion dynamic private public partial intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
            TextArea.SetKeywords(1, "void Null arguments Array Boolean Class Date DefinitionError Error EvalError Function int String uint Boolean Byte Char Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");


            // NUMBER MARGIN
            InitNumberMargin();

            // BOOKMARK MARGIN
            InitBookmarkMargin(tx, Items["BreakpointEnabled"]);

            // CODE FOLDING MARGIN
            InitCodeFolding();

            // INIT HOTKEYS
            InitHotkeys();

            // マーカ類の初期化
            //UtilMarker.InitMarker(e);

            //e.FindReplace.HighlightAll(e.FindReplace.FindAll(ToolStripComboBoxTexteARechercherRapide.Text));
            //e.FindReplace.MarkAll(e.FindReplace.FindAll(ToolStripComboBoxTexteARechercherRapide.Text));
        }
        /// <summary>
        /// ホットキーの基本設定
        /// </summary>
        private void InitHotkeys()
        {

            // ホットキーをフォームに登録する
            //HotKeyManager.AddHotKey(Form, OpenSearch, Keys.F, true);
            //HotKeyManager.AddHotKey(Form, OpenFindDialog, Keys.F, true, false, true);
            //HotKeyManager.AddHotKey(Form, OpenReplaceDialog, Keys.R, true);
            //HotKeyManager.AddHotKey(Form, OpenReplaceDialog, Keys.H, true);
            //HotKeyManager.AddHotKey(Form, Uppercase, Keys.U, true);
            //HotKeyManager.AddHotKey(Form, Lowercase, Keys.L, true);
            //HotKeyManager.AddHotKey(Form, ZoomIn, Keys.Oemplus, true);
            //HotKeyManager.AddHotKey(Form, ZoomOut, Keys.OemMinus, true);
            //HotKeyManager.AddHotKey(Form, ZoomDefault, Keys.D0, true);
            //HotKeyManager.AddHotKey(Form, CloseSearch, Keys.Escape);

            // scintillaから競合するホットキーを削除する
            TextArea.ClearCmdKey(Keys.Control | Keys.F);
            TextArea.ClearCmdKey(Keys.Control | Keys.R);
            TextArea.ClearCmdKey(Keys.Control | Keys.H);
            TextArea.ClearCmdKey(Keys.Control | Keys.L);
            TextArea.ClearCmdKey(Keys.Control | Keys.U);
            TextArea.ClearCmdKey(Keys.Control | Keys.S);
        }



        #region Utils


        //public void InvokeIfNeeded(Action action)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.BeginInvoke(action);
        //    }
        //    else
        //    {
        //        action.Invoke();
        //    }
        //}

        #endregion

    }
}
