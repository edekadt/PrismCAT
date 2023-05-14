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

        [System.Serializable]
        public class PaletteData
        {
            public string[] Custom;
        }

        public enum Palette { Default, Protanopia, Deuteranopia, Tritanopia };

        #region attributes
        [Tooltip("Enables switching between palettes at a single button press.")]
        [SerializeField] bool debug = false;

        [Tooltip("Enables edge-case warnings.")]
        [SerializeField] bool showWarnings = true;

        [Tooltip("Input axis that switches between colour palettes cyclically.")]
        [SerializeField] string debugButton;

        [SerializeField, Range(1, 10)] int SIZE = 10; // Number of colours supported.

        private int lastSize = 10; // To check if SIZE has changed

        private const int numberOfPalettes = 4;

        [SerializeField] Palette currentPalette;

        [Tooltip("Palette of up to 10 colours that PrismCAT will replace if colourblind settings are enabled.\n" +
            "These colours can be assigned to any GameObject with a CAT_ColourComponent. Note that the ColourComponent" +
            " will overwrite any other materials present on the object.")]
        [SerializeField] public Color[] CustomPalette;

        /// <summary>
        /// A set of three alternate palettes that PrismCAT will use to replace the colours in CustomPalette.
        /// The order of these colours will be recalculated at the start of the program to best match the similarities and
        /// differences between the base colours.
        /// </summary>
        Color[,] AltPalettes;

        /// <summary>
        /// The colours read from the json file to set the AltPalettes array. The size of this array will be 10 colours per 
        /// palette while the size of AltPalettes may be smaller.
        /// </summary>
        Color[,] JsonPalettes;

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
        /// If the index is greater than the number of colours in use (SIZE), the returned colour is equal 
        /// </summary>
        public Color GetColour(int index)
        {
            int clampedIndex = Math.Clamp(index, 0, SIZE - 1);
            if (clampedIndex != index)
                Debug.LogWarning("Requested colour index is greater than number of colours in use. " +
                    "The highest indexed colour within limits has been returned instead.");
            Color colour;
            switch (currentPalette)
            {
                case Palette.Default:
                    colour = CustomPalette[clampedIndex];
                    break;
                case Palette.Protanopia:
                    colour = AltPalettes[0, clampedIndex];
                    break;
                case Palette.Deuteranopia:
                    colour = AltPalettes[1, clampedIndex];
                    break;
                case Palette.Tritanopia:
                    colour = AltPalettes[2, clampedIndex];
                    break;
                default:
                    Debug.LogError("Unknown colour palette active.");
                    colour = Color.black;
                    break;
            }

            return colour;
        }

        /// <summary>
        /// Method that changes the current palette to a specific one.
        /// </summary>
        /// <param name="palette">Palettes available: Default, Protanopia, Deuteranopia and Tritanopia </param>
        public void ChangePalette(Palette palette)
        {
            currentPalette = palette;
            updateObjects();
        }

        public Palette GetPalette()
        {
            return currentPalette;
        }

        public static CAT_ColourManager Instance { get; private set; }

        #endregion

        #region private functions

        private void updateObjects()
        {
            if (managedObjects == null) return;

            foreach (CAT_ColourComponent obj in managedObjects)
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
            LoadPalettes();
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
            if (debug && Input.GetButtonDown(debugButton))
            {
                currentPalette = (Palette)((int)(currentPalette + 1) % numberOfPalettes);
                updateObjects();
            }
        }

        /// <summary>
        /// Writes any changes in the palette's colours to CustomPalette.json
        /// </summary>
        private void OnValidate()
        {
            string path = Application.dataPath + "/PrismCAT/Json/CustomPalette.json";
            PaletteData customPaletteData = new PaletteData();
            customPaletteData.Custom = new string[10];
            int i = 0;
            foreach (Color c in CustomPalette)
            {
                string hex = ColorUtility.ToHtmlStringRGB(c);
                customPaletteData.Custom[i] = "#" + hex;
                Debug.Log(customPaletteData.Custom[i]);
                ++i;
            }
            File.WriteAllText(path, JsonUtility.ToJson(customPaletteData, true));

            if(lastSize != SIZE)
            {
                lastSize = SIZE;
                CAT_ColourComponent[] components = FindObjectsOfType<CAT_ColourComponent>();
                foreach (CAT_ColourComponent comp in components)
                {
                    comp.OnValidateSize(SIZE);
                }
            }

            updateObjects();
        }

        private void LoadPalettes()
        {
            AltPalettes = new Color[3, 10];
            JsonPalettes = new Color[3, 10];
            ReadFromJsonFile();
            JsonToAltPalettes();
            ReorderAltPalettes();
        }

        /// <summary>
        /// Reads the palettes defined in the json file and loads them in the array JsonPalettes.
        /// </summary>
        private void ReadFromJsonFile()
        {
            string rutaArchivo = Application.dataPath + "/PrismCAT/Json/AltPalettes.json";
            string json = File.ReadAllText(rutaArchivo);
            ColorData colordata = JsonUtility.FromJson<ColorData>(json);
            for(int i = 0; i < colordata.Protanopia.Length; i++)
            {
                JsonPalettes[0,i] = ColorUtility.TryParseHtmlString(colordata.Protanopia[i], out Color parsedColor) ? parsedColor : Color.white;
            }
            for (int i = 0; i < colordata.Deuteranopia.Length; i++)
            {
                JsonPalettes[1, i] = ColorUtility.TryParseHtmlString(colordata.Deuteranopia[i], out Color parsedColor) ? parsedColor : Color.white;
            }
            for (int i = 0; i < colordata.Tritanopia.Length; i++)
            {
                JsonPalettes[2, i] = ColorUtility.TryParseHtmlString(colordata.Tritanopia[i], out Color parsedColor) ? parsedColor : Color.white;
            }
        }

        /// <summary>
        /// This function copies the colours from JsonPalettes to AltPalettes, considering that the size of AltPalettes may be smaller
        /// than the size of JsonPalettes.
        /// </summary>
        private void JsonToAltPalettes()
        {
            if(JsonPalettes.GetLength(1) == SIZE)
            {
                AltPalettes = JsonPalettes;
            }
            else
            {
                float step = (float)10 / SIZE;
                for(int i = 0; i < SIZE; i++)
                {
                    for(int j = 0; j < 3; j++)
                        AltPalettes[j, i] = JsonPalettes[j, Mathf.FloorToInt(i + step)];
                }
            }
        }

        /// <summary>
        /// This function reorders the colours in each of the alternate palettes to best match the custom palette in terms of brightness.
        /// </summary>
        void ReorderAltPalettes()
        {
            // Sort the CustomPalette by brightness
            Color[] orderedPalette = new Color[SIZE];
            for (int i = 0; i < SIZE; i++)
                orderedPalette[i] = CustomPalette[i];
            Array.Sort(orderedPalette, new BrightnessComparer());

            // Apply those changes to the other palettes
            Color[,] reorderedPalettes = new Color[3, SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                int pos = Array.IndexOf(CustomPalette, orderedPalette[i]);
                for (int j = 0; j < 3; j++)
                    reorderedPalettes[j, pos] = AltPalettes[j, i];
            }

            AltPalettes = reorderedPalettes;
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
