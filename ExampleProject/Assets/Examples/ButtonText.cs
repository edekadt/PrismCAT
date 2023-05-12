using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrismCAT;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour
{
    [SerializeField]
    private Text texto;

    private void Start()
    {
        if (!texto)
        {
            Debug.Log("Error: No text component detected in ButtonText component.");
            return;
        }

        if (!CAT_ColourManager.Instance)
        {
            Debug.Log("Error: No CAT_ColourManager Instance detected.");
            return;
        }

        ChangeText();
    }

    private void ChangeText()
    {
        int value = (int)CAT_ColourManager.Instance.GetPalette();
        var enumName = (CAT_ColourManager.Palette)value;
        texto.text = enumName.ToString();
    }

    public void OnButtonClick()
    {
        if (!CAT_ColourManager.Instance) return;

        CAT_ColourManager.Palette palette = (CAT_ColourManager.Palette)(((int)CAT_ColourManager.Instance.GetPalette() + 1) % 4);

        CAT_ColourManager.Instance.ChangePalette(palette);
        ChangeText();
    }
}
