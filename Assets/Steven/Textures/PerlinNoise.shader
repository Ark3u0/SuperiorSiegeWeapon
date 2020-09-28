Shader "Custom/PerlinNoise"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _CellSize ("Cell Size", Range(0, 1)) = 1
        _ScrollSpeed ("Scroll Speed", Range(0, 1)) = 1
        _DissolvePercentage ("Dissolve Percentage", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

		#include "Random.cginc"
        #include "Interpolation.cginc"

        sampler2D _MainTex;
        sampler2D _MaskTex;

        struct Input
        {
            float4 screenPos;
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _CellSize;
        float _ScrollSpeed;
        float _DissolvePercentage;

        float gradientNoise3d(float3 value) {
            float3 fraction = frac(value);

            float interpolatorX = easeInOut(fraction.x);
            float interpolatorY = easeInOut(fraction.y);
            float interpolatorZ = easeInOut(fraction.z);

            float3 cellNoiseZ[2];
            [unroll]
            for(int z=0;z<=1;z++){
                float3 cellNoiseY[2];
                [unroll]
                for(int y=0;y<=1;y++){
                    float3 cellNoiseX[2];
                    [unroll]
                    for(int x=0;x<=1;x++){
                        float3 cell = floor(value) + float3(x, y, z);
                        float3 cellDirection = rand3dTo3d(cell) * 2 - 1;
                        float3 compareVector = fraction - float3(x, y, z);
                        cellNoiseX[x] = dot(cellDirection, compareVector);
                    }
                    cellNoiseY[y] = lerp(cellNoiseX[0], cellNoiseX[1], interpolatorX);
                }
                cellNoiseZ[z] = lerp(cellNoiseY[0], cellNoiseY[1], interpolatorY);
            }
            float3 noise = lerp(cellNoiseZ[0], cellNoiseZ[1], interpolatorZ);
            return noise;
        }

        float gradientNoise2d(float2 value) {
            float2 lowerLeftDirection = rand2dTo2d(float2(floor(value.x), floor(value.y))) * 2 - 1;
            float2 lowerRightDirection = rand2dTo2d(float2(ceil(value.x), floor(value.y))) * 2 - 1;
            float2 upperLeftDirection = rand2dTo2d(float2(floor(value.x), ceil(value.y))) * 2 - 1;
            float2 upperRightDirection = rand2dTo2d(float2(ceil(value.x), ceil(value.y))) * 2 - 1;

            float2 fraction = frac(value);

            float2 lowerLeftFunctionValue = dot(lowerLeftDirection, fraction - float2(0, 0));
            float2 lowerRightFunctionValue = dot(lowerRightDirection, fraction - float2(0, 1));
            float2 upperLeftFunctionValue = dot(upperLeftDirection, fraction - float2(1, 0));
            float2 upperRightFunctionValue = dot(upperRightDirection, fraction - float2(1, 1));

            float interpolatorX = easeInOut(fraction.x);
            float interpolatorY = easeInOut(fraction.y);

            float lowerCells = lerp(lowerLeftFunctionValue, lowerRightFunctionValue, interpolatorX);
            float upperCells = lerp(upperLeftFunctionValue, upperRightFunctionValue, interpolatorX);

            float noise = lerp(lowerCells, upperCells, interpolatorY);
            return noise;
        }

        float gradientNoise1d(float value) {
            float fraction = frac(value);
            float interpolator = easeInOut(fraction);

            float previousCellInclination = rand1dTo1d(floor(value)) * 2 - 1;
            float previousCellLinePoint = previousCellInclination * fraction;

            float nextCellInclination = rand1dTo1d(ceil(value)) * 2 - 1;
            float nextCellLinePoint = nextCellInclination * (fraction - 1);

            return lerp(previousCellLinePoint, nextCellLinePoint, interpolator);
        }

        void surf(Input IN, inout SurfaceOutputStandard o) {
            float3 value = IN.worldPos.xyz / _CellSize;
            value.y += _Time.y * _ScrollSpeed;
            float noise = gradientNoise3d(value) + 0.5;

            // noise = frac(noise * 6);


            // distance from .5 in x and y (center of float2)
            // scale distance value based off ratio of [res x] : [res y]

            fixed4 c = fixed4(IN.screenPos.x, IN.screenPos.y, 0, 1);
            // fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            // fixed4 r = tex2D(_MaskTex, IN.screenPos).xy;



            // fixed3 r = tex2D(_MaskTex, IN.screenPos).r /;
            // half gradient = tex2D(_MainTex, IN.worldPos).r;
            
            // float pixelNoiseChange = fwidth(noise);
            // float heightLine = smoothstep(1 - pixelNoiseChange, 1, noise);
            // heightLine += smoothstep(pixelNoiseChange, 0, noise);

            // clip(clamp(noise, 0, .99) - _DissolvePercentage);
            // clip(1 - r);

            o.Albedo = c;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Standard"
}
