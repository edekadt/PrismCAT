using UnityEngine;
using UnityEngine.UI;

namespace PrismCAT
{
    public class CAT_Image : CAT_ColourComponent
    {
        [SerializeField] ColourIndex colour = ColourIndex.A;
        [SerializeField] float colourTransparency;

        private Image image;

        private new void Start()
        {
            base.Start();
            image = GetComponent<Image>();
            if (image == null)
                Debug.LogError("CAT_Image added to object with no Image component.");
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
            image.color = colourManager.GetColour((int)colour);
        }
    }
}
