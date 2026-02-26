using System.Collections.Generic;
using UnityEngine;

namespace TowerKingdomWars
{
    public class Path
    {
        public uint sourceStrongholdId;
        public Stronghold sourceStrongHold;
        public List<Vector2Int> pathNodes;
        public List<PathTile> pathTiles;
        public uint targetStrongholdId;
        public Stronghold targetStronghold;

        public Path(uint sourceStrongholdId, List<Vector2Int> pathNodes, uint targetStrongholdId)
        {
            this.sourceStrongholdId = sourceStrongholdId;
            this.pathNodes = pathNodes;
            this.targetStrongholdId = targetStrongholdId;
        }
    }
}