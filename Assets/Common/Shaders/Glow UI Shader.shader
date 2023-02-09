Shader "UGUI/UI Glow Shader"
{
	Properties
	{
		[PerRendererData] _MainTex("MainTex", 2D) = "white" {}
		[HDR]_GlowColor("GlowColor", Color) = (1,1,1,1)
		_GlowIntensity("GlowIntensity", Range(0, 1)) = 1
		[Toggle]_EnableGlow("EnableGlow", Float) = 1

		[Header(Stencil)]
		[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
		[HideInInspector]_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	#include "UnityUI.cginc"

	struct appdata
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};
	struct v2f
	{
		float4 vertex   : SV_POSITION;
		float4 color    : COLOR;
		half2 texcoord  : TEXCOORD0;
	};

	sampler2D _MainTex;
	float4 _GlowColor;
	float _GlowIntensity;
	float _EnableGlow;

	v2f vert(appdata i)
	{
		v2f o;
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

		o.vertex = UnityObjectToClipPos(i.vertex);
		o.texcoord = i.texcoord;
		o.color = i.color;
		return o;
	}

	half4 frag(v2f i) : SV_Target
	{
		half4 color = tex2D(_MainTex, i.texcoord) * i.color;
		if (_EnableGlow) {
			color = color + (_GlowColor * color.a) * _GlowIntensity;
		}
		return color;
	}
	ENDCG

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off 
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest[unity_GUIZTestMode]
			ColorMask[_ColorMask]

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			ENDCG
		}
	}
}