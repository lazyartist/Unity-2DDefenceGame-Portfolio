﻿Shader "2D/FadeInOutShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Timing("Timing", float) = 0
		_TimeOffset("Time Offset", float) = 0
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _Color;
			float _Timing;
			float _TimeOffset;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				//col.rgb = 1 - col.rgb;
				//col.a *= _Time.w;
				col.a *= _SinTime.a * _Timing + _TimeOffset;
				col *= _Color;
				return col;
			}
			ENDCG
		}
	}
}
