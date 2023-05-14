using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PrismCAT
{
    public class CAT_Object : CAT_ColourComponent
    {
        private int size = 10;
        [SerializeField, Range(0, 9)] int colour;

        private Renderer render;

        private new void Start()
        {
            base.Start();
            render = GetComponent<Renderer>();
            if (render == null)
                Debug.LogError("CAT_Object added to object with no Renderer component.");
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
            render.material.color = colourManager.GetColour((int)colour);
        }

        public override void OnValidateSize(int s)
        {
            size = s;
            colour = Mathf.Clamp(colour, 0, size - 1);
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

            EditorGUILayout.ColorField(LoadCustomPalette()[colour.intValue]);
        }
    }
}
