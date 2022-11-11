Shader "Hidden/ImageShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            float4 _color1;
            float4 _color2;
            float4 _color3;
            float4 _color4;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            float remap(float s, float a1, float a2, float b1, float b2)
            {
                return b1 + (s-a1)*(b2-b1)/(a2-a1);
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float4 finalCol = col;
                float avg =(col.r + col.b + col.g)/3.0;
                avg = remap(avg, 0.33, 0.86, 0.0, 1.0);

                if (avg > 0.0 && avg <= 0.25){
                    finalCol = _color4;
                }
                else if (avg > 0.25 && avg <= 0.5){
                    finalCol = _color3;
                }
                else if (avg > 0.5 && avg <= 0.75){
                    finalCol = _color2;
                }
                else{
                //else if (avg > 0.75 && avg <= 1){
                    finalCol = _color1;
                }
                return finalCol;
            }
            ENDCG
        }
    }
}
