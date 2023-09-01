Shader "Custom/Fog_v3_1"
{
    Properties
    {
        [NoScaleOffset]_MainTex("Color Texture", 2D) = "white" {}
        [NoScaleOffset]_MapTex("Map Texture", 2D) = "white" {}

        [NoScaleOffset]_Noise("Noise", 2D) = "black" {}

        _Cutoff("Map Cutoff", float) = 0.4

        _MapColor("Map Color", Color) = (1,1,1,1)
        _MapEdgeColor("Map Edge Color", Color) = (1,1,1,1)

        [NoScaleOffset] _MapBackground("Map Background Texture", 2D) = "white" {}
    }
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Standard

        struct Input
        {
            float3 worldPos;
            float2 uv_MainTex;
            float2 uv_MapTex;
        };

        float _MapSize;
        sampler2D _VisibilityMaskV3;

        sampler2D _MainTex;
        sampler2D _MapTex;

        sampler2D _Noise;

        float _Cutoff;

        float4 _MapColor;
        float4 _MapEdgeColor;
        sampler2D _MapBackground;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            _MapSize = float2(60,34);
            float4 maskVal = tex2D(_VisibilityMaskV3, IN.worldPos.xy / _MapSize);
            float4 tile = tex2D(_MainTex, IN.uv_MainTex);
            float4 tileMap = tex2D(_MapTex, IN.uv_MapTex);
            float4 mapBackground = tex2D(_MapBackground, IN.worldPos.xy / _MapSize);
            float noise = tex2D(_Noise, IN.worldPos.xy / _MapSize).r;

            float maskNoise = clamp(maskVal - pow(1.0f - maskVal, 0.01f) * noise, 0, 1);

            if (maskNoise < _Cutoff)
                tile = lerp(_MapColor * tileMap * mapBackground, _MapEdgeColor, maskNoise / _Cutoff);

            o.Albedo = tile.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}