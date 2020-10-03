Shader "LDJAM47/Smokeball" {
	//show values to edit in inspector
	Properties {
		_Color ("Tint", Color) = (0, 0, 0, 1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_OverlayTex ("Overlay Texture", 2D) = "white" {}
		_Smoothness ("Smoothness", Range(0, 1)) = 0
		_Metallic ("Metalness", Range(0, 1)) = 0
		[HDR] _Emission ("Emission", color) = (0,0,0)
		_ScrollSpeed1 ("Sroll Speed Layer 1", Range(0.1, 5)) = 1
		_ScrollSpeed2 ("Sroll Speed Layer 2", Range(0.1, 5)) = 1
		_FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
		_FresnelBooster ("FresnelBoost", Range(0, 1)) = 0
		[PowerSlider(4)] _FresnelExponent ("Fresnel Exponent", Range(0.25, 4)) = 1
		[MaterialToggle] _IsParticleMaterial ("Is Particle Material", Float) = 0
		_AlphaDissolve("Alpha Dissolve", Range(0, 1)) = 0
	}
	SubShader {

        Tags { "Queue"="Transparent" "RenderType"="Transparent" "DisableBatching"="True" }
        Blend SrcAlpha OneMinusSrcAlpha 

		CGPROGRAM

		//the shader is a surface shader, meaning that it will be extended by unity in the background to have fancy lighting and other features
		//our surface shader function is called surf and we use the standard lighting model, which means PBR lighting
		//fullforwardshadows makes sure unity adds the shadow passes the shader might need
		#pragma surface surf Standard fullforwardshadows alpha
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _OverlayTex;
		fixed4 _Color;

		half _Smoothness;
		half _Metallic;
		half3 _Emission;

		float3 _FresnelColor;
		float _FresnelExponent;
		float _FresnelBooster;

		float _ScrollSpeed1;
		float _ScrollSpeed2;

		float _IsParticleMaterial;
		float _AlphaDissolve;

		//input struct which is automatically filled by unity
		struct Input {
			float4 color : COLOR;
			float2 uv_MainTex;
			float2 uv_OverlayTex;
			float3 worldNormal;
			float3 viewDir;
			INTERNAL_DATA
		};

		//the surface shader function which sets parameters the lighting function then uses
		void surf (Input i, inout SurfaceOutputStandard o) {
			//sample and tint albedo texture
			fixed4 col = tex2D(_MainTex, float2(i.uv_MainTex.x + (_Time.x * _ScrollSpeed1 * 2), i.uv_MainTex.y + (_Time.x * _ScrollSpeed1) ));
			fixed4 col2 = tex2D(_OverlayTex, float2(i.uv_OverlayTex.x + (_Time.x * _ScrollSpeed2 * 2), i.uv_OverlayTex.y + (_Time.x * _ScrollSpeed2) ));

			//just apply the values for metalness and smoothness
			o.Metallic = 0;
			o.Smoothness = 0;

			//get the dot product between the normal and the view direction
			float fresnel = dot(i.worldNormal, i.viewDir);
			//invert the fresnel so the big values are on the outside
			fresnel = saturate(1 - fresnel  + _FresnelBooster);
			//raise the fresnel value to the exponents power to be able to adjust it
			fresnel = pow(fresnel, _FresnelExponent);

			o.Albedo = saturate(col.rgb * col2.r + col2.r + fresnel);
			o.Albedo *= _Color;

			float alphaValue = _IsParticleMaterial > 0 ? 1- i.color.r - .2 : _AlphaDissolve;

			clip(col2.r - alphaValue);

			//combine the fresnel value with a color
			float3 fresnelColor = fresnel * _FresnelColor;
			//apply the fresnel value to the emission
			o.Emission = (_Emission + fresnelColor) * alphaValue;
			o.Alpha = 1- alphaValue;
		}
		ENDCG
	}
	FallBack "Standard"
}

