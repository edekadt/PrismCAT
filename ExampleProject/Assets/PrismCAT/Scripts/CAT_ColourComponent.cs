using UnityEngine;

namespace PrismCAT
{
    public abstract class CAT_ColourComponent : MonoBehaviour
    {
        public enum ColourIndex { A, B, C, D, E, F, G, H, I, J };

        protected CAT_ColourManager colourManager;

        protected void Start()
        {
            colourManager = CAT_ColourManager.Instance;
            colourManager.addObject(this);
        }

        private void OnDestroy()
        {
            colourManager.removeObject(this);
        }

        abstract public void UpdateColour();
    }
}
