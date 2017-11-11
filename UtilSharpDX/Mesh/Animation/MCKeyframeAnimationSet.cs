using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.Math;

namespace UtilSharpDX.Mesh.Animation
{
    /// <summary>
    /// 複数のキーフレームモーションを管理する
    /// </summary>
    public class MCKeyframeAnimationSet
    {
        protected List<MCAnimatnSet> m_vIndexAnimationSet;

        protected Dictionary<string, MCAnimatnSet> m_mapNameAnimationSetsPtr;

        /// <summary>
        /// アニメーションセットの名前
        /// </summary>
		protected string m_name;
        /// <summary>
        /// 期間
        /// </summary>
        protected float m_period;
        /// <summary>
        ///  1秒間に経過するキーフレームティック数
        /// </summary>
        protected float m_ticksPerSecond;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fTicksPerSecond"></param>
        public MCKeyframeAnimationSet(string name, float fTicksPerSecond)
        {

        }
        ~MCKeyframeAnimationSet()
        {

        }

        /// <summary>
        /// 名前
        /// </summary>
        /// <returns></returns>
        public string GetName() { return m_name; }

        /// <summary>
        /// 期間
        /// </summary>
        /// <returns></returns>
        public float GetPeriod() { return m_period; }
        /// <summary>
        /// セットのローカルタイム時間位置を返す
        /// </summary>
        /// <param name="fPosition"></param>
        /// <returns></returns>
        public float GetPeriodicPosition(double fPosition)
        {
            return 0.0f;
        }

        /// <summary>
        /// アニメーションの名前
        /// </summary>
        /// <returns></returns>
        public int GetNumAnimations() { return m_vIndexAnimationSet.Count; }
        public int GetAnimationNameByIndex(int Index, out string name)
        {
            name = "";
            return 0;
        }
        public int GetAnimationIndexByName(string name, out int index)
        {
            index = 0;
            return 0;
        }

        // SRT
        public int GetSRT(
            float periodicPosition,
            int anmIdx,
            MCVector3 scale,
            MCQuaternion rotation,
            MCVector3 position)
        {
            return 0;
        }
        public int GetScaling(
            float periodicPosition,
            int anmIdx,
            MCVector3 scale
            )
        {
            return 0;
        }
        public int GetTranslate(
            float periodicPosition,
            int anmIdx,
            MCVector3 translate
            )
        {
            return 0;
        }
        public int GetRotation(
            float periodicPosition,
            int anmIdx,
            MCQuaternion rotation
            )
        {
            return 0;
        }

        public int RegisterAnimationSRTKeys(
                    string name,
                    List<MC_KEY_VECTOR3> pvScaleKey,
                    List<MC_KEY_QUATERNION> pvRotationKey,
                    List<MC_KEY_VECTOR3> pvPositionKey,
            int pIndex
            )
        {
            return 0;
        }

    }
}
