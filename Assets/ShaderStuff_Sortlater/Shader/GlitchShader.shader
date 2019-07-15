Shader "Custom/GlitchShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float nrand(float2 uv)
			{
				return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
			}

			bool glitchRand(float2 uv, float timeScale, float offset, float probability)
			{
				return nrand(uv + floor(sin(_Time.y * timeScale) + timeScale * _Time.y) + offset) < probability;
			}

			struct Input
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 position : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(Input v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.position);
				o.uv = v.uv;
				return o;
			}

			float2 verticalBands(float2 uv, float bandCount, bool enabled)
			{
				if (enabled)
				{
					uv.y = floor(uv.y * bandCount) / bandCount;
					return uv;
				}
				else
				{
					return uv;
				}
			}

			float2 bandOffset(float2 uv, float bandCount, float amount, bool enabled)
			{
				if (enabled)
				{
					uv.x += sin(floor(uv.y * bandCount)) * amount;
					return uv;
				}
				else
				{
					return uv;
				}
			}

			fixed4 chromaticAbberation(float2 uv, bool enabled)
			{
				if (enabled)
				{
					fixed r = tex2D(_MainTex, uv + float2(0.00, 0)).r;
					fixed g = tex2D(_MainTex, uv + float2(0.05, 0)).g;
					fixed b = tex2D(_MainTex, uv + float2(-0.05, 0)).b;
					return fixed4(r, g, b, 1);
				}
				else
				{
					return tex2D(_MainTex, uv);
				}
			}

			fixed4 channelSwizzle(fixed4 col, bool enabled)
			{
				if (enabled)
				{
					return col.brga;
				}
				else
				{
					return col;
				}
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				float2 randomUV = floor(i.uv * float2(100,30)) /float2(40,80);

				uv = verticalBands(uv, 10, glitchRand(randomUV, 8, 524, 0.1));
				uv = bandOffset(uv, 10, 0.6, glitchRand(randomUV, 5, 54, 0.2));
				fixed4 col = chromaticAbberation(uv, glitchRand(randomUV, 8, 524, 0.2));
				col = channelSwizzle(col, glitchRand(randomUV, 8, 524, 0.2));
				return col;
			}
			ENDCG
		}
	}
}