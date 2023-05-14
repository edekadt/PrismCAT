using UnityEngine;
using UnityEditor;
using System.IO;

namespace PrismCAT
{
    public abstract class CAT_ColourComponent : MonoBehaviour
    {
        protected int size = 10;

        [SerializeField, Range(0, 9)] protected int colour;
        public enum ColourIndex { A, B, C, D, E, F, G, H, I, J };

        protected CAT_ColourManager colourManager;

        protected void Start()
        {
            colourManager = CAT_ColourManager.Instance;
            if (colourManager == null)
                Debug.LogError("Error: CAT_ColourComponent instantiated with no CAT_ColourManager in scene." +
                    " To enable PrismCAT components to work, add the script CAT_ColourManager to an object in the scene" +
                    " or instantiate the provided prefab of the same name.");
            colourManager.addObject(this);
        }

        private void OnDestroy()
        {
            colourManager.removeObject(this);
        }

        abstract public void UpdateColour();

        private void OnValidate()
        {
            colour = Mathf.Clamp(colour, 0, size - 1);
            UpdateColour();
        }
        public void OnValidateSize(int s)
        {
            size = s;
        }
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
