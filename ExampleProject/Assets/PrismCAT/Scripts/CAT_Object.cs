using UnityEngine;
using UnityEngine.UI;

namespace PrismCAT
{
    public class CAT_Object : CAT_ColourComponent
    {
        private int size = 10;
        [SerializeField, Range(0, 9)] int colour;
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
            colour = Mathf.Clamp(colour, 0, size - 1);
            //gameObject.GetComponent<MeshRenderer>().material.color = colourManager.GetColour((int)colour);
        }

        private void OnDestroy()
        {
            colourManager.removeObject(this);
        }

        public void setColour(int c)
        {
            colour = c;
            UpdateColour();
        }

        public override void UpdateColour()
        {
            renderer.material.color = colourManager.GetColour((int)colour);
        }

        public override void OnValidateSize(int s)
        {
            size = s;
            colour = Mathf.Clamp(colour, 0, size - 1);
        }
    }
}
