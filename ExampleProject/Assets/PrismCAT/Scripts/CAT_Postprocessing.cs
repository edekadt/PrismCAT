using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAT_Postprocessing : MonoBehaviour
{
    public float intensity;
    public float delayTime;

    private Material material;
    private float timer;

    /// <summary>
    /// Private material is created for the postprocessing effect.
    /// </summary>
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/TestShader"));
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > delayTime)
        {
            timer -= Time.deltaTime;

            intensity += 0.01f;
            if (intensity > 1)
            {
                intensity = 0;
            }
        }
    }

    /// <summary>
    /// Method that processes the image applying the effect
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (intensity == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }
        material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }
}
