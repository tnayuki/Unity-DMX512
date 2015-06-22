Shader "Custom/EqualizerField" {
	Properties {
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 customColor;
		};

		half _Glossiness;
		half _Metallic;

        float3 hsv2rgb(float3 c) {
          c = float3(c.x, clamp(c.yz, 0.0, 1.0));
          float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
          float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
          return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
        }

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.customColor = hsv2rgb(float3(lerp(0.4, 0.0, v.vertex.y / 5.0), 1, 1));
			
			//if (fmod(v.vertex.x, 8 / 10) < 0.005) o.customColor = 0;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = IN.customColor;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1.0;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
