using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAT_AddImageColour : MonoBehaviour
{
    public Color colorWithTransparency;
    // Start is called before the first frame update
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        Material mat = rend.material;

        // Configura el modo de sombreado en "Transparent" para permitir la transparencia
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;

        // Aplica el nuevo color con transparencia
        Color matColor = mat.color;
        float alpha = matColor.a * colorWithTransparency.a;
        Color newColor = colorWithTransparency * alpha + matColor * (1 - alpha);
        mat.color = newColor;
    }
}
