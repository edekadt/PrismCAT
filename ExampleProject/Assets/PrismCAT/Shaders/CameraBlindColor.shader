Shader "Hidden/CameraBlindColor"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _R("Red", Color) = (1, 0, 0, 1)
        _G("Green", Color) = (0, 1, 0, 1)
        _B("Blue", Color) = (0, 0, 1, 1)
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform fixed3 _R;
            uniform fixed3 _G;
            uniform fixed3 _B;

            fixed4 frag (v2f_img i) : COLOR
            {
               fixed4 col = tex2D(_MainTex, i.uv);

                return fixed4
                (
                    col.r * _R[0] + col.g * _R[1] + col.b * _R[2],
                    col.r * _G[0] + col.g * _G[1] + col.b * _G[2],
                    col.r * _B[0] + col.g * _B[1] + col.b * _B[2],
                    col.a
                );
            }

            ENDCG
        }
    }
}


