using MC2DUtil;
using System;

namespace UtilSharpDX.DrawingCommand
{

    /// <summary>
    /// MCDrawCommandPriority で使用するマクロ群
    /// </summary>
    public enum MC_DCPRIORITY : ulong
    {
        /// <summary>
        /// d3Type 通常状態
        /// </summary>
        /// <see cref="MCDrawCommandPriority.d3Type"/>
        TYPE_DEFAULT = 0,
        /// <summary>
        /// d3Type ボーンが含まれる
        /// </summary>
        /// <see cref="MCDrawCommandPriority.d3Type"/>
        TYPE_BONE = 1,
        /// <summary>
        /// d3 2D描画（スプライトなど）
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3"/>
        D2RENDERING = 0,
        /// <summary>
        /// d3 3D描画			
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3"/>
        D3RENDERING = 1,
        /// <summary>
        /// テクニックの最大値[255]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.Technique"/>
        TECHNIC_MAX = 0x000000FF,
        /// <summary>
        /// 3D　G,B,T,M[127]	
        /// </summary>
        /// <see cref="MCDrawCommandPriority.d3State"/>
        D3STATE_MAX = 0x0000007F,
        /// <summary>
        /// 3D　描画優先順位[4,194,303]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3No"/>
        D3NO_MAX = 0x0003FFFF,
        /// <summary>
        /// 3D　タイプ（ボーンなど）[63]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.d3Type"/>
        D3TYPE_MAX = 0x0000003F,
        /// <summary>
        /// 3D　半透明[1]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3Translucent"/>
        D3TRANSLUCENT_MAX = 0x00000001,
        /// <summary>
        /// 3D　エフェクト番号[255]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.Effect"/>
        EFFECT_MAX = 0x000000FF,
        /// <summary>
        /// 2D  描画順の最大値[268,435,456]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D2No"/>
        D2NO_MAX = 0x7FFFFFFF,
        /// <summary>
        /// 2D  描画順の最大値[255]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.d2Type"/>
        D2TYPE_MAX = 0x000000FF,
        /// <summary>
        /// パス内での優先度最大値[17,592,186,044,415]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.Priority"/>
        PRIORITY_MAX = 0x00000FFFFFFFFFFF,
        /// <summary>
        /// 3Dか 2D？[1]
        /// </summary>
        /// <see cref="MCDrawCommandPriority.D3"/>
        D3_MAX = 0x00000001,
    }


    /// <summary>
    /// 描画コマンドで使用する。プライオリティー共用体
    /// </summary>
    [BitFieldNumberOfBitsAttribute(64)]
    public struct MCDrawCommandPriority : IBitField
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt64 EntireRegion
        {
            get
            {
                return this.ToUInt64();
            }
        }

        //----------------------------------------------------
        /// <summary>
        /// [0~255]		テクニック
        /// </summary>
        [BitFieldInfo(0, 8)]    //  8
        public byte Technique { get; set; }
        /// <summary>
        /// [0~127]		3D　G,B,T,M
        /// </summary>
        [BitFieldInfo(8, 7)]   //  7
        public byte D3State { get; set; }
        /// <summary>
        /// [0~4,194,303]	3D　描画優先順位
        /// </summary>
        [BitFieldInfo(15, 22)]  // 22
        public UInt32 D3No { get; set; }
        /// <summary>
        /// [0~63]			3D　タイプフラグ（ボーンなど）
        /// </summary>
        [BitFieldInfo(37, 6)]  //  6
        public byte D3TypeFlags { get; set; }
        /// <summary>
        /// [0~1]			3D　半透明
        /// </summary>
        [BitFieldInfo(43, 1)]  //  1
        public bool D3Translucent { get; set; }
        /// <summary>
        /// [0~255]		エフェクト番号
        /// </summary>
        [BitFieldInfo(44, 8)]  //  8
        public byte Effect { get; set; }

        //----------------------------------------------------
        /// <summary>
        /// [0~268,435,455]	2D　描画順
        /// </summary>
        [BitFieldInfo(8, 28)]    // 28
        public UInt32 D2No { get; set; }
        /// <summary>
        /// [0~255]			スプライト処理タイプの番号
        /// </summary>
        [BitFieldInfo(36, 8)]    //  8
        public UInt64 D2RenderType { get; set; }

        //----------------------------------------------------
        /// <summary>
        /// [0~17,592,186,044,415] パス内での優先度
        /// </summary>
        ///[BitFieldInfo(0, 52)]    // 52
        public UInt64 Priority
        {
            get
            {
                return this.ToUInt64() & 0xf_ffff_ffff_ffff;
            }
        }
        /// <summary>
        /// [0~1]          3Dタイプ（Mesh)なら1の値0の場合は 2D（スプライト）など
        /// </summary>
        [BitFieldInfo(52, 1)]    //  1
        public bool D3 { get; set; }

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
    }

}
