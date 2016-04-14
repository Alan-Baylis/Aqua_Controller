Shader "Specular Colored" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_GlossTex("Specular (RGB) Shininess (A)", 2D) = "gray" {}
	_BumpMap("Normalmap", 2D) = "bump" {}
	_Shininess("Shininess", Range(0, 10)) = 1
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
		//use custom lighting functions
#pragma surface surf BlinnPhongColor


		//custom surface output structure
	struct SurfaceOutputSpecColor {
		half3 Albedo;
		half3 Normal;
		half3 Emission;
		half Specular;
		half3 GlossColor; //Gloss is now three-channel
		half Alpha;


	};

	//forward lighting function
	inline half4 LightingBlinnPhongColor(SurfaceOutputSpecColor s, half3 lightDir, half3 viewDir, half atten) {
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif
		viewDir = normalize(viewDir);
		half3 h = normalize(lightDir + viewDir);

		half diff = max(0, dot(s.Normal, lightDir));

		float nh = max(0, dot(s.Normal, h));
		float spec = pow(nh, s.Specular*128.0);

		half4 c;
		//Use gloss colour instead of gloss
		c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * s.GlossColor * spec) * (atten * 2);
		//We use gloss luminance to determine its overbright contribution
		c.a = s.Alpha + _LightColor0.a * Luminance(s.GlossColor) * spec * atten;
		return c;
	}

	//deferred lighting function
	inline half4 LightingBlinnPhongColor_PrePass(SurfaceOutputSpecColor s, half4 light) {
		//Use gloss colour instead of gloss
		half3 spec = light.a * s.GlossColor;

		half4 c;
		c.rgb = (s.Albedo * light.rgb + light.rgb * spec.rgb);
		//We use gloss luminance to determine its overbright contribution
		c.a = s.Alpha + Luminance(spec);
		return c;
	}

	sampler2D _MainTex;
	sampler2D _GlossTex;
	sampler2D _BumpMap;
	float4 _Color;
	float _Shininess;

	struct Input {
		float2 uv_MainTex;
		float2 uv_GlossTex;
		float2 uv_BumpMap;

	};

	void surf(Input IN, inout SurfaceOutputSpecColor o) {
		half4 tex = tex2D(_MainTex, IN.uv_MainTex);
		half4 gloss = tex2D(_GlossTex, IN.uv_GlossTex);
		o.Albedo = tex.rgb * _Color.rgb;
		//Gloss colour come from RGB
		o.GlossColor = _Shininess * gloss.rgb;
		o.Alpha = tex.a * _Color.a;
		//Specular is mapped
		o.Specular = gloss.a;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}
	ENDCG
	}
		Fallback "Diffuse"
}