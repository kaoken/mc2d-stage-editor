using MC2DUtil;
using System;

namespace UtilSharpDX.DrawingCommand
{



    /// <summary>
    /// 描画コマンドで使用する。プライオリティー共用体
    /// </summary>
    public struct MCDrawCommandPriority
    {
        /// <summary>
        /// D3TypeFlags 通常状態
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3TypeFlags"/>
        public static readonly ulong TYPE_DEFAULT = 0;
        /// <summary>
        /// D3TypeFlags ボーンが含まれる
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3TypeFlags"/>
        public static readonly ulong TYPE_BONE = 1;
        /// <summary>
        /// d3 2D描画（スプライトなど）
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3"/>
        public static readonly ulong D2RENDERING = 0;
        /// <summary>
        /// d3 3D描画			
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3"/>
        public static readonly ulong D3RENDERING = 1;

        #region 共通
        /// <summary>
        /// テクニックの最大値[255]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.Technique"/>
        public static readonly ulong TECHNIC_MAX = 0x000000FF;
        /// <summary>
        /// 3D　エフェクト番号[255]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.Effect"/>
        public static readonly ulong EFFECT_MAX = 0x000000FF;
        /// <summary>
        /// パス内での優先度最大値[17;592;186;044;415]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.Priority"/>
        public static readonly ulong PRIORITY_MAX = 0x00000FFFFFFFFFFF;
        /// <summary>
        /// 3Dか 2D？[1]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3"/>
        public static readonly ulong D3_MAX = 0x00000001;
        #endregion

        #region 3D
        /// <summary>
        /// 3D　G;B;T;M[127]	
        /// </summary>
        /// <see cref="MCDrawCommandPriority.d3State"/>
        public static readonly ulong D3STATE_MAX = 0x0000007F;
        /// <summary>
        /// 3D　描画優先順位[4;194;303]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3No"/>
        public static readonly ulong D3NO_MAX = 0x0003FFFF;
        /// <summary>
        /// 3D　タイプ（ボーンなど）[63]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.d3Type"/>
        public static readonly ulong D3TYPE_MAX = 0x0000003F;
        /// <summary>
        /// 3D　半透明[1]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3Translucent"/>
        public static readonly ulong D3TRANSLUCENT_MAX = 0x00000001;
        #endregion

        #region 3D
        /// <summary>
        /// 2D  描画順の最大値[268;435;456]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D2No"/>
        public static readonly ulong D2NO_MAX = 0x7FFFFFFF;
        /// <summary>
        /// 2D  描画順の最大値[255]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.d2Type"/>
        public static readonly ulong D2TYPE_MAX = 0x000000FF;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UInt64 EntireRegion;

        //----------------------------------------------------
        /// <summary>
        /// [0~255]		テクニック [0,8]
        /// </summary>
        public byte Technique;
        /// <summary>
        /// [0~127]		3D　G,B,T,M [8,7]
        /// </summary>
        public byte D3State;
        /// <summary>
        /// [0~4,194,303]	3D　描画優先順位 [15,22]
        /// </summary>
        public UInt32 D3No;
        /// <summary>
        /// [0~63]			3D　タイプフラグ（ボーンなど）[37,6]
        /// </summary>
        public byte D3TypeFlags;
        /// <summary>
        /// [0~1]			3D　半透明 [43,1]
        /// </summary>
        public bool D3Translucent;
        /// <summary>
        /// [0~255]		エフェクト番号 [44,8]
        /// </summary>
        public byte Effect;

        //----------------------------------------------------
        /// <summary>
        /// [0~268,435,455]	2D　描画順 [0,28]
        /// </summary>
        public UInt32 D2No;
        /// <summary>
        /// [0~255]			スプライト処理タイプの番号 [36,8]
        /// </summary>
        public UInt64 D2RenderType;

        //----------------------------------------------------
        /// <summary>
        /// [0~17,592,186,044,415] パス内での優先度 [0, 52]
        /// </summary>
        public UInt64 Priority;
        /// <summary>
        /// [0~1]          3Dタイプ（Mesh)なら1の値0の場合は 2D（スプライト）など [52,1]
        /// </summary>
        public bool D3;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            Technique = 0;
            D3State = 0;
            D3No = 0;
            D3TypeFlags = 0;
            D3Translucent = false;
            Effect = 0;
            D2No = 0;
            D2RenderType = 0;
            D3 = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Build()
        {
            EntireRegion = (TECHNIC_MAX & Technique);
            if (D3)
            {
                EntireRegion |= (D3STATE_MAX & D3State) << 8;
                EntireRegion |= (D3NO_MAX & D3No) << 15;
                EntireRegion |= (D3TYPE_MAX & D3TypeFlags) << 37;
                EntireRegion |= Convert.ToUInt64(D3Translucent) << 43;
            }
            else
            {
                EntireRegion |= (D2NO_MAX & D2No);
                EntireRegion |= (D2TYPE_MAX & D2RenderType) << 28;
            }
            EntireRegion |= (EFFECT_MAX & Effect) << 44;
            EntireRegion |= Convert.ToUInt64(D3) << 52;

            Priority = EntireRegion & PRIORITY_MAX;
        }
    }

}
