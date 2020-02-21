Shader "2D/GrayscaleImageEffectShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Intensity("Intensity", Range(0,1)) = 0.5
		_IsGrayscale("IsGrayscale", Range(0,1)) = 0.0
	}

		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
				blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag 

				#include "UnityCG.cginc"

				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f {
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v) {
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				sampler2D _MainTex;
				float _Intensity;

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					col.rgb = (col.r + col.g + col.b) / 3 * _Intensity;
					return col;
				}
				ENDCG
			}
		}
}
