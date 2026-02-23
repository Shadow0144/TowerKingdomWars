using UnityEngine;

public class PathTile : Tile
{
    public override void Initialize(uint row, uint column)
    {
        ContainsStructure = true; // A path is itself a structure
        base.Initialize(row, column);
    }

    public void SetNeighbors(bool northIsPath, bool eastIsPath, bool southIsPath, bool westIsPath)
    {
        string path = "Textures/PathTileTexture";
        if (northIsPath)
        {
            path += "N";
        }
        if (eastIsPath)
        {
            path += "E";
        }
        if (southIsPath)
        {
            path += "S";
        }
        if (westIsPath)
        {
            path += "W";
        }
        Texture2D texture = Resources.Load<Texture2D>(path);
        _material.SetTexture("_MainTex", texture);
    }
}
