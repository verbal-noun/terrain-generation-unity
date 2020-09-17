// Implementation of the Phong Illumination Model 

Shader "PhongShader" {

    Properties {
        // Colour of our object - White by default 
        _Colour ("Colour", Color) = (1, 1, 1, 1)
        
        // Shininess 
        _Shininess ("Shininess", Float) = 10 

        // Texture 
        _Tex ("Pattern", 2D) = "white" {} 
    }

    SubShader {
        // No Transparent objects will be rendered 
        Tags { "RenderType" = "Opaque"}
        // Level of Detail 
        LOD 200 

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
    }
}
