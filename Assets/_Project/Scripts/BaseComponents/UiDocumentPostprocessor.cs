using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UiDocumentPostprocessor : AssetPostprocessor
{
    public static UnityEvent<string> OnUiDocumentUpdated = new UnityEvent<string>();

    // This method is called after importing any asset
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (string importedAsset in importedAssets)
        {
            // Check if the imported asset is of a specific type (e.g., .png)
            if (importedAsset.EndsWith(".uxml", System.StringComparison.OrdinalIgnoreCase))
            {
                // Run your custom function here
                EditorApplication.delayCall += () => MyCustomFunction(importedAsset);
            }
        }
    }

    private static void MyCustomFunction(string importedAsset)
    {
        // Your custom logic here
        //Debug.Log("Asset of type .uxml imported: " + importedAsset);
        OnUiDocumentUpdated.Invoke(Path.GetFileName(importedAsset));
        EditorApplication.delayCall -= () => MyCustomFunction(importedAsset);
    }
}