Shader "LDJAM47/CharacterShader_Alpha"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, .5)
        [MaterialToggle] _UseTexture("Use Texture", Float) = 0 
        _FresnelColor("Fresnel Color", Color) = (1, 1, 1, .5)
        [PowerSlider(4)] _FresnelPower ("Fresnel Power", Range(0.1, 8)) = 1
        [MaterialToggle] _EnableFresnel("Enable Fresnel", Float) = 0 
        [MaterialToggle] _EnableAlpha("Enable Alpha", Float) = 0 
        _Emissive("Emissive", Range(0,1)) = 0
    }
    SubShader {
		Tags{ "RenderType"="TransparentCutout" "Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
        CGPROGRAM
        #pragma surface surf Lambert alpha
        
        struct Input {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 viewDir;
            INTERNAL_DATA
        };
        
        sampler2D _MainTex;
        float4 _Color;
        float4 _FresnelColor;
        float _FresnelPower;
        float _UseTexture;
        float _EnableFresnel;
        float _EnableAlpha;
        float _Emissive;
        
        void surf (Input IN, inout SurfaceOutput o) {

            float fresnelCalc = dot(IN.worldNormal, IN.viewDir);
            fresnelCalc = saturate(1 - fresnelCalc);
            fresnelCalc = pow(fresnelCalc, _FresnelPower);
            //fresnelCalc 
            float3 fresnelColor = fresnelCalc * _FresnelColor;

            o.Albedo = _UseTexture < 1 ? _Color : tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
            o.Emission = _UseTexture < 1 ? _Color * _Emissive : tex2D(_MainTex, IN.uv_MainTex).rgb * _Emissive;
            o.Emission += fresnelColor * _EnableFresnel;
            o.Alpha = _EnableAlpha < 1 ? 1 : tex2D(_MainTex, IN.uv_MainTex).a;
        }
        ENDCG
    }
    Fallback "Diffuse"
}