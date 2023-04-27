using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrismCAT
{
    public class CAT_ColourManager : MonoBehaviour
    {
        enum Palettes { Default, Propanopia, Deuteranopia, Tritanopia };

        [Tooltip("Palette of up to 10 colours that PrismCAT will replace if colourblind settings are enabled.\n " +
            "These colours can be assigned to any GameObject with a CAT_ColourComponent. Note that the ColourComponent" +
            "will overwrite any other materials present on the object.")]
        [SerializeField] public Color[] CustomPalette;

        public static CAT_ColourManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CAT_ColourManager();
                }
                return instance;
            }
        }

        public void addObject(CAT_ColourComponent obj)
        {
            ManagedObjects.Add(obj);
        }

        /// <summary>
        /// Returns the colour corresponding to the passed index based on the current Colourblind setting.
        /// </summary>
        public Color getColor(int index)
        {
            return Color.black;
        }


        private static CAT_ColourManager instance = null;

        /// <summary>
        /// A set of three alternate palettes that PrismCAT will use to replace the colours in CustomPalette.
        /// The order of these colours will be recalculated at the start of the program to best match the similarities and
        /// differences between the base colours.
        /// </summary>
        Color[][] AltPalettes;

        /// <summary>
        /// List of objects with CAT_ColourComponents that need to be recoloured when settings are changed
        /// </summary>
        List<CAT_ColourComponent> ManagedObjects;

        /// <summary>
        /// Given a list of colours via CustomPalette, this function calculates the relative similarity between each pair of colours,
        /// storing this information in a graph.
        /// </summary>
        void GenerateColourGraph()
        {

        }

        /// <summary>
        /// Given the graphs produced by GenerateColourGraph, this function reorders the colours in each of the alternate palettes
        /// to best match the similarites and differences of the custom palette.
        /// </summary>
        void ReorderAltPalettes()
        {

        }

        /// <summary>
        /// Assign each managed object the colour corresponding to the current colourblind setting.
        /// </summary>
        void Start()
        {

        }
    }
}
