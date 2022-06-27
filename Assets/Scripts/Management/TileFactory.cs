using System.Collections.Generic;
using UnityEngine;

public static class TileFactory
{
    public static Dictionary<int, GameObject> SymbolsPrefab;
    public static void Initialize()
    {
        int i = 1;
        SymbolsPrefab = new Dictionary<int, GameObject>();
        foreach (GameObject symbol in Resources.LoadAll("Prefabs/Symbols", typeof(GameObject)))
        {
            SymbolsPrefab.Add(i, symbol);
            i++;
        }
    }

    public static GameObject CreateTile(int i)
    {
        if (!SymbolsPrefab.ContainsKey(i))
            throw new System.Exception("Cube assigned didn't exists");
        return SymbolsPrefab[i];
    }
}
