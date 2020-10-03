Shader "LDJAM47/CharacterShader"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, .5)
        [MaterialToggle] _UseTexture("Use Texture", Float) = 0 
        _FresnelColor("Fresnel Color", Color) = (1, 1, 1, .5)
        [PowerSlider(4)] _FresnelPower ("Fresnel Power", Range(0.25, 4)) = 1
        [MaterialToggle] _EnableFresnel("Enable Fresnel", Float) = 0 
        _Emissive("Emissive", Range(0,1)) = 0
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Opaque"}
        Blend SrcAlpha OneMinusSrcAlpha 
        CGPROGRAM
        #pragma surface surf Lambert alpha
        
        struct Input {
            float2 uv_MainTex;
            float3 worldNormal;
            float viewDir;
            INTERNAL_DATA
        };
        
        sampler2D _MainTex;
        float4 _Color;
        float4 _FresnelColor;
        float _FresnelPower;
        float _UseTexture;
        float _EnableFresnel;
        float _Emissive;
        
        void surf (Input IN, inout SurfaceOutput o) {

            float fresnelCalc = dot(IN.worldNormal, IN.viewDir);
            fresnelCalc = saturate(1 - fresnelCalc);
            //fresnelCalc 
            float3 fresnelColor = fresnelCalc * _FresnelColor;

            o.Albedo = _UseTexture < 1 ? _Color : tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
            o.Emission = _UseTexture < 1 ? _Color * _Emissive : tex2D(_MainTex, IN.uv_MainTex).rgb * _Emissive;
            o.Emission += fresnelColor * _EnableFresnel;
            o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;
        }
        ENDCG
    }
    Fallback "Diffuse"
}