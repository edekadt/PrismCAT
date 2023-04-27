using UnityEngine;

namespace PrismCat
{
    public class CAT_ColourComponent : MonoBehaviour
    {
        enum colourIndex { A, B, C, D, E, F, G, H, I, J };

        [SerializeField] colourIndex colour = colourIndex.A;
        //PrismCat::CAT_ColourManager colourManager;

        void Start()
        {
            //colourManager = CAT_ColourManager.instance();
            //colourManager.addComponent(this);
        }

        // Called when a variable changes on the inspector
        private void OnValidate()
        {
            Debug.Log("Nuevo index: " + (int)colour + "\n");
            //gameObject.GetComponent<MeshRenderer>().material.color = colourManager.GetColour((int)colour);
        }

        public void UpdateColour(int c)
        {
            //gameObject.GetComponent<MeshRenderer>().material.color = colourManager.GetColour((int)colour);
        }
    }
}
