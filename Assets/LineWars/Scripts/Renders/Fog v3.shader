Shader "Custom/FogV3"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Main Texture", 2D) = "white"{}
        [NoScaleOffset] _FogTex ("FogTexture", 2D) = "white" {}
        [NoScaleOffset] _FogEff ("FogEffects", 2D) = "white"{}
        [NoScaleOffset] _Noise ("Noise", 2D) = "black"{}
        _Cutoff ("Cutof", Range(0,1)) = 0.3
        _EdgeColor ("EdgeColor", Color) = (1,1,1,1)
        _FogColor ("FogColor", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Cull Off
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            fixed4 _FogColor;
            fixed4 _EdgeColor;

            sampler2D _MainTex;
            sampler2D _FogTex;
            sampler2D _FogEff;
            sampler2D _VisibilityMaskV3;
            sampler2D _Noise;

            float _Cutoff;

            v2f vert(appdata v)
            {
                v2f o;
                o.uv = v.uv;
                o.color = v.color;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);

                const fixed maskVal = tex2D(_VisibilityMaskV3, i.uv).a;
                const fixed noise = tex2D(_Noise, i.uv).r;

                const fixed maskNoise = clamp(maskVal - noise * pow(1.0f - maskVal, 0.001) * noise, 0, 1);
                if (maskNoise < _Cutoff)
                {
                    fixed4 fog = tex2D(_FogTex, i.uv);
                    fixed4 fogEff = tex2D(_FogEff, i.uv);
                    tex = lerp(_FogColor * fog * fogEff, _EdgeColor, maskNoise / _Cutoff);
                }
                return tex;
            }
            ENDCG
        }
    }
}