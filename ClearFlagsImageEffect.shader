Shader "Hidden/ClearFlagsImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PrevFrame("Previous Frame", 2D) = "black" {}
		_MaxTransparency("Max Transparency", Float) = 1.0
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
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _PrevFrame;
			float _MaxTransparency;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 prev = tex2D(_PrevFrame, i.uv);
				// if they've set max trans to 0, it means they want full transparency suport
				// otherwise they have set the smallest transparency they expect
				if ((_MaxTransparency == 0 && col.a == 0) || col.a < _MaxTransparency) {
					col = prev;
				}
				return col;
			}
			ENDCG
		}
	}
}
