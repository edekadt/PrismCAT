using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace PrismCAT
{

    public class CAT_ColourManager : MonoBehaviour
    {
        [System.Serializable]
        public class ColorData
        {
            public String[] Protanopia;
            public String[] Deuteranopia;
            public String[] Tritanopia;
        }
        public enum Palette { Default, Protanopia, Deuteranopia, Tritanopia };

        #region attributes

        [SerializeField, Range(1, 10)] int SIZE = 10; // Number of colours supported.

        [Tooltip("Palette of up to 10 colours that PrismCAT will replace if colourblind settings are enabled.\n" +
            "These colours can be assigned to any GameObject with a CAT_ColourComponent. Note that the ColourComponent" +
            " will overwrite any other materials present on the object.")]
        [SerializeField] public Color[] CustomPalette;
        [SerializeField] public Color[] ProtanopiaPalette;
        [SerializeField] public Color[] DeuteranopiaPalette;
        [SerializeField] public Color[] TritanopiaPalette;
        public Palette currentPalette;

        /// <summary>
        /// A set of three alternate palettes that PrismCAT will use to replace the colours in CustomPalette.
        /// The order of these colours will be recalculated at the start of the program to best match the similarities and
        /// differences between the base colours.
        /// </summary>
        Color[,] AltPalettes;

        /// <summary>
        /// List of objects with CAT_ColourComponents that need to be recoloured when settings are changed
        /// </summary>
        List<CAT_ColourComponent> managedObjects;

        #endregion

        #region public functions

        public void addObject(CAT_ColourComponent obj)
        {
            int size = managedObjects.Count;
            managedObjects.Add(obj);
        }

        public void removeObject(CAT_ColourComponent obj)
        {
            managedObjects.Remove(obj);
        }

        /// <summary>
        /// Returns the colour corresponding to the passed index based on the current Colourblind setting.
        /// </summary>
        public Color GetColour(int index)
        {
            Color colour;
            switch (currentPalette)
            {
                case Palette.Default:
                    colour = CustomPalette[index];
                    break;
                case Palette.Protanopia:
                    colour = AltPalettes[0, index];
                    break;
                case Palette.Deuteranopia:
                    colour = AltPalettes[1, index];
                    break;
                case Palette.Tritanopia:
                    colour = AltPalettes[2, index];
                    break;
                default:
                    Debug.LogError("Unknown colour palette active.");
                    colour = Color.black;
                    break;
            }
            //colour.r *= index / 10f;
            //colour.g *= index / 10f;
            //colour.b *= index / 10f;
            return colour;
        }

        public static CAT_ColourManager Instance { get; private set; }

        #endregion

        #region private functions

        float ColourDifference(Color a, Color b)
        {
            return 0.3f * Mathf.Pow(a.r - b.r, 2) + 0.59f * Mathf.Pow(a.g - b.g, 2) + 0.11f * Mathf.Pow(a.b - b.b, 2);
        }

        /// <summary>
        /// This function reorders the colours in each of the alternate palettes to best match the custom palette in terms of brightness.
        /// </summary>
        void ReorderAltPalettes()
        {
            // Sort the CustomPalette by brightness
            Color[] orderedPalette = (Color[])CustomPalette.Clone();
            Array.Sort(orderedPalette, new BrightnessComparer());

            // Apply those changes to the other palettes
            //Color[,] reorderedPalettes = new Color[3, SIZE];
            Color[] test = (Color[])ProtanopiaPalette.Clone(); // <-- Esto desaparecer� para a�adir la l�nea de arriba

            for (int i = 0; i < SIZE; i++)
            {
                int pos = Array.IndexOf(CustomPalette, orderedPalette[i]);
                //for(int j = 0; j < 3; j++)
                //    reorderedPalettes[j, i] = AltPalettes[j, pos];
                test[i] = ProtanopiaPalette[pos]; // <-- Esto desaparecer� para a�adir las 2 l�neas de arriba
            }

            //AltPalettes = reorderedPalettes;
            ProtanopiaPalette = test; // <-- Esto desaparecer� para a�adir la l�nea de arriba
            CustomPalette = orderedPalette; // <-- Esto desaparecer�. Es para ver c�mo queda la default ordenada
        }

        private void updateObjects()
        {
            foreach(CAT_ColourComponent obj in managedObjects)
            {
                obj.UpdateColour();
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.Log("Warning: Multiple CAT_ColourManager instances detected.");
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            managedObjects = new List<CAT_ColourComponent>();
            if (CustomPalette.Length != ProtanopiaPalette.Length ||
                CustomPalette.Length != DeuteranopiaPalette.Length ||
                CustomPalette.Length != TritanopiaPalette.Length)
                Debug.LogError("Warning: All colour palettes must contain the same number of elements.");
            AltPalettes = new Color[3, SIZE];
            ReadFromJsonFile();
            ReorderAltPalettes();
        }

        /// <summary>
        /// Assign each managed object the colour corresponding to the current colourblind setting.
        /// </summary>
        void Start()
        {
            foreach(CAT_ColourComponent obj in managedObjects)
            {
                obj.UpdateColour();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentPalette = (Palette)((int)(currentPalette + 1) % 2);
                updateObjects();
            }
        }
        private void ReadFromJsonFile()
        {
            string rutaArchivo = Application.dataPath + "/PrismCAT/Json/ColourPalette.json";
            string json = File.ReadAllText(rutaArchivo);
            ColorData colordata = JsonUtility.FromJson<ColorData>(json);
            for(int i = 0; i < colordata.Protanopia.Length; i++)
            {
                AltPalettes[0,i] = ColorUtility.TryParseHtmlString(colordata.Protanopia[i], out Color parsedColor) ? parsedColor : Color.white;
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
            //Color colorA = ColorUtility.TryParseHtmlString(prota.A, out Color parsedColor) ? parsedColor : Color.white;
        }
        #endregion
    }

    public class BrightnessComparer : IComparer<Color>
    {
        public int Compare(Color x, Color y)
        {
            if (x.grayscale < y.grayscale) return 1;
            else if (x.grayscale > y.grayscale) return -1;
            else return 0;
        }
    }
}
