using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.Camera;

namespace UtilSharpDX.DrawingCommand
{
    /// <summary>
    /// 描画処理するクラスすべてに派生して使う
    /// </summary>
    public abstract class MCDrawBase
    {
        /// <summary>
        ///  MCDrawCommandPriority構造体の一部のパラメーターを使う
        /// </summary>
        private MCDrawCommandPriority m_priority;

        /// <summary>
        /// テクニック
        /// </summary>
        public int Technique { get { return (int)m_priority.Technique; } set { m_priority.Technique = (byte)value; } }

        /// <summary>
        /// D3　G,B,T,M
        /// </summary>
        public int D3State { get { return (int)m_priority.D3State; } set { m_priority.D3State = (byte)value; } }

        /// <summary>
        /// D3　描画優先順位
        /// </summary>
        public int D3No { get { return (int)m_priority.D3No; } set { m_priority.D3No = (byte)value; } }

        /// <summary>
        /// D3　D3　タイプ（ボーンなど）のフラグ
        /// </summary>
        public int D3TypeFlags { get { return (int)m_priority.D3TypeFlags; } set { m_priority.D3TypeFlags = (byte)value; } }

        /// <summary>
        /// D3　半透明か？
        /// </summary>
        public bool IsD3Translucent { get { return m_priority.D3Translucent; } set { m_priority.D3Translucent = value; } }

        /// <summary>
        /// D3　半透明か？
        /// </summary>
        public int Effect { get { return (int)m_priority.Effect; } set { m_priority.Effect = (byte)value; } }

        /// <summary>
        /// D2　描画順
        /// </summary>
        public int D2No { get { return (int)m_priority.D2No; } set { m_priority.D2No = (uint)value; } }

        /// <summary>
        /// D2　スプライト処理タイプの番号
        /// </summary>
        public int D2RenderType { get { return (int)m_priority.D2RenderType; } internal set { m_priority.D2RenderType = (ulong)value; } }

        /// <summary>
        /// 3Dか？
        /// </summary>
        /// <returns></returns>
        public bool Is3D { get { return m_priority.D3; } set { m_priority.D3 = value; } }

        /// <summary>
        /// 2Dか？
        /// </summary>
        /// <returns></returns>
        public bool Is2D { get { return !m_priority.D3; } set { m_priority.D3 = !value; } }


        /// <summary>
        /// MCDrawCommandPriority
        /// </summary>
        /// <returns></returns>
        public MCDrawCommandPriority DrawCommandPriority { get { return m_priority; } }




        /// <summary>
        /// 描画時に呼び出されるスプライト処理
        /// </summary>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="render">スプライトのレンダー</param>
        /// <param name="bd"></param>
        /// <returns></returns>
        internal abstract int CallDrawingSprite(
            double totalTime,
            float elapsedTime,
            IMCSpriteRender render,
            MCCallBatchDrawing bd
        );



        /// <summary>
        /// 描画時に呼び出される３D処理
        /// </summary>
        /// <param name="immediateContext"></param>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="oldPriority">1つ前のプライオリティ</param>
        /// <param name="priority">プライオリティ</param>
        /// <param name="cbd"></param>
        /// <param name="callNum">レンダーが呼ばれた回数　0～</param>
        /// <param name="passCount">エフェクトパス 0～</param>
        /// <returns></returns>
        internal abstract int CallDraw3D(
            DeviceContext immediateContext,
            double totalTime,
            float elapsedTime,
            MCDrawCommandPriority oldPriority,
            MCDrawCommandPriority priority,
            MCCallBatchDrawing cbd,
            int callNum,
            int passCount
        );

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
        internal abstract bool CameraCollison(MCBaseCamera baseCamera, out float z);

        /// <summary>
        /// カメラからの距離を取得する。
        /// </summary>
        /// <returns>カメラからの距離を取得する。</returns>
		internal abstract float GetZValueFromCamera();

        /// <summary>
        /// カメラからの距離をセットする。
        /// </summary>
        /// <param name="fZ">カメラ化の距離</param>
		internal abstract void SetZValueFromCamera(float fZ);


        /// <summary>
        /// 描画コマンドで使用する。プライオリティーを取得する
        /// </summary>
        /// <returns>プライオリティーを取得する</returns>
		internal MCDrawCommandPriority GetPriority() { return m_priority; }

        /// <summary>
        /// 派生したクラスなどを示す任意のidを返す
        /// </summary>
        /// <returns>idを返す</returns>
		public abstract Guid GetID();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCDrawBase()
        { m_priority.Init(); }
    };
}
