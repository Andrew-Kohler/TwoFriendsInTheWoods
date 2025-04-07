Shader "Example/Test S"
{
    Properties
    {
        [NoScaleOffset] _MainTex("Light Texture", 2D) = "white" {}
        [NoScaleOffset] _MainTex2("Dark Texture", 2D) = "white" {}
    }
        SubShader
    {
        Pass
        {
            Tags {"LightMode" = "ForwardBase" "RenderPipeline" = "UniversalPipeline"}
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _RENDERING_CUTOUT
            #pragma shader_feature _SMOOTHNESS_ALBEDO
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        // compile shader into multiple variants, with and without shadows
        // (we don't care about any lightmaps yet, so skip these variants)
        #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
        // shadow helper functions and macros
        #include "AutoLight.cginc"

        struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv : TEXCOORD0;
            };

        struct Varyings
        {
            float2 uv : TEXCOORD0;
            SHADOW_COORDS(1) // put shadows data into TEXCOORD1
            fixed3 diff : COLOR0;
            fixed3 ambient : COLOR1;
            float4 positionCS : SV_POSITION;
        };


        Varyings vert(Attributes IN)
        {
            Varyings OUT;

            OUT.positionCS = UnityObjectToClipPos(IN.vertex);
            OUT.uv = IN.texcoord;

            half3 worldNormal = UnityObjectToWorldNormal(IN.normal);
            half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
            OUT.diff = nl * _LightColor0.rgb;
            OUT.ambient = ShadeSH9(half4(worldNormal,1));
            // compute shadows data
            TRANSFER_SHADOW(o)
            return o;
        }

        CBUFFER_START(UnityPerMaterial)
        sampler2D _MainTex;
        sampler2D _MainTex2;
        CBUFFER_END

        half4 frag(Varyings IN) : SV_Target
            {
            
            half4 col = tex2D(_MainTex, IN.uv);

            // Alpha clipping without properly accounting for shadow casting
            float alpha = tex2D(_MainTex, IN.uv.xy).a;
            clip(alpha - 0.5);
        

            // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
            half shadow = SHADOW_ATTENUATION(i);
            if (shadow < 1) // we have achieved the absolute
            {
                shadow = 1;
                col = tex2D(_MainTex2, IN.uv);
            }
            // darken light's illumination with shadow, keep ambient intactt
            half3 lighting = IN.diff * shadow + IN.ambient;
            col.rgb *= lighting;
            return col;
            }
        ENDHLSL
        }

//// shadow casting support
//UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
