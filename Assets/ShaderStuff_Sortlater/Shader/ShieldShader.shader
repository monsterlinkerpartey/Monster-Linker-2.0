Shader "Custom/ShieldShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ShieldColorLight("Shield Color Light", color) = (0.5, 0.5, 1, 1)
		_ShieldColorDark("Shield Color Dark", color) = (0.5, 0.5, 1, 1)
		_CloudsTex("Clouds Texture", 2D) = "black" {}
		_CloudsIntensity("Clouds Intensity", range(0.0,1.0)) = 0.5
		_FresnelParameter("Fresnel", vector) = (0,1,1,0)
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 100

			Zwrite Off
			Blend One OneMinusSrcAlpha
			Cull Off

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
				float3 normal : NORMAL;
				
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			half4 _ShieldColorLight;
			half4 _ShieldColorDark;

			sampler2D _CloudsTex;
			float4 _CloudsTex_ST;
			float _CloudsIntensity;
			float4 _FresnelParameter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
				o.normal = mul(UNITY_MATRIX_M, v.normal);
                o.uv = v.uv;
                return o;
            }

			float getFresnel(float3 worldPos, float3 normal)
			{
				float3 viewDirection = normalize(worldPos - _WorldSpaceCameraPos);
				normal = normalize(normal);
				float nDotV = dot(normal, viewDirection);
				float fresnel = clamp(1 - abs(nDotV), 0.0, 1.0);
				return lerp(_FresnelParameter.x, _FresnelParameter.y, pow(fresnel, _FresnelParameter.z));
			}

			half4 frag(v2f i) : SV_Target
			{
				float fresnel = getFresnel(i.worldPos, i.normal);
                // sample the texture
                half4 texCol = tex2D(_MainTex, i.uv * _MainTex_ST.xy + _MainTex_ST.zw);
				half4 cloudsCol = tex2D(_CloudsTex, i.uv * _CloudsTex_ST.xy + _CloudsTex_ST.zw * _Time.y);

				half4 col = lerp(_ShieldColorDark, _ShieldColorLight, texCol.r + cloudsCol.r * _CloudsIntensity * texCol.a);
				col.a = texCol.a;
				return col * fresnel;
            }
            ENDCG
        }
    }
}
