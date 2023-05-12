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

        abstract public void OnValidateSize(int s);
    }

    public abstract class CAT_ColourComponentEditor: Editor
    {
        /// <summary>
        /// Used to show up-to-date dropdown lists of colours in editor interface
        /// </summary>
        protected Color[] LoadCustomPalette()
        {
            Color[] colours = new Color[10];
            string rutaArchivo = Application.dataPath + "/PrismCAT/Json/CustomPalette.json";
            string json = File.ReadAllText(rutaArchivo);
            CAT_ColourManager.PaletteData colordata = JsonUtility.FromJson<CAT_ColourManager.PaletteData>(json);
            for (int i = 0; i < colordata.Custom.Length; i++)
            {
                colours[i] = ColorUtility.TryParseHtmlString(colordata.Custom[i], out Color parsedColor) ? parsedColor : Color.white;
            }
            return colours;
        }
    }
}
