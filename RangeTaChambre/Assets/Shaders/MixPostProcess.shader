﻿Shader "Custom/PostRendering/Mix"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MixTex("MixTex", 2D) = "White" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
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
			sampler2D _MixTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 mixCol = tex2D(_MixTex, i.uv);
				// just invert the colors
				col.rgb = col.rgb * mixCol.rgb;
				return col;
			}
			ENDCG
		}
	}
}
