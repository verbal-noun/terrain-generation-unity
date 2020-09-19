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
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        const static int maxColourCount = 8; 
        const static float epsilion = 1E-4; 
        int baseColourCount; 
        float3 baseColours[maxColourCount];
        float baseStartHeights[maxColourCount];

        // Values to control the blend 
        float baseBlends[maxColourCount];

        // Values to dictate the max and mix heights of our terrain     
        float minHeight; 
        float maxHeight;

        sampler2D _MainTex;

        struct Input
        {
            // Getting world input 
            float3 worldPos;
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float inverseLerp(float min, float max, float val) {
            return saturate((val - min) / (max - min));
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float heightPercent  = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
            for(int i = 0; i < baseColourCount; i++) {
                // Set the colour according to height 
                //float drawStrength = saturate(sign(heightPercent - baseStartHeights[i]));
                float drawStrength = inverseLerp(-baseBlends[i]/2, baseBlends[i]/2,
                 heightPercent - baseStartHeights[i]);
                o.Albedo = o.Albedo * (1 - drawStrength) + baseColours[i] * drawStrength;
            }  
        }
        ENDCG
    }
    FallBack "Diffuse"
}
