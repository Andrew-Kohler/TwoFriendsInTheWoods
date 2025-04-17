Shader "Unlit/CoreShadowShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("Light Texture", 2D) = "white" {}
        [NoScaleOffset] _MainTex2("Dark Texture", 2D) = "white" {}
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

            Pass
            {
                HLSLPROGRAM

                #pragma vertex vert
                #pragma fragment frag

                #pragma shader_feature _RENDERING_CUTOUT
                #pragma shader_feature _SMOOTHNESS_ALBEDO
                #include "UnityCG.cginc"
                #include "Lighting.cginc"

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct Attributes
                {
                    float4 positionOS   : POSITION;
                    float2 uv: TEXCOORD0;
                };

                struct Varyings
                {
                    float4 positionCS  : SV_POSITION;
                    float2 uv: TEXCOORD0;
                };

                TEXTURE2D(_BaseMap);
                SAMPLER(sampler_BaseMap);

                CBUFFER_START(UnityPerMaterial)
                sampler2D _MainTex;
                sampler2D _MainTex2;
                CBUFFER_END

                Varyings vert(Attributes IN)
                {
                    Varyings OUT;

                    OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                    OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                    return OUT;
                }

                half4 frag(Varyings IN) : SV_Target
                {
                    float4 texel = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                    return texel * _Color;
                }
                ENDHLSL
            }
        }
}
