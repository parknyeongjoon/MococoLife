using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MococoTile : Tile
{

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/GroundTile")]
    public static void CreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save MococoTile", "New MococoTile", "asset", "Save MococoTile", "Assets/Sprite/Tile");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MococoTile>(), path);
    }

#endif
}
