using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UtilSharpDX.Math;

namespace UtilSharpDX
{
    public enum LAYOUT_INPUT_ELEMENT_KIND
    {
        /// <summary>
        /// 頂点情報の組み合わせ。
        /// </summary>
        /// <see cref="MC_VERTEX_PC"/>
        PC = 1,
        /// <summary>
        /// 頂点情報の組み合わせ。
        /// </summary>
        /// <see cref="MC_VERTEX_PTx"/>
        PTx,
        /// <summary>
        /// 頂点情報の組み合わせ
        /// </summary>
        /// <see cref="MC_VERTEX_P"/>
        /// <see cref="MC_VERTEX_Tx"/>
        P_Tx,
        /// <summary>
        /// 頂点情報の組み合わせ
        /// </summary>
        /// <see cref="MC_VERTEX_PNC"/>
        PNC,
        /// <summary>
        /// 頂点情報の組み合わせ
        /// </summary>
        /// <see cref="MC_VERTEX_PCTx"/>
        PCTx,
        /// <summary>
        /// 頂点情報の組み合わせ
        /// </summary>
        /// <see cref="MC_VERTEX_P"/>
        /// <see cref="MC_VERTEX_Tx"/>
        P_Tx_Tx,
        /// <summary>
        /// 頂点情報の組み合わせ
        /// </summary>
        /// <see cref="MC_VERTEX_PCTxTx"/>
        PCTxTx,
        /// <summary>
        /// 頂点情報の組み合わせ
        /// </summary>
        /// <see cref="MC_VERTEX_PNCTx"/>
        PNCTx,
        /// <summary>
        /// 頂点情報の組み合わせ
        /// </summary>
        /// <see cref="MC_VERTEX_PNCCTx"/>
        PNCCTx,
        /// <summary>
        /// 終了を表す
        /// </summary>
        END,
        /// <summary>
        /// LAYOUT_INPUT_ELEMENT_KIND列挙体の最後の要素を表す
        /// </summary>
        MAX = -1
    }



    /// @name 頂点情報の組み合わせ
    //@{
    /// <summary>
    /// 頂点情報の組み合わせ。頂点のみ
    /// </summary>
    public struct MC_VERTEX_P
    {
        /// <summary>
        /// 頂点
        /// </summary>
        public MCVector3 p;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pos">頂点</param>
        public MC_VERTEX_P(MCVector3 pos)
        {
            p = pos;
        }

        /// <summary>
        /// 複製セット
        /// </summary>
        /// <param name="pos">頂点</param>
        public void Set(MCVector3 pos)
        {
            p = pos;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            p = new MCVector3();
        }
    };
    /// <summary>
    /// 頂点情報の組み合わせ。色のみ
    /// </summary>
    public struct MC_VERTEX_C
    {
        /// <summary>
        /// 色
        /// </summary>
        public Color4 c;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pos">頂点</param>
        public MC_VERTEX_C(Color4 color)
        {
            c = color;
        }

        /// <summary>
        /// 複製セット
        /// </summary>
        /// <param name="color">色</param>
        public void Set(Color4 color)
        {
            c = color;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            c = new Color4();
        }
    };
    /// <summary>
    /// 頂点情報の組み合わせ。UV座標のみ
    /// </summary>
    public struct MC_VERTEX_Tx
    {
        /// <summary>
        /// テクスチャUV座標
        /// </summary>
        public float u, v;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="a">テクスチャU座標</param>
        /// <param name="b">テクスチャV座標</param>
        public MC_VERTEX_Tx(float a, float b)
        {
            u = a;
            v = b;
        }

        /// <summary>
        /// 複製セット
        /// </summary>
        /// <param name="fU">テクスチャU座標</param>
        /// <param name="fV">テクスチャV座標</param>
        public void Set(float fU, float fV)
        {
            u = fU;
            v = fV;
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            u = v = 0;
        }
    };
    /// <summary>
    /// 頂点情報の組み合わせ。頂点と色のみ
    /// </summary>
    public struct MC_VERTEX_PC
    {
        /// <summary>
        /// 頂点
        /// </summary>
        public MCVector3 p;
        /// <summary>
        /// 色
        /// </summary>
        public Color4 c;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="color">色</param>
        public MC_VERTEX_PC(MCVector3 pos, Color4 color)
        {
            p = pos;
            c = color;
        }

        /// <summary>
        /// 複製セット
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="color">色</param>
        public void Set(MCVector3 pos, Color4 color)
        {
            p = pos;
            c = color;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            p = new MCVector3();
            c = new Color4();
        }
    };
    /// <summary>
    /// 頂点情報の組み合わせ。頂点とUV座標のみ
    /// </summary>
    public struct MC_VERTEX_PTx
    {
        /// <summary>
        /// 頂点
        /// </summary>
        public MCVector3 p;
        /// <summary>
        /// テクスチャUV座標
        /// </summary>
        public float u, v;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="a">テクスチャU座標</param>
        /// <param name="b">テクスチャV座標</param>
        public MC_VERTEX_PTx(MCVector3 pos, float a, float b)
        {
            p = pos;
            u = a;
            v = b;
        }

        /// <summary>
        /// 複製セット
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="fU">テクスチャU座標</param>
        /// <param name="fV">テクスチャV座標</param>
        public void Set(MCVector3 pos, float fU, float fV)
        {
            p = pos;
            u = fU;
            v = fV;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            p = new MCVector3();
            u = v = 0;
        }
    };
    /// <summary>
    /// 頂点情報の組み合わせ。正規化された接線ベクトル、重みとインデックスのみ
    /// </summary>
    public struct MC_VERTEX_TnBwBi
    {
        /// <summary>
        /// 正規化された接線ベクトル
        /// </summary>
        public float[] aTangent;
        /// <summary>
        /// 重み
        /// </summary>
        public float[] afWeight;
        /// <summary>
        /// インデックス
        /// </summary>
        public UInt32[] aIndex;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="a">正規化された接線ベクトル 4つ</param>
        /// <param name="b">重み 4つ</param>
        /// <param name="c">インデックス 4つ</param>
        public MC_VERTEX_TnBwBi(float[] a, float[] b, UInt32[] c)
        {
            aTangent = new float[4];
            afWeight = new float[4];
            aIndex = new UInt32[4];

            Array.Copy(a, aTangent, 4);
            Array.Copy(b, afWeight, 4);
            Array.Copy(c, aIndex, 4);
        }
        /// <summary>
        /// 複製セット
        /// </summary>
        /// <param name="a">正規化された接線ベクトル 4つ</param>
        /// <param name="b">重み 4つ</param>
        /// <param name="c">インデックス 4つ</param>
        public void Set(float[] a, float[] b, UInt32[] c)
        {
            aTangent = new float[4];
            afWeight = new float[4];
            aIndex = new UInt32[4];

            Array.Copy(a, aTangent, 4);
            Array.Copy(b, afWeight, 4);
            Array.Copy(c, aIndex, 4);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            aTangent = new float[4];
            afWeight = new float[4];
            aIndex = new UInt32[4];
        }
    };
    /// <summary>
    /// 頂点情報の組み合わせ。頂点、法線、色
    /// </summary>
    public struct MC_VERTEX_PNC
    {
        /// <summary>
        /// 頂点
        /// </summary>
        public MCVector3 p;
        /// <summary>
        /// 法線
        /// </summary>
        public MCVector3 n;
        /// <summary>
        /// 色
        /// </summary>
        public Color4 c;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="normal">法線</param>
        /// <param name="color">色</param>
        public MC_VERTEX_PNC(MCVector3 pos, MCVector3 normal, Color4 color)
        {
            p = pos;
            n = normal;
            c = color;
        }

        /// <summary>
        /// 複製セット
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="normal">法線</param>
        /// <param name="color">色</param>
        public void Set(MCVector3 pos, MCVector3 normal, Color4 color)
        {
            p = pos;
            n = normal;
            c = color;
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            p = new MCVector3();
            n = new MCVector3();
            c = new Color4();
        }
    };
    /// @brief 頂点情報の組み合わせ。頂点、法線、UV座標
    public struct MC_VERTEX_PCTx
    {
        /// <summary>
        /// 頂点
        /// </summary>
        public MCVector3 p;
        /// <summary>
        /// 色
        /// </summary>
        public Color4 c;
        /// <summary>
        /// テクスチャUV座標
        /// </summary>
        public float u, v;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="color">色</param>
        /// <param name="fU">テクスチャU座標</param>
        /// <param name="fV">テクスチャV座標</param>
        public MC_VERTEX_PCTx(MCVector3 pos, Color4 color, float fU, float fV)
        {
            p = pos;
            c = color;
            u = fU;
            v = fV;
        }
        /// <summary>
        /// セット
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="color">色</param>
        /// <param name="fU">テクスチャU座標</param>
        /// <param name="fV">テクスチャV座標</param>
        public void Set(MCVector3 pos, Color4 color, float fU, float fV)
        {
            p = pos;
            c = color;
            u = fU;
            v = fV;
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            p = new MCVector3();
            c = new Color4();
            u = v = 0;
        }
    };
    /// <summary>
    /// 頂点情報の組み合わせ。頂点、色、UV座標１，UV座標２
    /// </summary>
    public struct MC_VERTEX_PCTxTx
    {
        /// <summary>
        /// 頂点
        /// </summary>
        public MCVector3 p;
        /// <summary>
        /// 色
        /// </summary>
        public Color4 c;
        /// <summary>
        /// テクスチャUV座標
        /// </summary>
        public float u0, v0;
        /// <summary>
        /// テクスチャUV座標
        /// </summary>
        public float u1, v1;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="color">色</param>
        /// <param name="fU0">テクスチャU座標</param>
        /// <param name="fV0">テクスチャV座標</param>
        /// <param name="fU1">テクスチャU座標</param>
        /// <param name="fV1">テクスチャV座標</param>
        public MC_VERTEX_PCTxTx(MCVector3 pos, Color4 color, float fU0, float fV0, float fU1, float fV1)
        {
            p = pos;
            c = color;
            u0 = fU0;
            v0 = fV0;
            u1 = fU1;
            v1 = fV1;
        }


        /// <summary>
        /// 複製セット
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="color">色</param>
        /// <param name="fU0">テクスチャU座標</param>
        /// <param name="fV0">テクスチャV座標</param>
        /// <param name="fU1">テクスチャU座標</param>
        /// <param name="fV1">テクスチャV座標</param>
        public void Set(MCVector3 pos, Color4 color, float fU0, float fV0, float fU1, float fV1)
        {
            p = pos;
            c = color;
            u0 = fU0;
            v0 = fV0;
            u1 = fU1;
            v1 = fV1;
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            p = new MCVector3();
            c = new Color4();
            u0 = v0 = 0;
            u1 = v1 = 0;
        }
    };
    //@}

    public enum MC_SEMANTIC
    {
        /// <summary>P ：頂点（位置）</summary>
        P = 0,
        /// <summary>C ：色 <summary>
        C,
        /// <summary>Bw：ブレンドの重み</summary>
        Bw,
        /// <summary>Bi：ブレンドのインデックス値</summary>
        Bi,
        /// <summary>N ：法線</summary>
        N,
        /// <summary>Tx：テクスチャー座標UV</summary>
        Tx,
        /// <summary>Tn：正規化された接線ベクトル</summary>
        Tn,
        /// <summary>S ：</summary>
        S
    };


    internal class DefinedSemanticName
    {
        /// <summary>
        /// 頂点で使用する、各セマンティックの名前
        /// </summary>
        private static string[,] aSemanticName = new string[,]
        {
            /// P ：頂点（位置）
            {"POSITION0","POSITION1","POSITION2","POSITION3","POSITION4","POSITION5","POSITION6","POSITION7","POSITION8","POSITION9","POSITION" },
            /// C ：色
            {"COLOR0","COLOR1","COLOR2","COLOR3","COLOR4","COLOR5","COLOR6","COLOR7","COLOR8","COLOR9","COLOR" },
            /// Bw：ブレンドの重み
            {"BLENDWEIGHTS0","BLENDWEIGHTS1","BLENDWEIGHTS2","BLENDWEIGHTS3","BLENDWEIGHTS4","BLENDWEIGHTS5","BLENDWEIGHTS6","BLENDWEIGHTS7","BLENDWEIGHTS8","BLENDWEIGHTS9","BLENDWEIGHTS" },
            /// Bi：ブレンドのインデックス値
            {"BLENDINDICES0","BLENDINDICES1","BLENDINDICES2","BLENDINDICES3","BLENDINDICES4","BLENDINDICES5","BLENDINDICES6","BLENDINDICES7","BLENDINDICES8","BLENDINDICES9","BLENDINDICES" },
            /// N ：法線
            {"BLENDINDICES0","BLENDINDICES1","BLENDINDICES2","BLENDINDICES3","BLENDINDICES4","BLENDINDICES5","BLENDINDICES6","BLENDINDICES7","BLENDINDICES8","BLENDINDICES9","BLENDINDICES" },
            /// Tx：テクスチャー座標UV
            {"TEXCOORD0","TEXCOORD1","TEXCOORD2","TEXCOORD3","TEXCOORD4","TEXCOORD5","TEXCOORD6","TEXCOORD7","TEXCOORD8","TEXCOORD9","TEXCOORD" },
            /// Tn：変形
            {"TANGENT0","TANGENT1","TANGENT2","TANGENT3","TANGENT4","TANGENT5","TANGENT6","TANGENT7","TANGENT8","TANGENT9","TANGENT" },
            // S ：
            {"SOURCERECT0","SOURCERECT1","SOURCERECT2","SOURCERECT3","SOURCERECT4","SOURCERECT5","SOURCERECT6","SOURCERECT7","SOURCERECT8","SOURCERECT9","SOURCERECT" },
        };
        public static string Get(MC_SEMANTIC i, int semanticNo)
        {
            return aSemanticName[(int)i, semanticNo];
        }
    }

    //SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
    //S
    //SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
    public struct D3D11_INPUT_ELEMENT_DESC_EX : IEquatable<D3D11_INPUT_ELEMENT_DESC_EX>
    {
        private InputElement element;
        public MC_SEMANTIC SemanticType;
        public int SemanticNo;

        public string SemanticName { get { return element.SemanticName;} set { element.SemanticName=value; } }
        public int SemanticIndex { get { return element.SemanticIndex; } set { element.SemanticIndex = value; } }
        public Format Format { get { return element.Format; } set { element.Format = value; } }
        public int Slot { get { return element.Slot; } set { element.Slot = value; } }
        public int AlignedByteOffset { get { return element.AlignedByteOffset; } set { element.AlignedByteOffset = value; } }
        public InputClassification Classification { get { return element.Classification; } set { element.Classification = value; } }
        public int InstanceDataStepRate { get { return element.InstanceDataStepRate; } set { element.InstanceDataStepRate = value; } }


        public D3D11_INPUT_ELEMENT_DESC_EX(string name, int index, Format format, int slot)
        {
            element = new InputElement(name, index, format, slot);
            SemanticType = 0;
            SemanticNo = 0;
        }
        public D3D11_INPUT_ELEMENT_DESC_EX(string name, int index, Format format, int offset, int slot)
        {
            element = new InputElement(name, index, format, offset, slot);
            SemanticType = 0;
            SemanticNo = 0;
        }
        public D3D11_INPUT_ELEMENT_DESC_EX(string name, int index, Format format, int offset, int slot, InputClassification slotClass, int stepRate)
        {
            element = new InputElement(name, index, format, offset, slot, slotClass, stepRate);
            SemanticType = 0;
            SemanticNo = 0;
        }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b">セマティク名の最後につく番号(0-9) 例えば、0と入力した場合、POSTION なら POTION0 となる。9より大きい数値の場合、番号はつかない。</param>
        /// <param name="c">要素のセマンティクス インデックスです。セマンティクス インデックスは、整数のインデックス番号によってセマンティクスを修飾するものです。
        ///  セマンティクス インデックスは、同じセマンティクスの要素が複数ある場合にのみ必要です。
        ///  たとえば、4x4 のマトリクスには 4 個の構成要素があり、それぞれの構成要素にはセマンティクス名として matrix が付けられますが、
        ///  4 個の構成要素にはそれぞれ異なるセマンティクス インデックス(0、1、2、3) が割り当てられます。
        ///  </param>
        /// <param name="d">要素データのデータ型です。「DXGI_FORMAT」を参照してください。</param>
        /// <param name="e">入力アセンブラーを識別する整数値です(「入力スロット」を参照してください)。有効な値は 0 ～ 15 であり、D3D11.h で定義されています。</param>
        /// <param name="f">(省略可能)各要素間のオフセット(バイト単位) です。前の要素の直後で現在の要素を定義するには、D3D11_APPEND_ALIGNED_ELEMENT を使用すると便利です。必要に応じてパッキング処理も指定できます。</param>
        /// <param name="g">単一の入力スロットの入力データ クラスを識別します(「D3D11_INPUT_CLASSIFICATION」を参照してください)。</param>
        /// <param name="h">バッファーの中で要素の 1 つ分進む前に、インスタンス単位の同じデータを使用して描画するインスタンスの数です。
        /// 頂点単位のデータを持つ要素(スロット クラスは InputClassification.PerVertexData に設定されています) では、この値が 0 であることが必要です。</param>
        public void Set(MC_SEMANTIC a, int b, int c, Format d, int e, int f, InputClassification g, int h)
        {
            SemanticType = a;
            SemanticNo = b > 9 ? 10 : b;
            // シェーダー入力署名でこの要素に関連付けられている HLSL セマンティクスです。
            SemanticName = DefinedSemanticName.Get(a,SemanticNo);
            SemanticIndex = c;
            Format = d;
            Slot = e;
            AlignedByteOffset = f;
            Classification = g;
            InstanceDataStepRate = h;
        }
        /// <summary>
        /// セットする
        /// </summary>
        public void Set(D3D11_INPUT_ELEMENT_DESC_EX o)
        {
            SemanticType = o.SemanticType;
            SemanticNo = o.SemanticNo;
            SemanticName = o.SemanticName;
            SemanticIndex = o.SemanticIndex;
            Format = o.Format;
            Slot = o.Slot;
            AlignedByteOffset = o.AlignedByteOffset;
            Classification = o.Classification;
            InstanceDataStepRate = o.InstanceDataStepRate;
        }
        /// <summary>
        /// セットする
        /// </summary>
        public void Get(ref InputElement to)
        {
            to.SemanticName = SemanticName;
            to.SemanticIndex = SemanticIndex;
            to.Format = Format;
            to.Slot = Slot;
            to.AlignedByteOffset = AlignedByteOffset;
            to.Classification = Classification;
            to.InstanceDataStepRate = InstanceDataStepRate;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object value)
        {
            return base.Equals(value);
        }
        public bool Equals(ref D3D11_INPUT_ELEMENT_DESC_EX other)
        {
            return this == other;
        }
        public bool Equals(D3D11_INPUT_ELEMENT_DESC_EX other)
        {
            return this == other;
        }

        public static bool operator ==(D3D11_INPUT_ELEMENT_DESC_EX l, D3D11_INPUT_ELEMENT_DESC_EX r)
        {
            if(l.element == r.element)
            {
                return l.SemanticType == r.SemanticType && l.SemanticNo == r.SemanticNo;
            }
            return true;
        }
        public static bool operator !=(D3D11_INPUT_ELEMENT_DESC_EX l, D3D11_INPUT_ELEMENT_DESC_EX r) { return !(l == r); }

    }
    //SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
    //S
    //SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
    public class InputElementDescInfo
    {
        public InputElement[] aDesc;
        public MC_SEMANTIC[] aAsemanticType;
        public int numAry;

        public InputElementDescInfo()
        {

        }
        /// <summary>
        /// セットする
        /// </summary>
        public void Set(D3D11_INPUT_ELEMENT_DESC_EX [] a, int len)
        {
            int min = System.Math.Min(a.Length, len);
            aDesc = new InputElement[min];
            aAsemanticType = new MC_SEMANTIC[min];

            for (int i = 0; i < min; ++i)
            {
                a[i].Get(ref aDesc[i]);
                aAsemanticType[i] = a[i].SemanticType;
                aDesc[i].SemanticName = DefinedSemanticName.Get(a[i].SemanticType,a[i].SemanticNo);
            }
            numAry = len;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            return base.Equals(0);
        }
        public static bool operator ==(InputElementDescInfo l, InputElementDescInfo r)
        {
            if (l.numAry != r.numAry) return false;
            for (uint i = 0; i < l.numAry; ++i)
            {
                if (l.aAsemanticType[i] != r.aAsemanticType[i] || l.aDesc[i]!= r.aDesc[i])
                    return false;
            }
            return true;
        }
        public static bool operator !=(InputElementDescInfo l, InputElementDescInfo r) { return !(l == r); }
    }


    /// <summary>
    /// レイアウトマネージャー
    /// </summary>
    public sealed class MCInputLayoutMgr
	{
        /// <summary>
        /// 現在のレイアウトID
        /// </summary>
        private int m_nowLayoutID;

        /// <summary>
        /// アプリ
        /// </summary>
        public Application App { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private class InputElementDescInfoEx : InputElementDescInfo
        {
            public Dictionary<EffectPass, InputLayout> index = new Dictionary<EffectPass, InputLayout>();
            public InputElementDescInfoEx(){ }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, InputElementDescInfoEx> m_index = new Dictionary<int, InputElementDescInfoEx>();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCInputLayoutMgr(Application app)
        {
            App = app;



            // レイアウトidは１から
            m_nowLayoutID = 1;
            //
            int layoutID;
            D3D11_INPUT_ELEMENT_DESC_EX[] a = new D3D11_INPUT_ELEMENT_DESC_EX[8];
            Action<int, MC_SEMANTIC, int, int, Format, int, InputClassification, int> SET_IEDE =
                (i, b, c, d, e, f, g, h) =>
            {
                a[i].SemanticType = b;
                a[i].SemanticNo = c;
                a[i].SemanticName = DefinedSemanticName.Get(b, c);
                a[i].SemanticIndex = d;
                a[i].Format = e;
                a[i].Slot = f;
                a[i].AlignedByteOffset = -1;    // APPEND_ALIGNED_ELEMENT
                a[i].Classification = g;
                a[i].InstanceDataStepRate = h;
            };

            // LAYOUT_PC
            a = new D3D11_INPUT_ELEMENT_DESC_EX[2];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.C, 10, 0, Format.R32G32B32A32_Float, 0, InputClassification.PerVertexData, 0);
            GetLayoutID(a, 2, out layoutID);

            // LAYOUT_PTx
            a = new D3D11_INPUT_ELEMENT_DESC_EX[2];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.Tx, 10, 0, Format.R32G32_Float, 0, InputClassification.PerVertexData, 0);
            GetLayoutID(a, 2, out layoutID);

            // LAYOUT_P_Tx
            a = new D3D11_INPUT_ELEMENT_DESC_EX[2];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.Tx, 10, 0, Format.R32G32_Float, 1, InputClassification.PerVertexData, 0);
            GetLayoutID(a, 2, out layoutID);


            // LAYOUT_PNC
            a = new D3D11_INPUT_ELEMENT_DESC_EX[3];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.N, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(2, MC_SEMANTIC.Tx, 10, 0, Format.R32G32_Float, 0, InputClassification.PerVertexData, 0);
            GetLayoutID(a, 3, out layoutID);


            // LAYOUT_PCTx
            a = new D3D11_INPUT_ELEMENT_DESC_EX[3];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.C, 10, 0, Format.R32G32B32A32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(2, MC_SEMANTIC.Tx, 10, 0, Format.R32G32_Float, 0, InputClassification.PerVertexData, 0);
            GetLayoutID(a, 3, out layoutID);

            // LAYOUT_P_Tx_Tx
            a = new D3D11_INPUT_ELEMENT_DESC_EX[3];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.Tx, 0, 0, Format.R32G32_Float, 1, InputClassification.PerVertexData, 0);
            SET_IEDE(2, MC_SEMANTIC.Tx, 1, 1, Format.R32G32_Float, 2, InputClassification.PerVertexData, 0);
            GetLayoutID(a, 3, out layoutID);

            // LAYOUT_PCTxTx
            a = new D3D11_INPUT_ELEMENT_DESC_EX[4];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.C, 10, 0, Format.R32G32B32A32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(2, MC_SEMANTIC.Tx, 0, 0, Format.R32G32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(3, MC_SEMANTIC.Tx, 1, 1, Format.R32G32_Float, 0, InputClassification.PerVertexData, 0);
            GetLayoutID(a, 4, out layoutID);

            // LAYOUT_PNCTx
            a = new D3D11_INPUT_ELEMENT_DESC_EX[4];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.N, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(2, MC_SEMANTIC.C, 10, 0, Format.R32G32B32A32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(3, MC_SEMANTIC.Tx, 10, 0, Format.R32G32_Float, 0, InputClassification.PerVertexData, 0);
            GetLayoutID(a, 4, out layoutID);

            // LAYOUT_PNCCTx
            a = new D3D11_INPUT_ELEMENT_DESC_EX[5];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.N, 10, 0, Format.R32G32B32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(2, MC_SEMANTIC.C, 0, 0, Format.R32G32B32A32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(3, MC_SEMANTIC.C, 1, 0, Format.R32G32B32A32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(4, MC_SEMANTIC.Tx, 10, 0, Format.R32G32_Float, 0, InputClassification.PerVertexData, 0);
            GetLayoutID(a, 5, out layoutID);

            a = new D3D11_INPUT_ELEMENT_DESC_EX[8];
            SET_IEDE(0, MC_SEMANTIC.P, 10, 0, Format.R32G32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(1, MC_SEMANTIC.Tn, 10, 0, Format.R32G32_Float, 0, InputClassification.PerVertexData, 0);
            SET_IEDE(2, MC_SEMANTIC.Tn, 0, 0, Format.R32G32B32A32_Float, 1, InputClassification.PerInstanceData, 1);
            SET_IEDE(3, MC_SEMANTIC.Tn, 1, 1, Format.R32G32B32A32_Float, 1, InputClassification.PerInstanceData, 1);
            SET_IEDE(4, MC_SEMANTIC.Tn, 2, 2, Format.R32G32B32A32_Float, 1, InputClassification.PerInstanceData, 1);
            SET_IEDE(5, MC_SEMANTIC.Tn, 3, 3, Format.R32G32B32A32_Float, 1, InputClassification.PerInstanceData, 1);
            SET_IEDE(6, MC_SEMANTIC.C, 10, 0, Format.R32G32B32A32_Float, 1, InputClassification.PerInstanceData, 1);
            SET_IEDE(7, MC_SEMANTIC.S, 10, 0, Format.R32G32B32A32_Float, 1, InputClassification.PerInstanceData, 1);
            GetLayoutID(a, 8, out layoutID);
        }

        /// <summary>
        /// レイアウトの作成
        /// </summary>
        /// <param name="p"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private InputLayout CreateLayout( EffectPass p, InputElementDescInfo info)
	    {
		    return new InputLayout(App.DXDevice, p.Description.Signature, info.aDesc);
        }
        //      public ~MCInputLayoutMgr();

        /// <summary>
        /// レイアウトidを取得する
        /// </summary>
        /// <param name="a">配列</param>
        /// <param name="len">配列の長さ</param>
        /// <param name="outID">レイアウトidを格納</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        public int GetLayoutID(D3D11_INPUT_ELEMENT_DESC_EX [] a, int len, out int outID)
        {
            InputElementDescInfoEx tmp = new InputElementDescInfoEx();

            tmp.Set(a, len);

            foreach (var val in m_index)
            {
                if (val.Value == tmp)
                {
                    outID = val.Key;
                    return 0;
                }
            }
            if (!m_index.ContainsKey(m_nowLayoutID))
            {
                m_index.Add(m_nowLayoutID, tmp);
                outID = m_nowLayoutID;
                ++m_nowLayoutID;
            }
            else
            {
                outID = 0;
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// 指定エフェクトパスにレイアウトIDからレイアウトをセットする
        /// </summary>
        /// <param name="p">エフェクトパス</param>
        /// <param name="layoutID">レイアウトid</param>
        /// <returns></returns>
        public int IASetInputLayout(EffectPass p, int layoutID)
        {
            InputLayout il=null;

            if (Search(p, layoutID, out il) !=0)
                return -1;

            App.ImmediateContext.InputAssembler.InputLayout = il;
            return 0;
        }

        /// <summary>
        /// 挿入または検索
        /// </summary>
        /// <param name="p">エフェクトパス</param>
        /// <param name="rInfo"></param>
        /// <param name="inputLayout"></param>
        /// <returns></returns>
        public int InsertOrSearch(EffectPass p, InputElementDescInfo rInfo, out InputLayout inputLayout)
        {
            inputLayout = null;
            foreach (var val in m_index)
            {
                if (val.Value == rInfo)
                {
                    if (val.Value.index.ContainsKey(p))
                    {
                        // 既にキーが存在しない
                        if ((inputLayout = CreateLayout(p, val.Value))==null) return -1;
                        val.Value.index.Add(p, inputLayout);
                    }
                    else
                    {
                        inputLayout = val.Value.index[p];
                    }
                    return 0;
                }
            }

            return -1;
        }

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="p">エフェクトパス</param>
        /// <param name="layoutID">検索したいレイアウトid</param>
        /// <param name="outLayout"></param>
        /// <returns></returns>
        public int Search(EffectPass p, int layoutID, out InputLayout outLayout)
        {
            outLayout = null;

            if (!m_index.ContainsKey(layoutID))
                return -1;
            if( !m_index[layoutID].index.ContainsKey(p) )
            {
                // 無いので作る
                if ((outLayout = CreateLayout(p, m_index[layoutID])) == null)
                    return -1;
                outLayout.DebugName = ((LAYOUT_INPUT_ELEMENT_KIND)layoutID).ToString();
                m_index[layoutID].index.Add(p, outLayout);
            }

            outLayout = m_index[layoutID].index[p];

            return 0;
        }

        /// <summary>
        /// デバイス作成時に呼ばれる
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        internal int OnCreateDevice(SharpDX.Direct3D11.Device dev)
        {
            return 0;
        }

        /// <summary>
        /// MainLoopを抜け出し、終了後に呼ばれる
        /// </summary>
        internal void OnDestroyDevice()
        {
            foreach (var val in m_index)
            {
                val.Value.index.Clear();
            }
        }
    }
}
