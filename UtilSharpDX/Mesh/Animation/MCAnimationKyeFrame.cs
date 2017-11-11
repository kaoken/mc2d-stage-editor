using SharpDX;
using System.Collections.Generic;
using UtilSharpDX.Math;

namespace UtilSharpDX.Mesh.Animation
{
    /// <summary>
    /// クォータニオンキー構造体
    /// </summary>
    public struct MC_KEY_QUATERNION
    {
        /// <summary>
        /// 時間
        /// </summary>
        public float totalTime;
        /// <summary>
        /// クォータニオン
        /// </summary>
        public MCQuaternion Value;
    }

    /// <summary>
    /// 2属性ベクトル構造体
    /// </summary>
    public struct MC_KEY_VECTOR2
    {
        /// <summary>
        /// 時間
        /// </summary>
        float totalTime;
        /// <summary>
        ///  2属性ベクトル
        /// </summary>
        MCVector2 Value;
    }
    /// <summary>
    /// ３属性ベクトル構造体
    /// </summary>
    public struct MC_KEY_VECTOR3
    {
        /// <summary>
        /// 時間
        /// </summary>
        float totalTime;
        /// <summary>
        /// 3属性ベクトル
        /// </summary>
        MCVector3 Value;
    }

    /// <summary>
    /// マトリクスキー構造体
    /// </summary>
    public struct MC_KEY_MATRIX
    {
        /// <summary>
        /// 時間
        /// </summary>
        float totalTime;
        /// <summary>
        /// マトリクス
        /// </summary>
        MCMatrix4x4 Value;
    }

    /// <summary>
    /// 一つのアニメーションキーを管理
    /// </summary>
    public class MCAnimationKyeFrame
    {
        /// <summary>
        /// アニメーションの名前
        /// </summary>
        public string m_animationName;
        /// <summary>
        /// 対象フレーム名前
        /// </summary>
        public string m_frameName;
        /// <summary>
        /// キー数
        /// </summary>
        public int m_keys;

        /// <summary>
        /// 回転キー数
        /// </summary>
        public int m_RotCnt;
        public List<MC_KEY_QUATERNION> m_rotations;


        /// <summary>
        /// 移動キー数
        /// </summary>
        public int m_TrCnt;
        public List<MC_KEY_VECTOR3> m_translations;

        /// <summary>
        /// スケールキー数
        /// </summary>
        public int m_ScCnt;
        public List<MC_KEY_VECTOR3> m_scalings;

        /// <summary>
        /// マトリクスキー数
        /// </summary>
        public int m_MxCnt;
        public List<MC_KEY_MATRIX> m_matrixs;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCAnimationKyeFrame()
        {
            m_keys = 0;
            m_RotCnt = 0;
            m_TrCnt = 0;
            m_ScCnt = 0;
            m_MxCnt = 0;

        }
    };
}
