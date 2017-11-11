using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorMC2D.Docking.Docment.Editors.Parsers.AngelScript
{
    public enum Kind : int
    {
        EOF,
        Ident,
        IntCon,
        RealCon,
        CharCon,
        StringCon,
        Abstract,
        As,
        Base,
        Bool,
        Break,
        Byte,
        Case,
        Catch,
        Char,
        Checked,
        Class,
        Const,
        Continue,
        Decimal,
        Default,
        Delegate,
        Do,
        Double,
        Else,
        Enum,
        Event,
        Explicit,
        Extern,
        False,
        Finally,
        Fixed,
        Float,
        For,
        Foreach,
        Goto,
        If,
        Implicit,
        In,
        Int,
        Interface,
        Internal,
        Is,
        Lock,
        Long,
        Namespace,
        New,
        Null,
        Object,
        Operator,
        Out,
        Override,
        Params,
        Private,
        Protected,
        Public,
        Readonly,
        Ref,
        Return,
        Sbyte,
        Sealed,
        Short,
        Sizeof,
        Stackalloc,
        Static,
        String,
        Struct,
        Switch,
        This,
        Throw,
        True,
        Try,
        Typeof,
        Uint,
        Ulong,
        Unchecked,
        Unsafe,
        Ushort,
        UsingKW,
        Virtual,
        Void,
        Volatile,
        While,
        And,
        Andassgn,
        Assgn,
        Colon,
        Comma,
        Dec,
        Divassgn,
        Dot,
        DblColon,
        Eq,
        Gt,
        Gteq,
        Inc,
        LBrace,
        LBrack,
        LPar,
        Lshassgn,
        Lt,
        Ltlt,
        Minus,
        Minusassgn,
        Modassgn,
        Neq,
        Not,
        Orassgn,
        Plus,
        Plusassgn,
        Question,
        RBrace,
        RBrack,
        RPar,
        SColon,
        Tilde,
        Times,
        Timesassgn,
        Xorassgn,
        Partial,
        Yield,
        NullCoalescing,
        ConditionalOR,
        ConditionalAND,
        LogicalOR,
        LogicalXOR,
        LessEqual,
        Division,
        Modulus,
        DereferencePtr,
        MaxT,
        PPDefine,
        PPUndef,
        PPIf,
        PPElif,
        PPElse,
        PPEndif,
        PPLine,
        PPError,
        PPWarning,
        PPRegion,
        PPEndReg,
        InvClassType,
        InvClassMemberDecl,
        InvClassMemberDecl2,
        InvStructMemberDecl,
        InvStructMemberDecl2,
        InvStructMemberDecl3,
        InvStructMemberDecl4,
        InvStructMemberDecl5,
        InvStructMemberDecl6,
        InvStructMemberDecl7,
        InvStructMemberDecl8,
        InvStructMemberDecl9,
        InvStructMemberDecl10,
        InvExpression,
        InvEventAccessorDecl,
        InvEventAccessorDecl2,
        InvOverloadableOp,
        InvAccessorDecl,
        InvAccessorDecl2,
        InvAccessorDecl3,
        InvAccessorDecl4,
        InvInterfaceAccessors,
        InvInterfaceAccessors2,
        InvLocalVariableDecl,
        InvVariableInit,
        InvKeyword,
        InvAttributeArguments,
        InvPrimitiveType,
        InvPointerOrArray,
        InvResolvedType,
        InvInternalClassType,
        InvStatement,
        InvEmbeddedStatement,
        InvEmbeddedStatement2,
        InvEmbeddedStatement3,
        InvEmbeddedStatement4,
        InvStatementExpression,
        InvForInitializer,
        InvCatchClauses,
        InvResourceAcquisition,
        InvUnary,
        InvAssignmentOperator,
        InvSwitchLabel,
        InvRelExpr,
        InvRelExpr2,
        InvShiftExpr,
        InvPrimary,
        InvPrimary2,
        InvPrimary3,
        InvLiteral
    };

    public class Token
    {
        public Kind kind;   // トークンの種類
        public int pos;     // ソーステキスト内のトークンの位置（0から始まる）
        public int col;     // トークン列（1から始まる）
        public int line;    // トークンの行（1から始まる）
        public string val;  // トークン値
        public Token next;  // ML2005-03-11トークンはリンクされたリストに保持されます。
    }

    //-----------------------------------------------------------------------------------
    // Buffer
    //-----------------------------------------------------------------------------------
    public class Buffer
    {
        // このバッファは、次のような場合はサポートしています:
        // 1) シーク可能なストリーム（ファイル）
        //    a) バッファ内のストリーム全体
        //    b) バッファ内のストリームの一部
        // 2) 非シークストリーム（ネットワーク、コンソール）

        public const int EOF = char.MaxValue + 1;
        const int MIN_BUFFER_LENGTH = 1024; // 1KB
        const int MAX_BUFFER_LENGTH = MIN_BUFFER_LENGTH * 64; // 64KB
        byte[] buf;         // 入力バッファ
        int bufStart;       // 入力ストリームからの相対バッファ内の最初のバイトの位置
        int bufLen;         // バッファの長さ
        int fileLen;        // 入力ストリームの長さは、（ストリームがファイルでない場合変更されることがあります）
        int bufPos;         // バッファ内の現在位置
        Stream stream;      // 入力ストリーム（位置）
        bool isUserStream;  // ストリームは、ユーザによって開かれたか？

        public Buffer(Stream s, bool isUserStream)
        {
            stream = s; this.isUserStream = isUserStream;

            if (stream.CanSeek)
            {
                fileLen = (int)stream.Length;
                bufLen = Math.Min(fileLen, MAX_BUFFER_LENGTH);
                bufStart = Int32.MaxValue; // これまでバッファに何もない
            }
            else
            {
                fileLen = bufLen = bufStart = 0;
            }

            buf = new byte[(bufLen > 0) ? bufLen : MIN_BUFFER_LENGTH];
            if (fileLen > 0) Pos = 0; // 位置を0に設定バッファ（スタート）
            else bufPos = 0; // index 0 is already after the file, thus Pos = 0 is invalid
            if (bufLen == fileLen && stream.CanSeek) Close();
        }

        protected Buffer(Buffer b)
        { // UTF8Bufferコンストラクタで呼び出された
            buf = b.buf;
            bufStart = b.bufStart;
            bufLen = b.bufLen;
            fileLen = b.fileLen;
            bufPos = b.bufPos;
            stream = b.stream;
            // ストリームを閉じてからデストラクタを保つ。
            b.stream = null;
            isUserStream = b.isUserStream;
        }

        ~Buffer() { Close(); }

        protected void Close()
        {
            if (!isUserStream && stream != null)
            {
                stream.Close();
                stream = null;
            }
        }

        public virtual int Read()
        {
            if (bufPos < bufLen)
            {
                return buf[bufPos++];
            }
            else if (Pos < fileLen)
            {
                Pos = Pos; // 位置にバッファの先頭をシフト。
                return buf[bufPos++];
            }
            else if (stream != null && !stream.CanSeek && ReadNextStreamChunk() > 0)
            {
                return buf[bufPos++];
            }
            else
            {
                return EOF;
            }
        }

        public int Peek()
        {
            int curPos = Pos;
            int ch = Read();
            Pos = curPos;
            return ch;
        }

        public string GetString(int beg, int end)
        {
            int len = end - beg;
            char[] buf = new char[len];
            int oldPos = Pos;
            Pos = beg;
            for (int i = 0; i < len; i++) buf[i] = (char)Read();
            Pos = oldPos;
            return new String(buf);
        }

        public int Pos
        {
            get { return bufPos + bufStart; }
            set
            {
                if (value >= fileLen && stream != null && !stream.CanSeek)
                {
                    // 所望のポジションは、バッファした後で、ストリームがシークできる
                    // 例えばネットワークまたはコンソールではない
                    // 所望の位置が視界にあるまでこのように、手動でストリームを読まなければならない。
                    while (value >= fileLen && ReadNextStreamChunk() > 0) ;
                }

                if (value < 0 || value > fileLen)
                {
                    throw new FatalError("境界アクセス外のバッファ、位置: " + value);
                }

                if (value >= bufStart && value < bufStart + bufLen)
                { // 既にバッファ内
                    bufPos = value - bufStart;
                }
                else if (stream != null)
                { // スワップインされている必要があり
                    stream.Seek(value, SeekOrigin.Begin);
                    bufLen = stream.Read(buf, 0, buf.Length);
                    bufStart = value; bufPos = 0;
                }
                else
                {
                    // ファイルの最後に位置を設定し、posはfileLenを返します。
                    bufPos = fileLen - bufStart;
                }
            }
        }

        // ストリームからバイトの次のチャンクを読む、バッファが増加
        // ストリームからバイトの次のチャンクは、必要に応じて
        // バッファを増やし、更新フィールドfileLenとBUFLEN読む
        private int ReadNextStreamChunk()
        {
            int free = buf.Length - bufLen;
            if (free == 0)
            {
                // 成長している入力ストリームの場合、どちらストリームに求めることはできない、
                // また、我々は最大の長さを予測することができ、このように、要求に応じて
                // バッファサイズを適応しなければならない。
                byte[] newBuf = new byte[bufLen * 2];
                Array.Copy(buf, newBuf, bufLen);
                buf = newBuf;
                free = bufLen;
            }
            int read = stream.Read(buf, bufLen, free);
            if (read > 0)
            {
                fileLen = bufLen = (bufLen + read);
                return read;
            }
            // ストリームの終わりに達した
            return 0;
        }
    }

    //-----------------------------------------------------------------------------------
    // UTF8Buffer
    //-----------------------------------------------------------------------------------
    public class UTF8Buffer : Buffer
    {
        public UTF8Buffer(Buffer b) : base(b) { }

        public override int Read()
        {
            int ch;
            do
            {
                ch = base.Read();
                // UFT8の開始（の0xxxxxxxまたは11xxxxxx）が見つかるまで
            } while ((ch >= 128) && ((ch & 0xC0) != 0xC0) && (ch != EOF));
            if (ch < 128 || ch == EOF)
            {
                // 何の関係もない最初の127文字は、ASCIIとUTF8の0xxxxxxxで同じまたはファイルの文字の終わりです
            }
            else if ((ch & 0xF0) == 0xF0)
            {
                // 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
                int c1 = ch & 0x07; ch = base.Read();
                int c2 = ch & 0x3F; ch = base.Read();
                int c3 = ch & 0x3F; ch = base.Read();
                int c4 = ch & 0x3F;
                ch = (((((c1 << 6) | c2) << 6) | c3) << 6) | c4;
            }
            else if ((ch & 0xE0) == 0xE0)
            {
                // 1110xxxx 10xxxxxx 10xxxxxx
                int c1 = ch & 0x0F; ch = base.Read();
                int c2 = ch & 0x3F; ch = base.Read();
                int c3 = ch & 0x3F;
                ch = (((c1 << 6) | c2) << 6) | c3;
            }
            else if ((ch & 0xC0) == 0xC0)
            {
                // 110xxxxx 10xxxxxx
                int c1 = ch & 0x1F; ch = base.Read();
                int c2 = ch & 0x3F;
                ch = (c1 << 6) | c2;
            }
            return ch;
        }
    }

    //-----------------------------------------------------------------------------------
    // Scanner
    //-----------------------------------------------------------------------------------
    public class Scanner
    {
        const char EOL = '\n';
        const int eofSym = 0; /* pdt */
        const Kind maxT = Kind.MaxT;
        const Kind noSym = Kind.MaxT;


        public Buffer buffer; // スキャナーバッファ

        Token t;          // 現在のトークン
        int ch;           // 現在の入力文字
        int pos;          // 現在の文字のバイト位置
        int col;          // 現在の文字の列番号
        int line;         // 現在の文字の行番号
        int oldEols;      // コメントに登場したEOL;
        Dictionary<int, int> start; // マップ·ファースト·トークン文字が開始状態。

        Token tokens;     // すでにピークトークンのリスト（最初のトークンはダミーです）
        Token pt;         // 現在のピークトークン

        char[] tval = new char[128]; // 現在のトークンのテキスト
        int tlen;         // 現在のトークンの長さ

        public Scanner(string fileName)
        {
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new Buffer(stream, false);
                Init();
            }
            catch (IOException)
            {
                throw new FatalError("Cannot open file " + fileName);
            }
        }

        public Scanner(Stream s)
        {
            buffer = new Buffer(s, true);
            Init();
        }

        void Init()
        {
            pos = -1; line = 1; col = 0;
            oldEols = 0;
            NextCh();
            if (ch == 0xEF)
            { // check optional byte order mark for UTF-8
                NextCh(); int ch1 = ch;
                NextCh(); int ch2 = ch;
                if (ch1 != 0xBB || ch2 != 0xBF)
                {
                    throw new FatalError(String.Format("illegal byte order mark: EF {0,2:X} {1,2:X}", ch1, ch2));
                }
                buffer = new UTF8Buffer(buffer); col = 0;
                NextCh();
            }
            start = new Dictionary<int, int>(128);
            for (int i = 65; i <= 90; ++i) start[i] = 1;
            for (int i = 95; i <= 95; ++i) start[i] = 1;
            for (int i = 97; i <= 122; ++i) start[i] = 1;
            for (int i = 170; i <= 170; ++i) start[i] = 1;
            for (int i = 181; i <= 181; ++i) start[i] = 1;
            for (int i = 186; i <= 186; ++i) start[i] = 1;
            for (int i = 192; i <= 214; ++i) start[i] = 1;
            for (int i = 216; i <= 246; ++i) start[i] = 1;
            for (int i = 248; i <= 255; ++i) start[i] = 1;
            for (int i = 49; i <= 57; ++i) start[i] = 159;
            start[92] = 15;
            start[64] = 160;
            start[48] = 161;
            start[46] = 162;
            start[39] = 44;
            start[34] = 61;
            start[38] = 196;
            start[61] = 163;
            start[58] = 164;
            start[44] = 79;
            start[45] = 197;
            start[47] = 198;
            start[62] = 165;
            start[43] = 166;
            start[123] = 86;
            start[91] = 87;
            start[40] = 88;
            start[60] = 199;
            start[37] = 200;
            start[33] = 167;
            start[124] = 201;
            start[63] = 202;
            start[125] = 95;
            start[93] = 96;
            start[41] = 97;
            start[59] = 98;
            start[126] = 99;
            start[42] = 168;
            start[94] = 203;
            start[35] = 169;
            start[Buffer.EOF] = -1;

            pt = tokens = new Token();  // first token is a dummy
        }

        void NextCh()
        {
            if (oldEols > 0) { ch = EOL; oldEols--; }
            else
            {
                pos = buffer.Pos;
                ch = buffer.Read(); col++;
                // replace isolated '\r' by '\n' in order to make
                // eol handling uniform across Windows, Unix and Mac
                if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
                if (ch == EOL) { line++; col = 0; }
            }

        }

        void AddCh()
        {
            if (tlen >= tval.Length)
            {
                char[] newBuf = new char[2 * tval.Length];
                Array.Copy(tval, 0, newBuf, 0, tval.Length);
                tval = newBuf;
            }
            tval[tlen++] = (char)ch;
            NextCh();
        }



        bool Comment0()
        {
            int level = 1, pos0 = pos, line0 = line, col0 = col;
            NextCh();
            if (ch == '/')
            {
                NextCh();
                for (; ; )
                {
                    if (ch == 10)
                    {
                        level--;
                        if (level == 0) { oldEols = line - line0; NextCh(); return true; }
                        NextCh();
                    }
                    else if (ch == Buffer.EOF) return false;
                    else NextCh();
                }
            }
            else
            {
                buffer.Pos = pos0; NextCh(); line = line0; col = col0;
            }
            return false;
        }

        bool Comment1()
        {
            int level = 1, pos0 = pos, line0 = line, col0 = col;
            NextCh();
            if (ch == '*')
            {
                NextCh();
                for (; ; )
                {
                    if (ch == '*')
                    {
                        NextCh();
                        if (ch == '/')
                        {
                            level--;
                            if (level == 0) { oldEols = line - line0; NextCh(); return true; }
                            NextCh();
                        }
                    }
                    else if (ch == Buffer.EOF) return false;
                    else NextCh();
                }
            }
            else
            {
                buffer.Pos = pos0; NextCh(); line = line0; col = col0;
            }
            return false;
        }


        void CheckLiteral()
        {
            switch (t.val)
            {
                case "abstract": t.kind = Kind.Abstract; break;
                case "as": t.kind = Kind.As; break;
                case "base": t.kind = Kind.Base; break;
                case "bool": t.kind = Kind.Bool; break;
                case "break": t.kind = Kind.Break; break;
                case "byte": t.kind = Kind.Byte; break;
                case "case": t.kind = Kind.Case; break;
                case "catch": t.kind = Kind.Catch; break;
                case "char": t.kind = Kind.Char; break;
                case "checked": t.kind = Kind.Checked; break;
                case "class": t.kind = Kind.Class; break;
                case "const": t.kind = Kind.Const; break;
                case "continue": t.kind = Kind.Continue; break;
                case "decimal": t.kind = Kind.Decimal; break;
                case "default": t.kind = Kind.Default; break;
                case "delegate": t.kind = Kind.Delegate; break;
                case "do": t.kind = Kind.Do; break;
                case "double": t.kind = Kind.Double; break;
                case "else": t.kind = Kind.Else; break;
                case "enum": t.kind = Kind.Enum; break;
                case "event": t.kind = Kind.Event; break;
                case "explicit": t.kind = Kind.Explicit; break;
                case "extern": t.kind = Kind.Extern; break;
                case "false": t.kind = Kind.False; break;
                case "finally": t.kind = Kind.Finally; break;
                case "fixed": t.kind = Kind.Fixed; break;
                case "float": t.kind = Kind.Float; break;
                case "for": t.kind = Kind.For; break;
                case "foreach": t.kind = Kind.Foreach; break;
                case "goto": t.kind = Kind.Goto; break;
                case "if": t.kind = Kind.If; break;
                case "implicit": t.kind = Kind.Implicit; break;
                case "in": t.kind = Kind.In; break;
                case "int": t.kind = Kind.Int; break;
                case "interface": t.kind = Kind.Interface; break;
                case "internal": t.kind = Kind.Internal; break;
                case "is": t.kind = Kind.Is; break;
                case "lock": t.kind = Kind.Lock; break;
                case "long": t.kind = Kind.Long; break;
                case "namespace": t.kind = Kind.Namespace; break;
                case "new": t.kind = Kind.New; break;
                case "null": t.kind = Kind.Null; break;
                case "object": t.kind = Kind.Object; break;
                case "operator": t.kind = Kind.Operator; break;
                case "out": t.kind = Kind.Out; break;
                case "override": t.kind = Kind.Override; break;
                case "params": t.kind = Kind.Params; break;
                case "private": t.kind = Kind.Private; break;
                case "protected": t.kind = Kind.Protected; break;
                case "public": t.kind = Kind.Public; break;
                case "readonly": t.kind = Kind.Readonly; break;
                case "ref": t.kind = Kind.Ref; break;
                case "return": t.kind = Kind.Return; break;
                case "sbyte": t.kind = Kind.Sbyte; break;
                case "sealed": t.kind = Kind.Sealed; break;
                case "short": t.kind = Kind.Short; break;
                case "sizeof": t.kind = Kind.Sizeof; break;
                case "stackalloc": t.kind = Kind.Stackalloc; break;
                case "static": t.kind = Kind.Static; break;
                case "string": t.kind = Kind.String; break;
                case "struct": t.kind = Kind.Struct; break;
                case "switch": t.kind = Kind.Switch; break;
                case "this": t.kind = Kind.This; break;
                case "throw": t.kind = Kind.Throw; break;
                case "true": t.kind = Kind.True; break;
                case "try": t.kind = Kind.Try; break;
                case "typeof": t.kind = Kind.Typeof; break;
                case "uint": t.kind = Kind.Uint; break;
                case "ulong": t.kind = Kind.Ulong; break;
                case "unchecked": t.kind = Kind.Unchecked; break;
                case "unsafe": t.kind = Kind.Unsafe; break;
                case "ushort": t.kind = Kind.Ushort; break;
                case "using": t.kind = Kind.UsingKW; break;
                case "virtual": t.kind = Kind.Virtual; break;
                case "void": t.kind = Kind.Void; break;
                case "volatile": t.kind = Kind.Volatile; break;
                case "while": t.kind = Kind.While; break;
                case "partial": t.kind = Kind.Partial; break;
                case "yield": t.kind = Kind.Yield; break;
                default: break;
            }
        }
        private bool IsNumCh() { return ch >= '0' && ch <= '9'; }
        private bool IsAZCh() { return ch >= 'A' && ch <= 'Z'; }
        private bool Is_azCh() { return ch >= 'a' && ch <= 'z'; }
        private bool IsAFCh() { return ch >= 'A' && ch <= 'F'; }
        private bool Is_afCh() { return ch >= 'a' && ch <= 'f'; }

        Token NextToken()
        {
            while (ch == ' ' ||
                ch >= 9 && ch <= 10 || ch == 13
            ) NextCh();
            if (ch == '/' && Comment0() || ch == '/' && Comment1()) return NextToken();
            int apx = 0;
            t = new Token();
            t.pos = pos; t.col = col; t.line = line;
            int state;
            try { state = start[ch]; }
            catch (KeyNotFoundException) { state = 0; }
            tlen = 0; AddCh();

            switch (state)
            {
                case -1: { t.kind = eofSym; break; } // NextCh already done
                case 0: { t.kind = noSym; break; }   // NextCh already done
                case 1:
                    if (IsNumCh() || IsAZCh() || ch == '_' || Is_azCh() || ch == 160 || ch == 170 || ch == 181 || ch == 186 || ch >= 192 && ch <= 214 || ch >= 216 && ch <= 246 || ch >= 248 && ch <= 255) { AddCh(); goto case 1; }
                    else if (ch == 92) { AddCh(); goto case 2; }
                    else { t.kind = Kind.Ident; t.val = new String(tval, 0, tlen); CheckLiteral(); return t; }
                case 2:
                    if (ch == 'u') { AddCh(); goto case 3; }
                    else if (ch == 'U') { AddCh(); goto case 7; }
                    else { t.kind = noSym; break; }
                case 3:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 4; }
                    else { t.kind = noSym; break; }
                case 4:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 5; }
                    else { t.kind = noSym; break; }
                case 5:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 6; }
                    else { t.kind = noSym; break; }
                case 6:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 1; }
                    else { t.kind = noSym; break; }
                case 7:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 8; }
                    else { t.kind = noSym; break; }
                case 8:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 9; }
                    else { t.kind = noSym; break; }
                case 9:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 10; }
                    else { t.kind = noSym; break; }
                case 10:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 11; }
                    else { t.kind = noSym; break; }
                case 11:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 12; }
                    else { t.kind = noSym; break; }
                case 12:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 13; }
                    else { t.kind = noSym; break; }
                case 13:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 14; }
                    else { t.kind = noSym; break; }
                case 14:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 1; }
                    else { t.kind = noSym; break; }
                case 15:
                    if (ch == 'u') { AddCh(); goto case 16; }
                    else if (ch == 'U') { AddCh(); goto case 20; }
                    else { t.kind = noSym; break; }
                case 16:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 17; }
                    else { t.kind = noSym; break; }
                case 17:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 18; }
                    else { t.kind = noSym; break; }
                case 18:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 19; }
                    else { t.kind = noSym; break; }
                case 19:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 1; }
                    else { t.kind = noSym; break; }
                case 20:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 21; }
                    else { t.kind = noSym; break; }
                case 21:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 22; }
                    else { t.kind = noSym; break; }
                case 22:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 23; }
                    else { t.kind = noSym; break; }
                case 23:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 24; }
                    else { t.kind = noSym; break; }
                case 24:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 25; }
                    else { t.kind = noSym; break; }
                case 25:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 26; }
                    else { t.kind = noSym; break; }
                case 26:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 27; }
                    else { t.kind = noSym; break; }
                case 27:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 1; }
                    else { t.kind = noSym; break; }
                case 28:
                    if (IsNumCh()) { apx = 0; AddCh(); goto case 28; }
                    else if (ch == 'U') { apx = 0; AddCh(); goto case 170; }
                    else if (ch == 'u') { apx = 0; AddCh(); goto case 171; }
                    else if (ch == 'L') { apx = 0; AddCh(); goto case 172; }
                    else if (ch == 'l') { apx = 0; AddCh(); goto case 173; }
                    else
                    {
                        tlen -= apx;
                        buffer.Pos = t.pos; NextCh(); line = t.line; col = t.col;
                        for (int i = 0; i < tlen; i++) NextCh();
                        t.kind = Kind.IntCon; break;
                    }
                case 29:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 30; }
                    else { t.kind = noSym; break; }
                case 30:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 30; }
                    else if (ch == 'U') { AddCh(); goto case 174; }
                    else if (ch == 'u') { AddCh(); goto case 175; }
                    else if (ch == 'L') { AddCh(); goto case 176; }
                    else if (ch == 'l') { AddCh(); goto case 177; }
                    else { t.kind = Kind.IntCon; break; }
                case 31:
                    { t.kind = Kind.IntCon; break; }
                case 32:
                    if (IsNumCh()) { AddCh(); goto case 32; }
                    else if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') { AddCh(); goto case 43; }
                    else if (ch == 'E' || ch == 'e') { AddCh(); goto case 33; }
                    else { t.kind = Kind.RealCon; break; }
                case 33:
                    if (IsNumCh()) { AddCh(); goto case 35; }
                    else if (ch == '+' || ch == '-') { AddCh(); goto case 34; }
                    else { t.kind = noSym; break; }
                case 34:
                    if (IsNumCh()) { AddCh(); goto case 35; }
                    else { t.kind = noSym; break; }
                case 35:
                    if (IsNumCh()) { AddCh(); goto case 35; }
                    else if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') { AddCh(); goto case 43; }
                    else { t.kind = Kind.RealCon; break; }
                case 36:
                    if (IsNumCh()) { AddCh(); goto case 36; }
                    else if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') { AddCh(); goto case 43; }
                    else if (ch == 'E' || ch == 'e') { AddCh(); goto case 37; }
                    else { t.kind = Kind.RealCon; break; }
                case 37:
                    if (IsNumCh()) { AddCh(); goto case 39; }
                    else if (ch == '+' || ch == '-') { AddCh(); goto case 38; }
                    else { t.kind = noSym; break; }
                case 38:
                    if (IsNumCh()) { AddCh(); goto case 39; }
                    else { t.kind = noSym; break; }
                case 39:
                    if (IsNumCh()) { AddCh(); goto case 39; }
                    else if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') { AddCh(); goto case 43; }
                    else { t.kind = Kind.RealCon; break; }
                case 40:
                    if (IsNumCh()) { AddCh(); goto case 42; }
                    else if (ch == '+' || ch == '-') { AddCh(); goto case 41; }
                    else { t.kind = noSym; break; }
                case 41:
                    if (IsNumCh()) { AddCh(); goto case 42; }
                    else { t.kind = noSym; break; }
                case 42:
                    if (IsNumCh()) { AddCh(); goto case 42; }
                    else if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') { AddCh(); goto case 43; }
                    else { t.kind = Kind.RealCon; break; }
                case 43:
                    { t.kind = Kind.RealCon; break; }
                case 44:
                    if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '[' || ch >= ']' && ch <= 65535) { AddCh(); goto case 45; }
                    else if (ch == 92) { AddCh(); goto case 178; }
                    else { t.kind = noSym; break; }
                case 45:
                    if (ch == 39) { AddCh(); goto case 60; }
                    else { t.kind = noSym; break; }
                case 46:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 47; }
                    else { t.kind = noSym; break; }
                case 47:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 179; }
                    else if (ch == 39) { AddCh(); goto case 60; }
                    else { t.kind = noSym; break; }
                case 48:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 49; }
                    else { t.kind = noSym; break; }
                case 49:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 50; }
                    else { t.kind = noSym; break; }
                case 50:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 51; }
                    else { t.kind = noSym; break; }
                case 51:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 45; }
                    else { t.kind = noSym; break; }
                case 52:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 53; }
                    else { t.kind = noSym; break; }
                case 53:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 54; }
                    else { t.kind = noSym; break; }
                case 54:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 55; }
                    else { t.kind = noSym; break; }
                case 55:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 56; }
                    else { t.kind = noSym; break; }
                case 56:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 57; }
                    else { t.kind = noSym; break; }
                case 57:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 58; }
                    else { t.kind = noSym; break; }
                case 58:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 59; }
                    else { t.kind = noSym; break; }
                case 59:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 45; }
                    else { t.kind = noSym; break; }
                case 60:
                    { t.kind = Kind.CharCon; break; }
                case 61:
                    if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) { AddCh(); goto case 61; }
                    else if (ch == '"') { AddCh(); goto case 77; }
                    else if (ch == 92) { AddCh(); goto case 181; }
                    else { t.kind = noSym; break; }
                case 62:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 63; }
                    else { t.kind = noSym; break; }
                case 63:
                    if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) { AddCh(); goto case 61; }
                    else if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 182; }
                    else if (ch == '"') { AddCh(); goto case 77; }
                    else if (ch == 92) { AddCh(); goto case 181; }
                    else { t.kind = noSym; break; }
                case 64:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 65; }
                    else { t.kind = noSym; break; }
                case 65:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 66; }
                    else { t.kind = noSym; break; }
                case 66:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 67; }
                    else { t.kind = noSym; break; }
                case 67:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 61; }
                    else { t.kind = noSym; break; }
                case 68:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 69; }
                    else { t.kind = noSym; break; }
                case 69:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 70; }
                    else { t.kind = noSym; break; }
                case 70:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 71; }
                    else { t.kind = noSym; break; }
                case 71:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 72; }
                    else { t.kind = noSym; break; }
                case 72:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 73; }
                    else { t.kind = noSym; break; }
                case 73:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 74; }
                    else { t.kind = noSym; break; }
                case 74:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 75; }
                    else { t.kind = noSym; break; }
                case 75:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 61; }
                    else { t.kind = noSym; break; }
                case 76:
                    if (ch <= '!' || ch >= '#' && ch <= 65535) { AddCh(); goto case 76; }
                    else if (ch == '"') { AddCh(); goto case 184; }
                    else { t.kind = noSym; break; }
                case 77:
                    { t.kind = Kind.StringCon; break; }
                case 78:
                    { t.kind = Kind.Andassgn; break; }
                case 79:
                    { t.kind = Kind.Comma; break; }
                case 80:
                    { t.kind = Kind.Dec; break; }
                case 81:
                    { t.kind = Kind.Divassgn; break; }
                case 82:
                    { t.kind = Kind.DblColon; break; }
                case 83:
                    { t.kind = Kind.Eq; break; }
                case 84:
                    { t.kind = Kind.Gteq; break; }
                case 85:
                    { t.kind = Kind.Inc; break; }
                case 86:
                    { t.kind = Kind.LBrace; break; }
                case 87:
                    { t.kind = Kind.LBrack; break; }
                case 88:
                    { t.kind = Kind.LPar; break; }
                case 89:
                    { t.kind = Kind.Lshassgn; break; }
                case 90:
                    { t.kind = Kind.Minusassgn; break; }
                case 91:
                    { t.kind = Kind.Modassgn; break; }
                case 92:
                    { t.kind = Kind.Neq; break; }
                case 93:
                    { t.kind = Kind.Orassgn; break; }
                case 94:
                    { t.kind = Kind.Plusassgn; break; }
                case 95:
                    { t.kind = Kind.RBrace; break; }
                case 96:
                    { t.kind = Kind.RBrack; break; }
                case 97:
                    { t.kind = Kind.RPar; break; }
                case 98:
                    { t.kind = Kind.SColon; break; }
                case 99:
                    { t.kind = Kind.Tilde; break; }
                case 100:
                    { t.kind = Kind.Timesassgn; break; }
                case 101:
                    { t.kind = Kind.Xorassgn; break; }
                case 102:
                    if (ch == 'e') { AddCh(); goto case 103; }
                    else { t.kind = noSym; break; }
                case 103:
                    if (ch == 'f') { AddCh(); goto case 104; }
                    else { t.kind = noSym; break; }
                case 104:
                    if (ch == 'i') { AddCh(); goto case 105; }
                    else { t.kind = noSym; break; }
                case 105:
                    if (ch == 'n') { AddCh(); goto case 106; }
                    else { t.kind = noSym; break; }
                case 106:
                    if (ch == 'e') { AddCh(); goto case 107; }
                    else { t.kind = noSym; break; }
                case 107:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 108; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 107; }
                    else { t.kind = noSym; break; }
                case 108:
                    { t.kind = Kind.PPDefine; break; }
                case 109:
                    if (ch == 'n') { AddCh(); goto case 110; }
                    else { t.kind = noSym; break; }
                case 110:
                    if (ch == 'd') { AddCh(); goto case 111; }
                    else { t.kind = noSym; break; }
                case 111:
                    if (ch == 'e') { AddCh(); goto case 112; }
                    else { t.kind = noSym; break; }
                case 112:
                    if (ch == 'f') { AddCh(); goto case 113; }
                    else { t.kind = noSym; break; }
                case 113:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 114; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 113; }
                    else { t.kind = noSym; break; }
                case 114:
                    { t.kind = Kind.PPUndef; break; }
                case 115:
                    if (ch == 'f') { AddCh(); goto case 116; }
                    else { t.kind = noSym; break; }
                case 116:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 117; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 116; }
                    else { t.kind = noSym; break; }
                case 117:
                    { t.kind = Kind.PPIf; break; }
                case 118:
                    if (ch == 'f') { AddCh(); goto case 119; }
                    else { t.kind = noSym; break; }
                case 119:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 120; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 119; }
                    else { t.kind = noSym; break; }
                case 120:
                    { t.kind = Kind.PPElif; break; }
                case 121:
                    if (ch == 'e') { AddCh(); goto case 122; }
                    else { t.kind = noSym; break; }
                case 122:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 123; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 122; }
                    else { t.kind = noSym; break; }
                case 123:
                    { t.kind = Kind.PPElse; break; }
                case 124:
                    if (ch == 'f') { AddCh(); goto case 125; }
                    else { t.kind = noSym; break; }
                case 125:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 126; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 125; }
                    else { t.kind = noSym; break; }
                case 126:
                    { t.kind = Kind.PPEndif; break; }
                case 127:
                    if (ch == 'i') { AddCh(); goto case 128; }
                    else { t.kind = noSym; break; }
                case 128:
                    if (ch == 'n') { AddCh(); goto case 129; }
                    else { t.kind = noSym; break; }
                case 129:
                    if (ch == 'e') { AddCh(); goto case 130; }
                    else { t.kind = noSym; break; }
                case 130:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 131; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 130; }
                    else { t.kind = noSym; break; }
                case 131:
                    { t.kind = Kind.PPLine; break; }
                case 132:
                    if (ch == 'r') { AddCh(); goto case 133; }
                    else { t.kind = noSym; break; }
                case 133:
                    if (ch == 'o') { AddCh(); goto case 134; }
                    else { t.kind = noSym; break; }
                case 134:
                    if (ch == 'r') { AddCh(); goto case 135; }
                    else { t.kind = noSym; break; }
                case 135:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 136; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 135; }
                    else { t.kind = noSym; break; }
                case 136:
                    { t.kind = Kind.PPError; break; }
                case 137:
                    if (ch == 'a') { AddCh(); goto case 138; }
                    else { t.kind = noSym; break; }
                case 138:
                    if (ch == 'r') { AddCh(); goto case 139; }
                    else { t.kind = noSym; break; }
                case 139:
                    if (ch == 'n') { AddCh(); goto case 140; }
                    else { t.kind = noSym; break; }
                case 140:
                    if (ch == 'i') { AddCh(); goto case 141; }
                    else { t.kind = noSym; break; }
                case 141:
                    if (ch == 'n') { AddCh(); goto case 142; }
                    else { t.kind = noSym; break; }
                case 142:
                    if (ch == 'g') { AddCh(); goto case 143; }
                    else { t.kind = noSym; break; }
                case 143:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 144; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 143; }
                    else { t.kind = noSym; break; }
                case 144:
                    { t.kind = Kind.PPWarning; break; }
                case 145:
                    if (ch == 'e') { AddCh(); goto case 146; }
                    else { t.kind = noSym; break; }
                case 146:
                    if (ch == 'g') { AddCh(); goto case 147; }
                    else { t.kind = noSym; break; }
                case 147:
                    if (ch == 'i') { AddCh(); goto case 148; }
                    else { t.kind = noSym; break; }
                case 148:
                    if (ch == 'o') { AddCh(); goto case 149; }
                    else { t.kind = noSym; break; }
                case 149:
                    if (ch == 'n') { AddCh(); goto case 150; }
                    else { t.kind = noSym; break; }
                case 150:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 151; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 150; }
                    else { t.kind = noSym; break; }
                case 151:
                    { t.kind = Kind.PPRegion; break; }
                case 152:
                    if (ch == 'e') { AddCh(); goto case 153; }
                    else { t.kind = noSym; break; }
                case 153:
                    if (ch == 'g') { AddCh(); goto case 154; }
                    else { t.kind = noSym; break; }
                case 154:
                    if (ch == 'i') { AddCh(); goto case 155; }
                    else { t.kind = noSym; break; }
                case 155:
                    if (ch == 'o') { AddCh(); goto case 156; }
                    else { t.kind = noSym; break; }
                case 156:
                    if (ch == 'n') { AddCh(); goto case 157; }
                    else { t.kind = noSym; break; }
                case 157:
                    if (ch == 10 || ch == 13) { AddCh(); goto case 158; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535) { AddCh(); goto case 157; }
                    else { t.kind = noSym; break; }
                case 158:
                    { t.kind = Kind.PPEndReg; break; }
                case 159:
                    if (IsNumCh()) { apx = 0; AddCh(); goto case 159; }
                    else if (ch == 'U') { apx = 0; AddCh(); goto case 170; }
                    else if (ch == 'u') { apx = 0; AddCh(); goto case 171; }
                    else if (ch == 'L') { apx = 0; AddCh(); goto case 172; }
                    else if (ch == 'l') { apx = 0; AddCh(); goto case 173; }
                    else if (ch == '.') { apx++; AddCh(); goto case 185; }
                    else if (ch == 'E' || ch == 'e') { apx = 0; AddCh(); goto case 40; }
                    else if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') { apx = 0; AddCh(); goto case 43; }
                    else { t.kind = Kind.IntCon; break; }
                case 160:
                    if (IsAZCh() || ch == '_' || Is_azCh() || ch == 170 || ch == 181 || ch == 186 || ch >= 192 && ch <= 214 || ch >= 216 && ch <= 246 || ch >= 248 && ch <= 255) { AddCh(); goto case 1; }
                    else if (ch == 92) { AddCh(); goto case 15; }
                    else if (ch == '"') { AddCh(); goto case 76; }
                    else { t.kind = noSym; break; }
                case 161:
                    if (IsNumCh()) { apx = 0; AddCh(); goto case 159; }
                    else if (ch == 'U') { apx = 0; AddCh(); goto case 170; }
                    else if (ch == 'u') { apx = 0; AddCh(); goto case 171; }
                    else if (ch == 'L') { apx = 0; AddCh(); goto case 172; }
                    else if (ch == 'l') { apx = 0; AddCh(); goto case 173; }
                    else if (ch == '.') { apx++; AddCh(); goto case 185; }
                    else if (ch == 'X' || ch == 'x') { apx = 0; AddCh(); goto case 29; }
                    else if (ch == 'E' || ch == 'e') { apx = 0; AddCh(); goto case 40; }
                    else if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') { apx = 0; AddCh(); goto case 43; }
                    else { t.kind = Kind.IntCon; break; }
                case 162:
                    if (IsNumCh()) { AddCh(); goto case 32; }
                    else { t.kind = Kind.Dot; break; }
                case 163:
                    if (ch == '=') { AddCh(); goto case 83; }
                    else { t.kind = Kind.Assgn; break; }
                case 164:
                    if (ch == ':') { AddCh(); goto case 82; }
                    else { t.kind = Kind.Colon; break; }
                case 165:
                    if (ch == '=') { AddCh(); goto case 84; }
                    else { t.kind = Kind.Gt; break; }
                case 166:
                    if (ch == '+') { AddCh(); goto case 85; }
                    else if (ch == '=') { AddCh(); goto case 94; }
                    else { t.kind = Kind.Plus; break; }
                case 167:
                    if (ch == '=') { AddCh(); goto case 92; }
                    else { t.kind = Kind.Not; break; }
                case 168:
                    if (ch == '=') { AddCh(); goto case 100; }
                    else { t.kind = Kind.Times; break; }
                case 169:
                    if (ch == 9 || ch >= 11 && ch <= 12 || ch == ' ') { AddCh(); goto case 169; }
                    else if (ch == 'd') { AddCh(); goto case 102; }
                    else if (ch == 'u') { AddCh(); goto case 109; }
                    else if (ch == 'i') { AddCh(); goto case 115; }
                    else if (ch == 'e') { AddCh(); goto case 187; }
                    else if (ch == 'l') { AddCh(); goto case 127; }
                    else if (ch == 'w') { AddCh(); goto case 137; }
                    else if (ch == 'r') { AddCh(); goto case 145; }
                    else { t.kind = noSym; break; }
                case 170:
                    if (ch == 'L' || ch == 'l') { AddCh(); goto case 31; }
                    else { t.kind = Kind.IntCon; break; }
                case 171:
                    if (ch == 'L' || ch == 'l') { AddCh(); goto case 31; }
                    else { t.kind = Kind.IntCon; break; }
                case 172:
                    if (ch == 'U' || ch == 'u') { AddCh(); goto case 31; }
                    else { t.kind = Kind.IntCon; break; }
                case 173:
                    if (ch == 'U' || ch == 'u') { AddCh(); goto case 31; }
                    else { t.kind = Kind.IntCon; break; }
                case 174:
                    if (ch == 'L' || ch == 'l') { AddCh(); goto case 31; }
                    else { t.kind = Kind.IntCon; break; }
                case 175:
                    if (ch == 'L' || ch == 'l') { AddCh(); goto case 31; }
                    else { t.kind = Kind.IntCon; break; }
                case 176:
                    if (ch == 'U' || ch == 'u') { AddCh(); goto case 31; }
                    else { t.kind = Kind.IntCon; break; }
                case 177:
                    if (ch == 'U' || ch == 'u') { AddCh(); goto case 31; }
                    else { t.kind = Kind.IntCon; break; }
                case 178:
                    if (ch == '"' || ch == 39 || ch == '0' || ch == 92 || ch >= 'a' && ch <= 'b' || ch == 'f' || ch == 'n' || ch == 'r' || ch == 't' || ch == 'v') { AddCh(); goto case 45; }
                    else if (ch == 'x') { AddCh(); goto case 46; }
                    else if (ch == 'u') { AddCh(); goto case 48; }
                    else if (ch == 'U') { AddCh(); goto case 52; }
                    else { t.kind = noSym; break; }
                case 179:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 180; }
                    else if (ch == 39) { AddCh(); goto case 60; }
                    else { t.kind = noSym; break; }
                case 180:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 45; }
                    else if (ch == 39) { AddCh(); goto case 60; }
                    else { t.kind = noSym; break; }
                case 181:
                    if (ch == '"' || ch == 39 || ch == '0' || ch == 92 || ch >= 'a' && ch <= 'b' || ch == 'f' || ch == 'n' || ch == 'r' || ch == 't' || ch == 'v') { AddCh(); goto case 61; }
                    else if (ch == 'x') { AddCh(); goto case 62; }
                    else if (ch == 'u') { AddCh(); goto case 64; }
                    else if (ch == 'U') { AddCh(); goto case 68; }
                    else { t.kind = noSym; break; }
                case 182:
                    if (IsNumCh() || IsAFCh() || Is_afCh()) { AddCh(); goto case 183; }
                    else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535) { AddCh(); goto case 61; }
                    else if (ch == '"') { AddCh(); goto case 77; }
                    else if (ch == 92) { AddCh(); goto case 181; }
                    else { t.kind = noSym; break; }
                case 183:
                    if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) { AddCh(); goto case 61; }
                    else if (ch == '"') { AddCh(); goto case 77; }
                    else if (ch == 92) { AddCh(); goto case 181; }
                    else { t.kind = noSym; break; }
                case 184:
                    if (ch == '"') { AddCh(); goto case 76; }
                    else { t.kind = Kind.StringCon; break; }
                case 185:
                    if (ch <= '/' || ch >= ':' && ch <= 65535) { apx++; AddCh(); goto case 28; }
                    else if (IsNumCh()) { apx = 0; AddCh(); goto case 36; }
                    else { t.kind = noSym; break; }
                case 186:
                    if (ch == '=') { AddCh(); goto case 89; }
                    else { t.kind = Kind.Ltlt; break; }
                case 187:
                    if (ch == 'l') { AddCh(); goto case 188; }
                    else if (ch == 'n') { AddCh(); goto case 189; }
                    else if (ch == 'r') { AddCh(); goto case 132; }
                    else { t.kind = noSym; break; }
                case 188:
                    if (ch == 'i') { AddCh(); goto case 118; }
                    else if (ch == 's') { AddCh(); goto case 121; }
                    else { t.kind = noSym; break; }
                case 189:
                    if (ch == 'd') { AddCh(); goto case 190; }
                    else { t.kind = noSym; break; }
                case 190:
                    if (ch == 'i') { AddCh(); goto case 124; }
                    else if (ch == 'r') { AddCh(); goto case 152; }
                    else { t.kind = noSym; break; }
                case 191:
                    { t.kind = Kind.NullCoalescing; break; }
                case 192:
                    { t.kind = Kind.ConditionalOR; break; }
                case 193:
                    { t.kind = Kind.ConditionalAND; break; }
                case 194:
                    { t.kind = Kind.LessEqual; break; }
                case 195:
                    { t.kind = Kind.DereferencePtr; break; }
                case 196:
                    if (ch == '=') { AddCh(); goto case 78; }
                    else if (ch == '&') { AddCh(); goto case 193; }
                    else { t.kind = Kind.And; break; }
                case 197:
                    if (ch == '-') { AddCh(); goto case 80; }
                    else if (ch == '=') { AddCh(); goto case 90; }
                    else if (ch == '>') { AddCh(); goto case 195; }
                    else { t.kind = Kind.Minus; break; }
                case 198:
                    if (ch == '=') { AddCh(); goto case 81; }
                    else { t.kind = Kind.Division; break; }
                case 199:
                    if (ch == '<') { AddCh(); goto case 186; }
                    else if (ch == '=') { AddCh(); goto case 194; }
                    else { t.kind = Kind.Lt; break; }
                case 200:
                    if (ch == '=') { AddCh(); goto case 91; }
                    else { t.kind = Kind.Modulus; break; }
                case 201:
                    if (ch == '=') { AddCh(); goto case 93; }
                    else if (ch == '|') { AddCh(); goto case 192; }
                    else { t.kind = Kind.LogicalOR; break; }
                case 202:
                    if (ch == '?') { AddCh(); goto case 191; }
                    else { t.kind = Kind.Question; break; }
                case 203:
                    if (ch == '=') { AddCh(); goto case 101; }
                    else { t.kind = Kind.LogicalXOR; break; }

            }
            t.val = new String(tval, 0, tlen);
            return t;
        }

        // 次のトークンを取得（おそらく既にピーク間に見られたトークン）
        public Token Scan()
        {
            if (tokens.next == null)
            {
                return NextToken();
            }
            else
            {
                pt = tokens = tokens.next;
                return tokens;
            }
        }

        // peek for the next token, ignore pragmas
        public Token Peek()
        {
            if (pt.next == null)
            {
                do
                {
                    pt = pt.next = NextToken();
                } while (pt.kind > maxT); // skip pragmas
            }
            else
            {
                do
                {
                    pt = pt.next;
                } while (pt.kind > maxT);
            }
            return pt;
        }

        // ピークは、現在のスキャン位置で始動することを確認
        public void ResetPeek() { pt = tokens; }

    } // end Scanner
}