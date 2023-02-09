Shader "LWVN/Gaussian Blur Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Blur("Blur", Range(0, 1)) = 1

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

    struct appdata
    {
        float4 vertex : POSITION;
        float4 color : COLOR;
        float2 texcoord : TEXCOORD;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        float4 color : COLOR;
        float2 texcoord : TEXCOORD;
    };

    half4 GaussianBlur(sampler2D tex, float2 uv, float blur)
    {
        // 偏移系数校正
        float offset = blur * 0.0018;
        // 对应中 - 四角 - 四方的权重
        float weights[3] = {0.147761, 0.118318, 0.0947416};

        // 中
        half4 color = tex2D(tex, float2(uv.x, uv.y)) * weights[0];
        
        // 上
        color += tex2D(tex, float2(uv.x, uv.y - offset)) * weights[1];
        // 下
        color += tex2D(tex, float2(uv.x, uv.y + offset)) * weights[1];
        // 左
        color += tex2D(tex, float2(uv.x - offset, uv.y)) * weights[1];
        // 右
        color += tex2D(tex, float2(uv.x + offset, uv.y)) * weights[1];

        // 左上
        color += tex2D(tex, float2(uv.x - offset, uv.y - offset)) * weights[2];
        // 右上
        color += tex2D(tex, float2(uv.x + offset, uv.y + offset)) * weights[2];
        // 左下
        color += tex2D(tex, float2(uv.x - offset, uv.y + offset)) * weights[2]; 
        // 右下
        color += tex2D(tex, float2(uv.x + offset, uv.y - offset)) * weights[2];

        return color;
    }

    sampler2D _MainTex;
    float _Blur;

    v2f vert(appdata i)
    {
        v2f o;
        o.vertex =  UnityObjectToClipPos(i.vertex);
        o.color = i.color;
        o.texcoord = i.texcoord;
        return o;
    }
    
    half4 frag(v2f i) : SV_TARGET
    {
        half4 color = GaussianBlur(_MainTex, i.texcoord, _Blur);
        return color * i.color;
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
