using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace PrismCAT
{
    public class CAT_Image : CAT_ColourComponent
    {
        private int size = 10;
        [SerializeField, Range(0, 9)] int colour;
        
        [Tooltip ("Transparency at which the indexed colour is applied over the base image. \n" +
            "At 0, colour is applied in full opacity. At 1, overlayed colour is fully transparent.")]
        [SerializeField, Range(0f, 1f)] float colourTransparency;

        private Image image;
        private int initialColour;
        private new void Start()
        {
            base.Start();
            image = GetComponent<Image>();
            if (image == null)
                Debug.LogError("CAT_Image added to object with no Image component.");
            UpdateColour();
            initialColour = colour;
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
            colour = Mathf.Clamp(initialColour, 0, size - 1);
        }
    }

    [CustomEditor(typeof(CAT_Image))]
    [CanEditMultipleObjects]
    public class CAT_Image_Editor : CAT_ColourComponentEditor
    {
        SerializedProperty colour;
        SerializedProperty transparency;

        void OnEnable()
        {
            colour = serializedObject.FindProperty("colour");
            transparency = serializedObject.FindProperty("colourTransparency");
        }

        /// <summary>
        /// Creates a slider that allows the developer to change 
        /// the number of colors
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(colour);
            EditorGUILayout.PropertyField(transparency);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.ColorField(LoadCustomPalette()[colour.intValue]);
        }
    }
}
