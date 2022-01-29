// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Mega/Add Premultiply" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_Saturation("Saturation", Range(0.010000,3.000000)) = .5000000
	}
	SubShader{
			LOD 100
			Tags{ "QUEUE" = "Transparent" "IGNOREPROJECTOR" = "true" "RenderType" = "Transparent" "PreviewType" = "Plane" }

		// Grab the screen behind the object into _BackgroundTexture
		/*
		GrabPass {
			"bgtex"
		}
		*/

		// Render the object with the texture generated above, and invert the colors
		Pass{
			// Draw ourselves after all opaque geometry
		Tags{ "QUEUE" = "Transparent" "IGNOREPROJECTOR" = "true" "RenderType" = "Transparent" "PreviewType" = "Plane" }
			Cull off
			ZWrite Off
			blend One OneMinusSrcAlpha

			CGPROGRAM
			// use "vert" function as the vertex shader
			#pragma vertex vert
				// use "frag" function as the pixel (fragment) shader
			#pragma fragment frag

				// vertex shader inputs
			struct appdata {
				float4 vertex : POSITION; // vertex position
				float2 uv : TEXCOORD0; // texture coordinate     
				float4 color : COLOR;
			};

			// vertex shader outputs ("vertex to fragment")
			struct v2f {
				float2 uv : TEXCOORD0; // texture coordinate
				float4 vertex : SV_POSITION; // clip space position
				float4 color : COLOR; // clip space position
			};

			// vertex shader
			v2f vert(appdata v) {
				v2f o;
				// transform position to clip space
				// (multiply with model*view*projection matrix)
				o.vertex = UnityObjectToClipPos(v.vertex);
				// just pass the texture coordinate
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}

			// texture we will sample
			sampler2D _MainTex;
			float _Saturation;

			// pixel shader; returns low precision ("fixed4" type)
			// color ("SV_Target" semantic)
			fixed4 frag(v2f i) : SV_Target{

				// sample texture and return it
				fixed4 col = tex2D(_MainTex, i.uv) * i.color;

				col.rgb = col.rgb * col.a;

				col.a = (col.a) * (1 - col.a) * _Saturation;

				return col;
			}

			ENDCG
		}

	}

}