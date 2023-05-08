using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorBlind
{
    Normal = 0,
    Protanopia = 1
}

public class CAT_CameraColorFilter : MonoBehaviour
{
    private Material material;

    [SerializeField] ColorBlind colorBlind; 

    private static Color[,] RGB =
    {
        //Normal
        {   
            new Color(1.0f, 0.0f, 0.0f),   
            new Color(0.0f, 1.0f, 0.0f), 
            new Color(0.0f, 0.0f, 1.0f) 
        },

        // Protanopia
        { 
            new Color(0.56f, 0.44f, 0.0f), 
            new Color(0.56f, 0.44f, 0.0f), 
            new Color(0.0f, 0.24f, 0.76f) 
        }    
    };
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/CameraBlindColor"));
        material.SetColor("_R", RGB[0, 0]);
        material.SetColor("_G", RGB[0, 1]);
        material.SetColor("_B", RGB[0, 2]);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (colorBlind == ColorBlind.Normal)
            Graphics.Blit(source, destination);

        else if (colorBlind == ColorBlind.Protanopia)
        {
            material.SetColor("_R", RGB[1, 0]);
            material.SetColor("_G", RGB[1, 1]);
            material.SetColor("_B", RGB[1, 2]);

            Graphics.Blit(source, destination, material);
        }
    }
}
