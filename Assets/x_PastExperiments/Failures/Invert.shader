Shader "Custom/Invert"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        [NoScaleOffset] _MainTex("Main Texture", 2D) = "white" {}
        [NoScaleOffset] _MainTex2("Main Texture 2", 2D) = "white" {}
        [NoScaleOffset] _Cutoff("Alpha Cutoff", Float) = 0.1

    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 200
        Blend OneMinusDstColor Zero
        //Blend One Zero

        PASS
        {
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0 Alpha:Blend
            #pragma shader_feature _RENDERING_CUTOUT
            #pragma shader_feature _SMOOTHNESS_ALBEDO
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            //#pragma surface surf Lambert alpha

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;

            };

            fixed4 _Color;
            sampler2D _MainTex;

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord; //float2(0,0);
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal, 1));
                TRANSFER_SHADOW(o)
                //clip(o.alpha - _Cutoff);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                fixed4 col = (1,1,1,1);

                float alpha = tex2D(_MainTex, i.uv.xy).a;
                clip(alpha - 0.01);

                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // Set ShadowVal here
                if (shadow < 1) // we have achieved the absolute
                {
                    // If this fragment is fully lit, discard it
                    discard;
                }

                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;

                return col;
            }

            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
    FallBack "Diffuse"
}