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

        /// <summary>
        /// A set of three alternate palettes that PrismCAT will use to replace the colours in CustomPalette.
        /// The order of these colours will be recalculated at the start of the program to best match the similarities and
        /// differences between the base colours.
        /// </summary>
        Color[][] AltPalettes;

        /// <summary>
        /// Given a list of colours via CustomPalette, this function calculates the relative similarity between each pair of colours.
        /// </summary>
        void GenerateColourGraph()
        {

        }


        void Start()
        {

        }

        void Update()
        {

        }
    }
}
