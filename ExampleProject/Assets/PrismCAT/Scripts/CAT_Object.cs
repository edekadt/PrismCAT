using UnityEngine;
using UnityEngine.UI;

namespace PrismCAT
{
    public class CAT_Object : CAT_ColourComponent
    {
        [SerializeField] ColourIndex colour = ColourIndex.A;
        //[SerializeField] float colourTransparency;

        private Renderer renderer;

        private new void Start()
        {
            base.Start();
            renderer = GetComponent<Renderer>();
            if (renderer == null)
                Debug.LogError("CAT_Object added to object with no Renderer component.");
            UpdateColour();
        }

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

        public override void UpdateColour()
        {
            renderer.material.color = colourManager.GetColour((int)colour);
        }
    }
}
