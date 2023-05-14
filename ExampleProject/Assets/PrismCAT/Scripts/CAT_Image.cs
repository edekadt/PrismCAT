using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace PrismCAT
{
    public class CAT_Image : CAT_ColourComponent
    {
        private int size = 10;
        [SerializeField, Range(0, 9)] int colour;

        private Image image;

        private new void Start()
        {
            base.Start();
            image = GetComponent<Image>();
            if (image == null)
                Debug.LogError("CAT_Image added to object with no Image component.");
            UpdateColour();
        }

        private void OnValidate()
        {
            colour = Mathf.Clamp(colour, 0, size - 1);
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
            image.color = colourManager.GetColour(colour);
        }

        public override void OnValidateSize(int s)
        {
            size = s;
            colour = Mathf.Clamp(colour, 0, size - 1);
        }
    }

    [CustomEditor(typeof(CAT_Image))]
    [CanEditMultipleObjects]
    public class CAT_Image_Editor : CAT_ColourComponentEditor
    {
        SerializedProperty colour;

        void OnEnable()
        {
            colour = serializedObject.FindProperty("colour");
        }

        /// <summary>
        /// Creates a slider that allows the developer to change 
        /// the number of colors
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(colour);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.ColorField(LoadCustomPalette()[colour.intValue]);
        }
    }
}
