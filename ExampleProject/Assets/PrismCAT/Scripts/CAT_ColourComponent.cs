using UnityEngine;
using UnityEditor;
using System.IO;

namespace PrismCAT
{
    public abstract class CAT_ColourComponent : MonoBehaviour
    {
        public enum ColourIndex { A, B, C, D, E, F, G, H, I, J };

        protected CAT_ColourManager colourManager;

        protected void Start()
        {
            colourManager = CAT_ColourManager.Instance;
            colourManager.addObject(this);
        }

        private void OnDestroy()
        {
            colourManager.removeObject(this);
        }

        abstract public void UpdateColour();
    }

    public abstract class CAT_ColourComponentEditor: Editor
    {
        [System.Serializable]
        public class PaletteData
        {
            public string[] Custom;
        }
        /// <summary>
        /// Used to show up-to-date dropdown lists of colours in editor interface
        /// </summary>
        protected Color[] LoadCustomPalette()
        {
            Color[] colours = new Color[10];
            string rutaArchivo = Application.dataPath + "/PrismCAT/Json/CustomPalette.json";
            string json = File.ReadAllText(rutaArchivo);
            PaletteData colordata = JsonUtility.FromJson<PaletteData>(json);
            for (int i = 0; i < colordata.Custom.Length; i++)
            {
                colours[i] = ColorUtility.TryParseHtmlString(colordata.Custom[i], out Color parsedColor) ? parsedColor : Color.white;
            }
            //Debug.Log("Loaded colour 0: " + colours[0]);
            //Debug.Log("Loaded colour 1: " + colours[1]);
            //Debug.Log("Loaded colour 2: " + colours[2]);
            //Debug.Log("Loaded colour 3: " + colours[3]);
            //Debug.Log("Loaded colour 4: " + colours[4]);
            //Debug.Log("Loaded colour 5: " + colours[5]);
            //Debug.Log("Loaded colour 6: " + colours[6]);
            //Debug.Log("Loaded colour 7: " + colours[7]);
            //Debug.Log("Loaded colour 8: " + colours[8]);
            //Debug.Log("Loaded colour 9: " + colours[9]);
            return colours;
        }






        [System.Serializable]
        public class ColorData
        {
            public string[] Protanopia;
            public string[] Deuteranopia;
            public string[] Tritanopia;
        }
        Color[,] AltPalettes = new Color[3, 10];
        protected void ReadFromJsonFile()
        {
            string rutaArchivo = Application.dataPath + "/PrismCAT/Json/ColourPalette.json";
            string json = File.ReadAllText(rutaArchivo);
            ColorData colordata = JsonUtility.FromJson<ColorData>(json);
            for (int i = 0; i < colordata.Protanopia.Length; i++)
            {
                AltPalettes[0, i] = ColorUtility.TryParseHtmlString(colordata.Protanopia[i], out Color parsedColor) ? parsedColor : Color.white;
                //ProtanopiaPalette[i] = ColorUtility.TryParseHtmlString(colordata.Protanopia[i], out Color parsedColor) ? parsedColor : Color.white;
            }
            for (int i = 0; i < colordata.Deuteranopia.Length; i++)
            {
                AltPalettes[1, i] = ColorUtility.TryParseHtmlString(colordata.Deuteranopia[i], out Color parsedColor) ? parsedColor : Color.white;
                //DeuteranopiaPalette[i] = ColorUtility.TryParseHtmlString(colordata.Deuteranopia[i], out Color parsedColor) ? parsedColor : Color.white;
            }
            for (int i = 0; i < colordata.Tritanopia.Length; i++)
            {
                AltPalettes[2, i] = ColorUtility.TryParseHtmlString(colordata.Tritanopia[i], out Color parsedColor) ? parsedColor : Color.white;
                //TritanopiaPalette[i] = ColorUtility.TryParseHtmlString(colordata.Tritanopia[i], out Color parsedColor) ? parsedColor : Color.white;
            }
            Debug.Log("Loaded colour 0: " + AltPalettes[0, 0]);
            Debug.Log("Loaded colour 1: " + AltPalettes[0, 1]);
            Debug.Log("Loaded colour 2: " + AltPalettes[0, 2]);
            Debug.Log("Loaded colour 3: " + AltPalettes[0, 3]);
            Debug.Log("Loaded colour 4: " + AltPalettes[0, 4]);
            Debug.Log("Loaded colour 5: " + AltPalettes[0, 5]);
            Debug.Log("Loaded colour 6: " + AltPalettes[0, 6]);
            Debug.Log("Loaded colour 7: " + AltPalettes[0, 7]);
            Debug.Log("Loaded colour 8: " + AltPalettes[0, 8]);
            Debug.Log("Loaded colour 9: " + AltPalettes[0, 9]);
        }
    }
}
