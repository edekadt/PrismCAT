using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrismCAT
{
    public class CAT_ColourManager : MonoBehaviour
    {
        public enum Palette { Default, Propanopia, Deuteranopia, Tritanopia };

        #region attributes

        [SerializeField] int SIZE = 10; // Number of colours supported.

        [Tooltip("Palette of up to 10 colours that PrismCAT will replace if colourblind settings are enabled.\n" +
            "These colours can be assigned to any GameObject with a CAT_ColourComponent. Note that the ColourComponent" +
            "will overwrite any other materials present on the object.")]
        [SerializeField] public Color[] CustomPalette;
        public Palette currentPalette;

        /// <summary>
        /// A set of three alternate palettes that PrismCAT will use to replace the colours in CustomPalette.
        /// The order of these colours will be recalculated at the start of the program to best match the similarities and
        /// differences between the base colours.
        /// </summary>
        Color[,] AltPalettes;

        /// <summary>
        /// A graph for each palette, that represents how different two colours from the same palette are. 
        /// Each graph is shaped SIZExSIZE, and has zeros on the main diagonal.
        /// 
        /// These graphs are only used for calculating the order of the palettes
        /// </summary>
        float[,] CustomColorGraph;
        float[,] PropanopiaColorGraph;
        float[,] DeuteranopiaColorGraph;
        float[,] TritanopiaColorGraph;

        /// <summary>
        /// List of objects with CAT_ColourComponents that need to be recoloured when settings are changed
        /// </summary>
        List<CAT_ColourComponent> ManagedObjects;

        #endregion

        #region public functions

        public void addObject(CAT_ColourComponent obj)
        {
            int size = ManagedObjects.Count;
            ManagedObjects.Add(obj);
            if (ManagedObjects.Count > size)
                Debug.Log("Object added successfully");
            else
                Debug.Log("Error adding object");
        }

        public void removeObject(CAT_ColourComponent obj)
        {
            ManagedObjects.Remove(obj);
        }

        /// <summary>
        /// Returns the colour corresponding to the passed index based on the current Colourblind setting.
        /// </summary>
        public Color GetColour(int index)
        {
            return Color.black;
        }

        public static CAT_ColourManager Instance { get; private set; }

        #endregion

        #region private functions

        float ColourDifference(Color a, Color b)
        {
            return 0.3f * Mathf.Pow(a.r - b.r, 2) + 0.59f * Mathf.Pow(a.g - b.g, 2) + 0.11f * Mathf.Pow(a.b - b.b, 2);
        }

        /// <summary>
        /// Given a list of colours via CustomPalette, this function calculates the relative similarity between each pair of colours,
        /// storing this information in a graph.
        /// </summary>
        void GenerateColourGraph()
        {
            CustomColorGraph = new float[SIZE, SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    float dif =ColourDifference(CustomPalette[i], CustomPalette[j]);
                    CustomColorGraph[i, j] = dif;
                    CustomColorGraph[j, i] = dif;
                }
            }
        }

        /// <summary>
        /// Given the graphs produced by GenerateColourGraph, this function reorders the colours in each of the alternate palettes
        /// to best match the similarites and differences of the custom palette.
        /// </summary>
        void ReorderAltPalettes()
        {

        }

        private void updateObjects()
        {
            foreach(CAT_ColourComponent obj in ManagedObjects)
            {
                obj.UpdateColour();
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.Log("Singleton already exists");
                Destroy(this);
            }
            else
            {
                Instance = this;
                Debug.Log("Singleton instantiated");
            }
            ManagedObjects = new List<CAT_ColourComponent>();
        }

        /// <summary>
        /// Assign each managed object the colour corresponding to the current colourblind setting.
        /// </summary>
        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentPalette = (Palette)((int)(currentPalette + 1) % 4);
                updateObjects();
            }
        }


        #endregion
    }
}
