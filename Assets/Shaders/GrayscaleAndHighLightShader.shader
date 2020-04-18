Shader "2D/GrayscaleAndHighLightShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_HighLightTex("Texture", 2D) = "white" {}
		_HighLightChannel("HighLight Channel Index", int) = 0
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
				sampler2D _HighLightTex;
				int _HighLightChannel;

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					fixed4 colMask = tex2D(_HighLightTex, i.uv);
					float mask = colMask[_HighLightChannel];
					float gray = (col.r + col.g + col.b) / 3;
					float4 diffCol = col - gray;
					col.r = gray + diffCol.r * mask;
					col.g = gray + diffCol.g * mask;
					col.b = gray + diffCol.b * mask;
					return col;
				}
				ENDCG
			}
		}
}
