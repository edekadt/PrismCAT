using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace PrismCAT
{
    public class CAT_Image : CAT_ColourComponent
    {
        [SerializeField] ColourIndex colour = ColourIndex.A;
        //[SerializeField] float colourTransparency;

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

    [CustomEditor(typeof(CAT_Image))]
    [CanEditMultipleObjects]
    public class CAT_Image_Editor : CAT_ColourComponentEditor
    {
        SerializedProperty colour;

        void OnEnable()
        {
            colour = serializedObject.FindProperty("colour");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(colour);
            serializedObject.ApplyModifiedProperties();
            if (colour.intValue < 2)
            {
                EditorGUILayout.LabelField("(Low value)");
            }
            if (colour.intValue > 7)
            {
                EditorGUILayout.LabelField("(High value)");
            }
            EditorGUILayout.ColorField(LoadCustomPalette()[colour.intValue]);
            //ReadFromJsonFile();
            //LoadCustomPalette();
        }
    }
}
