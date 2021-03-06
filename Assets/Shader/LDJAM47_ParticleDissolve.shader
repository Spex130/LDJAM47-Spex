﻿Shader "LDJAM47/LDJAM47_ParticleDissolve"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [MaterialToggle] _EmissionOverride("Override with Emissive", Float) = 0
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _AlphaDissolve("Alpha Dissolve", Range(0, 1)) = 0
        [MaterialToggle] _IsParticleMaterial ("Is Particle Material", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha One
        Cull Off 
        ZWrite Off
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float4 color : COLOR;
            float2 uv_MainTex;
        };

        fixed4 _Color;
        float _AlphaDissolve;
        float _EmissionOverride;
        float _IsParticleMaterial;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float alphaValue = _IsParticleMaterial > 0 ? 1- IN.color.r - .2  : _AlphaDissolve;
            clip(c.r - alphaValue);

            float3 finalColor = _EmissionOverride > 0 ? c.rgb : _Color;

            o.Albedo = c.rgb;
            o.Emission = _Color * _EmissionOverride * (c.a + alphaValue);
            o.Alpha = c.a + alphaValue;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
