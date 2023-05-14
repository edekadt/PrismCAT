using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorBlind
{
    Normal = 0,
    Protanopia = 1,
    Deuteranopia = 2,
    Tritanopia = 3
}

public class CAT_CameraColorFilter : MonoBehaviour
{
    private Material material;
    private ColorBlind previousColor;

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
        },

        // Deuteranopia
        {
            new Color(0.625f, 0.375f, 0.0f),
            new Color(0.7f, 0.3f, 0.0f),
            new Color(0.0f, 0.3f, 0.7f)
        },

        // Tritanopia
        {
            new Color(0.95f, 0.05f, 0.0f),
            new Color(0.0f, 0.43f, 0.57f),
            new Color(0.0f, 0.475f, 0.525f),
        }
    };
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/CameraBlindColor"));
        material.SetColor("_R", RGB[0, 0]);
        material.SetColor("_G", RGB[0, 1]);
        material.SetColor("_B", RGB[0, 2]);
    }

    /// <summary>
    /// Change the color filter from the camera using a shader
    /// </summary>
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (colorBlind == ColorBlind.Normal) {
            Graphics.Blit(source, destination);
            return;
        } else if (colorBlind != previousColor)
        {
            int color = (int)colorBlind;
            material.SetColor("_R", RGB[color, 0]);
            material.SetColor("_G", RGB[color, 1]);
            material.SetColor("_B", RGB[color, 2]);
            previousColor = colorBlind;
        }
        
        Graphics.Blit(source, destination, material);
    }
}
