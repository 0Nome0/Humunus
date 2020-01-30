Shader "NerShader/ColorSelect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color_R ("Color_R", Color) = (1,1,1,1)
        _Color_G ("Color_G", Color) = (1,1,1,1)
        _Color_B ("Color_B", Color) = (1,1,1,1)
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
            fixed4 _Color_R;
            fixed4 _Color_G;
            fixed4 _Color_B;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                fixed r = col.r;
                fixed g = col.g;
                fixed b = col.b;
                fixed a = step(col.a, 0);

                fixed w = (r + g + b) / 3;
                col = fixed4(0, 0, 0, 0);
                col += _Color_R * r / w;
                col += _Color_G * g / w;
                col += _Color_B * b / w;
                col.a -= col.a * a;

                return col;
            }
            ENDCG
        }
    }
}
