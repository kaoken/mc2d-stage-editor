using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.D2;

namespace UtilSharpDX.Mesh
{
    public class MCMaterial
    {
        /// <summary>
        /// 拡散光テクスチャー
        /// </summary>
        public MCBaseTexture txDiffuse=null;
        /// <summary>
        /// 法線テクスチャー
        /// </summary>
        public MCBaseTexture txNormal = null;
        /// <summary>
        /// 鏡面反射光テクスチャー
        /// </summary>
        public MCBaseTexture txSpecular = null;

        /// <summary>
        /// 拡散光
        /// </summary>
        public Color4 diffuse = new Color4();
        /// <summary>
        /// 環境光
        /// </summary>
        public Color4 ambient = new Color4();
        /// <summary>
        /// 鏡面反射光
        /// </summary>
        public Color4 specular = new Color4();
        /// <summary>
        /// 自己発光
        /// </summary>
        public Color4 emissive = new Color4();
        /// <summary>
        /// 鏡面反射光のハイライト（これが高くなると鋭くなります。)
        /// </summary>
        float power=1.0f;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCMaterial()
        {

        }
    }
}
