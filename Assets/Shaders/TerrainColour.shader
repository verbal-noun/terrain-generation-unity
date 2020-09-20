 Shader "Custom/NewSurfaceShader"
{
    Properties
    {   
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200


        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard noshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // Maximum layer count 
        const static int maxLayerCount = 8;
        // Value to ensure very low heights work 
        const static float epsilion = 1E-4; 
        // Number of available terrain layers 
        int layerCount; 
        // Base colours 
        float3 baseColours[maxLayerCount];
        // The start of different layers 
        float baseStartHeights[maxLayerCount];
        // Strength of colour over texture 
        float baseColourStrength[maxLayerCount];
        // Texture size 
        float baseTextureScales[maxLayerCount];

        // Values to control the blend 
        float baseBlends[maxLayerCount];

        // Values to dictate the max and mix heights of our terrain     
        float minHeight; 
        float maxHeight;

        // Texture 
        sampler2D testTexture;
        float testScale;

        UNITY_DECLARE_TEX2DARRAY(baseTextures);

        struct Input
        {
            // Getting world input 
            float3 worldPos;
            float3 worldNormal;
        };

        // half _Glossiness;
        // half _Metallic;
        // fixed4 _Color;

        
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float inverseLerp(float min, float max, float val) {
            return saturate((val - min) / (max - min));
        }

        // A function to calculate the texture colour 
        float3 triplanar(float3 worldPos, float scale, float3 blendAxes, int textureIndex) {
			float3 scaledWorldPos = worldPos / scale;
			float3 xProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, float3(scaledWorldPos.y, scaledWorldPos.z, textureIndex)) * blendAxes.x;
			float3 yProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, float3(scaledWorldPos.x, scaledWorldPos.z, textureIndex)) * blendAxes.y;
			float3 zProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, float3(scaledWorldPos.x, scaledWorldPos.y, textureIndex)) * blendAxes.z;
			return xProjection + yProjection + zProjection;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {   
            // Extracting the height of the point in the world 
            float heightPercent  = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
            // The normals of textures 
            float3 blendAxes = abs(IN.worldNormal);
            blendAxes /= blendAxes.x + blendAxes.y + blendAxes.z;

            // For each layer of the texture calculate base colour and texture colour 
            for(int i = 0; i < layerCount; i++) {
                // Set the colour according to height 
                float drawStrength = inverseLerp(-baseBlends[i]/2-epsilion, baseBlends[i]/2,
                 heightPercent - baseStartHeights[i]);

                // Calculating the base colour and texture colour 
                float3 baseColour = baseColours[i] * baseColourStrength[i];
				float3 textureColour = triplanar(IN.worldPos, baseTextureScales[i], blendAxes, i) * (1-baseColourStrength[i]);

                // Outputting the value 
				o.Albedo = o.Albedo * (1-drawStrength) + (baseColour + textureColour) * drawStrength;
            }  

        }
        ENDCG
        
    }
    FallBack "Diffuse"
}
