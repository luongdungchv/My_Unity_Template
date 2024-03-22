Shader "Unlit/GrassComputeUnlit"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _Scale ("Noise Scale", float) = 1
        
        _TopColor ("Top Color", Color) = (1,1,1,1)
        _BotColor ("Bot Color", Color) = (1,1,1,1)
        
        _TopColor1 ("Top Color", Color) = (1,1,1,1)
        _BotColor1 ("Bot Color", Color) = (1,1,1,1)
        
        _BlendFactor("Blend Factor", float) = 0.5
        _SmoothnessState("Smoothness State", float) = 0
    }

    CGINCLUDE
    #pragma vertex vert
    #pragma fragment frag

    #include "Assets/Shader Headers/PerlinNoise.cginc"
    #include "AutoLight.cginc"

    struct Props{
        float3 pos, normal;
        float4x4 trs;
        int colorIndex;  
    };

    sampler2D _MainTex;
    float4 _MainTex_ST;

    half _Glossiness; 
    half _Metallic;
    fixed4 _Color; 
    float _Scale;
    float4 _TopColor, _BotColor, _TopColor1, _BotColor1;
    float _BlendFactor;
    float _SmoothnessState;
    StructuredBuffer<float4> GeometryBuffer;
    //StructuredBuffer<Props> props; 
    ENDCG

    SubShader
    {
        Tags { "IgnoreProjector"="True" "RenderType"="Grass" "DisableBatching"="True" }
        LOD 100
        Cull Off

        Pass
        {
            Tags { "LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma multi_compile_fwdbase

            

            struct appdata{
                float4 vertex: POSITION;
                float2 texcoord: TEXCOORD0;
                float3 normal: NORMAL;
                float3 color: COLOR0;
                uint id : SV_VertexID;
                uint inst : SV_InstanceID;
            };
            

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos: TEXCOORD1;
                float3 worldNormal: NORMAL;
                float4 _ShadowCoord : TEXCOORD2;
            };

            

            float4 ComputeScreenPos (float4 p)
            {
                float4 o = p * 0.5;
                return float4(o.x + o.w, o.y*_ProjectionParams.x + o.w, p.zw);    
            }

            v2f vert (appdata data)
            {
                v2f vs;
                float4 buffer = GeometryBuffer[data.inst];              
                vs.vertex = mul(UNITY_MATRIX_VP, buffer + data.vertex);
                vs.uv = data.texcoord;
                vs._ShadowCoord = ComputeScreenPos(vs.vertex);
                return vs;
            }

            
            fixed4 frag (v2f i) : SV_Target
            {
                float4 topCol = _TopColor;
                float4 botCol = _BotColor;
                fixed4 c = lerp(topCol, botCol, i.uv.y);
                float4 shadow = SHADOW_ATTENUATION(i);
                c *= shadow;
                return c;
            }
            ENDCG
        }
        Pass
        {
            Tags{ "LightMode" = "ShadowCaster" }  
            Cull Off                  
            CGPROGRAM
                                                       
            void vert (inout float4 vertex:POSITION,inout float2 uv:TEXCOORD0,uint i:SV_InstanceID)
            {              
                vertex = mul(UNITY_MATRIX_VP,GeometryBuffer[i]+vertex);
            }
           
            float4 frag (float4 vertex:POSITION, float2 uv:TEXCOORD0) : SV_Target
            {
                return 0;
            }
            ENDCG
        }   
    }
}
