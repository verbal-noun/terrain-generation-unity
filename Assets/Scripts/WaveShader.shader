//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/WaveShader"
{
	Properties
	{
		_Color("Colour", Color) = (0,0,0,1)
		 
	}
	SubShader
	{
		Tags{
			"RenderType" = "transparent"
		}
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float4 _Color;

			struct vertIn
			{
				float4 vertex : POSITION;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				// Displace the original vertex in model space
				
				
				float4 newOutputVertex = v.vertex;
				float4 displacement = float4(0.0f, 0.5 * sin(newOutputVertex.x + _Time.z), 0.0f, 0.0f);
				newOutputVertex += displacement;
				newOutputVertex = mul(UNITY_MATRIX_MVP, newOutputVertex);
				vertOut o;
				o.vertex = newOutputVertex;
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : COLOR{
				return _Color;
			}
			ENDCG
		}
	}
}