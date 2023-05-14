using UnityEditor;
using UnityEngine;

namespace PrismCAT
{
    public class CAT_Object : CAT_ColourComponent
    {
        [Tooltip("If true, automatically sets object render mode to Transparent.")]
        [SerializeField] bool renderTransparent = false;
        
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

            if (renderTransparent)
                setTransparent();

            UpdateColour();
        }

        private void setTransparent()
        {
            Material mat = render.material;

            // Configura el modo de sombreado en "Transparent" para permitir la transparencia
            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
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
        SerializedProperty renderTransparent;
        SerializedProperty colour;
        SerializedProperty transparency;

        void OnEnable()
        {
            renderTransparent = serializedObject.FindProperty("renderTransparent");
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
            EditorGUILayout.PropertyField(renderTransparent);
            EditorGUILayout.PropertyField(colour);
            EditorGUILayout.PropertyField(transparency);
            serializedObject.ApplyModifiedProperties();

            GUI.enabled = false;
            EditorGUILayout.ColorField(GUIContent.none, LoadCustomPalette()[colour.intValue], false, true, false);
            GUI.enabled = true;
        }
    }
}
