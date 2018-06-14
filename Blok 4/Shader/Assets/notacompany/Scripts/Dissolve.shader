Shader "Custom/Dissolve" 
{

	Properties 
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_Progression("Progression", Range(0,1)) = 0.0

		_Start("Dissolve Start Point", Vector) = (1, 1, 1, 1)
		_End("Dissolve End Point", Vector) = (0, 0, 0, 1)
		_Size("Dissolve Band Size", Float) = 0.25

		_GlowIntensity("Glow Intensity", Range(0.0, 5.0)) = 0.05
		_GlowScale("Glow Size", Range(0.0, 5.0)) = 1.0
		_GlowStart("Glow Start Color", Color) = (1, 1, 1, 1)
		_GlowEnd("Glow End Color", Color) = (1, 1, 1, 1)
		_GlowColFac("Glow Colorshift", Range(0.01, 2.0)) = 0.75
	}

		SubShader
		{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Fade"
		}

		CGPROGRAM


		#pragma surface surf Standard fullforwardshadows alpha:fade vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NoiseTex;
		float _Progression;
		float _Size;
		float3 _Start;
		float3 _End;
		fixed4 _Color;

		float _GlowIntensity;
		float _GlowScale;
		float4 _GlowStart;
		float4 _GlowEnd;
		float _GlowColFac;


		static float3 direction = normalize(_End - _Start);
		static float3 converted = _Start - _Size * direction;
		static float factor = 1.0f / _Size;

		struct Input 
		{
			float2 uv_MainTex;
			float3 geometry;
		};


		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 tex = tex2D(_NoiseTex, IN.uv_MainTex);
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			half base = -2.0f * _Progression + 1.0f;
			half read = tex.r + base;
			half final = read + IN.geometry;
			half alpha = clamp(final, 0.0f, 1.0f);
			half prediction = (_GlowScale - final) * _GlowIntensity;
			half predictionColor = (_GlowScale * _GlowColFac - final) * _GlowIntensity;

			fixed4 glowColor = prediction * lerp(_GlowStart, _GlowEnd, clamp(predictionColor, 0.0f, 1.0f));
			glowColor = clamp(glowColor, 0.0f, 1.0f);

			o.Albedo = col.rgb;
			o.Alpha = alpha;
			o.Emission = glowColor;
		}


		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			float3 position = lerp(converted, _End, _Progression);

			o.geometry = dot(v.vertex - position, direction) * factor;
		}

		ENDCG
	}

	FallBack "Diffuse"
}