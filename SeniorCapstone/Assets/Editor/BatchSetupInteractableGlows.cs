using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class BatchSetupInteractableGlows : EditorWindow
{
    private const string GlowChildName = "Glow";
    private const string GlowMaterialName = "Interactable_glow";

    // Final recommended subtle shell scale.
    private const float GlowScale = 1.02f;

    [MenuItem("Tools/Interactables/Setup Glow On Selected Objects")]
    public static void SetupGlowOnSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected. Select your interactable objects first.");
            return;
        }

        Material glowMaterial = FindGlowMaterial();

        if (glowMaterial == null)
        {
            Debug.LogError("Could not find material named '" + GlowMaterialName + "'.");
            return;
        }

        ConfigureGlowMaterial(glowMaterial);

        int setupCount = 0;
        int skippedCount = 0;

        foreach (GameObject obj in selectedObjects)
        {
            MeshFilter originalMeshFilter = obj.GetComponent<MeshFilter>();
            MeshRenderer originalRenderer = obj.GetComponent<MeshRenderer>();

            if (originalMeshFilter == null || originalRenderer == null)
            {
                Debug.LogWarning("Skipped " + obj.name + " because it does not have both MeshFilter and MeshRenderer.");
                skippedCount++;
                continue;
            }

            Undo.RegisterFullObjectHierarchyUndo(obj, "Setup Interactable Glow");

            GameObject glowObject = GetOrCreateGlowChild(obj);

            glowObject.transform.localPosition = Vector3.zero;
            glowObject.transform.localRotation = Quaternion.identity;
            glowObject.transform.localScale = Vector3.one * GlowScale;
            glowObject.SetActive(true);

            MeshFilter glowMeshFilter = glowObject.GetComponent<MeshFilter>();
            if (glowMeshFilter == null)
            {
                glowMeshFilter = glowObject.AddComponent<MeshFilter>();
            }

            glowMeshFilter.sharedMesh = originalMeshFilter.sharedMesh;

            MeshRenderer glowRenderer = glowObject.GetComponent<MeshRenderer>();
            if (glowRenderer == null)
            {
                glowRenderer = glowObject.AddComponent<MeshRenderer>();
            }

            glowRenderer.sharedMaterial = glowMaterial;
            glowRenderer.shadowCastingMode = ShadowCastingMode.Off;
            glowRenderer.receiveShadows = false;
            glowRenderer.enabled = true;

            RemoveExtraComponentsFromGlow(glowObject);

            InteractableGlow glowScript = obj.GetComponent<InteractableGlow>();
            if (glowScript == null)
            {
                glowScript = obj.AddComponent<InteractableGlow>();
            }

            // Final values.
            glowScript.glowRenderer = glowRenderer;

            glowScript.normalColor = new Color(0.2f, 0.7f, 1f, 0.08f);
            glowScript.normalEmissionStrength = 0.6f;

            glowScript.hoverColor = new Color(0.75f, 0.95f, 1f, 0.30f);
            glowScript.hoverEmissionStrength = 1.8f;

            EditorUtility.SetDirty(obj);
            EditorUtility.SetDirty(glowObject);
            EditorUtility.SetDirty(glowMaterial);

            setupCount++;
        }

        Debug.Log("Glow setup complete. Set up " + setupCount + " object(s). Skipped " + skippedCount + " object(s).");
    }

    private static GameObject GetOrCreateGlowChild(GameObject parent)
    {
        Transform existingGlow = parent.transform.Find(GlowChildName);

        if (existingGlow != null)
        {
            return existingGlow.gameObject;
        }

        GameObject glowObject = new GameObject(GlowChildName);
        Undo.RegisterCreatedObjectUndo(glowObject, "Create Glow Object");
        glowObject.transform.SetParent(parent.transform);

        return glowObject;
    }

    private static void RemoveExtraComponentsFromGlow(GameObject glowObject)
    {
        Collider collider = glowObject.GetComponent<Collider>();
        if (collider != null)
        {
            Undo.DestroyObjectImmediate(collider);
        }

        Rigidbody rb = glowObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Undo.DestroyObjectImmediate(rb);
        }

        InteractableGlow glowScript = glowObject.GetComponent<InteractableGlow>();
        if (glowScript != null)
        {
            Undo.DestroyObjectImmediate(glowScript);
        }
    }

    private static Material FindGlowMaterial()
    {
        string[] guids = AssetDatabase.FindAssets(GlowMaterialName + " t:Material");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && mat.name == GlowMaterialName)
            {
                return mat;
            }
        }

        return null;
    }

    private static void ConfigureGlowMaterial(Material mat)
    {
        // These settings assume URP/Lit.
        mat.SetFloat("_Surface", 1); // Transparent
        mat.SetFloat("_Blend", 0);   // Alpha
        mat.SetFloat("_Cull", 2);    // Front face rendering. Use 1 for Back if you prefer outline-only.
        mat.SetFloat("_AlphaClip", 0);

        mat.SetColor("_BaseColor", new Color(0.2f, 0.7f, 1f, 0.08f));
        mat.SetColor("_EmissionColor", new Color(0.2f, 0.7f, 1f, 1f) * 0.6f);

        mat.EnableKeyword("_EMISSION");
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");

        mat.renderQueue = (int)RenderQueue.Transparent;
    }
}