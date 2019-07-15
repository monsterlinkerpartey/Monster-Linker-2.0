Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_LightColor("Light Color Color", color) = (1,1,1,1)
		_MediumColor("Medium Color Color", color) = (0.5,0.5,0.5,1)
		_DarkColor("Dark Color", color) = (0,0,0,1)

		_ScrollSpeed("Scroll Speed", vector) = (0,0,0,0)
		_WobbleAmount("Wobble Amount", vector) = (0,0,0,0)

		_Transparency("Transparency", range(0.0, 1.0)) = 0.5

    }
    SubShader
    {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		Zwrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

			fixed4 _LightColor;
			fixed4 _MediumColor;
			fixed4 _DarkColor;
			float4 _ScrollSpeed;
			float4 _WobbleAmount;
			float _Transparency;
			

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				uv *= _MainTex_ST.xy;
				uv += _MainTex_ST.zw;
				uv.x += _Time.y * _ScrollSpeed.x;
				uv.y += _Time.y * _ScrollSpeed.y;
				uv.x += sin(uv.x * _WobbleAmount.x * 1 + _Time.y * _ScrollSpeed.z * 0.652341) * 0.2;
				uv.y += sin(uv.y * _WobbleAmount.y * 1 + _Time.y * _ScrollSpeed.w * 0.412341) * 0.2;
				// sample the texture
			   fixed4 col = tex2D(_MainTex, uv) * _LightColor;

			   float2 uv2 = i.uv;
			   uv2 *= _MainTex_ST.xy;
			   uv2 += _MainTex_ST.zw;
			   uv2.x += _Time.y * _ScrollSpeed.x;
			   uv2.y += _Time.y * _ScrollSpeed.y;
			   uv2.x += sin(uv.x * _WobbleAmount.x * 0.5 + _Time.y * _ScrollSpeed.z * 0.45623313) * 0.2;
			   uv2.y += sin(uv.y * _WobbleAmount.y * 0.5 + _Time.y * _ScrollSpeed.w * 0.23436561) * 0.2;
			   // sample the texture
			   fixed4 col2 = tex2D(_MainTex, uv2) * _MediumColor;
			   
			   fixed4 finalCol = col + col2 + _DarkColor;
			   finalCol.a = _Transparency;

			   return finalCol;
            }
            ENDCG
        }
    }
}
