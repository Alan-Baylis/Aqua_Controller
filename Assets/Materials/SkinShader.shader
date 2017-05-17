Shader "My Shaders/Skin Shader" {
	Properties{
		_Texture("Texture", 2D) = "White" {}
		_BlendTexture("Scattering Texture", 2D) = "White" {}
		_blendColor("Blend Color", Color) = (1, 0.2, 0.2, 0)
		_blendAmount("Scattering strength", Range(1, 0)) = 0.8
		_blendContrast("Scattering contrast", Range (-0.6, 0.6)) = 0
		_skinShineColor("Skin Shine Color", Color) = (1,1,0,1)
		_skinShinePower("Skin Shine Strength", Range(0, 1)) = 0.1
		_BumpMap("BumpMap", 2D) = "bump" {}
		_bumpStrength("Bump Strength", Range(1, 0.04)) = 0.8
		_bumpContrast("Bump Contrast", Range(-0.6, 0.6)) = 0
		_SpecularMap("Specular Map", 2D) = "specular" {}
		_specularColor("Specular Color", Color) = (1,1,1,1)
		_specularRollof("Specular Rollof", Range(10, 0.1)) = 5
		_specularSize("Specular Size", Range(-2, 0)) = 0.5
		_specularContrast("Specular Contrast", Range(-1, 1)) = 0


		// Specular V.1 // _Shininess("Shininess", Float) = 10
		/*GrabbPass*/ // _Size("Size", Range(0, 20)) = 1


		// Look at:
		// shader tutorial https://cgcookie.com/archive/noob-to-pro-shader-writing-for-unity-4-beginner/
		// Specular higlits https://en.wikibooks.org/wiki/Cg_Programming/Unity/Specular_Highlights
		// for layers https://en.wikibooks.org/wiki/Cg_Programming/Unity/Layers_of_Textures
		// Transparency https://en.wikibooks.org/wiki/Cg_Programming/Unity/Transparencys
		// GrabPass translucency example https://forum.unity3d.com/threads/simple-optimized-blur-shader.185327/
		// GrabPass Unity https://docs.unity3d.com/Manual/SL-GrabPass.html
		// Gausian blur https://forum.unity3d.com/threads/simple-optimized-blur-shader.185327/
		//Shadows https://forum.unity3d.com/threads/adding-shadows-to-custom-shader-vert-frag.108612/
		// Contrast http://www.dfstudios.co.uk/articles/programming/image-programming-algorithms/image-processing-algorithms-part-5-contrast-adjustment/
		/*
		// after CGPROGRAM;
		#include "AutoLight.cginc"
 
		// in v2f struct;
		LIGHTING_COORDS(0,1) // replace 0 and 1 with the next available TEXCOORDs in your shader, don't put a semicolon at the end of this line.
 
		// in vert shader;
		TRANSFER_VERTEX_TO_FRAGMENT(o); // Calculates shadow and light attenuation and passes it to the frag shader.
 
		//in frag shader;
		float atten = LIGHT_ATTENUATION(i); // This is a float for your shadow/attenuation value, multiply your lighting value by this to get shadows. Replace i with whatever you've defined your input struct to be called (e.g. frag(v2f [b]i[/b]) : COLOR { ... ).
 
		*/
	}

		SubShader{

				/*
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
			GrabPass
			{
				//"_BackgroundTexture"
				//Tags{ "LightMode" = "Always" }
			}*/
			Pass{

				Tags{ /*"LightMode" = "Always"*/ "LightMode" = "ForwardBase" }
				
				CGPROGRAM

				//#include "AutoLight.cginc" //Only for For Shadows

				//float4 _Color;
				float4 lightDirection;
				float4 _skinShineColor;
				float4 _specularColor;
				float _specularRollof;
				float _specularSize;
				float _specularContrast;
				float4 _LightColor0;
				sampler2D _Texture;
				sampler2D _BumpMap;
				sampler2D _SpecularMap;
				sampler2D _BlendTexture;
				float4 _blendColor;
				float _blendAmount;
				float _blendContrast;
				float _blendStrength;
				float lightStrength;
				float _skinShinePower;
				float _bumpStrength;
				float _bumpContrast;


				// Specular V.1 // float _Shininess;



			#pragma vertex vertexShader
			#pragma fragment fragmentShader
			//#pragma multi_compile_fwdbase //Shadows
		

			struct vertexShaderIn {
				float4 vertexPos : POSITION;
				float4 colorOfTexture : TEXCOORD0; //Can be float2
				float3 normal : NORMAL;
				float4 tangent : TANGENT; // Vector that is tangent to the face normal 
			};

			struct vertexShaderOut {
				float4 pos : SV_POSITION;
				float4 colorOfTexture : TEXCOORD0;
				float3 normal : NORMAL;
				float3 worldVertPos : TEXCOORD1;
				float3 worldTangent : TEXCOORD2; //New
				float3 worldNormal : TEXCOORD3; //New
				float3 worldbiNormal :TEXCOORD4; //New
				//float4 color : TEXCOORD5;

				//LIGHTING_COORDS(5, 6) // Shadows Only

			};

			/* From Unity CG  // For Shadows Only

			inline float4 ComputeNonStereoScreenPos(float4 pos) {
				float4 o = pos * 0.5f;
				#if defined(UNITY_HALF_TEXEL_OFFSET)
					o.xy = float2(o.x, o.y*_ProjectionParams.x) + o.w * _ScreenParams.zw;
				#else
					o.xy = float2(o.x, o.y*_ProjectionParams.x) + o.w;
				#endif
				o.zw = pos.zw;
				return o;
			}
			inline float4 ComputeScreenPos(float4 pos) {
				float4 o = ComputeNonStereoScreenPos(pos);
				#ifdef UNITY_SINGLE_PASS_STEREO
				o.xy = TransformStereoScreenSpaceTex(o.xy, pos.w);
				#endif
				return o;
			}

			*/

			vertexShaderOut vertexShader(vertexShaderIn input) {
				vertexShaderOut o;
				o.worldNormal = normalize(mul(input.normal, unity_ObjectToWorld)); //Bump map, normal as a world normal direction
				o.worldTangent = normalize(mul(unity_ObjectToWorld, input.tangent)); //Bump map, local tangent vector to world tangent of normal
				o.worldbiNormal = normalize(cross(o.worldNormal, o.worldTangent) * input.tangent.w); //Bump map, binormal vector, perpendicular to normal and tangent vectors, multiplying it by tangent, gets it to correct length
				o.pos = mul(UNITY_MATRIX_MVP, input.vertexPos); //Always have this, This will project correct vertex "filled siluet shape"
				o.normal = input.normal;
				o.worldVertPos = mul(unity_ObjectToWorld, input.vertexPos); // not used in first pass, as it is only to calculate distance to light to vertex

				//Without GrabPass// o.colorOfTexture.xy = input.colorOfTexture.xy;
				//Without GrabPass// o.colorOfTexture.zw = o.pos.zw;

				o.colorOfTexture = input.colorOfTexture;
				//TRANSFER_VERTEX_TO_FRAGMENT(o); //Shadows only
				//With GrabPass 
				/*#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
				#else
					float scale = 1.0;
				#endif
				o.colorOfTexture.xy = (float2(o.pos.x, o.pos.y * scale) + o.pos.w) * 0.5;

				o.colorOfTexture.zw = o.pos.zw;
				*/
				return o;
			}

			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
			float _Size;

			half4 fragmentShader(vertexShaderOut input) : COLOR{

				//General
				float3 lightPosition = _WorldSpaceLightPos0.xyz;
				float3 cameraDirection = normalize(_WorldSpaceCameraPos);
				float3 lightPositionNormalized = normalize(_WorldSpaceLightPos0.xyz);

				//Shadows
				//float atten = LIGHT_ATTENUATION(input);
				float4 light = _LightColor0; //* atten;

				//Texture
				float4 colorOfTexture = tex2D(_Texture, input.colorOfTexture.xy);

				//BumMap
				float4 bumpContrast = (_bumpContrast + 1) / (1 - _bumpContrast);
				float4 bumpMap = tex2D(_BumpMap, input.colorOfTexture.xy);
				//Unpack normal 
				float3 localCoords = float3(2.0 * bumpMap.ag - float2(1.0, 1.0), 0.0) * bumpContrast;
				localCoords.z = _bumpStrength;
				//Normal transpose matrix
				float3x3 local2WorldTranspose = float3x3(input.worldTangent, input.worldbiNormal, input.worldNormal);

				float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose)); //Always have this, will covert local normal vectors to world. Without this light would casted only on predefined side

				//Calculate strenght of other lights than directional
				if (_WorldSpaceLightPos0.w != 0.0) {
					float distance = length(lightPosition - input.worldVertPos);
					lightStrength = 1 / pow(distance, distance * 2); // pow() of 4 could be changed to user accesable variable
					lightPosition = normalize(lightPosition - input.worldVertPos);
				}
				else {
					lightStrength = 1;
				}

				//Difuse shading
				float4 diffuseShading = lightStrength  * max(0.0, dot(normalDirection, lightPosition)) * light + UNITY_LIGHTMODEL_AMBIENT; // dot() will return higher value if angle is smallest, that is why objects are lit the most, in straighest line to the vertex point (they have closest to 0 angle, which will produce closest to 1 result)

				//Specular shading
				float4 specularMap = tex2D(_SpecularMap, input.colorOfTexture.xy);
				//Specular V.1 // float4 specularShading = pow(max(0.0, dot(reflect(-lightPosition, normalDirection), normalize(_WorldSpaceCameraPos))) * max(0.0, dot(normalDirection, lightPosition)), _Shininess);
				float4 specularContrast = (_specularContrast + 1) / (1 - _specularContrast);
				float4 specularShading = lightStrength * specularContrast * pow(max(0.0, dot(cameraDirection, reflect(-lightPosition, normalDirection)) + _specularSize)   * _specularColor * specularMap * max(0.0, dot(normalDirection, lightPosition)), _specularRollof);

				//Skin shine against light. First skin layer of "Oil"
				float4 skinShine = lightStrength * max(0.0, (1 - dot(lightPosition, _WorldSpaceCameraPos))) * max(0.0, dot(normalDirection, lightPosition) * _skinShineColor) * _skinShinePower;


				//Real sublayer satering
				/*
				//Blur
				half4 sum = half4(0, 0, 0, 0);

				#define GRABPIXEL(weight,kernelx) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(input.colorOfTexture.x + _GrabTexture_TexelSize.x * kernelx * _Size, input.colorOfTexture.y, input.colorOfTexture.z, input.colorOfTexture.w))) * weight
				//Gausian blur values and kernel
				sum += GRABPIXEL(0.05, -4.0);
				sum += GRABPIXEL(0.09, -3.0);
				sum += GRABPIXEL(0.12, -2.0);
				sum += GRABPIXEL(0.15, -1.0);
				sum += GRABPIXEL(0.18, 0.0);
				sum += GRABPIXEL(0.15, +1.0);
				sum += GRABPIXEL(0.12, +2.0);
				sum += GRABPIXEL(0.09, +3.0);
				sum += GRABPIXEL(0.05, +4.0);
				*/

				//Final light
				float4 lightFinal = (diffuseShading * colorOfTexture) + specularShading + skinShine;

				//Fake sublayer scatering
				float4 blendTexture = tex2D(_BlendTexture, input.colorOfTexture.xy);
				float4 blendTextureContrast = (_blendContrast + 1) / (1 - _blendContrast);
				float4 blendFinal = lightStrength * blendTexture * blendTextureContrast * _blendColor * colorOfTexture * max(0.0, dot(normalDirection, lightPosition));

				/*	Contrast
					factor = (259 * (contrast + 255)) / (255 * (259 - contrast))
					colour = GetPixelColour(x, y)
					newRed   = Truncate(factor * (Red(colour)   - 128) + 128)
					newGreen = Truncate(factor * (Green(colour) - 128) + 128)
					newBlue  = Truncate(factor * (Blue(colour)  - 128) + 128)
					PutPixelColour(x, y) = RGB(newRed, newGreen, newBlue)
				*/

				//Gaussian blur
				//float4 colorNew = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(colorOfTexture));

				float4 outFinal = lerp(lightFinal + blendFinal, lightFinal, _blendAmount);

				return outFinal;
			}


				//Not complete code, probably for UnrealEngine
				/*
				float4 BlurPS(vertexShaderOut input, uniform float2 step) : SV_TARGET{
					// Gaussian weights for the six samples around the current pixel:
					//   -3 -2 -1 +1 +2 +3
				float w[6] = { 0.006,   0.061,   0.242,  0.242,  0.061, 0.006 };
				float o[6] = { -1.0, -0.6667, -0.3333, 0.3333, 0.6667,   1.0 };

				// Fetch color and linear depth for current pixel:
				float4 colorM = tex2D(_Texture, input.pos);
				float depthM = tex2D(_SpecularMap, input.pos);

				// Accumulate center sample, multiplying it with its gaussian weight:
				float4 colorBlurred = colorM;
				colorBlurred.rgb *= 0.382;

				// Calculate the step that we will use to fetch the surrounding pixels,
				// where "step" is:
				//     step = sssStrength * gaussianWidth * pixelSize * dir
				// The closer the pixel, the stronger the effect needs to be, hence
				// the factor 1.0 / depthM.
				float2 finalStep = colorM.a * step / depthM;

				// Accumulate the other samples:
				[unroll]
				for (int i = 0; i < 6; i++) {
					// Fetch color and depth for current sample:
					float2 offset = input.colorOfTexture + o[i] * finalStep;
					float3 color = colorTex.SampleLevel(LinearSampler, offset, 0).rgb;
					float depth = depthTex.SampleLevel(PointSampler, offset, 0);

					// If the difference in depth is huge, we lerp color back to "colorM":
					float s = min(0.0125 * correction * abs(depthM - depth), 1.0);
					color = lerp(color, colorM.rgb, s);

					// Accumulate:
					colorBlurred.rgb += w[i] * color;
				}

				// The result will be alpha blended with current buffer by using specific
				// RGB weights. For more details, I refer you to the GPU Pro chapter :)
				return colorBlurred;
				}
				*/

					ENDCG
				}



				/*
				GrabPass{
					Tags{ "LightMode" = "Always" }
				}*/
				Pass{
					Tags{ /*"LightMode" = "Always"*/ "LightMode" = "ForwardAdd" }
					Blend one one

				CGPROGRAM
				//#include "AutoLight.cginc" //For Shadows
				
				float4 lightDirection;
				float4 _skinShineColor;
				float4 _specularColor;
				float _specularRollof;
				float _specularSize;
				float4 _specularContrast;
				float4 _LightColor0;
				sampler2D _Texture;
				sampler2D _BumpMap;
				sampler2D _SpecularMap;
				sampler2D _BlendTexture;
				float4 _blendColor;
				float _blendAmount;
				float _blendContrast;
				float _blendStrength;
				float lightStrength;
				float _skinShinePower;
				float _bumpStrength;
				float _bumpContrast;

			#pragma vertex vertexShader
			#pragma fragment fragmentShader

			struct vertexShaderIn {
				float4 vertexPos : POSITION;
				float4 colorOfTexture : TEXCOORD0; //Can be float2
				float3 normal : NORMAL;
				float4 tangent : TANGENT; // Vector that is tangent to the face normal 
			};

			struct vertexShaderOut {
				float4 pos : SV_POSITION;
				float4 colorOfTexture : TEXCOORD0;
				float3 normal : NORMAL;
				float3 worldVertPos : TEXCOORD1;
				float3 worldTangent : TEXCOORD2; //New
				float3 worldNormal : TEXCOORD3; //New
				float3 worldbiNormal :TEXCOORD4; //New
				//float4 color : TEXCOORD5;
			};


			vertexShaderOut vertexShader(vertexShaderIn input) {
				vertexShaderOut o;
				o.worldNormal = normalize(mul(input.normal, unity_ObjectToWorld)); //Bump map, normal as a world normal direction
				o.worldTangent = normalize(mul(unity_ObjectToWorld, input.tangent)); //Bump map, local tangent vector to world tangent of normal
				o.worldbiNormal = normalize(cross(o.worldNormal, o.worldTangent) * input.tangent.w); //Bump map, binormal vector, perpendicular to normal and tangent vectors, multiplying it by tangent, gets it to correct length
				o.pos = mul(UNITY_MATRIX_MVP, input.vertexPos); //Always have this, This will project correct vertex "filled siluet shape"
				o.normal = input.normal;
				o.worldVertPos = mul(unity_ObjectToWorld, input.vertexPos); // only to calculate where to light up

				//Without GrabPass// o.colorOfTexture.xy = input.colorOfTexture.xy;
				//Without GrabPass// o.colorOfTexture.zw = o.pos.zw;

				o.colorOfTexture = input.colorOfTexture;
				//With GrabPass 
				/*
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				o.colorOfTexture.xy = (float2(o.pos.x, o.pos.y * scale) + o.pos.w) * 0.5;

				o.colorOfTexture.zw = o.pos.zw;
				*/
				return o;
			}

			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
			float _Size;

			half4 fragmentShader(vertexShaderOut input) : COLOR{

				//General
				float3 lightPosition = _WorldSpaceLightPos0.xyz;
				float3 cameraDirection = normalize(_WorldSpaceCameraPos);

				//Shadows
				//float atten = LIGHT_ATTENUATION(input);
				float4 light = _LightColor0; //* atten;

				//Texture
				float4 colorOfTexture = tex2D(_Texture, input.colorOfTexture.xy);

				//BumMap
				float4 bumpMap = tex2D(_BumpMap, input.colorOfTexture.xy);
				//Unpack normal 
				float3 localCoords = float3(2.0 * bumpMap.ag - float2(1.0, 1.0), 0.0);
				localCoords.z = _bumpStrength;
				//Normal transpose matrix
				float3x3 local2WorldTranspose = float3x3(input.worldTangent, input.worldbiNormal, input.worldNormal);

				float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose)); //Always have this, will covert local normal vectors to world. Without this light would casted only on predefined side

				//Calculate strenght of other lights than directional
				if (_WorldSpaceLightPos0.w != 0.0) {
					float distance = length(lightPosition - input.worldVertPos);
					lightStrength = 1 / pow(distance, distance * 2); // pow() of 2 could be changed to user accesable variable
					lightPosition = normalize(lightPosition - input.worldVertPos);
				}
				else {
					lightStrength = 1;
				}

				//Difuse shading
				float4 diffuseShading = lightStrength * max(0.0, dot(normalDirection, lightPosition) * light); // dot() will return higher value if angle is smallest, that is why objects are lit the most, in straighest line to the vertex point (they have closest to 0 angle, which will produce closest to 1 result)

				//Specular shading
				float4 specularMap = tex2D(_SpecularMap, input.colorOfTexture.xy);
				// Specular V.1 // float4 specularShading = pow(max(0.0, dot(reflect(-lightPosition, normalDirection), normalize(_WorldSpaceCameraPos))) * max(0.0, dot(normalDirection, lightPosition)), _Shininess);
				float4 specularContrast = (_specularContrast + 1) / (1 - _specularContrast);
				float4 specularShading = lightStrength * specularContrast * pow(max(0.0, dot(cameraDirection, reflect(-lightPosition, normalDirection)) + _specularSize)   * _specularColor * specularMap * max(0.0, dot(normalDirection, lightPosition)), _specularRollof);

				//Skin shine against light. First skin layer of "Oil"
				float4 skinShine = lightStrength * max(0.0, (1 - dot(lightPosition, _WorldSpaceCameraPos))) * max(0.0, dot(normalDirection, lightPosition) * _skinShineColor) * _skinShinePower;
				
				//Real sublayer satering
				/*
				//Blur
				half4 sum = half4(0, 0, 0, 0);

				#define GRABPIXEL(weight,kernelx) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(input.colorOfTexture.x + _GrabTexture_TexelSize.x * kernelx * _Size, input.colorOfTexture.y, input.colorOfTexture.z, input.colorOfTexture.w))) * weight
				//Gausian blur values and kernel
				sum += GRABPIXEL(0.05, -4.0);
				sum += GRABPIXEL(0.09, -3.0);
				sum += GRABPIXEL(0.12, -2.0);
				sum += GRABPIXEL(0.15, -1.0);
				sum += GRABPIXEL(0.18, 0.0);
				sum += GRABPIXEL(0.15, +1.0);
				sum += GRABPIXEL(0.12, +2.0);
				sum += GRABPIXEL(0.09, +3.0);
				sum += GRABPIXEL(0.05, +4.0);
				*/

				//Final light
				float4 lightFinal = (diffuseShading * colorOfTexture) + specularShading;

				//Fake sublayer scatering
				float4 blendTexture = tex2D(_BlendTexture, input.colorOfTexture.xy);
				float4 blendFinal = lightStrength * blendTexture * _blendColor * colorOfTexture * max(0.0, dot(normalDirection, lightPosition));

				//Gaussian blur
				//float4 colorNew = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(colorOfTexture));

				float4 outFinal = lerp(lightFinal + blendFinal, lightFinal, _blendAmount);

				return outFinal;
			}
				ENDCG
			}

			}
}

