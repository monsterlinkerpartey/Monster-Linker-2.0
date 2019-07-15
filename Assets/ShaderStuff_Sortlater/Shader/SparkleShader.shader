Shader "Custom/SparkleShader"
{
    Properties
    {
        _Tint ("Tint", color) = (0.5, 0.5, 0.5, 1)
		_ShadowColor ("Shadow Color", color) = (0,0,0,1)

		_NoiseTex("Noise Texture", 2D) = "white" {}
		_NoiseSize ("Noise Size", float) = 2
		_ShiningSpeed("Shining Speed", float) = 0.1
		_SparkleColor("Sparkle Color", color) = (1,1,1,1)
		_SparklePower ("Sparkle Power", float) = 10

		_Specular ("Specular", range(0,1)) = 0.5
		_Gloss ("Gloss", range(0,1)) = 0.5

		_RimColor ("RimColor", Color) = (0.17, 0.36, 0.81, 0.0)
		_RimPower ("Rim Power", range(0.6,36.0)) = 8.0
		_RimIntensity ("Rim Intensity", Range(0.0, 100.0)) = 1.0

		_SpecSparkleRate("Specular Sparkle Rate", float) = 6
		_RimSparkleRate("Rim Sparkle Rate", float) = 10
		_DiffSparkleRate ("Diffuse Sparkle Rate", float) = 1

		_ParallaxMap ("Parallax Map", 2D) = "white" {}
		_HeightFactor("Height Scale", range(-1,1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			Tags{ "LightMode" = "ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fwdadd_fullshadows

			float4 _Tint;
			float4 _ShadowColor;
							
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			float _NoiseSize;
			float _ShiningSpeed;
			float4 _SparkleColor;
			float _SparklePower;
							
			float _Specular;
			float _Gloss;
							
			float4 _RimColor;
			float _RimPower;
			float _RimIntensity;
							
			float _SpecSparkleRate;
			float _RimSparkleRate;
			float _DiffSparkleRate;

			sampler2D _ParallaxMap;
			float4 _ParallaxMap_ST;
			float _HeightFactor;

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
				float4 tangent : TANGENT;
            };

            struct v2f
            {
				float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
				float4 posWorld : TEXCORD1;
				float3 normalDir : TEXCOORD2;
				float3 lightDir : TEXCOORD3;
				float3 viewDir : TEXCOORD4;
				float3 lightDir_tangent : TEXCOORD5;
				float3 viewDir_tangent : TEXCOORD6;
				LIGHTING_COORDS(7,8)
            };

            //calculate parallax uv offset 
			inline float2 CalculateParallaxUV(v2f i, float heightMulti)
			{
				float height = tex2D(_ParallaxMap, i.uv).r;
				//normalize view Direction
				float3 viewDir = normalize(i.lightDir_tangent);

				float2 offset = i.lightDir_tangent.xy * height * _HeightFactor * heightMulti;
				return offset;
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);

				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - o.posWorld.xyz);
				o.lightDir = normalize(_WorldSpaceLightPos0.xyz);

				TANGENT_SPACE_ROTATION;
				o.lightDir_tangent = normalize(mul(rotation, ObjSpaceLightDir(v.vertex)));
				o.viewDir_tangent = normalize(mul(rotation, ObjSpaceViewDir(v.vertex)));

				TRANSFER_VERTEX_TO_FRAGMENT (o)

                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				i.normalDir = normalize(i.normalDir);

				//attenuation
				float attenuation = LIGHT_ATTENUATION(i);
				float3 attenColor = attenuation * _LightColor0.xyz;

				//specular
				float specularPow = exp2((1 - _Gloss) * 10.0 + 1.0);
				float3 specularColor = float4 (_Specular,_Specular,_Specular,1);

				float3 halfVector = normalize(i.lightDir + i.viewDir);
				float3 directSpecular = pow(max(0, dot(halfVector, i.normalDir)), specularPow) * specularColor;
				float3 specular = directSpecular * attenColor;

				//sparkle
				float2 uvOffset = CalculateParallaxUV(i, 1);
				float  noise1 = tex2D(_NoiseTex, i.uv * _NoiseSize + float2(0, _Time.x * _ShiningSpeed) + uvOffset).r;
				float  noise2 = tex2D(_NoiseTex, i.uv* _NoiseSize * 1.4 + float2(_Time.x * _ShiningSpeed, 0) + uvOffset).r;
				float  sparkle1 = pow(noise1 * noise2 * 2, _SparklePower);

				uvOffset = CalculateParallaxUV(i, 2);
				noise1 = tex2D(_NoiseTex, i.uv * _NoiseSize + float2(0.3, _Time.x * _ShiningSpeed) + uvOffset).r;
				noise2 = tex2D(_NoiseTex, i.uv * _NoiseSize * 1.4 + float2(_Time.x * _ShiningSpeed, 0.3) + uvOffset).r;
				float sparkle2 = pow(noise1 * noise2 * 2, _SparklePower);

				uvOffset = CalculateParallaxUV(i, 3);
				noise1 = tex2D(_NoiseTex, i.uv * _NoiseSize + float2(0.6, _Time.x * _ShiningSpeed) + uvOffset).r;
				noise2 = tex2D(_NoiseTex, i.uv * _NoiseSize * 1.4 + float2(_Time.x * _ShiningSpeed, 0.6) + uvOffset).r;
				float sparkle3 = pow(noise1 * noise2 * 2, _SparklePower);

				//diffuse
				float NdotL = saturate(dot(i.normalDir, i.lightDir));
				float3 directDiffuse = NdotL * attenColor;
				float3 diffuseCol = lerp(_ShadowColor, _Tint, directDiffuse);

				//Rim
				float rim = 1.0 - max(0, dot(i.normalDir, i.lightDir));
				fixed3 rimCol = _RimColor.rgb * pow(rim, _RimPower) * _RimIntensity;

				//final color
				fixed3 sparkleCol1 = sparkle1 * (specular * _SpecSparkleRate + directDiffuse * _DiffSparkleRate + rimCol * _RimSparkleRate) * lerp(_SparkleColor, fixed3(1, 1, 1), 0.5);
				fixed3 sparkleCol2 = sparkle2 * (specular * _SpecSparkleRate + directDiffuse * _DiffSparkleRate + rimCol * _RimSparkleRate) * _SparkleColor;
				fixed3 sparkleCol3 = sparkle3 * (specular * _SpecSparkleRate + directDiffuse * _DiffSparkleRate + rimCol * _RimSparkleRate) * 0.5 * _SparkleColor;

                fixed4 finalCol = fixed4(diffuseCol + specular + sparkleCol1 + sparkleCol2 + sparkleCol3 + rimCol, 1);
                return finalCol;
            }
            ENDCG
        }
    }
}
