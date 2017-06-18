Shader "RM/Skin Shader" {
Properties {
	_Color 			("Main Color", Color) = (1,1,1,1)
	_SpecColor		("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess 		("Shininess", Range (0, 10)) = 0.078125
	_Power          ("Power", Range (0, 10)) = 0.078125
	_SubPower 		("SubPower", Range (0, 5)) = 0.078125
	_MainTex 		("Base (RGB), Spec (A)", 2D) = "white" {}
	_NormalMap 		("Normalmap", 2D) = "bump" {}
	_Specularmap    ("Specularmap", 2D) = "white" {}
	_Cube           ("Cubemap", CUBE) = "" {}
}

SubShader {
	Tags {"RenderType"="Opaque"}
	LOD 300
		
		
CGPROGRAM
#pragma surface surf BlinnPhong 
#pragma target 3.0
#pragma glsl

sampler2D _MainTex;
sampler2D _NormalMap;
sampler2D _Specularmap;
samplerCUBE _Cube;


float4 _Color;
float _Shininess;
float _SubPower;
float _Power;

struct Input {
float3 worldRefl;
INTERNAL_DATA
	float2 uv_MainTex;
	float2 uv_NormalMap;
	float2 uv_Specularmap;
	float3 viewDir;
	};
			
void surf (Input IN, inout SurfaceOutput o) {
			
    fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
	half3 normal = UnpackNormal(tex2D(_NormalMap,IN.uv_NormalMap));	
	fixed4 spe = tex2D(_Specularmap, IN.uv_Specularmap);
	
	o.Normal = normal; 
	o.Gloss = spe.a*_SubPower;
	o.Alpha = tex.a * _Color.a;
	
	
	float3 worldRefl = WorldReflectionVector (IN, o.Normal);
	fixed4 reflcol = texCUBE (_Cube, worldRefl);
	reflcol *= spe.a;
	o.Emission = reflcol.rgb*_SpecColor.rgb*spe.a;
	o.Albedo = tex.rgb * _Color.rgb;
	
	half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
    o.Emission *=  spe.a*_Power*rim;
	o.Specular = _Shininess*rim;
	
}
ENDCG  
	}
FallBack "Bumped Specular"
}

//Skin Shader
//Created by Rispat Momit
