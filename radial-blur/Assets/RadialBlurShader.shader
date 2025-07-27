Shader "Custom/RadialBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurStrength ("Blur Strength", Range(0, 1)) = 0.5
        _BlurRadius ("Blur Radius", Range(0, 1)) = 0.5
        _CenterX ("Center X", Range(0, 1)) = 0.5
        _CenterY ("Center Y", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        
        Pass
        {
            Name "RadialBlur"
            ZTest Always
            ZWrite Off
            Cull Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _BlurStrength;
                float _BlurRadius;
                float _CenterX;
                float _CenterY;
            CBUFFER_END
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                float2 center = float2(_CenterX, _CenterY);
                float2 direction = input.uv - center;
                float distance = length(direction);
                
                // If blur strength is 0, just return the original texture
                if (_BlurStrength <= 0.0)
                {
                    return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                }
                
                // Create radial blur effect
                half4 color = 0;
                int samples = 8; // Reduced samples for better performance
                
                // Calculate blur amount based on distance from center
                float blurAmount = saturate(distance / _BlurRadius) * _BlurStrength;
                
                // Normalize direction for consistent blur
                direction = normalize(direction) * blurAmount;
                
                for(int i = 0; i < samples; i++)
                {
                    float offset = (float(i) / float(samples - 1) - 0.5) * 0.02; // Smaller offset for more subtle effect
                    float2 sampleUV = input.uv + direction * offset;
                    
                    // Clamp UV coordinates to prevent sampling outside texture bounds
                    sampleUV = saturate(sampleUV);
                    
                    color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, sampleUV);
                }
                
                color /= samples;
                return color;
            }
            ENDHLSL
        }
    }
}
