using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SyncXRSimpleInteractables
{
    [MenuItem("Tools/ISS/Sync Selected Guided Tour XR Simple Interactables")]
    public static void SyncSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No Guided Tour objects selected.");
            return;
        }

        Dictionary<string, GameObject> freeRoamLookup = BuildFreeRoamLookup();

        int updated = 0;
        int skipped = 0;

        foreach (GameObject guidedObj in selectedObjects)
        {
            if (guidedObj == null)
                continue;

            if (!freeRoamLookup.TryGetValue(guidedObj.name, out GameObject freeRoamObj))
            {
                Debug.LogWarning($"No Free Roam match found for: {guidedObj.name}");
                skipped++;
                continue;
            }

            XRSimpleInteractable sourceInteractable = freeRoamObj.GetComponent<XRSimpleInteractable>();
            if (sourceInteractable == null)
            {
                Debug.LogWarning($"Free Roam object {freeRoamObj.name} does not have XR Simple Interactable.");
                skipped++;
                continue;
            }

            Undo.RegisterCompleteObjectUndo(guidedObj, "Sync XR Simple Interactable");

            XRSimpleInteractable targetInteractable = guidedObj.GetComponent<XRSimpleInteractable>();
            if (targetInteractable == null)
            {
                targetInteractable = Undo.AddComponent<XRSimpleInteractable>(guidedObj);
            }

            EditorUtility.CopySerialized(sourceInteractable, targetInteractable);

            EditorUtility.SetDirty(guidedObj);
            EditorUtility.SetDirty(targetInteractable);

            updated++;
        }

        Debug.Log($"Sync complete. Updated {updated} objects. Skipped {skipped} objects.");
    }

    private static Dictionary<string, GameObject> BuildFreeRoamLookup()
    {
        Dictionary<string, GameObject> lookup = new Dictionary<string, GameObject>();

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj == null)
                continue;

            if (EditorUtility.IsPersistent(obj))
                continue;

            if (!obj.scene.isLoaded)
                continue;

            // Change this scene name check if needed
            if (obj.scene.name == "FreeRoamScene")
            {
                if (!lookup.ContainsKey(obj.name))
                {
                    lookup[obj.name] = obj;
                }
            }
        }

        return lookup;
    }
}