using UnityEngine;
using UnityEngine.UI;

namespace PrismCAT
{
    public class CAT_ColourComponent : MonoBehaviour
    {
        enum ColourIndex { A, B, C, D, E, F, G, H, I, J };

        [SerializeField] ColourIndex colour = ColourIndex.A;
        CAT_ColourManager colourManager;

        void Start()
        {
            colourManager = CAT_ColourManager.Instance;
            colourManager.addObject(this);
        }

        // Called when a variable changes on the inspector
        private void OnValidate()
        {
            Debug.Log("Nuevo index: " + (int)colour + "\n");
            //gameObject.GetComponent<MeshRenderer>().material.color = colourManager.GetColour((int)colour);
        }

        private void OnDestroy()
        {
            colourManager.removeObject(this);
        }

        public void setColour(int c)
        {
            colour = (ColourIndex)c;
            UpdateColour();
        }

        public void UpdateColour()
        {
        }
    }
}
