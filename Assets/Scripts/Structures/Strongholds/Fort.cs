using System.Collections.Generic;
using UnityEngine;

public class Fort : Stronghold
{
    private static List<Vector2Int> _footprint = new List<Vector2Int>()
    {
        new Vector2Int(-1, -1),
        new Vector2Int(-1, +0),
        new Vector2Int(-1, +1),
        new Vector2Int(+0, -1),
        new Vector2Int(+0, +0),
        new Vector2Int(+0, +1),
        new Vector2Int(+1, -1),
        new Vector2Int(+1, +0),
        new Vector2Int(+1, +1)
    };
    public new static List<Vector2Int> Footprint => _footprint;
    public override List<Vector2Int> GetFootprint()
    {
        return _footprint;
    }
}
