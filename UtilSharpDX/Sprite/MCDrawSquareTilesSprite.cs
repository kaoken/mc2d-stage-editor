using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UtilSharpDX.Camera;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{
    /// <summary>
    ///  1つのメッシュスクエア・タイルデータ
    /// </summary>
    public struct MCSquareTileData
    {
        /// <summary>
        /// タイル番号
        /// </summary>
        internal ushort tileNo;
        /// <summary>
        /// フリップフラグ
        /// </summary>
        internal ushort flip;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="a">タイル番号</param>
        /// <param name="b">フリップフラグ</param>
        MCSquareTileData(int a, int b)
        {
            tileNo = (ushort)a;
            flip = (ushort)b;
        }
    }

    /// <summary>
    /// スクエア・タイル群描画スプライト
    /// </summary>
    public sealed class MCDrawSquareTilesSprite : MCDrawSpriteBase, IApp
    {
        public static readonly Guid DrawSpriteID = new Guid("B3A9BF61-7410-4FCA-9644-6554BA3B53DB");

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }

        /// <summary>
        /// スプライトデータ
        /// </summary>
        private MCSprite m_sprite;


        /// <summary>
        /// 対象スプライト
        /// </summary>
        public MCSprite Sprite { get { return m_sprite; } internal set { m_sprite = value; } }


        /// <summary>
        /// タイル配列ポインタ
        /// </summary>
        public List<MCSquareTileData> Chips { get; private set; }
        /// <summary>
        /// タイル数
        /// </summary>
        public int TileNum { get; private set; }
        /// <summary>
        /// X側のタイル数
        /// </summary>
        public int TileNumX { get; private set; }
        /// <summary>
        /// Y側のタイル数
        /// </summary>
        public int TileNumY { get; private set; }
        /// <summary>
        /// タイルの一辺の長さ
        /// </summary>
        public int TileLength { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        public MCDrawSquareTilesSprite(Application app):base()
        {
            App = app;
            Chips = new List<MCSquareTileData>();
            TileNum = 0;
            TileNumX = 0;
            TileNumY = 0;
            TileLength = 32;
        }
        ~MCDrawSquareTilesSprite() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Guid GetID() { return DrawSpriteID; }


        #region MCDrawBase
        /// <summary>
        /// 描画時に呼び出されるスプライト処理
        /// </summary>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="render">スプライトのレンダー</param>
        /// <param name="bd"></param>
        /// <returns></returns>
        internal override int CallDrawingSprite(
            double totalTime,
            float elapsedTime,
            IMCSpriteRender render,
            MCCallBatchDrawing cbd
        )
        {
            int hr = 0;
            MCMatrix4x4 mTmp = MCMatrix4x4.Identity, mWVP = MCMatrix4x4.Identity;
            var spriteRender = (MCRenderSquareTilesSprite)render;

            // 頂点値を更新
            spriteRender.VertexUpdate(this);

            // 位置の計算
            if (IsBillbord) { 
                // ビルボードである(使用できない
                Debug.Assert(false);
            }
		    else{
			    // 通常スプライト
                // 位置
                mTmp.M41 += m_position.X;
                mTmp.M42 += m_position.Y;
                mTmp.M43 += m_position.Z;

                mWVP = mTmp* cbd.GetCurrentCamera().ViewProjMatrix;
		    }


            cbd.SetWVP(ref mWVP);
            cbd.SetUniqueValue(m_uniquetValue);

            Sprite.Texture00.SetResource(cbd.GetDiffuseTexture());
            EffectPass pass = cbd.GetEffect().GetCurrentEffectPass();

            hr = App.LayoutMgr.IASetInputLayout(pass, (int)spriteRender.GetLayoutKind());
            spriteRender.VertexUpdate(this);
            spriteRender.Render(App.ImmediateContext, pass, this);
            return hr;
	    }

        /// <summary>
        /// 描画時に呼び出される３D処理
        /// </summary>
        /// <param name="pImmediateContext"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="pOldPriority"></param>
        /// <param name="pPriority"></param>
        /// <param name="cbd"></param>
        /// <param name="callNum"></param>
        /// <param name="passCount"></param>
        /// <returns></returns>
        internal override int CallDraw3D(
            DeviceContext immediateContext,
            double totalTime,
            float elapsedTime,
            MCDrawCommandPriority oldPriority,
            MCDrawCommandPriority priority,
            MCCallBatchDrawing cbd,
            int callNum,
            int passCount
        )
        { return 0; }

        /// <summary>
        /// カメラとの衝突判定
        ///
        /// 描画コマンド管理クラスから呼び出される。
        /// カメラの視錘台との当たり判定をここでプログラミングする
        /// </summary>
        /// <note>※補足
        ///  カメラとオブジェクトの判定をして、対象オブジェクトを半透明化することもできる。
        ///  m_transparencyを1．0より小さくし、m_priorityのd3Translucentに1の値をセット
        ///  すれば良い。
        /// </note>
        /// <param name="baseCamera"></param>
        /// <param name="z">Z値を返す</param>
        /// <returns>衝突している場合はtrueを返す。trueを返すことによって描画される。</returns>
        internal override bool CameraCollison(MCBaseCamera baseCamera, out float z)
        {
            z = 0;
            if (IsBillbord)
            {
                // ビルボードである(使用できない
                return true;
            }
            else
            {
                // スプライトである
            }
            return true;
        }

        /// <summary>
        /// カメラからの距離を取得する。
        /// </summary>
        /// <returns>カメラからの距離を取得する。</returns>
        internal override float GetZValueFromCamera() { return 0.0f; }

        /// <summary>
        /// カメラからの距離をセットする。
        /// </summary>
        /// <param name="fZ">カメラ化の距離</param>
		internal override void SetZValueFromCamera(float fZ) { return; }

        /// <summary>
        /// メンバー変数から構築する
        /// </summary>
        internal void Build()
        {
            Is3D = false;
        }
        #endregion

    }
}
