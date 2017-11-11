using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorMC2D.Docking.Docment.Editors.Parsers.AngelScript
{
    public class Parser
    {


        const bool T = true;
        const bool x = false;
        const int minErrDist = 2;
        private string m_Current = "";

        public Scanner scanner;
        public Errors errors;

        public Token t;    // 最後に認識されたトークン
        public Token la;   // 先読みトークン
        int errDist = minErrDist;

        ArrayList ccs = new ArrayList();
        public ArrayList exts = new ArrayList();
        private AngelScriptCodeInfo m_CodeInfo = new AngelScriptCodeInfo();

        public AngelScriptCodeInfo CodeInfo
        {
            get { return this.m_CodeInfo; }
        }

        public void AddConditionalCompilationSymbols(String[] symbols)
        {
            if (symbols != null)
            {
                for (int i = 0; i < symbols.Length; ++i)
                {
                    symbols[i] = symbols[i].Trim();
                    if (symbols[i].Length > 0 && !ccs.Contains(symbols[i]))
                    {
                        ccs.Add(symbols[i]);
                    }
                }
            }
        }

        //与えられた、ホワイトスペースの端を返します
        //空白文字列であれば、そうでなければ真である
        //非空白の終了を返します。
        int EndOf(String symbol, int start, bool whitespaces)
        {
            while ((start < symbol.Length) && (Char.IsWhiteSpace(symbol[start]) ^ !whitespaces))
            {
                ++start;
            }

            return start;
        }

        // input:        "#" {ws} directive ws {ws} {not-newline} {newline}
        // valid input:  "#" {ws} directive ws {ws} {non-ws} {ws} {newline}
        // output:       {non-ws}
        String RemPPDirective(String symbol)
        {
            int start = 1;
            int end;

            // skip {ws}
            start = EndOf(symbol, start, true);
            // skip directive  
            start = EndOf(symbol, start, false);
            // skip ws {ws}
            start = EndOf(symbol, start, true);
            // search end of symbol
            end = EndOf(symbol, start, false);

            return symbol.Substring(start, end - start);
        }

        void AddCCS(String symbol)
        {
            symbol = RemPPDirective(symbol);
            if (!ccs.Contains(symbol))
            {
                ccs.Add(symbol);
            }
        }

        void RemCCS(String symbol)
        {
            ccs.Remove(RemPPDirective(symbol));
        }

        bool IsCCS(String symbol)
        {
            return ccs.Contains(RemPPDirective(symbol));
        }

        // search for the correct alternative and enter
        // drop everything before the correct alternative
        void IfPragma(String symbol)
        {
            if (!IsCCS(symbol))
            {
                int state = 0;
                Token cur = scanner.Scan();

                for (; ; )
                {
                    switch (cur.kind)
                    {
                        case Kind.PPIf: ++state; break;
                        case Kind.PPEndif:
                            if (state == 0) { return; }
                            --state;
                            break;
                        case Kind.PPElif:
                            if (state == 0 && IsCCS(cur.val)) { return; }
                            break;
                        case Kind.PPElse:
                            if (state == 0) { return; }
                            break;
                        case Kind.EOF: Error("Incomplete file."); return;
                        default: break;
                    }
                    cur = scanner.Scan();
                }
            }
        }

        // drop everything until the end of this if, elif, else directive
        void ElifOrElsePragma()
        {
            int state = 0;
            Token cur = scanner.Scan();

            for (; ; )
            {
                switch (cur.kind)
                {
                    case Kind.PPIf: ++state; break;
                    case Kind.PPEndif:
                        if (state == 0) { return; }
                        --state;
                        break;
                    default: break;
                }
                cur = scanner.Scan();
            }
        }

        /*----------------------------- token sets -------------------------------*/

        const int MaxTerminals = (int)Kind.InvAccessorDecl2;  // set size

        static BitArray NewSet(params Kind[] values)
        {
            BitArray a = new BitArray(MaxTerminals);
            foreach (int x in values) a[x] = true;
            return a;
        }

        static BitArray
          unaryOp = NewSet(Kind.Plus, Kind.Minus, Kind.Not, Kind.Tilde, Kind.Inc, Kind.Dec, Kind.True, Kind.False),
          typeKW = NewSet(Kind.Char, Kind.Bool, Kind.Object, Kind.String, Kind.Sbyte, Kind.Byte, Kind.Short,
                         Kind.Ushort, Kind.Int, Kind.Uint, Kind.Long, Kind.Ulong, Kind.Float, Kind.Double, Kind.Decimal),
          unaryHead = NewSet(Kind.Plus, Kind.Minus, Kind.Not, Kind.Tilde, Kind.Times, Kind.Inc, Kind.Dec, Kind.And),
          assnStartOp = NewSet(Kind.Plus, Kind.Minus, Kind.Not, Kind.Tilde, Kind.Times),
          castFollower = NewSet(Kind.Tilde, Kind.Not, Kind.LPar, Kind.Ident,
            /* literals */
                         Kind.IntCon, Kind.RealCon, Kind.CharCon, Kind.StringCon,
            /* any keyword expect as and is */
                         Kind.Abstract, Kind.Base, Kind.Bool, Kind.Break, Kind.Byte, Kind.Case, Kind.Catch,
                         Kind.Char, Kind.Checked, Kind.Class, Kind.Const, Kind.Continue, Kind.Decimal, Kind.Default,
                         Kind.Delegate, Kind.Do, Kind.Double, Kind.Else, Kind.Enum, Kind.Event, Kind.Explicit,
                         Kind.Extern, Kind.False, Kind.Finally, Kind.Fixed, Kind.Float, Kind.For, Kind.Foreach,
                         Kind.Goto, Kind.If, Kind.Implicit, Kind.In, Kind.Int, Kind.Interface, Kind.Internal,
                         Kind.Lock, Kind.Long, Kind.Namespace, Kind.New, Kind.Null, Kind.Object, Kind.Operator,
                         Kind.Out, Kind.Override, Kind.Params, Kind.Private, Kind.Protected, Kind.Public,
                         Kind.Readonly, Kind.Ref, Kind.Return, Kind.Sbyte, Kind.Sealed, Kind.Short, Kind.Sizeof,
                         Kind.Stackalloc, Kind.Static, Kind.String, Kind.Struct, Kind.Switch, Kind.This, Kind.Throw,
                         Kind.True, Kind.Try, Kind.Typeof, Kind.Uint, Kind.Ulong, Kind.Unchecked, Kind.Unsafe,
                         Kind.Ushort, Kind.UsingKW, Kind.Virtual, Kind.Void, Kind.Volatile, Kind.While
                         ),
          typArgLstFol = NewSet(Kind.LPar, Kind.RPar, Kind.RBrack, Kind.Colon, Kind.SColon, Kind.Comma, Kind.Dot,
                         Kind.Question, Kind.Eq, Kind.Neq),
          keyword = NewSet(Kind.Abstract, Kind.As, Kind.Base, Kind.Bool, Kind.Break, Kind.Byte, Kind.Case, Kind.Catch,
                         Kind.Char, Kind.Checked, Kind.Class, Kind.Const, Kind.Continue, Kind.Decimal, Kind.Default,
                         Kind.Delegate, Kind.Do, Kind.Double, Kind.Else, Kind.Enum, Kind.Event, Kind.Explicit,
                         Kind.Extern, Kind.False, Kind.Finally, Kind.Fixed, Kind.Float, Kind.For, Kind.Foreach,
                         Kind.Goto, Kind.If, Kind.Implicit, Kind.In, Kind.Int, Kind.Interface, Kind.Internal,
                         Kind.Is, Kind.Lock, Kind.Long, Kind.Namespace, Kind.New, Kind.Null, Kind.Object, Kind.Operator,
                         Kind.Out, Kind.Override, Kind.Params, Kind.Private, Kind.Protected, Kind.Public,
                         Kind.Readonly, Kind.Ref, Kind.Return, Kind.Sbyte, Kind.Sealed, Kind.Short, Kind.Sizeof,
                         Kind.Stackalloc, Kind.Static, Kind.String, Kind.Struct, Kind.Switch, Kind.This, Kind.Throw,
                         Kind.True, Kind.Try, Kind.Typeof, Kind.Uint, Kind.Ulong, Kind.Unchecked, Kind.Unsafe,
                         Kind.Ushort, Kind.UsingKW, Kind.Virtual, Kind.Void, Kind.Volatile, Kind.While),
          assgnOps = NewSet(Kind.Assgn, Kind.Plusassgn, Kind.Minusassgn, Kind.Timesassgn, Kind.Divassgn,
                         Kind.Modassgn, Kind.Andassgn, Kind.Orassgn, Kind.Xorassgn, Kind.Lshassgn) /* rshassgn: ">" ">="  no whitespace allowed*/
                         ;

        /*---------------------------- auxiliary methods ------------------------*/

        void Error(string s)
        {
            if (errDist >= minErrDist) errors.SemErr(la.line, la.col, s);
            errDist = 0;
        }

        // Return the n-th token after the current lookahead token
        Token Peek(int n)
        {
            scanner.ResetPeek();
            Token x = la;
            while (n > 0) { x = scanner.Peek(); n--; }
            return x;
        }

        // ident "="
        bool IsAssignment()
        {
            return la.kind == Kind.Ident && Peek(1).kind == Kind.Assgn;
        }

        /* True, if the comma is not a trailing one, *
         * like the last one in: a, b, c,            */
        bool NotFinalComma()
        {
            Kind peek = Peek(1).kind;
            return la.kind == Kind.Comma && peek != Kind.RBrace && peek != Kind.RBrack;
        }

        /* Checks whether the next sequence of tokens is a qualident *
         * and returns the qualident string                          *
         * !!! Proceeds from current peek position !!!               */
        bool IsQualident(ref Token pt, out string qualident)
        {
            qualident = "";
            if (pt.kind == Kind.Ident)
            {
                qualident = pt.val;
                pt = scanner.Peek();
                while (pt.kind == Kind.Dot)
                {
                    pt = scanner.Peek();
                    if (pt.kind != Kind.Ident) return false;
                    qualident += "." + pt.val;
                    pt = scanner.Peek();
                }
                return true;
            }
            else return false;
        }

        bool IsGeneric()
        {
            scanner.ResetPeek();
            Token pt = la;
            if (!IsTypeArgumentList(ref pt))
            {
                return false;
            }
            return typArgLstFol[(int)pt.kind];
        }

        bool IsTypeArgumentList(ref Token pt)
        {
            if (pt.kind == Kind.Lt)
            {
                pt = scanner.Peek();
                while (true)
                {
                    if (!IsType(ref pt))
                    {
                        return false;
                    }
                    if (pt.kind == Kind.Gt)
                    {
                        // list recognized
                        pt = scanner.Peek();
                        break;
                    }
                    else if (pt.kind == Kind.Comma)
                    {
                        // another argument
                        pt = scanner.Peek();
                    }
                    else
                    {
                        // error in type argument list
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        // Type
        bool IsType(ref Token pt)
        {
            String dummyId;

            if (typeKW[(int)pt.kind])
            {
                pt = scanner.Peek();
            }
            else if (pt.kind == Kind.Void)
            {
                pt = scanner.Peek();
                if (pt.kind != Kind.Times)
                {
                    return false;
                }
                pt = scanner.Peek();
            }
            else if (pt.kind == Kind.Ident)
            {
                pt = scanner.Peek();
                if (pt.kind == Kind.DblColon || pt.kind == Kind.Dot)
                {
                    // either namespace alias qualifier "::" or first
                    // part of the qualident
                    pt = scanner.Peek();
                    if (!IsQualident(ref pt, out dummyId))
                    {
                        return false;
                    }
                }
                if (pt.kind == Kind.Lt && !IsTypeArgumentList(ref pt))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            if (pt.kind == Kind.Question)
            {
                pt = scanner.Peek();
            }
            return SkipPointerOrDims(ref pt);
        }

        // Type ident
        // (Type can be void*)
        bool IsLocalVarDecl()
        {
            Token pt = la;
            scanner.ResetPeek();
            return IsType(ref pt) && pt.kind == Kind.Ident;
        }

        // "[" ("," | "]")
        bool IsDims()
        {
            Kind peek = Peek(1).kind;
            return la.kind == Kind.LBrack && (peek == Kind.Comma || peek == Kind.RBrack);
        }

        // "*" | "[" ("," | "]")
        bool IsPointerOrDims()
        {
            return la.kind == Kind.Times || IsDims();
        }

        /* skip: { "[" { "," } "]" | "*" }             */
        /* !!! Proceeds from current peek position !!! */
        bool SkipPointerOrDims(ref Token pt)
        {
            for (; ; )
            {
                if (pt.kind == Kind.LBrack)
                {
                    do pt = scanner.Peek();
                    while (pt.kind == Kind.Comma);
                    if (pt.kind != Kind.RBrack) return false;
                }
                else if (pt.kind != Kind.Times) break;
                pt = scanner.Peek();
            }
            return true;
        }

        // Is attribute target specifier
        // (ident | keyword) ":"
        bool IsAttrTargSpec()
        {
            return (la.kind == Kind.Ident || keyword[(int)la.kind]) && Peek(1).kind == Kind.Colon;
        }

        // ident ("," | "=" | ";")
        bool IsFieldDecl()
        {
            Kind peek = Peek(1).kind;
            return la.kind == Kind.Ident &&
                   (peek == Kind.Comma || peek == Kind.Assgn || peek == Kind.SColon);
        }

        bool IsTypeCast()
        {
            if (la.kind != Kind.LPar) { return false; }
            if (IsSimpleTypeCast()) { return true; }
            return GuessTypeCast();
        }

        // "(" typeKW ")"
        bool IsSimpleTypeCast()
        {
            // assert: la.kind == Kind.LPar
            scanner.ResetPeek();
            Token pt1 = scanner.Peek();
            Token pt2 = scanner.Peek();
            return typeKW[(int)pt1.kind] &&
                    (pt2.kind == Kind.RPar ||
                    (pt2.kind == Kind.Question && scanner.Peek().kind == Kind.RPar));
        }

        // "(" Type ")" castFollower
        bool GuessTypeCast()
        {
            // assert: la.kind == Kind.LPar
            scanner.ResetPeek();
            Token pt = scanner.Peek();
            if (!IsType(ref pt))
            {
                return false;
            }
            if (pt.kind != Kind.RPar)
            {
                return false;
            }
            pt = scanner.Peek();
            return castFollower[(int)pt.kind];
        }

        // "[" "assembly"
        bool IsGlobalAttrTarget()
        {
            Token pt = Peek(1);
            return la.kind == Kind.LBrack && pt.kind == Kind.Ident && ("assembly".Equals(pt.val) || "module".Equals(pt.val));
        }

        // "extern" "alias"
        // where alias is an identifier, no keyword
        bool IsExternAliasDirective()
        {
            return la.kind == Kind.Extern && "alias".Equals(Peek(1).val);
        }

        // true: anyToken"<"
        // no whitespace between the token and the "<" allowed
        // anything else will return false.
        bool IsLtNoWs()
        {
            return (la.kind == Kind.Lt) && ((t.pos + t.val.Length) == la.pos);
        }

        bool IsNoSwitchLabelOrRBrace()
        {
            return (la.kind != Kind.Case && la.kind != Kind.Default && la.kind != Kind.RBrace) ||
                   (la.kind == Kind.Default && Peek(1).kind != Kind.Colon);
        }

        bool IsShift()
        {
            Token pt = Peek(1);
            return (la.kind == Kind.Ltlt) ||
                   (la.kind == Kind.Gt &&
                     pt.kind == Kind.Gt &&
                     (la.pos + la.val.Length == pt.pos)
                   );
        }

        // true: TypeArgumentList followed by anything but "("
        bool IsPartOfMemberName()
        {
            scanner.ResetPeek();
            Token pt = la;
            if (!IsTypeArgumentList(ref pt))
            {
                return false;
            }
            return pt.kind != Kind.LPar;
        }


        enum TypeKind { simple, array, pointer, @void }

        [Flags]
        enum Operator
        {
            plus = 0x00000001, minus = 0x00000002, not = 0x00000004, tilde = 0x00000008,
            inc = 0x00000010, dec = 0x00000020, @true = 0x00000040, @false = 0x00000080,
            times = 0x00000100, div = 0x00000200, mod = 0x00000400, and = 0x00000800,
            or = 0x00001000, xor = 0x00002000, lshift = 0x00004000, rshift = 0x00008000,
            eq = 0x00010000, neq = 0x00020000, gt = 0x00040000, lt = 0x00080000,
            gte = 0x00100000, lte = 0x00200000,
            unary = plus | minus | not | tilde | inc | dec | @true | @false,
            binary = plus | minus | times | div | mod | and | or | xor | lshift | rshift | eq | neq | gt | lt | gte | lte
        }

        /*------------------------- modifier handling -----------------------------*/

        [Flags]
        enum Modifier
        {
            @new = 0x0001, @public = 0x0002, @protected = 0x0004, @internal = 0x0008,
            @private = 0x0010, @unsafe = 0x0020, @static = 0x0040, @readonly = 0x0080,
            @volatile = 0x0100, @virtual = 0x0200, @sealed = 0x0400, @override = 0x0800,
            @abstract = 0x1000, @extern = 0x2000,

            /* sets of modifiers that can be attached to certain program elements    *
             * e.g., "constants" marks all modifiers that may be used with constants */
            none = 0x0000,
            classes = @new | @public | @protected | @internal | @private | @unsafe | @abstract | @sealed | @static,
            constants = @new | @public | @protected | @internal | @private,
            fields = @new | @public | @protected | @internal | @private | @unsafe | @static | @readonly | @volatile,
            propEvntMeths = @new | @public | @protected | @internal | @private | @unsafe | @static | @virtual | @sealed | @override | @abstract | @extern,
            accessorsPossib1 = @private,
            accessorsPossib2 = @protected | @internal,
            indexers = @new | @public | @protected | @internal | @private | @unsafe | @virtual | @sealed | @override | @abstract | @extern,
            operators = @public | @unsafe | @static | @extern,
            operatorsMust = @public | @static,
            constructors = @public | @protected | @internal | @private | @unsafe | @extern,
            staticConstr = @extern | @static,
            staticConstrMust = @static,
            nonClassTypes = @new | @public | @protected | @internal | @private | @unsafe,
            destructors = @extern | @unsafe,
            all = 0x3fff
        }

        class Modifiers
        {
            private Modifier cur = Modifier.none;
            private Parser parser;

            public Modifiers(Parser parser)
            {
                this.parser = parser;
            }

            public void Add(Modifier m)
            {
                if ((cur & m) == 0) cur |= m;
                else parser.Error("modifier " + m + " already defined");
            }

            public void Add(Modifiers m) { Add(m.cur); }

            public bool IsNone { get { return cur == Modifier.none; } }

            public void Check(Modifier allowed)
            {
                Modifier wrong = cur & (allowed ^ Modifier.all);
                if (wrong != Modifier.none)
                    parser.Error("modifier(s) " + wrong + " not allowed here");
            }

            public void Check(Modifier allowEither, Modifier allowOr)
            {
                Modifier wrong = cur & ((allowEither | allowOr) ^ Modifier.all);
                if ((allowEither & allowOr) != Modifier.none)
                {
                    parser.Error("modifiers providerd must not overlap");
                }
                else if (wrong != Modifier.none)
                {
                    parser.Error("modifier(s) " + wrong + " not allowed here");
                }
                else if (((cur & allowEither) != Modifier.none) && ((cur & allowOr) != Modifier.none))
                {
                    parser.Error("modifier(s) may either be " + allowEither + " or " + allowOr);
                }
            }

            public void CheckMust(Modifier mustHave)
            {
                Modifier missing = (cur & mustHave) ^ mustHave;
                if (missing != Modifier.none)
                {
                    parser.Error("modifier(s) " + missing + " must be applied here");
                }
            }

            public bool Has(Modifier mod)
            {
                return (cur & mod) == mod;
            }
        }


        /*------------------------------------------------------------------------*
         *----- SCANNER DESCRIPTION ----------------------------------------------*
         *------------------------------------------------------------------------*/



        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            errors = new Errors();
        }

        void SynErr(Kind n)
        {
            if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
            errDist = 0;
        }

        public void SemErr(string msg)
        {
            if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
            errDist = 0;
        }

        void Get()
        {
            for (; ; )
            {
                t = la;
                la = scanner.Scan();
                if (la.kind <= Kind.MaxT) { ++errDist; break; }
                if (la.kind == Kind.PPDefine)
                {
                    AddCCS(la.val);
                }
                if (la.kind == Kind.PPUndef)
                {
                    RemCCS(la.val);
                }
                if (la.kind == Kind.PPIf)
                {
                    IfPragma(la.val);
                }
                if (la.kind == Kind.PPElif)
                {
                    ElifOrElsePragma();
                }
                if (la.kind == Kind.PPElse)
                {
                    ElifOrElsePragma();
                }
                if (la.kind == Kind.PPEndif)
                {
                }
                if (la.kind == Kind.PPLine)
                {
                }
                if (la.kind == Kind.PPError)
                {
                }
                if (la.kind == Kind.PPWarning)
                {
                }
                if (la.kind == Kind.PPRegion)
                {
                }
                if (la.kind == Kind.PPEndReg)
                {
                }

                la = t;
            }
        }

        void Expect(Kind n)
        {
            if (la.kind == n) Get(); else { SynErr(n); }
        }

        bool StartOf(int s)
        {
            return set[s, (int)la.kind];
        }

        void ExpectWeak(Kind n, int follow)
        {
            if (la.kind == n) Get();
            else
            {
                SynErr(n);
                while (!StartOf(follow)) Get();
            }
        }


        bool WeakSeparator(Kind n, int syFol, int repFol)
        {
            Kind kind = la.kind;
            if (kind == n) { Get(); return true; }
            else if (StartOf(repFol)) { return false; }
            else
            {
                SynErr(n);
                while (!(set[syFol, (int)kind] || set[repFol, (int)kind] || set[0, (int)kind]))
                {
                    Get();
                    kind = la.kind;
                }
                return StartOf(syFol);
            }
        }


        void CS2()
        {
            while (IsExternAliasDirective())
            {
                ExternAliasDirective();
            }
            while (la.kind == Kind.UsingKW)
            {
                UsingDirective();
            }
            while (IsGlobalAttrTarget())
            {
                GlobalAttributes();
            }
            while (StartOf(1))
            {
                NamespaceMemberDeclaration();
            }
        }

        void ExternAliasDirective()
        {
            Expect(Kind.Extern);
            Expect(Kind.Ident);
            if (t.val != "alias")
            {
                Error("alias expected");
            }
            else
            {
                exts.Add(t.val);
            }

            Expect(Kind.Ident);
            Expect(Kind.SColon);
        }

        void UsingDirective()
        {
            Expect(Kind.UsingKW);
            if (IsAssignment())
            {
                Expect(Kind.Ident);
                Expect(Kind.Assgn);
            }
            TokenMatch tm = new TokenMatch();
            tm.Position = la.pos;
            this.m_Current = la.val;
            TypeName();
            tm.Value = this.m_Current;
            this.m_CodeInfo.Usings.Add(tm);
            Expect(Kind.SColon);
        }

        void GlobalAttributes()
        {
            Expect(Kind.LBrack);
            Expect(Kind.Ident);
            if (!"assembly".Equals(t.val) && !"module".Equals(t.val)) Error("global attribute target specifier \"assembly\" or \"module\" expected");

            Expect(Kind.Colon);
            Attribute();
            while (NotFinalComma())
            {
                Expect(Kind.Comma);
                Attribute();
            }
            if (la.kind == Kind.Comma)
            {
                Get();
            }
            Expect(Kind.RBrack);
        }

        void NamespaceMemberDeclaration()
        {
            Modifiers m = new Modifiers(this);
            if (la.kind == Kind.Namespace)
            {
                Get();
                this.m_Current = la.val;
                Expect(Kind.Ident);
                while (la.kind == Kind.Dot)
                {
                    Get();
                    this.m_Current += "." + la.val;
                    Expect(Kind.Ident);
                }
                Expect(Kind.LBrace);
                while (IsExternAliasDirective())
                {
                    ExternAliasDirective();
                }
                while (la.kind == Kind.UsingKW)
                {
                    UsingDirective();
                }
                while (StartOf(1))
                {
                    NamespaceMemberDeclaration();
                }
                Expect(Kind.RBrace);
                if (la.kind == Kind.SColon)
                {
                    Get();
                }
                this.m_CodeInfo.NameSpaces.Add(this.m_Current);
            }
            else if (StartOf(2))
            {
                while (la.kind == Kind.LBrack)
                {
                    Attributes();
                }
                ModifierList(m);
                TypeDeclaration(m);
            }
            else SynErr(Kind.PPDefine);
        }


        void TypeName()
        {
            Expect(Kind.Ident);
            if (la.kind == Kind.DblColon)
            {
                Get();
                Expect(Kind.Ident);
            }
            if (la.kind == Kind.Lt)
            {
                TypeArgumentList();
            }
            while (la.kind == Kind.Dot)
            {
                Get();
                this.m_Current += "." + la.val;
                Expect(Kind.Ident);
                if (la.kind == Kind.Lt)
                {
                    TypeArgumentList();
                }
            }
        }

        void Attributes()
        {
            Expect(Kind.LBrack);
            if (IsAttrTargSpec())
            {
                if (la.kind == Kind.Ident)
                {
                    Get();
                }
                else if (StartOf(3))
                {
                    Keyword();
                }
                else SynErr(Kind.PPUndef);
                Expect(Kind.Colon);
            }
            Attribute();
            while (la.kind == Kind.Comma && Peek(1).kind != Kind.RBrack)
            {
                Expect(Kind.Comma);
                Attribute();
            }
            if (la.kind == Kind.Comma)
            {
                Get();
            }
            Expect(Kind.RBrack);
        }

        void ModifierList(Modifiers m)
        {
            while (StartOf(4))
            {
                switch (la.kind)
                {
                    case Kind.New:
                        {
                            Get();
                            m.Add(Modifier.@new);
                            break;
                        }
                    case Kind.Public:
                        {
                            Get();
                            m.Add(Modifier.@public);
                            break;
                        }
                    case Kind.Protected:
                        {
                            Get();
                            m.Add(Modifier.@protected);
                            break;
                        }
                    case Kind.Internal:
                        {
                            Get();
                            m.Add(Modifier.@internal);
                            break;
                        }
                    case Kind.Private:
                        {
                            Get();
                            m.Add(Modifier.@private);
                            break;
                        }
                    case Kind.Unsafe:
                        {
                            Get();
                            m.Add(Modifier.@unsafe);
                            break;
                        }
                    case Kind.Static:
                        {
                            Get();
                            m.Add(Modifier.@static);
                            break;
                        }
                    case Kind.Readonly:
                        {
                            Get();
                            m.Add(Modifier.@readonly);
                            break;
                        }
                    case Kind.Volatile:
                        {
                            Get();
                            m.Add(Modifier.@volatile);
                            break;
                        }
                    case Kind.Virtual:
                        {
                            Get();
                            m.Add(Modifier.@virtual);
                            break;
                        }
                    case Kind.Sealed:
                        {
                            Get();
                            m.Add(Modifier.@sealed);
                            break;
                        }
                    case Kind.Override:
                        {
                            Get();
                            m.Add(Modifier.@override);
                            break;
                        }
                    case Kind.Abstract:
                        {
                            Get();
                            m.Add(Modifier.@abstract);
                            break;
                        }
                    case Kind.Extern:
                        {
                            Get();
                            m.Add(Modifier.@extern);
                            break;
                        }
                }
            }
        }

        void TypeDeclaration(Modifiers m)
        {
            TypeKind dummy;
            if (StartOf(5))
            {
                if (la.kind == Kind.Partial)
                {
                    Get();
                }
                if (la.kind == Kind.Class)
                {
                    m.Check(Modifier.classes);
                    Get();
                    Expect(Kind.Ident);
                    if (la.kind == Kind.Lt)
                    {
                        TypeParameterList();
                    }
                    if (la.kind == Kind.Colon)
                    {
                        ClassBase();
                    }
                    while (la.kind == Kind.Ident)
                    {
                        TypeParameterConstraintsClause();
                    }
                    ClassBody();
                    if (la.kind == Kind.SColon)
                    {
                        Get();
                    }
                }
                else if (la.kind == Kind.Struct)
                {
                    m.Check(Modifier.nonClassTypes);
                    Get();
                    Expect(Kind.Ident);
                    if (la.kind == Kind.Lt)
                    {
                        TypeParameterList();
                    }
                    if (la.kind == Kind.Colon)
                    {
                        Get();
                        TypeName();
                        while (la.kind == Kind.Comma)
                        {
                            Get();
                            TypeName();
                        }
                    }
                    while (la.kind == Kind.Ident)
                    {
                        TypeParameterConstraintsClause();
                    }
                    StructBody();
                    if (la.kind == Kind.SColon)
                    {
                        Get();
                    }
                }
                else if (la.kind == Kind.Interface)
                {
                    m.Check(Modifier.nonClassTypes);
                    Get();
                    Expect(Kind.Ident);
                    if (la.kind == Kind.Lt)
                    {
                        TypeParameterList();
                    }
                    if (la.kind == Kind.Colon)
                    {
                        Get();
                        TypeName();
                        while (la.kind == Kind.Comma)
                        {
                            Get();
                            TypeName();
                        }
                    }
                    while (la.kind == Kind.Ident)
                    {
                        TypeParameterConstraintsClause();
                    }
                    Expect(Kind.LBrace);
                    while (StartOf(6))
                    {
                        InterfaceMemberDeclaration();
                    }
                    Expect(Kind.RBrace);
                    if (la.kind == Kind.SColon)
                    {
                        Get();
                    }
                }
                else SynErr(Kind.PPIf);
            }
            else if (la.kind == Kind.Enum)
            {
                m.Check(Modifier.nonClassTypes);
                Get();
                Expect(Kind.Ident);
                if (la.kind == Kind.Colon)
                {
                    Get();
                    IntegralType();
                }
                EnumBody();
                if (la.kind == Kind.SColon)
                {
                    Get();
                }
            }
            else if (la.kind == Kind.Delegate)
            {
                m.Check(Modifier.nonClassTypes);
                Get();
                Type(out dummy, true);
                Expect(Kind.Ident);
                if (la.kind == Kind.Lt)
                {
                    TypeParameterList();
                }
                Expect(Kind.LPar);
                if (StartOf(7))
                {
                    FormalParameterList();
                }
                Expect(Kind.RPar);
                while (la.kind == Kind.Ident)
                {
                    TypeParameterConstraintsClause();
                }
                Expect(Kind.SColon);
            }
            else SynErr(Kind.PPElif);
        }

        void TypeParameterList()
        {
            Expect(Kind.Lt);
            while (la.kind == Kind.LBrack)
            {
                Attributes();
            }
            Expect(Kind.Ident);
            while (la.kind == Kind.Comma)
            {
                Get();
                while (la.kind == Kind.LBrack)
                {
                    Attributes();
                }
                Expect(Kind.Ident);
            }
            Expect(Kind.Gt);
        }

        void ClassBase()
        {
            Expect(Kind.Colon);
            ClassType();
            while (la.kind == Kind.Comma)
            {
                Get();
                TypeName();
            }
        }

        void TypeParameterConstraintsClause()
        {
            Expect(Kind.Ident);
            if (t.val != "where")
            {
                Error("type parameter constraints clause must start with: where");
            }

            Expect(Kind.Ident);
            Expect(Kind.Colon);
            if (StartOf(8))
            {
                if (la.kind == Kind.Class)
                {
                    Get();
                }
                else if (la.kind == Kind.Struct)
                {
                    Get();
                }
                else if (la.kind == Kind.Object)
                {
                    Get();
                }
                else if (la.kind == Kind.String)
                {
                    Get();
                }
                else
                {
                    TypeName();
                }
                while (la.kind == Kind.Comma && Peek(1).kind != Kind.New)
                {
                    Expect(Kind.Comma);
                    TypeName();
                }
                if (la.kind == Kind.Comma)
                {
                    Get();
                    Expect(Kind.New);
                    Expect(Kind.LPar);
                    Expect(Kind.RPar);
                }
            }
            else if (la.kind == Kind.New)
            {
                Get();
                Expect(Kind.LPar);
                Expect(Kind.RPar);
            }
            else SynErr(Kind.PPElse);
        }

        void ClassBody()
        {
            Expect(Kind.LBrace);
            while (StartOf(9))
            {
                while (la.kind == Kind.LBrack)
                {
                    Attributes();
                }
                Modifiers m = new Modifiers(this);
                ModifierList(m);
                ClassMemberDeclaration(m);
            }
            Expect(Kind.RBrace);
        }

        void StructBody()
        {
            Expect(Kind.LBrace);
            while (StartOf(10))
            {
                while (la.kind == Kind.LBrack)
                {
                    Attributes();
                }
                Modifiers m = new Modifiers(this);
                ModifierList(m);
                StructMemberDeclaration(m);
            }
            Expect(Kind.RBrace);
        }

        void InterfaceMemberDeclaration()
        {
            Modifiers m = new Modifiers(this);
            TypeKind dummy;

            while (la.kind == Kind.LBrack)
            {
                Attributes();
            }
            if (la.kind == Kind.New)
            {
                Get();
            }
            if (StartOf(11))
            {
                Type(out dummy, true);
                if (la.kind == Kind.Ident)
                {
                    Get();
                    if (la.kind == Kind.LPar || la.kind == Kind.Lt)
                    {
                        if (la.kind == Kind.Lt)
                        {
                            TypeParameterList();
                        }
                        Expect(Kind.LPar);
                        if (StartOf(7))
                        {
                            FormalParameterList();
                        }
                        Expect(Kind.RPar);
                        while (la.kind == Kind.Ident)
                        {
                            TypeParameterConstraintsClause();
                        }
                        Expect(Kind.SColon);
                    }
                    else if (la.kind == Kind.LBrace)
                    {
                        Get();
                        InterfaceAccessors(m);
                        Expect(Kind.RBrace);
                    }
                    else SynErr(Kind.PPEndif);
                }
                else if (la.kind == Kind.This)
                {
                    Get();
                    Expect(Kind.LBrack);
                    FormalParameterList();
                    Expect(Kind.RBrack);
                    Expect(Kind.LBrace);
                    InterfaceAccessors(m);
                    Expect(Kind.RBrace);
                }
                else SynErr(Kind.PPLine);
            }
            else if (la.kind == Kind.Event)
            {
                Get();
                Type(out dummy, false);
                Expect(Kind.Ident);
                Expect(Kind.SColon);
            }
            else SynErr(Kind.PPError);
        }

        void IntegralType()
        {
            switch (la.kind)
            {
                case Kind.Sbyte:
                    {
                        Get();
                        break;
                    }
                case Kind.Byte:
                    {
                        Get();
                        break;
                    }
                case Kind.Short:
                    {
                        Get();
                        break;
                    }
                case Kind.Ushort:
                    {
                        Get();
                        break;
                    }
                case Kind.Int:
                    {
                        Get();
                        break;
                    }
                case Kind.Uint:
                    {
                        Get();
                        break;
                    }
                case Kind.Long:
                    {
                        Get();
                        break;
                    }
                case Kind.Ulong:
                    {
                        Get();
                        break;
                    }
                case Kind.Char:
                    {
                        Get();
                        break;
                    }
                default: SynErr(Kind.PPWarning); break;
            }
        }

        void EnumBody()
        {
            Expect(Kind.LBrace);
            if (la.kind == Kind.Ident || la.kind == Kind.LBrack)
            {
                EnumMemberDeclaration();
                while (NotFinalComma())
                {
                    Expect(Kind.Comma);
                    EnumMemberDeclaration();
                }
                if (la.kind == Kind.Comma)
                {
                    Get();
                }
            }
            Expect(Kind.RBrace);
        }

        void Type(out TypeKind type, bool voidAllowed)
        {
            type = TypeKind.simple;
            if (StartOf(12))
            {
                PrimitiveType();
            }
            else if (la.kind == Kind.Ident || la.kind == Kind.Object || la.kind == Kind.String)
            {
                ClassType();
            }
            else if (la.kind == Kind.Void)
            {
                Get();
                type = TypeKind.@void;
            }
            else SynErr(Kind.PPRegion);
            if (la.kind == Kind.Question)
            {
                Get();
                if (type == TypeKind.@void) { Error("Unexpected token ?, void must not be nullable."); }
            }
            PointerOrArray(ref type);
            if (type == TypeKind.@void && !voidAllowed) { Error("type expected, void found, maybe you mean void*"); }
        }

        void FormalParameterList()
        {
            TypeKind type;
            while (la.kind == Kind.LBrack)
            {
                Attributes();
            }
            if (StartOf(13))
            {
                if (la.kind == Kind.Out || la.kind == Kind.Ref)
                {
                    if (la.kind == Kind.Ref)
                    {
                        Get();
                    }
                    else
                    {
                        Get();
                    }
                }
                Type(out type, false);
                Expect(Kind.Ident);
                if (la.kind == Kind.Comma)
                {
                    Get();
                    this.m_Current += ", " + la.val;
                    FormalParameterList();
                }
            }
            else if (la.kind == Kind.Params)
            {
                Get();
                Type(out type, false);
                if (type != TypeKind.array) { Error("params argument must be an array"); }
                Expect(Kind.Ident);
            }
            else SynErr(Kind.PPEndReg);
        }

        void ClassType()
        {
            if (la.kind == Kind.Ident)
            {
                TypeName();
            }
            else if (la.kind == Kind.Object || la.kind == Kind.String)
            {
                InternalClassType();
            }
            else SynErr(Kind.InvClassType);
        }

        void ClassMemberDeclaration(Modifiers m)
        {
            if (StartOf(14))
            {
                StructMemberDeclaration(m);
            }
            else if (la.kind == Kind.Tilde)
            {
                Get();
                Expect(Kind.Ident);
                Expect(Kind.LPar);
                Expect(Kind.RPar);
                if (la.kind == Kind.LBrace)
                {
                    Block();
                }
                else if (la.kind == Kind.SColon)
                {
                    Get();
                }
                else SynErr(Kind.InvClassMemberDecl);
            }
            else SynErr(Kind.InvClassMemberDecl2);
        }

        void StructMemberDeclaration(Modifiers m)
        {
            TypeKind type; Operator op;
            string dataType = la.val;
            if (la.kind == Kind.Const)
            {
                TokenMatch tm = new TokenMatch();
                m.Check(Modifier.constants);
                Get();
                dataType = la.val;
                Type(out type, false);
                tm.Position = la.pos;
                tm.Value = la.val + ":" + dataType;
                Expect(Kind.Ident);
                tm.Value += " = ";
                Expect(Kind.Assgn);
                tm.Value += la.val;
                Expression();
                while (la.kind == Kind.Comma)
                {
                    Get();
                    Expect(Kind.Ident);
                    Expect(Kind.Assgn);
                    Expression();
                }
                Expect(Kind.SColon);
                this.m_CodeInfo.Fields.Add(tm);
            }
            else if (la.kind == Kind.Event)
            {
                m.Check(Modifier.propEvntMeths);
                Get();
                Type(out type, false);
                if (IsFieldDecl())
                {
                    VariableDeclarators();
                    Expect(Kind.SColon);
                }
                else if (la.kind == Kind.Ident)
                {
                    TypeName();
                    Expect(Kind.LBrace);
                    EventAccessorDeclarations();
                    Expect(Kind.RBrace);
                }
                else SynErr(Kind.InvStructMemberDecl);
            }
            else if (la.kind == Kind.Ident && Peek(1).kind == Kind.LPar)
            {
                TokenMatch tm = new TokenMatch(la.val, la.pos);
                this.m_Current = "(";
                m.Check(Modifier.constructors | Modifier.staticConstr);
                Expect(Kind.Ident);
                Expect(Kind.LPar);
                this.m_Current += la.val;
                if (StartOf(7))
                {
                    m.Check(Modifier.constructors);
                    FormalParameterList();
                }
                Expect(Kind.RPar);
                if (!this.m_Current.Equals("()"))
                {
                    this.m_Current += ")";
                }
                tm.Value += this.m_Current;
                this.m_CodeInfo.Constructors.Add(tm);

                if (la.kind == Kind.Colon)
                {
                    m.Check(Modifier.constructors);
                    Get();
                    if (la.kind == Kind.Base)
                    {
                        Get();
                    }
                    else if (la.kind == Kind.This)
                    {
                        Get();
                    }
                    else SynErr(Kind.InvStructMemberDecl2);
                    Expect(Kind.LPar);
                    if (StartOf(15))
                    {
                        Argument();
                        while (la.kind == Kind.Comma)
                        {
                            Get();
                            Argument();
                        }
                    }
                    Expect(Kind.RPar);
                }
                if (la.kind == Kind.LBrace)
                {
                    Block();
                }
                else if (la.kind == Kind.SColon)
                {
                    Get();
                }
                else SynErr(Kind.InvStructMemberDecl3);
            }
            else if (StartOf(11))
            {
                Type(out type, true);
                if (la.kind == Kind.Operator)
                {
                    m.Check(Modifier.operators);
                    m.CheckMust(Modifier.operatorsMust);
                    if (type == TypeKind.@void) { Error("operator not allowed on void"); }

                    Get();
                    OverloadableOp(out op);
                    Expect(Kind.LPar);
                    Type(out type, false);
                    Expect(Kind.Ident);
                    if (la.kind == Kind.Comma)
                    {
                        Get();
                        Type(out type, false);
                        Expect(Kind.Ident);
                        if ((op & Operator.binary) == 0) Error("too many operands for unary operator");
                    }
                    else if (la.kind == Kind.RPar)
                    {
                        if ((op & Operator.unary) == 0) Error("too few operands for binary operator");
                    }
                    else SynErr(Kind.InvStructMemberDecl4);
                    Expect(Kind.RPar);
                    if (la.kind == Kind.LBrace)
                    {
                        Block();
                    }
                    else if (la.kind == Kind.SColon)
                    {
                        Get();
                    }
                    else SynErr(Kind.InvStructMemberDecl5);
                }
                else if (IsFieldDecl())
                {
                    m.Check(Modifier.fields);
                    if (type == TypeKind.@void) { Error("field type must not be void"); }

                    this.m_CodeInfo.Fields.Add(new TokenMatch(la.val + ":" + dataType, la.pos));
                    VariableDeclarators();
                    Expect(Kind.SColon);
                }
                else if (la.kind == Kind.Ident)
                {
                    TokenMatch tm = new TokenMatch(la.val, la.pos);
                    //this.m_CodeInfo.Methods.Add(new TokenMatch(la.val + ":" + dataType, la.pos));
                    MemberName();
                    if (la.kind == Kind.LBrace)
                    {
                        m.Check(Modifier.propEvntMeths);
                        if (type == TypeKind.@void) { Error("property type must not be void"); }

                        this.m_Current = "(";
                        Get();
                        AccessorDeclarations(m);
                        Expect(Kind.RBrace);
                        this.m_Current += ")";

                        tm.Value += this.m_Current;
                        tm.Value += ":" + dataType;
                        this.m_CodeInfo.Properties.Add(tm);
                    }
                    else if (la.kind == Kind.Dot)
                    {
                        m.Check(Modifier.indexers);
                        if (type == TypeKind.@void) { Error("indexer type must not be void"); }

                        Get();
                        Expect(Kind.This);
                        Expect(Kind.LBrack);
                        FormalParameterList();
                        Expect(Kind.RBrack);
                        Expect(Kind.LBrace);
                        AccessorDeclarations(m);
                        Expect(Kind.RBrace);
                    }
                    else if (la.kind == Kind.LPar || la.kind == Kind.Lt)
                    {
                        m.Check(Modifier.propEvntMeths);
                        if (la.kind == Kind.Lt)
                        {
                            TypeParameterList();
                        }
                        Expect(Kind.LPar);
                        this.m_Current = "(" + la.val;
                        if (StartOf(7))
                        {
                            FormalParameterList();
                        }
                        if (!this.m_Current.Equals("()"))
                        {
                            this.m_Current += ")";
                        }
                        tm.Value += this.m_Current;
                        Expect(Kind.RPar);
                        while (la.kind == Kind.Ident)
                        {
                            TypeParameterConstraintsClause();
                        }
                        if (la.kind == Kind.LBrace)
                        {
                            Block();
                        }
                        else if (la.kind == Kind.SColon)
                        {
                            Get();
                        }
                        else SynErr(Kind.InvStructMemberDecl6);

                        tm.Value += ":" + dataType;
                        this.m_CodeInfo.Methods.Add(tm);
                    }
                    else SynErr(Kind.InvStructMemberDecl7);
                }
                else if (la.kind == Kind.This)
                {
                    m.Check(Modifier.indexers);
                    if (type == TypeKind.@void) { Error("indexer type must not be void"); }

                    Get();
                    Expect(Kind.LBrack);
                    FormalParameterList();
                    Expect(Kind.RBrack);
                    Expect(Kind.LBrace);
                    AccessorDeclarations(m);
                    Expect(Kind.RBrace);
                }
                else SynErr(Kind.InvStructMemberDecl8);
            }
            else if (la.kind == Kind.Explicit || la.kind == Kind.Implicit)
            {
                m.Check(Modifier.operators);
                m.CheckMust(Modifier.operatorsMust);

                if (la.kind == Kind.Implicit)
                {
                    Get();
                }
                else
                {
                    Get();
                }
                Expect(Kind.Operator);
                Type(out type, false);
                if (type == TypeKind.@void) { Error("cast type must not be void"); }
                Expect(Kind.LPar);
                Type(out type, false);
                Expect(Kind.Ident);
                Expect(Kind.RPar);
                if (la.kind == Kind.LBrace)
                {
                    Block();
                }
                else if (la.kind == Kind.SColon)
                {
                    Get();
                }
                else SynErr(Kind.InvStructMemberDecl9);
            }
            else if (StartOf(16))
            {
                TypeDeclaration(m);
            }
            else SynErr(Kind.InvStructMemberDecl10);
        }

        void EnumMemberDeclaration()
        {
            while (la.kind == Kind.LBrack)
            {
                Attributes();
            }
            Expect(Kind.Ident);
            if (la.kind == Kind.Assgn)
            {
                Get();
                Expression();
            }
        }

        void Block()
        {
            Expect(Kind.LBrace);
            while (StartOf(17))
            {
                Statement();
            }
            Expect(Kind.RBrace);
        }

        void Expression()
        {
            Unary();
            if (assgnOps[(int)la.kind] || (la.kind == Kind.Gt && Peek(1).kind == Kind.Gteq))
            {
                AssignmentOperator();
                Expression();
            }
            else if (StartOf(18))
            {
                NullCoalescingExpr();
                if (la.kind == Kind.Question)
                {
                    Get();
                    Expression();
                    Expect(Kind.Colon);
                    Expression();
                }
            }
            else SynErr(Kind.InvExpression);
        }

        void VariableDeclarators()
        {
            Expect(Kind.Ident);
            if (la.kind == Kind.Assgn)
            {
                Get();
                VariableInitializer();
            }
            while (la.kind == Kind.Comma)
            {
                Get();
                Expect(Kind.Ident);
                if (la.kind == Kind.Assgn)
                {
                    Get();
                    VariableInitializer();
                }
            }
        }

        void EventAccessorDeclarations()
        {
            bool addFound = false, remFound = false;
            while (la.kind == Kind.LBrack)
            {
                Attributes();
            }
            if ("add".Equals(la.val))
            {
                Expect(Kind.Ident);
                addFound = true;
            }
            else if ("remove".Equals(la.val))
            {
                Expect(Kind.Ident);
                remFound = true;
            }
            else if (la.kind == Kind.Ident)
            {
                Get();
                Error("add or remove expected");
            }
            else SynErr(Kind.InvEventAccessorDecl);
            Block();
            if (la.kind == Kind.Ident || la.kind == Kind.LBrack)
            {
                while (la.kind == Kind.LBrack)
                {
                    Attributes();
                }
                if ("add".Equals(la.val))
                {
                    Expect(Kind.Ident);
                    if (addFound) Error("add already declared");
                }
                else if ("remove".Equals(la.val))
                {
                    Expect(Kind.Ident);
                    if (remFound) Error("remove already declared");
                }
                else if (la.kind == Kind.Ident)
                {
                    Get();
                    Error("add or remove expected");
                }
                else SynErr(Kind.InvEventAccessorDecl2);
                Block();
            }
        }

        void Argument()
        {
            if (la.kind == Kind.Out || la.kind == Kind.Ref)
            {
                if (la.kind == Kind.Ref)
                {
                    Get();
                }
                else
                {
                    Get();
                }
            }
            Expression();
        }

        void OverloadableOp(out Operator op)
        {
            op = Operator.plus;
            switch (la.kind)
            {
                case Kind.Plus:
                    {
                        Get();
                        break;
                    }
                case Kind.Minus:
                    {
                        Get();
                        op = Operator.minus;
                        break;
                    }
                case Kind.Not:
                    {
                        Get();
                        op = Operator.not;
                        break;
                    }
                case Kind.Tilde:
                    {
                        Get();
                        op = Operator.tilde;
                        break;
                    }
                case Kind.Inc:
                    {
                        Get();
                        op = Operator.inc;
                        break;
                    }
                case Kind.Dec:
                    {
                        Get();
                        op = Operator.dec;
                        break;
                    }
                case Kind.True:
                    {
                        Get();
                        op = Operator.@true;
                        break;
                    }
                case Kind.False:
                    {
                        Get();
                        op = Operator.@false;
                        break;
                    }
                case Kind.Times:
                    {
                        Get();
                        op = Operator.times;
                        break;
                    }
                case Kind.Division:
                    {
                        Get();
                        op = Operator.div;
                        break;
                    }
                case Kind.Modulus:
                    {
                        Get();
                        op = Operator.mod;
                        break;
                    }
                case Kind.And:
                    {
                        Get();
                        op = Operator.and;
                        break;
                    }
                case Kind.LogicalOR:
                    {
                        Get();
                        op = Operator.or;
                        break;
                    }
                case Kind.LogicalXOR:
                    {
                        Get();
                        op = Operator.xor;
                        break;
                    }
                case Kind.Ltlt:
                    {
                        Get();
                        op = Operator.lshift;
                        break;
                    }
                case Kind.Eq:
                    {
                        Get();
                        op = Operator.eq;
                        break;
                    }
                case Kind.Neq:
                    {
                        Get();
                        op = Operator.neq;
                        break;
                    }
                case Kind.Gt:
                    {
                        Get();
                        op = Operator.gt;
                        if (la.kind == Kind.Gt)
                        {
                            if (la.pos > t.pos + 1) Error("no whitespace allowed in right shift operator");
                            Get();
                            op = Operator.rshift;
                        }
                        break;
                    }
                case Kind.Lt:
                    {
                        Get();
                        op = Operator.lt;
                        break;
                    }
                case Kind.Gteq:
                    {
                        Get();
                        op = Operator.gte;
                        break;
                    }
                case Kind.LessEqual:
                    {
                        Get();
                        op = Operator.lte;
                        break;
                    }
                default: SynErr(Kind.InvOverloadableOp); break;
            }
        }

        void MemberName()
        {
            Expect(Kind.Ident);
            if (la.kind == Kind.DblColon)
            {
                Get();
                Expect(Kind.Ident);
            }
            if (la.kind == Kind.Lt && IsPartOfMemberName())
            {
                TypeArgumentList();
            }
            while (la.kind == Kind.Dot && Peek(1).kind == Kind.Ident)
            {
                Expect(Kind.Dot);
                Expect(Kind.Ident);
                if (la.kind == Kind.Lt && IsPartOfMemberName())
                {
                    TypeArgumentList();
                }
            }
        }

        void AccessorDeclarations(Modifiers m)
        {
            Modifiers am = new Modifiers(this);
            bool getFound = false, setFound = false;

            while (la.kind == Kind.LBrack)
            {
                Attributes();
            }
            ModifierList(am);
            am.Check(Modifier.accessorsPossib1, Modifier.accessorsPossib2);
            if ("get".Equals(la.val))
            {
                if (!this.m_Current.Equals("("))
                {
                    this.m_Current += ", ";
                }
                this.m_Current += "get";
                Expect(Kind.Ident);
                getFound = true;
            }
            else if ("set".Equals(la.val))
            {
                if (!this.m_Current.Equals("("))
                {
                    this.m_Current += ", ";
                }
                this.m_Current += "set";
                Expect(Kind.Ident);
                setFound = true;
            }
            else if (la.kind == Kind.Ident)
            {
                Get();
                Error("set or get expected");
            }
            else SynErr(Kind.InvAccessorDecl);
            if (la.kind == Kind.LBrace)
            {
                Block();
            }
            else if (la.kind == Kind.SColon)
            {
                Get();
            }
            else SynErr(Kind.InvAccessorDecl2);
            if (StartOf(19))
            {
                am = new Modifiers(this);
                while (la.kind == Kind.LBrack)
                {
                    Attributes();
                }
                ModifierList(am);
                am.Check(Modifier.accessorsPossib1, Modifier.accessorsPossib2);
                if ("get".Equals(la.val))
                {
                    Expect(Kind.Ident);
                    if (getFound) Error("get already declared");
                }
                else if ("set".Equals(la.val))
                {
                    Expect(Kind.Ident);
                    if (setFound) Error("set already declared");
                }
                else if (la.kind == Kind.Ident)
                {
                    Get();
                    Error("set or get expected");
                }
                else SynErr(Kind.InvAccessorDecl3);
                if (la.kind == Kind.LBrace)
                {
                    Block();
                }
                else if (la.kind == Kind.SColon)
                {
                    Get();
                }
                else SynErr(Kind.InvAccessorDecl4);
            }
        }

        void InterfaceAccessors(Modifiers m)
        {
            bool getFound = false, setFound = false;
            while (la.kind == Kind.LBrack)
            {
                Attributes();
            }
            if ("get".Equals(la.val))
            {
                Expect(Kind.Ident);
                getFound = true;
            }
            else if ("set".Equals(la.val))
            {
                Expect(Kind.Ident);
                setFound = true;
            }
            else if (la.kind == Kind.Ident)
            {
                Get();
                Error("set or get expected");
            }
            else SynErr(Kind.InvInterfaceAccessors);
            Expect(Kind.SColon);
            if (la.kind == Kind.Ident || la.kind == Kind.LBrack)
            {
                while (la.kind == Kind.LBrack)
                {
                    Attributes();
                }
                if ("get".Equals(la.val))
                {
                    Expect(Kind.Ident);
                    if (getFound) Error("get already declared");
                }
                else if ("set".Equals(la.val))
                {
                    Expect(Kind.Ident);
                    if (setFound) Error("set already declared");
                }
                else if (la.kind == Kind.Ident)
                {
                    Get();
                    Error("set or get expected");
                }
                else SynErr(Kind.InvInterfaceAccessors2);
                Expect(Kind.SColon);
            }
        }

        void LocalVariableDeclaration()
        {
            TypeKind dummy;
            Type(out dummy, false);
            LocalVariableDeclarator();
            while (la.kind == Kind.Comma)
            {
                Get();
                LocalVariableDeclarator();
            }
        }

        void LocalVariableDeclarator()
        {
            TypeKind dummy;
            Expect(Kind.Ident);
            if (la.kind == Kind.Assgn)
            {
                Get();
                if (StartOf(20))
                {
                    VariableInitializer();
                }
                else if (la.kind == Kind.Stackalloc)
                {
                    Get();
                    Type(out dummy, false);
                    Expect(Kind.LBrack);
                    Expression();
                    Expect(Kind.RBrack);
                }
                else SynErr(Kind.InvLocalVariableDecl);
            }
        }

        void VariableInitializer()
        {
            if (StartOf(21))
            {
                Expression();
            }
            else if (la.kind == Kind.LBrace)
            {
                ArrayInitializer();
            }
            else SynErr(Kind.InvVariableInit);
        }

        void ArrayInitializer()
        {
            Expect(Kind.LBrace);
            if (StartOf(20))
            {
                VariableInitializer();
                while (NotFinalComma())
                {
                    Expect(Kind.Comma);
                    VariableInitializer();
                }
                if (la.kind == Kind.Comma)
                {
                    Get();
                }
            }
            Expect(Kind.RBrace);
        }

        void Attribute()
        {
            TypeName();
            if (la.kind == Kind.LPar)
            {
                AttributeArguments();
            }
        }

        void Keyword()
        {
            switch (la.kind)
            {
                case Kind.Abstract:
                    {
                        Get();
                        break;
                    }
                case Kind.As:
                    {
                        Get();
                        break;
                    }
                case Kind.Base:
                    {
                        Get();
                        break;
                    }
                case Kind.Bool:
                    {
                        Get();
                        break;
                    }
                case Kind.Break:
                    {
                        Get();
                        break;
                    }
                case Kind.Byte:
                    {
                        Get();
                        break;
                    }
                case Kind.Case:
                    {
                        Get();
                        break;
                    }
                case Kind.Catch:
                    {
                        Get();
                        break;
                    }
                case Kind.Char:
                    {
                        Get();
                        break;
                    }
                case Kind.Checked:
                    {
                        Get();
                        break;
                    }
                case Kind.Class:
                    {
                        Get();
                        break;
                    }
                case Kind.Const:
                    {
                        Get();
                        break;
                    }
                case Kind.Continue:
                    {
                        Get();
                        break;
                    }
                case Kind.Decimal:
                    {
                        Get();
                        break;
                    }
                case Kind.Default:
                    {
                        Get();
                        break;
                    }
                case Kind.Delegate:
                    {
                        Get();
                        break;
                    }
                case Kind.Do:
                    {
                        Get();
                        break;
                    }
                case Kind.Double:
                    {
                        Get();
                        break;
                    }
                case Kind.Else:
                    {
                        Get();
                        break;
                    }
                case Kind.Enum:
                    {
                        Get();
                        break;
                    }
                case Kind.Event:
                    {
                        Get();
                        break;
                    }
                case Kind.Explicit:
                    {
                        Get();
                        break;
                    }
                case Kind.Extern:
                    {
                        Get();
                        break;
                    }
                case Kind.False:
                    {
                        Get();
                        break;
                    }
                case Kind.Finally:
                    {
                        Get();
                        break;
                    }
                case Kind.Fixed:
                    {
                        Get();
                        break;
                    }
                case Kind.Float:
                    {
                        Get();
                        break;
                    }
                case Kind.For:
                    {
                        Get();
                        break;
                    }
                case Kind.Foreach:
                    {
                        Get();
                        break;
                    }
                case Kind.Goto:
                    {
                        Get();
                        break;
                    }
                case Kind.If:
                    {
                        Get();
                        break;
                    }
                case Kind.Implicit:
                    {
                        Get();
                        break;
                    }
                case Kind.In:
                    {
                        Get();
                        break;
                    }
                case Kind.Int:
                    {
                        Get();
                        break;
                    }
                case Kind.Interface:
                    {
                        Get();
                        break;
                    }
                case Kind.Internal:
                    {
                        Get();
                        break;
                    }
                case Kind.Is:
                    {
                        Get();
                        break;
                    }
                case Kind.Lock:
                    {
                        Get();
                        break;
                    }
                case Kind.Long:
                    {
                        Get();
                        break;
                    }
                case Kind.Namespace:
                    {
                        Get();
                        break;
                    }
                case Kind.New:
                    {
                        Get();
                        break;
                    }
                case Kind.Null:
                    {
                        Get();
                        break;
                    }
                case Kind.Object:
                    {
                        Get();
                        break;
                    }
                case Kind.Operator:
                    {
                        Get();
                        break;
                    }
                case Kind.Out:
                    {
                        Get();
                        break;
                    }
                case Kind.Override:
                    {
                        Get();
                        break;
                    }
                case Kind.Params:
                    {
                        Get();
                        break;
                    }
                case Kind.Private:
                    {
                        Get();
                        break;
                    }
                case Kind.Protected:
                    {
                        Get();
                        break;
                    }
                case Kind.Public:
                    {
                        Get();
                        break;
                    }
                case Kind.Readonly:
                    {
                        Get();
                        break;
                    }
                case Kind.Ref:
                    {
                        Get();
                        break;
                    }
                case Kind.Return:
                    {
                        Get();
                        break;
                    }
                case Kind.Sbyte:
                    {
                        Get();
                        break;
                    }
                case Kind.Sealed:
                    {
                        Get();
                        break;
                    }
                case Kind.Short:
                    {
                        Get();
                        break;
                    }
                case Kind.Sizeof:
                    {
                        Get();
                        break;
                    }
                case Kind.Stackalloc:
                    {
                        Get();
                        break;
                    }
                case Kind.Static:
                    {
                        Get();
                        break;
                    }
                case Kind.String:
                    {
                        Get();
                        break;
                    }
                case Kind.Struct:
                    {
                        Get();
                        break;
                    }
                case Kind.Switch:
                    {
                        Get();
                        break;
                    }
                case Kind.This:
                    {
                        Get();
                        break;
                    }
                case Kind.Throw:
                    {
                        Get();
                        break;
                    }
                case Kind.True:
                    {
                        Get();
                        break;
                    }
                case Kind.Try:
                    {
                        Get();
                        break;
                    }
                case Kind.Typeof:
                    {
                        Get();
                        break;
                    }
                case Kind.Uint:
                    {
                        Get();
                        break;
                    }
                case Kind.Ulong:
                    {
                        Get();
                        break;
                    }
                case Kind.Unchecked:
                    {
                        Get();
                        break;
                    }
                case Kind.Unsafe:
                    {
                        Get();
                        break;
                    }
                case Kind.Ushort:
                    {
                        Get();
                        break;
                    }
                case Kind.UsingKW:
                    {
                        Get();
                        break;
                    }
                case Kind.Virtual:
                    {
                        Get();
                        break;
                    }
                case Kind.Void:
                    {
                        Get();
                        break;
                    }
                case Kind.Volatile:
                    {
                        Get();
                        break;
                    }
                case Kind.While:
                    {
                        Get();
                        break;
                    }
                default: SynErr(Kind.InvKeyword); break;
            }
        }

        void AttributeArguments()
        {
            bool nameFound = false;
            Expect(Kind.LPar);
            if (StartOf(21))
            {
                if (IsAssignment())
                {
                    nameFound = true;
                    Expect(Kind.Ident);
                    Expect(Kind.Assgn);
                }
                Expression();
                while (la.kind == Kind.Comma)
                {
                    Get();
                    if (IsAssignment())
                    {
                        nameFound = true;
                        Expect(Kind.Ident);
                        Expect(Kind.Assgn);
                    }
                    else if (StartOf(21))
                    {
                        if (nameFound) Error("no positional argument after named arguments");
                    }
                    else SynErr(Kind.InvAttributeArguments);
                    Expression();
                }
            }
            Expect(Kind.RPar);
        }

        void PrimitiveType()
        {
            if (StartOf(22))
            {
                IntegralType();
            }
            else if (la.kind == Kind.Float)
            {
                Get();
            }
            else if (la.kind == Kind.Double)
            {
                Get();
            }
            else if (la.kind == Kind.Decimal)
            {
                Get();
            }
            else if (la.kind == Kind.Bool)
            {
                Get();
            }
            else SynErr(Kind.InvPrimitiveType);
        }

        void PointerOrArray(ref TypeKind type)
        {
            while (IsPointerOrDims())
            {
                if (la.kind == Kind.Times)
                {
                    Get();
                    type = TypeKind.pointer;
                }
                else if (la.kind == Kind.LBrack)
                {
                    Get();
                    while (la.kind == Kind.Comma)
                    {
                        Get();
                    }
                    this.m_Current += "[]";
                    Expect(Kind.RBrack);
                    type = TypeKind.array;
                }
                else SynErr(Kind.InvPointerOrArray);
            }
        }

        void ResolvedType()
        {
            TypeKind type = TypeKind.simple;
            if (StartOf(12))
            {
                PrimitiveType();
            }
            else if (la.kind == Kind.Object)
            {
                Get();
            }
            else if (la.kind == Kind.String)
            {
                Get();
            }
            else if (la.kind == Kind.Ident)
            {
                Get();
                if (la.kind == Kind.DblColon)
                {
                    Get();
                    Expect(Kind.Ident);
                }
                if (IsGeneric())
                {
                    TypeArgumentList();
                }
                while (la.kind == Kind.Dot)
                {
                    Get();
                    Expect(Kind.Ident);
                    if (IsGeneric())
                    {
                        TypeArgumentList();
                    }
                }
            }
            else if (la.kind == Kind.Void)
            {
                Get();
                type = TypeKind.@void;
            }
            else SynErr(Kind.InvResolvedType);
            PointerOrArray(ref type);
            if (type == TypeKind.@void) Error("type expected, void found, maybe you mean void*");
        }

        void TypeArgumentList()
        {
            TypeKind dummy;
            Expect(Kind.Lt);
            if (StartOf(11))
            {
                Type(out dummy, false);
            }
            while (la.kind == Kind.Comma)
            {
                Get();
                if (StartOf(11))
                {
                    Type(out dummy, false);
                }
            }
            Expect(Kind.Gt);
        }

        void InternalClassType()
        {
            if (la.kind == Kind.Object)
            {
                Get();
            }
            else if (la.kind == Kind.String)
            {
                Get();
            }
            else SynErr(Kind.InvInternalClassType);
        }

        void Statement()
        {
            TypeKind dummy;
            if (la.kind == Kind.Ident && Peek(1).kind == Kind.Colon)
            {
                Expect(Kind.Ident);
                Expect(Kind.Colon);
                Statement();
            }
            else if (la.kind == Kind.Const)
            {
                Get();
                Type(out dummy, false);
                Expect(Kind.Ident);
                Expect(Kind.Assgn);
                Expression();
                while (la.kind == Kind.Comma)
                {
                    Get();
                    Expect(Kind.Ident);
                    Expect(Kind.Assgn);
                    Expression();
                }
                Expect(Kind.SColon);
            }
            else if (IsLocalVarDecl())
            {
                LocalVariableDeclaration();
                Expect(Kind.SColon);
            }
            else if (StartOf(23))
            {
                EmbeddedStatement();
            }
            else SynErr(Kind.InvStatement);
        }

        void EmbeddedStatement()
        {
            TypeKind type;
            if (la.kind == Kind.LBrace)
            {
                Block();
            }
            else if (la.kind == Kind.SColon)
            {
                Get();
            }
            else if (la.kind == Kind.Checked && Peek(1).kind == Kind.LBrace)
            {
                Expect(Kind.Checked);
                Block();
            }
            else if (la.kind == Kind.Unchecked && Peek(1).kind == Kind.LBrace)
            {
                Expect(Kind.Unchecked);
                Block();
            }
            else if (StartOf(21))
            {
                StatementExpression();
                Expect(Kind.SColon);
            }
            else if (la.kind == Kind.If)
            {
                Get();
                Expect(Kind.LPar);
                Expression();
                Expect(Kind.RPar);
                EmbeddedStatement();
                if (la.kind == Kind.Else)
                {
                    Get();
                    EmbeddedStatement();
                }
            }
            else if (la.kind == Kind.Switch)
            {
                Get();
                Expect(Kind.LPar);
                Expression();
                Expect(Kind.RPar);
                Expect(Kind.LBrace);
                while (la.kind == Kind.Case || la.kind == Kind.Default)
                {
                    SwitchSection();
                }
                Expect(Kind.RBrace);
            }
            else if (la.kind == Kind.While)
            {
                Get();
                Expect(Kind.LPar);
                Expression();
                Expect(Kind.RPar);
                EmbeddedStatement();
            }
            else if (la.kind == Kind.Do)
            {
                Get();
                EmbeddedStatement();
                Expect(Kind.While);
                Expect(Kind.LPar);
                Expression();
                Expect(Kind.RPar);
                Expect(Kind.SColon);
            }
            else if (la.kind == Kind.For)
            {
                Get();
                Expect(Kind.LPar);
                if (StartOf(24))
                {
                    ForInitializer();
                }
                Expect(Kind.SColon);
                if (StartOf(21))
                {
                    Expression();
                }
                Expect(Kind.SColon);
                if (StartOf(21))
                {
                    ForIterator();
                }
                Expect(Kind.RPar);
                EmbeddedStatement();
            }
            else if (la.kind == Kind.Foreach)
            {
                Get();
                Expect(Kind.LPar);
                Type(out type, false);
                Expect(Kind.Ident);
                Expect(Kind.In);
                Expression();
                Expect(Kind.RPar);
                EmbeddedStatement();
            }
            else if (la.kind == Kind.Break)
            {
                Get();
                Expect(Kind.SColon);
            }
            else if (la.kind == Kind.Continue)
            {
                Get();
                Expect(Kind.SColon);
            }
            else if (la.kind == Kind.Goto)
            {
                Get();
                if (la.kind == Kind.Ident)
                {
                    Get();
                }
                else if (la.kind == Kind.Case)
                {
                    Get();
                    Expression();
                }
                else if (la.kind == Kind.Default)
                {
                    Get();
                }
                else SynErr(Kind.InvEmbeddedStatement);
                Expect(Kind.SColon);
            }
            else if (la.kind == Kind.Return)
            {
                Get();
                if (StartOf(21))
                {
                    Expression();
                }
                Expect(Kind.SColon);
            }
            else if (la.kind == Kind.Throw)
            {
                Get();
                if (StartOf(21))
                {
                    Expression();
                }
                Expect(Kind.SColon);
            }
            else if (la.kind == Kind.Try)
            {
                Get();
                Block();
                if (la.kind == Kind.Catch)
                {
                    CatchClauses();
                    if (la.kind == Kind.Finally)
                    {
                        Get();
                        Block();
                    }
                }
                else if (la.kind == Kind.Finally)
                {
                    Get();
                    Block();
                }
                else SynErr(Kind.InvEmbeddedStatement2);
            }
            else if (la.kind == Kind.Lock)
            {
                Get();
                Expect(Kind.LPar);
                Expression();
                Expect(Kind.RPar);
                EmbeddedStatement();
            }
            else if (la.kind == Kind.UsingKW)
            {
                Get();
                Expect(Kind.LPar);
                ResourceAcquisition();
                Expect(Kind.RPar);
                EmbeddedStatement();
            }
            else if (la.kind == Kind.Yield)
            {
                Get();
                if (la.kind == Kind.Return)
                {
                    Get();
                    Expression();
                }
                else if (la.kind == Kind.Break)
                {
                    Get();
                }
                else SynErr(Kind.InvEmbeddedStatement3);
                Expect(Kind.SColon);
            }
            else if (la.kind == Kind.Unsafe)
            {
                Get();
                Block();
            }
            else if (la.kind == Kind.Fixed)
            {
                Get();
                Expect(Kind.LPar);
                Type(out type, false);
                if (type != TypeKind.pointer) Error("can only fix pointer types");
                Expect(Kind.Ident);
                Expect(Kind.Assgn);
                Expression();
                while (la.kind == Kind.Comma)
                {
                    Get();
                    Expect(Kind.Ident);
                    Expect(Kind.Assgn);
                    Expression();
                }
                Expect(Kind.RPar);
                EmbeddedStatement();
            }
            else SynErr(Kind.InvEmbeddedStatement4);
        }

        void StatementExpression()
        {
            bool isAssignment = assnStartOp[(int)la.kind] || IsTypeCast();
            Unary();
            if (StartOf(25))
            {
                AssignmentOperator();
                Expression();
            }
            else if (la.kind == Kind.Comma || la.kind == Kind.RPar || la.kind == Kind.SColon)
            {
                if (isAssignment) Error("error in assignment.");
            }
            else SynErr(Kind.InvStatementExpression);
        }

        void SwitchSection()
        {
            SwitchLabel();
            while (la.kind == Kind.Case || (la.kind == Kind.Default && Peek(1).kind == Kind.Colon))
            {
                SwitchLabel();
            }
            Statement();
            while (IsNoSwitchLabelOrRBrace())
            {
                Statement();
            }
        }

        void ForInitializer()
        {
            if (IsLocalVarDecl())
            {
                LocalVariableDeclaration();
            }
            else if (StartOf(21))
            {
                StatementExpression();
                while (la.kind == Kind.Comma)
                {
                    Get();
                    StatementExpression();
                }
            }
            else SynErr(Kind.InvForInitializer);
        }

        void ForIterator()
        {
            StatementExpression();
            while (la.kind == Kind.Comma)
            {
                Get();
                StatementExpression();
            }
        }

        void CatchClauses()
        {
            Expect(Kind.Catch);
            if (la.kind == Kind.LBrace)
            {
                Block();
            }
            else if (la.kind == Kind.LPar)
            {
                Get();
                ClassType();
                if (la.kind == Kind.Ident)
                {
                    Get();
                }
                Expect(Kind.RPar);
                Block();
                if (la.kind == Kind.Catch)
                {
                    CatchClauses();
                }
            }
            else SynErr(Kind.InvCatchClauses);
        }

        void ResourceAcquisition()
        {
            if (IsLocalVarDecl())
            {
                LocalVariableDeclaration();
            }
            else if (StartOf(21))
            {
                Expression();
            }
            else SynErr(Kind.InvResourceAcquisition);
        }

        void Unary()
        {
            TypeKind dummy;
            while (unaryHead[(int)la.kind] || IsTypeCast())
            {
                switch (la.kind)
                {
                    case Kind.Plus:
                        {
                            Get();
                            break;
                        }
                    case Kind.Minus:
                        {
                            Get();
                            break;
                        }
                    case Kind.Not:
                        {
                            Get();
                            break;
                        }
                    case Kind.Tilde:
                        {
                            Get();
                            break;
                        }
                    case Kind.Inc:
                        {
                            Get();
                            break;
                        }
                    case Kind.Dec:
                        {
                            Get();
                            break;
                        }
                    case Kind.Times:
                        {
                            Get();
                            break;
                        }
                    case Kind.And:
                        {
                            Get();
                            break;
                        }
                    case Kind.LPar:
                        {
                            Get();
                            Type(out dummy, false);
                            Expect(Kind.RPar);
                            break;
                        }
                    default: SynErr(Kind.InvUnary); break;
                }
            }
            Primary();
        }

        void AssignmentOperator()
        {
            switch (la.kind)
            {
                case Kind.Assgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Plusassgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Minusassgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Timesassgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Divassgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Modassgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Andassgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Orassgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Xorassgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Lshassgn:
                    {
                        Get();
                        break;
                    }
                case Kind.Gt:
                    {
                        Get();
                        int pos = t.pos;
                        Expect(Kind.Gteq);
                        if (pos + 1 < t.pos) Error("no whitespace allowed in right shift assignment");
                        break;
                    }
                default: SynErr(Kind.InvAssignmentOperator); break;
            }
        }

        void SwitchLabel()
        {
            if (la.kind == Kind.Case)
            {
                Get();
                Expression();
                Expect(Kind.Colon);
            }
            else if (la.kind == Kind.Default)
            {
                Get();
                Expect(Kind.Colon);
            }
            else SynErr(Kind.InvSwitchLabel);
        }

        void NullCoalescingExpr()
        {
            OrExpr();
            while (la.kind == Kind.NullCoalescing)
            {
                Get();
                Unary();
                OrExpr();
            }
        }

        void OrExpr()
        {
            AndExpr();
            while (la.kind == Kind.ConditionalOR)
            {
                Get();
                Unary();
                AndExpr();
            }
        }

        void AndExpr()
        {
            BitOrExpr();
            while (la.kind == Kind.ConditionalAND)
            {
                Get();
                Unary();
                BitOrExpr();
            }
        }

        void BitOrExpr()
        {
            BitXorExpr();
            while (la.kind == Kind.LogicalOR)
            {
                Get();
                Unary();
                BitXorExpr();
            }
        }

        void BitXorExpr()
        {
            BitAndExpr();
            while (la.kind == Kind.LogicalXOR)
            {
                Get();
                Unary();
                BitAndExpr();
            }
        }

        void BitAndExpr()
        {
            EqlExpr();
            while (la.kind == Kind.While)
            {
                Get();
                Unary();
                EqlExpr();
            }
        }

        void EqlExpr()
        {
            RelExpr();
            while (la.kind == Kind.Eq || la.kind == Kind.Neq)
            {
                if (la.kind == Kind.Neq)
                {
                    Get();
                }
                else
                {
                    Get();
                }
                Unary();
                RelExpr();
            }
        }

        void RelExpr()
        {
            ShiftExpr();
            while (StartOf(26))
            {
                if (StartOf(27))
                {
                    if (la.kind == Kind.Lt)
                    {
                        Get();
                    }
                    else if (la.kind == Kind.Gt)
                    {
                        Get();
                    }
                    else if (la.kind == Kind.LessEqual)
                    {
                        Get();
                    }
                    else if (la.kind == Kind.Gteq)
                    {
                        Get();
                    }
                    else SynErr(Kind.InvRelExpr);
                    Unary();
                    ShiftExpr();
                }
                else
                {
                    if (la.kind == Kind.Is)
                    {
                        Get();
                    }
                    else if (la.kind == Kind.As)
                    {
                        Get();
                    }
                    else SynErr(Kind.InvRelExpr2);
                    ResolvedType();
                }
            }
        }

        void ShiftExpr()
        {
            AddExpr();
            while (IsShift())
            {
                if (la.kind == Kind.Ltlt)
                {
                    Get();
                }
                else if (la.kind == Kind.Gt)
                {
                    Get();
                    Expect(Kind.Gt);
                }
                else SynErr(Kind.InvShiftExpr);
                Unary();
                AddExpr();
            }
        }

        void AddExpr()
        {
            MulExpr();
            while (la.kind == Kind.Minus || la.kind == Kind.Plus)
            {
                if (la.kind == Kind.Plus)
                {
                    Get();
                }
                else
                {
                    Get();
                }
                Unary();
                MulExpr();
            }
        }

        void MulExpr()
        {
            while (la.kind == Kind.Times || la.kind == Kind.Division || la.kind == Kind.Modulus)
            {
                if (la.kind == Kind.Times)
                {
                    Get();
                }
                else if (la.kind == Kind.Division)
                {
                    Get();
                }
                else
                {
                    Get();
                }
                Unary();
            }
        }

        void Primary()
        {
            TypeKind type; bool isArrayCreation = false;
            switch (la.kind)
            {
                case Kind.IntCon:
                case Kind.RealCon:
                case Kind.CharCon:
                case Kind.StringCon:
                case Kind.False:
                case Kind.Null:
                case Kind.True:
                    {
                        Literal();
                        break;
                    }
                case Kind.LPar:
                    {
                        Get();
                        Expression();
                        Expect(Kind.RPar);
                        break;
                    }
                case Kind.Bool:
                case Kind.Byte:
                case Kind.Char:
                case Kind.Decimal:
                case Kind.Double:
                case Kind.Float:
                case Kind.Int:
                case Kind.Long:
                case Kind.Object:
                case Kind.Sbyte:
                case Kind.Short:
                case Kind.String:
                case Kind.Uint:
                case Kind.Ulong:
                case Kind.Ushort:
                    {
                        switch (la.kind)
                        {
                            case Kind.Bool:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Byte:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Char:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Decimal:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Double:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Float:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Int:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Long:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Object:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Sbyte:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Short:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.String:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Uint:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Ulong:
                                {
                                    Get();
                                    break;
                                }
                            case Kind.Ushort:
                                {
                                    Get();
                                    break;
                                }
                        }
                        Expect(Kind.Dot);
                        Expect(Kind.Ident);
                        if (IsGeneric())
                        {
                            TypeArgumentList();
                        }
                        break;
                    }
                case Kind.Ident:
                    {
                        Get();
                        if (la.kind == Kind.DblColon)
                        {
                            Get();
                            Expect(Kind.Ident);
                            if (la.kind == Kind.Lt)
                            {
                                TypeArgumentList();
                            }
                            Expect(Kind.Dot);
                            Expect(Kind.Ident);
                        }
                        if (IsGeneric())
                        {
                            TypeArgumentList();
                        }
                        break;
                    }
                case Kind.This:
                    {
                        Get();
                        break;
                    }
                case Kind.Base:
                    {
                        Get();
                        if (la.kind == Kind.Dot)
                        {
                            Get();
                            Expect(Kind.Ident);
                            if (IsGeneric())
                            {
                                TypeArgumentList();
                            }
                        }
                        else if (la.kind == Kind.LBrack)
                        {
                            Get();
                            Expression();
                            while (la.kind == Kind.Comma)
                            {
                                Get();
                                Expression();
                            }
                            Expect(Kind.RBrack);
                        }
                        else SynErr(Kind.InvPrimary);
                        break;
                    }
                case Kind.New:
                    {
                        Get();
                        Type(out type, false);
                        if (la.kind == Kind.LPar)
                        {
                            Get();
                            if (StartOf(15))
                            {
                                Argument();
                                while (la.kind == Kind.Comma)
                                {
                                    Get();
                                    Argument();
                                }
                            }
                            Expect(Kind.RPar);
                        }
                        else if (la.kind == Kind.LBrack)
                        {
                            Get();
                            Expression();
                            while (la.kind == Kind.Comma)
                            {
                                Get();
                                Expression();
                            }
                            Expect(Kind.RBrack);
                            while (IsDims())
                            {
                                Expect(Kind.LBrack);
                                while (la.kind == Kind.Comma)
                                {
                                    Get();
                                }
                                Expect(Kind.RBrack);
                            }
                            if (la.kind == Kind.LBrace)
                            {
                                ArrayInitializer();
                            }
                            isArrayCreation = true;
                        }
                        else if (la.kind == Kind.LBrace)
                        {
                            ArrayInitializer();
                            if (type != TypeKind.array) Error("array type expected");
                            isArrayCreation = true;

                        }
                        else SynErr(Kind.InvPrimary2);
                        break;
                    }
                case Kind.Typeof:
                    {
                        Get();
                        Expect(Kind.LPar);
                        Type(out type, true);
                        Expect(Kind.RPar);
                        break;
                    }
                case Kind.Checked:
                    {
                        Get();
                        Expect(Kind.LPar);
                        Expression();
                        Expect(Kind.RPar);
                        break;
                    }
                case Kind.Unchecked:
                    {
                        Get();
                        Expect(Kind.LPar);
                        Expression();
                        Expect(Kind.RPar);
                        break;
                    }
                case Kind.Default:
                    {
                        Get();
                        Expect(Kind.LPar);
                        Primary();
                        Expect(Kind.RPar);
                        break;
                    }
                case Kind.Delegate:
                    {
                        Get();
                        if (la.kind == Kind.LPar)
                        {
                            Get();
                            if (StartOf(13))
                            {
                                AnonymousMethodParameter();
                                while (la.kind == Kind.Comma)
                                {
                                    Get();
                                    AnonymousMethodParameter();
                                }
                            }
                            Expect(Kind.RPar);
                        }
                        Block();
                        break;
                    }
                case Kind.Sizeof:
                    {
                        Get();
                        Expect(Kind.LPar);
                        Type(out type, false);
                        Expect(Kind.RPar);
                        break;
                    }
                default: SynErr(Kind.InvPrimary3); break;
            }
            while (StartOf(28))
            {
                switch (la.kind)
                {
                    case Kind.Inc:
                        {
                            Get();
                            break;
                        }
                    case Kind.Dec:
                        {
                            Get();
                            break;
                        }
                    case Kind.DereferencePtr:
                        {
                            Get();
                            Expect(Kind.Ident);
                            if (IsGeneric())
                            {
                                TypeArgumentList();
                            }
                            break;
                        }
                    case Kind.Dot:
                        {
                            Get();
                            Expect(Kind.Ident);
                            if (IsGeneric())
                            {
                                TypeArgumentList();
                            }
                            break;
                        }
                    case Kind.LPar:
                        {
                            Get();
                            if (StartOf(15))
                            {
                                Argument();
                                while (la.kind == Kind.Comma)
                                {
                                    Get();
                                    Argument();
                                }
                            }
                            Expect(Kind.RPar);
                            break;
                        }
                    case Kind.LBrack:
                        {
                            if (isArrayCreation) Error("要素へのアクセスは、配列の作成時に許可されてない");
                            Get();
                            Expression();
                            while (la.kind == Kind.Comma)
                            {
                                Get();
                                Expression();
                            }
                            Expect(Kind.RBrack);
                            break;
                        }
                }
            }
        }

        void Literal()
        {
            switch (la.kind)
            {
                case Kind.IntCon:
                    {
                        Get();
                        break;
                    }
                case Kind.RealCon:
                    {
                        Get();
                        break;
                    }
                case Kind.CharCon:
                    {
                        Get();
                        break;
                    }
                case Kind.StringCon:
                    {
                        Get();
                        break;
                    }
                case Kind.True:
                    {
                        Get();
                        break;
                    }
                case Kind.False:
                    {
                        Get();
                        break;
                    }
                case Kind.Null:
                    {
                        Get();
                        break;
                    }
                default: SynErr(Kind.InvLiteral); break;
            }
        }

        void AnonymousMethodParameter()
        {
            TypeKind dummy;
            if (la.kind == Kind.Out || la.kind == Kind.Ref)
            {
                if (la.kind == Kind.Ref)
                {
                    Get();
                }
                else
                {
                    Get();
                }
            }
            Type(out dummy, false);
            Expect(Kind.Ident);
        }



        public void Parse()
        {
            la = new Token();
            la.val = "";
            Get();
            CS2();

            Expect(0);
        }

        bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,T,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,T,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, T,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, x,x,x,x, T,x,x,x, x,T,x,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,T,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,T,x, x,x,x,x, x,T,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,T,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,x, T,x,T,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,T,T, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,T, x,x,x,x, T,T,T,x, x,x,x,x, T,T,T,x, x,T,x,x, T,x,T,T, T,T,T,x, T,x,x,x, x,T,T,T, T,T,T,T, T,x,x,x},
		{x,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,x,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,T,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,T,T, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, T,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,T,x,x, x,T,x,x, x,x,x,T, x,x,x,T, T,x,x,T, x,T,x,x, x,x,x,x, x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,x,x,T, x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x}

	};
    } // end Parser


    public class Errors
    {
        public int count = 0;                                    // 検出されたエラーの数
        public System.IO.TextWriter errorStream = Console.Out;   // エラーメッセージは、このストリームに行く
        public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=行, 1=列, 2=テキスト

        public void SynErr(int line, int col, Kind n)
        {
            string s;
            switch (n)
            {
                case Kind.EOF: s = "EOF expected"; break;
                case Kind.Ident: s = "ident expected"; break;
                case Kind.IntCon: s = "intCon expected"; break;
                case Kind.RealCon: s = "realCon expected"; break;
                case Kind.CharCon: s = "charCon expected"; break;
                case Kind.StringCon: s = "stringCon expected"; break;
                case Kind.Abstract: s = "abstract expected"; break;
                case Kind.As: s = "as expected"; break;
                case Kind.Base: s = "base expected"; break;
                case Kind.Bool: s = "bool expected"; break;
                case Kind.Break: s = "break expected"; break;
                case Kind.Byte: s = "byte expected"; break;
                case Kind.Case: s = "case expected"; break;
                case Kind.Catch: s = "catch expected"; break;
                case Kind.Char: s = "char expected"; break;
                case Kind.Checked: s = "checked expected"; break;
                case Kind.Class: s = "class expected"; break;
                case Kind.Const: s = "const expected"; break;
                case Kind.Continue: s = "continue expected"; break;
                case Kind.Decimal: s = "decimal expected"; break;
                case Kind.Default: s = "default expected"; break;
                case Kind.Delegate: s = "delegate expected"; break;
                case Kind.Do: s = "do expected"; break;
                case Kind.Double: s = "double expected"; break;
                case Kind.Else: s = "else expected"; break;
                case Kind.Enum: s = "enum expected"; break;
                case Kind.Event: s = "event expected"; break;
                case Kind.Explicit: s = "explicit expected"; break;
                case Kind.Extern: s = "extern expected"; break;
                case Kind.False: s = "false expected"; break;
                case Kind.Finally: s = "finally expected"; break;
                case Kind.Fixed: s = "fixed expected"; break;
                case Kind.Float: s = "float expected"; break;
                case Kind.For: s = "for expected"; break;
                case Kind.Foreach: s = "foreach expected"; break;
                case Kind.Goto: s = "goto expected"; break;
                case Kind.If: s = "if expected"; break;
                case Kind.Implicit: s = "implicit expected"; break;
                case Kind.In: s = "in expected"; break;
                case Kind.Int: s = "int expected"; break;
                case Kind.Interface: s = "interface expected"; break;
                case Kind.Internal: s = "internal expected"; break;
                case Kind.Is: s = "is expected"; break;
                case Kind.Lock: s = "lock expected"; break;
                case Kind.Long: s = "long expected"; break;
                case Kind.Namespace: s = "namespace expected"; break;
                case Kind.New: s = "new expected"; break;
                case Kind.Null: s = "null expected"; break;
                case Kind.Object: s = "object expected"; break;
                case Kind.Operator: s = "operator expected"; break;
                case Kind.Out: s = "out expected"; break;
                case Kind.Override: s = "override expected"; break;
                case Kind.Params: s = "params expected"; break;
                case Kind.Private: s = "private expected"; break;
                case Kind.Protected: s = "protected expected"; break;
                case Kind.Public: s = "public expected"; break;
                case Kind.Readonly: s = "readonly expected"; break;
                case Kind.Ref: s = "ref expected"; break;
                case Kind.Return: s = "return expected"; break;
                case Kind.Sbyte: s = "sbyte expected"; break;
                case Kind.Sealed: s = "sealed expected"; break;
                case Kind.Short: s = "short expected"; break;
                case Kind.Sizeof: s = "sizeof expected"; break;
                case Kind.Stackalloc: s = "stackalloc expected"; break;
                case Kind.Static: s = "static expected"; break;
                case Kind.String: s = "string expected"; break;
                case Kind.Struct: s = "struct expected"; break;
                case Kind.Switch: s = "switch expected"; break;
                case Kind.This: s = "this expected"; break;
                case Kind.Throw: s = "throw expected"; break;
                case Kind.True: s = "true expected"; break;
                case Kind.Try: s = "try expected"; break;
                case Kind.Typeof: s = "typeof expected"; break;
                case Kind.Uint: s = "uint expected"; break;
                case Kind.Ulong: s = "ulong expected"; break;
                case Kind.Unchecked: s = "unchecked expected"; break;
                case Kind.Unsafe: s = "unsafe expected"; break;
                case Kind.Ushort: s = "ushort expected"; break;
                case Kind.UsingKW: s = "usingKW expected"; break;
                case Kind.Virtual: s = "virtual expected"; break;
                case Kind.Void: s = "void expected"; break;
                case Kind.Volatile: s = "volatile expected"; break;
                case Kind.While: s = "while expected"; break;
                case Kind.And: s = "and expected"; break;
                case Kind.Andassgn: s = "andassgn expected"; break;
                case Kind.Assgn: s = "assgn expected"; break;
                case Kind.Colon: s = "colon expected"; break;
                case Kind.Comma: s = "comma expected"; break;
                case Kind.Dec: s = "dec expected"; break;
                case Kind.Divassgn: s = "divassgn expected"; break;
                case Kind.Dot: s = "dot expected"; break;
                case Kind.DblColon: s = "dblcolon expected"; break;
                case Kind.Eq: s = "eq expected"; break;
                case Kind.Gt: s = "gt expected"; break;
                case Kind.Gteq: s = "gteq expected"; break;
                case Kind.Inc: s = "inc expected"; break;
                case Kind.LBrace: s = "lbrace expected"; break;
                case Kind.LBrack: s = "lbrack expected"; break;
                case Kind.LPar: s = "lpar expected"; break;
                case Kind.Lshassgn: s = "lshassgn expected"; break;
                case Kind.Lt: s = "lt expected"; break;
                case Kind.Ltlt: s = "ltlt expected"; break;
                case Kind.Minus: s = "minus expected"; break;
                case Kind.Minusassgn: s = "minusassgn expected"; break;
                case Kind.Modassgn: s = "modassgn expected"; break;
                case Kind.Neq: s = "neq expected"; break;
                case Kind.Not: s = "not expected"; break;
                case Kind.Orassgn: s = "orassgn expected"; break;
                case Kind.Plus: s = "plus expected"; break;
                case Kind.Plusassgn: s = "plusassgn expected"; break;
                case Kind.Question: s = "question expected"; break;
                case Kind.RBrace: s = "rbrace expected"; break;
                case Kind.RBrack: s = "rbrack expected"; break;
                case Kind.RPar: s = "rpar expected"; break;
                case Kind.SColon: s = "scolon expected"; break;
                case Kind.Tilde: s = "tilde expected"; break;
                case Kind.Times: s = "times expected"; break;
                case Kind.Timesassgn: s = "timesassgn expected"; break;
                case Kind.Xorassgn: s = "xorassgn expected"; break;
                case Kind.Partial: s = "\"partial\" expected"; break;
                case Kind.Yield: s = "\"yield\" expected"; break;
                case Kind.NullCoalescing: s = "\"??\" expected"; break;
                case Kind.ConditionalOR: s = "\"||\" expected"; break;
                case Kind.ConditionalAND: s = "\"&&\" expected"; break;
                case Kind.LogicalOR: s = "\"|\" expected"; break;
                case Kind.LogicalXOR: s = "\"^\" expected"; break;
                case Kind.LessEqual: s = "\"<=\" expected"; break;
                case Kind.Division: s = "\"/\" expected"; break;
                case Kind.Modulus: s = "\"%\" expected"; break;
                case Kind.DereferencePtr: s = "\"->\" expected"; break;
                case Kind.MaxT: s = "??? expected"; break;
                case Kind.PPDefine: s = "invalid NamespaceMemberDeclaration"; break;
                case Kind.PPUndef: s = "invalid Attributes"; break;
                case Kind.PPIf: s = "invalid TypeDeclaration"; break;
                case Kind.PPElif: s = "invalid TypeDeclaration"; break;
                case Kind.PPElse: s = "invalid TypeParameterConstraintsClause"; break;
                case Kind.PPEndif: s = "invalid InterfaceMemberDeclaration"; break;
                case Kind.PPLine: s = "invalid InterfaceMemberDeclaration"; break;
                case Kind.PPError: s = "invalid InterfaceMemberDeclaration"; break;
                case Kind.PPWarning: s = "invalid IntegralType"; break;
                case Kind.PPRegion: s = "invalid Type"; break;
                case Kind.PPEndReg: s = "invalid FormalParameterList"; break;
                case Kind.InvClassType: s = "invalid ClassType"; break;
                case Kind.InvClassMemberDecl: s = "invalid ClassMemberDeclaration"; break;
                case Kind.InvClassMemberDecl2: s = "invalid ClassMemberDeclaration"; break;
                case Kind.InvStructMemberDecl: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvStructMemberDecl2: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvStructMemberDecl3: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvStructMemberDecl4: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvStructMemberDecl5: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvStructMemberDecl6: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvStructMemberDecl7: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvStructMemberDecl8: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvStructMemberDecl9: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvStructMemberDecl10: s = "invalid StructMemberDeclaration"; break;
                case Kind.InvExpression: s = "invalid Expression"; break;
                case Kind.InvEventAccessorDecl: s = "invalid EventAccessorDeclarations"; break;
                case Kind.InvEventAccessorDecl2: s = "invalid EventAccessorDeclarations"; break;
                case Kind.InvOverloadableOp: s = "invalid OverloadableOp"; break;
                case Kind.InvAccessorDecl: s = "invalid AccessorDeclarations"; break;
                case Kind.InvAccessorDecl2: s = "invalid AccessorDeclarations"; break;
                case Kind.InvAccessorDecl3: s = "invalid AccessorDeclarations"; break;
                case Kind.InvAccessorDecl4: s = "invalid AccessorDeclarations"; break;
                case Kind.InvInterfaceAccessors: s = "invalid InterfaceAccessors"; break;
                case Kind.InvInterfaceAccessors2: s = "invalid InterfaceAccessors"; break;
                case Kind.InvLocalVariableDecl: s = "invalid LocalVariableDeclarator"; break;
                case Kind.InvVariableInit: s = "invalid VariableInitializer"; break;
                case Kind.InvKeyword: s = "invalid Keyword"; break;
                case Kind.InvAttributeArguments: s = "invalid AttributeArguments"; break;
                case Kind.InvPrimitiveType: s = "invalid PrimitiveType"; break;
                case Kind.InvPointerOrArray: s = "invalid PointerOrArray"; break;
                case Kind.InvResolvedType: s = "invalid ResolvedType"; break;
                case Kind.InvInternalClassType: s = "invalid InternalClassType"; break;
                case Kind.InvStatement: s = "invalid Statement"; break;
                case Kind.InvEmbeddedStatement: s = "invalid EmbeddedStatement"; break;
                case Kind.InvEmbeddedStatement2: s = "invalid EmbeddedStatement"; break;
                case Kind.InvEmbeddedStatement3: s = "invalid EmbeddedStatement"; break;
                case Kind.InvEmbeddedStatement4: s = "invalid EmbeddedStatement"; break;
                case Kind.InvStatementExpression: s = "invalid StatementExpression"; break;
                case Kind.InvForInitializer: s = "invalid ForInitializer"; break;
                case Kind.InvCatchClauses: s = "invalid CatchClauses"; break;
                case Kind.InvResourceAcquisition: s = "invalid ResourceAcquisition"; break;
                case Kind.InvUnary: s = "invalid Unary"; break;
                case Kind.InvAssignmentOperator: s = "invalid AssignmentOperator"; break;
                case Kind.InvSwitchLabel: s = "invalid SwitchLabel"; break;
                case Kind.InvRelExpr: s = "invalid RelExpr"; break;
                case Kind.InvRelExpr2: s = "invalid RelExpr"; break;
                case Kind.InvShiftExpr: s = "invalid ShiftExpr"; break;
                case Kind.InvPrimary: s = "invalid Primary"; break;
                case Kind.InvPrimary2: s = "invalid Primary"; break;
                case Kind.InvPrimary3: s = "invalid Primary"; break;
                case Kind.InvLiteral: s = "invalid Literal"; break;

                default: s = "error " + n; break;
            }
            errorStream.WriteLine(errMsgFormat, line, col, s);
            count++;
        }

        public void SemErr(int line, int col, string s)
        {
            errorStream.WriteLine(errMsgFormat, line, col, s);
            count++;
        }

        public void SemErr(string s)
        {
            errorStream.WriteLine(s);
            count++;
        }

        public void Warning(int line, int col, string s)
        {
            errorStream.WriteLine(errMsgFormat, line, col, s);
        }

        public void Warning(string s)
        {
            errorStream.WriteLine(s);
        }
    } // Errors


    public class FatalError : Exception
    {
        public FatalError(string m) : base(m) { }
    }
}
