Shader "Custom/CameraOcclusion"
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
        _Radius ("Radius", Range(0, 5)) = 0.3
        _CameraPosition ("Camera Position", Vector) = (0, 0, 0, 0)
        _TargetPosition ("Target Position", Vector) = (0, 0, 0, 0)
        _GradientThreshold ("Gradient Threshold", Range(0, 1)) = 0.5
        _CenterX ("Center X", Range(0, 1)) = 0.5
        _CenterY ("Center Y", Range(0, 1)) = 0.5
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
        float _Radius;
        float _GradientThreshold;
        float _CenterX;
        float _CenterY;
        float4 _CameraPosition;
        float4 _TargetPosition;

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

        float circle(fixed2 screenPos, float _radius, float _gradientThreshold, float2 center) {
            float2 dist = screenPos - center;
            return 1.0 - smoothstep(
                _radius - (_radius * _gradientThreshold),
                _radius + (_radius * _gradientThreshold),
                dot(dist, dist) * 5.0);
        }

        float quadratic(fixed2 screenPos, float _width, float _gradientThreshold, float2 origin) {
            float xOffset = screenPos.x - origin.x;
            float yOffset = _width * xOffset * xOffset;
            return smoothstep(
                yOffset - (yOffset * _gradientThreshold),
                yOffset + (yOffset * _gradientThreshold),
                screenPos.y - origin.y
            );
        }


        float fov() {
            float t = unity_CameraProjection._m11;
            const float Rad2Deg = 180 / UNITY_PI;
            float fov = atan(1.0f / t ) * 2.0 * Rad2Deg;
            return fov;
        }

        void surf(Input IN, inout SurfaceOutputStandard o) {
            float3 value = IN.worldPos.xyz / _CellSize;
            value.y += _Time.y * _ScrollSpeed;
            float noise = gradientNoise3d(value) + 0.5;

            noise = frac(noise * 6);

            float2 textureCoordinate = IN.screenPos.xy / IN.screenPos.w;
            float aspect = _ScreenParams.x / _ScreenParams.y;
            textureCoordinate.x = textureCoordinate.x * aspect;


            float mask = circle(textureCoordinate, _Radius, _GradientThreshold, float2(_CenterX * aspect, _CenterY));
            // float mask = quadratic(textureCoordinate, _Radius, _GradientThreshold, float2(_CenterX * aspect, _CenterY));

            float2 deltaXZFromWorldPosToCamera = IN.worldPos.xz - _CameraPosition.xz;
            float2 deltaXZFromTargetToCamera = _TargetPosition.xz - _CameraPosition.xz;
            float distanceDiscriminator = step(dot(deltaXZFromWorldPosToCamera, deltaXZFromWorldPosToCamera), dot(deltaXZFromTargetToCamera, deltaXZFromTargetToCamera));
            float dissolveDiscriminator = clamp(noise, 0, .99) - mask;
            clip(dissolveDiscriminator * distanceDiscriminator);

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Standard"
}
