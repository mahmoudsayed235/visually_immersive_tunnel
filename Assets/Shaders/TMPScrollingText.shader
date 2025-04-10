Shader "Custom/TMPScrollingText"
{
    Properties
    {
        _MainTex("Font Texture", 2D) = "white" {}
        _FaceColor("Face Color", Color) = (1,1,1,1)
        _ScrollSpeed("Scroll Speed", Float) = 50.0
        _TextWidth("Text Width", Float) = 500.0   // Set to your text mesh width in object space
        _MinX("Min X", Float) = 0.0                // Set to the left boundary of your text mesh
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            LOD 100

            Pass
            {
                // Turn off ZWrite and back-face culling for GUI elements.
                ZWrite Off
                Cull Off
                Blend SrcAlpha OneMinusSrcAlpha

                CGPROGRAM
            // Use vertex and fragment program; do not redefine _Time
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _FaceColor;
            float _ScrollSpeed;
            float _TextWidth;
            float _MinX;

            // Vertex input structure – standard for TMP.
            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            // Data passed from vertex to fragment.
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                // Compute horizontal scrolling offset (object space)
                float offset = _Time.y * _ScrollSpeed;

                // Calculate relative position (distance from the left boundary)
                float relative = v.vertex.x - _MinX;
                // Wrap the vertex position using modulo arithmetic.
                // Adding _TextWidth ensures the value remains positive.
                float newRelative = fmod(relative - offset + _TextWidth, _TextWidth);
                v.vertex.x = newRelative + _MinX;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the font texture (the SDF for TMP)
                fixed4 texCol = tex2D(_MainTex, i.texcoord);
            // Combine the sampled texture with the face color and vertex color.
            fixed4 col = _FaceColor * i.color * texCol;
            return col;
        }
        ENDCG
    }
        }
            // Fallback to TMP's distance field shader if needed.
            Fallback "TextMeshPro/Distance Field"
}
