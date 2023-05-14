using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PrismCAT
{
    public class CAT_Material : CAT_ColourComponent
    {
        Material mat;

        public override void UpdateColour()
        {
            if (colourManager != null)
            {
                // Aplica el nuevo color con transparencia
                Color matColor = mat.color;
                float alpha = matColor.a * colourManager.GetColour(colour).a;
                Color newColor = colourManager.GetColour(colour) * alpha + matColor * (1 - alpha);
                newColor.a = matColor.a;
                mat.color = newColor;
            }
        }

        // Start is called before the first frame update
        private new void Start()
        {
            base.Start();
            Renderer rend = GetComponent<Renderer>();
            mat = rend.material;

            // Configura el modo de sombreado en "Transparent" para permitir la transparencia
            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;

            UpdateColour();
        }
    }

    [CustomEditor(typeof(CAT_Material))]
    [CanEditMultipleObjects]
    public class CAT_Material_Editor : CAT_ColourComponentEditor
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

            GUI.enabled = false;
            EditorGUILayout.ColorField(GUIContent.none, LoadCustomPalette()[colour.intValue], false, true, false);
            GUI.enabled = true;
        }
    }
}