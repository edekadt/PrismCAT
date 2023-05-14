using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PrismCAT
{
    public class CAT_Object : CAT_ColourComponent
    {
        [Tooltip("Transparency at which the indexed colour is applied over the base image. \n" +
            "At 0, colour is applied in full opacity. At 1, overlayed colour is fully transparent.")]
        [SerializeField, Range(0f, 1f)] float colourTransparency;

        private Renderer render;

        // Initial colour of the object
        Color baseColour;

        private new void Start()
        {
            base.Start();
            render = GetComponent<Renderer>();
            if (render == null)
                Debug.LogError("CAT_Object added to object with no Renderer component.");
            baseColour = render.material.color;
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
                render.material.color = Color.Lerp(colourManager.GetColour(colour), baseColour, colourTransparency);
        }
    }

    [CustomEditor(typeof(CAT_Object))]
    [CanEditMultipleObjects]
    public class CAT_Object_Editor : CAT_ColourComponentEditor
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

            GUI.enabled = false;
            EditorGUILayout.ColorField(GUIContent.none, LoadCustomPalette()[colour.intValue], false, true, false);
            GUI.enabled = true;
        }
    }
}
