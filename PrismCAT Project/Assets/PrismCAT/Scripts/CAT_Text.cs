using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace PrismCAT
{
    public class CAT_Text : CAT_ColourComponent
    {
        [Tooltip("Transparency at which the indexed colour is applied over the base image. \n" +
            "At 0, colour is applied in full opacity. At 1, overlayed colour is fully transparent.")]
        [SerializeField, Range(0f, 1f)] float colourTransparency;

        private Text text;

        // Initial colour of the text, over which the indexed colour is applied at the desired transparency. 
        Color baseColour;

        private new void Start()
        {
            base.Start();
            text = GetComponent<Text>();
            if (text == null)
                Debug.LogError("CAT_Text added to object with no Text component.");
            baseColour = text.color;

            UpdateColour();
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
            if (colourManager != null)
                text.color = Color.Lerp(colourManager.GetColour(colour), baseColour, colourTransparency);
        }
    }

    [CustomEditor(typeof(CAT_Text))]
    [CanEditMultipleObjects]
    public class CAT_Text_Editor : CAT_ColourComponentEditor
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

            GUI.enabled = false;
            EditorGUILayout.ColorField(GUIContent.none, LoadCustomPalette()[colour.intValue], false, true, false);
            GUI.enabled = true;
        }
    }
}

