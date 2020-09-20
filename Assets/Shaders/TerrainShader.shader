 Shader "Custom/TerrainShader"
{
    Properties
    {   
        
    }
        SubShader
    {

        Pass {
            // For our sun light source 
            Tags { "LightMode" = "ForwardBase"}

        CGPROGRAM 
            #pragma vertex vert
            #pragma fragment frag 

            // Importing light, camera information 
            #include "UnityCG.cginc"

            // UnityCG 
            uniform float4 _LightColor0;

            //Used for texture
            sampler2D _Tex; 
            //For tiling 
            float4 _Tex_ST; 

            uniform float4 _Colour;
            uniform float4 _SpecColour;
            uniform float _Shininess;

            struct vertIn
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL; 
                float2 uv : TEXCOORD0;
            
            };

            struct vertOut
            {
                float4 pos : POSITION;
                float3 normal : NORMAL; 
                float2 uv: TEXCOORD0; 
                float4 posWorld : TEXCOORD1; 
            }; 

            // Transforming the position of vertices 
            vertOut vert(vertIn v)
            {
                vertOut o; 
                // Calculating the world positon of our object 
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                //Calculating the normal 
                o.normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                // Updating the position 
                o.pos = UnityObjectToClipPos(v.vertex);
                // UVs for mapping the texture 
                o.uv = TRANSFORM_TEX(v.uv, _Tex);
                return o; 
            }

            // Updating the colour 
            fixed4 frag(vertOut i) : COLOR 
            {
                float3 normalDirection = normalize(i.normal);
                // Our eye will be the main camera 
                float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);

                float3 vert2LightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
                float oneOverDistance = 1.0 / length(vert2LightSource);
                float3 lightDirection = _WorldSpaceLightPos0.xyz - i.posWorld.xyz * _WorldSpaceLightPos0.w;

                //Attenunation 
                float attenuation = lerp(1.0, oneOverDistance, _WorldSpaceLightPos0.w); 

                // Ambient component 
                float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Colour.rgb;
                // Diffuse component 
                float3 diffuseReflection = _LightColor0.rgb * _Colour.rgb * 
                max(0.0, dot(normalDirection, lightDirection)) * attenuation;   

                // Calculating colour based on the three components 
                float3 color = (ambientLighting + diffuseReflection) * tex2D(_Tex, i.uv);
                return float4(color, 1.0);
            }
        
        ENDCG
        
    }

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
