using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainShader : MonoBehaviour {
    // Start is called before the first frame update
    // Variables to standardize texture size & format
    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;
    // Array to hold different textures 
    public TerrainLayer[] layers;

    [System.Serializable]
    public class TerrainLayer {
        public Texture2D texture;
        public Color tint;
        [Range (0, 1)]
        public float tintStrength;
        [Range (0, 1)]
        public float startHeight;
        [Range (0, 1)]
        public float blendStrength;
        public float textureScale;
    }

    Material material;
    void Start () {
        material = this.GetComponent<MeshRenderer> ().material;
    }

    Texture2DArray generateTextureArray (Texture2D[] textures) {
        Texture2DArray textureArray = new Texture2DArray (textureSize, textureSize,
            textures.Length, textureFormat, true);

        for (int i = 0; i < textures.Length; i++) {
            textureArray.SetPixels (textures[i].GetPixels (), i);
        }
        textureArray.Apply ();

        return textureArray;
    }

    public void updateShader (float minHeight, float maxHeight) {

        // Pass max and min height into the material's shader 
        material.SetFloat ("minHeight", minHeight);
        material.SetFloat ("maxHeight", maxHeight);
        material.SetInt ("layerCount", layers.Length);
        material.SetColorArray ("baseColours", layers.Select (x => x.tint).ToArray ());
        material.SetFloatArray ("baseStartHeights", layers.Select (x => x.startHeight).ToArray ());
        material.SetFloatArray ("baseBlends", layers.Select (x => x.blendStrength).ToArray ());
        material.SetFloatArray ("baseColourStrength", layers.Select (x => x.tintStrength).ToArray ());
        material.SetFloatArray ("baseTextureScales", layers.Select (x => x.textureScale).ToArray ());

        Texture2DArray texturesArray = generateTextureArray (layers.Select (x => x.texture).ToArray ());
        material.SetTexture ("baseTextures", texturesArray);
    }
}