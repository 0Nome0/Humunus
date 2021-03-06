﻿Shader "NerShader/Filled"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount("FillAmount",Range(0,1)) = 0
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
		{
			"Queue" = "Transparent"
		}

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
            float _FillAmount;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv, _MainTex_ST.xy, _MainTex_ST.zw);
                col.a = step(0, _FillAmount - i.uv.x);
                col.rgb *= _Color.rgb;
                return col;
            }
            ENDCG
        }
    }
}