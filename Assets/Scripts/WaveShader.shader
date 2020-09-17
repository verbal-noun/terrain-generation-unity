//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/WaveShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			uniform sampler2D _MainTex;
			uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};


			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};

			fixed4 _Color;

			// Contains code from workshop - 5 solution 
			vertOut vert(vertIn v)
			{
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
				
				float amplitude = 3.0f;
				float4 displacement = float4(0.0f, amplitude * sin(v.vertex.x + v.vertex.z + _Time.z), 0.0f, 0.0f);
				v.vertex += displacement;

				vertOut o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;

				// Pass out the world vertex position and world normal to be interpolated
				// in the fragment shader (and utilised)
				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;
				return o;
			}
			

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				
				// Our interpolated normal might not be of length 1
				float3 interpNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float Ka = 1;
				float3 amb = _Color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float Kd = 1;
				float3 L;
				float attenuation;
				if (_WorldSpaceLightPos0.w == 0.0){
					//directional light
					L = _WorldSpaceLightPos0;
					attenuation = 1.0;
				}
				else {
					//point light
					L = normalize(_WorldSpaceLightPos0.xyz - v.worldVertex.xyz);
					attenuation = 1.0 / length(_WorldSpaceLightPos0.xyz - v.worldVertex.xyz);
				}


				float LdotN = dot(L, interpNormal);
				float3 dif = attenuation * _LightColor0 * Kd * _Color.rgb * saturate(LdotN);

				// Calculate specular reflections
				float Ks = 1;
				float specN = 5; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);

				specN = 25; // We usually need a higher specular power when using Blinn-Phong
				float3 H = normalize(V + L);
				float3 spe = attenuation * _LightColor0 * Ks * pow(saturate(dot(interpNormal, H)), specN);

				// Combine Phong illumination model components
				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
				returnColor.a = _Color.a;

				return returnColor;
			}
			ENDCG
		}
	}
}