using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapBuilder
{
    private const float _TILE_SPACING = 1.025f;

    private const float LEFT_CAMERA_PADDING = -1.0f;
    private const float RIGHT_CAMERA_PADDING = 1.0f;
    private const float TOP_CAMERA_PADDING = 1.0f;
    private const float BOTTOM_CAMERA_PADDING = 2.0f;

    private uint rows;
    private uint columns;

    public enum TileType
    {
        Tile,
        PathTile
    };
    private TileType[,] tiles;

    private struct PathDirections
    {
        public bool North;
        public bool East;
        public bool South;
        public bool West;
    }
    private PathDirections[,] pathDirections;

    public enum StructureType
    {
        Castle,
        Fort,
        ArrowTower,
        FlameTower,
        FrostTower
    };

    private uint nextStructureNumber;
    private struct StructureSpawnInfo
    {
        public uint structureNumber;
        public StructureType structureType;
        public uint row;
        public uint col;
        public uint playerSlot;
    }
    private List<StructureSpawnInfo> structures;

    private List<List<Vector2Int>> paths;

    private List<List<int>> strongholdPathIndices;

    public MapBuilder(uint rows, uint columns)
    {
        this.rows = rows;
        this.columns = columns;
        nextStructureNumber = 0u;
        tiles = new TileType[rows, columns];
        pathDirections = new PathDirections[rows, columns];
        structures = new List<StructureSpawnInfo>();
        strongholdPathIndices = new List<List<int>>();
        paths = new List<List<Vector2Int>>();
    }

    public void SetTile(uint row, uint column, TileType tileType)
    {
        tiles[row, column] = tileType;
    }

    public void AddStructure(StructureType structureType, uint row, uint column, uint playerSlot)
    {
        structures.Add(new StructureSpawnInfo
        {
            structureNumber = nextStructureNumber,
            structureType = structureType,
            row = row,
            col = column,
            playerSlot = playerSlot
        });
        nextStructureNumber++;
    }

    public void AddPath(uint strongholdNumber, List<Vector2Int> pathRowCols)
    {
        while (strongholdPathIndices.Count <= strongholdNumber)
        {
            strongholdPathIndices.Add(new List<int>());
        }
        strongholdPathIndices[(int)strongholdNumber].Add(paths.Count);
        paths.Add(pathRowCols);

        int lastRow = -1;
        int lastCol = -1;
        // x is row, y is col
        foreach (Vector2Int pathRowCol in pathRowCols)
        {
            tiles[pathRowCol.x, pathRowCol.y] = TileType.PathTile;
            if (lastRow > -1 && lastCol > -1)
            {
                if (lastRow < pathRowCol.x)
                {
                    pathDirections[lastRow, lastCol].South = true;
                    pathDirections[pathRowCol.x, pathRowCol.y].North = true;
                }
                else if (lastCol < pathRowCol.y)
                {
                    pathDirections[lastRow, lastCol].West = true;
                    pathDirections[pathRowCol.x, pathRowCol.y].East = true;
                }
                else if (lastRow > pathRowCol.x)
                {
                    pathDirections[lastRow, lastCol].North = true;
                    pathDirections[pathRowCol.x, pathRowCol.y].South = true;
                }
                else if (lastCol > pathRowCol.y)
                {
                    pathDirections[lastRow, lastCol].East = true;
                    pathDirections[pathRowCol.x, pathRowCol.y].West = true;
                }
            }
            lastRow = pathRowCol.x;
            lastCol = pathRowCol.y;
        }
    }

    public void Build()
    {
        MapController mapController = GameSceneController.Instance.Map;

        // Construct all the tiles
        Vector3 topLeft = new Vector3(0.0f, 0.0f, 0.0f);
        topLeft.x -= (columns - 1) * _TILE_SPACING * 0.5f;
        topLeft.z += (rows - 1) * _TILE_SPACING * 0.5f;
        topLeft.y = 0.1f;
        Vector3 tilePosition = topLeft;
        Tile[,] instantiatedTiles = new Tile[rows, columns];
        for (uint r = 0u; r < rows; r++)
        {
            for (uint c = 0u; c < columns; c++)
            {
                switch (tiles[r, c])
                {
                    case TileType.Tile:
                        {
                            instantiatedTiles[r, c] = TileFactory.SpawnTile(tilePosition, r, c);
                        }
                        break;
                    case TileType.PathTile:
                        {
                            PathDirections pd = pathDirections[r, c];
                            instantiatedTiles[r, c] = TileFactory.SpawnPathTile(tilePosition, r, c);
                            (instantiatedTiles[r, c] as PathTile).SetNeighbors(pd.North, pd.East, pd.South, pd.West);
                        }
                        break;
                }
                tilePosition.x += _TILE_SPACING;
            }
            tilePosition.z -= _TILE_SPACING;
            tilePosition.x = topLeft.x;
        }
        GameSceneController.Instance.Map.TileMatrix = instantiatedTiles;

        // Limit the player camera movement
        GameSceneController.Instance.LocalPlayer.SetBounds(
            topLeft + new Vector3(LEFT_CAMERA_PADDING, -topLeft.y, TOP_CAMERA_PADDING),
            -topLeft + new Vector3(RIGHT_CAMERA_PADDING, -topLeft.y, BOTTOM_CAMERA_PADDING));

        // Add the structures
        foreach (StructureSpawnInfo structureSpawnInfo in structures)
        {
            switch (structureSpawnInfo.structureType)
            {
                case StructureType.Castle:
                    {
                        List<Tile> tiles = new List<Tile>();
                        List<Vector2Int> footprintOffsets = Castle.Footprint;
                        foreach (Vector2Int tileOffset in footprintOffsets)
                        {
                            Tile includedTile = instantiatedTiles[structureSpawnInfo.row + tileOffset.x, structureSpawnInfo.col + tileOffset.y];
                            tiles.Add(includedTile);
                            includedTile.ContainsStructure = true;
                        }
                        List<List<PathTile>> pathSet = new List<List<PathTile>>();
                        foreach (int pathIndex in strongholdPathIndices[(int)structureSpawnInfo.structureNumber])
                        {
                            List<Vector2Int> path = paths[pathIndex];
                            List<PathTile> pathTiles = new List<PathTile>();
                            foreach (Vector2Int pathPiece in path)
                            {
                                pathTiles.Add(instantiatedTiles[pathPiece.x, pathPiece.y] as PathTile);
                            }
                            pathSet.Add(pathTiles);
                        }
                        StructureFactory.SpawnCastle(
                            structureSpawnInfo.playerSlot,
                            instantiatedTiles[structureSpawnInfo.row, structureSpawnInfo.col].transform.position,
                            tiles,
                            pathSet);
                    }
                    break;
                case StructureType.Fort:
                    {
                        List<Tile> tiles = new List<Tile>();
                        List<Vector2Int> footprintOffsets = Fort.Footprint;
                        foreach (Vector2Int tileOffset in footprintOffsets)
                        {
                            Tile includedTile = instantiatedTiles[structureSpawnInfo.row + tileOffset.x, structureSpawnInfo.col + tileOffset.y];
                            tiles.Add(includedTile);
                            includedTile.ContainsStructure = true;
                        }
                        List<List<PathTile>> pathSet = new List<List<PathTile>>();
                        foreach (int pathIndex in strongholdPathIndices[(int)structureSpawnInfo.structureNumber])
                        {
                            List<Vector2Int> path = paths[pathIndex];
                            List<PathTile> pathTiles = new List<PathTile>();
                            foreach (Vector2Int pathPiece in path)
                            {
                                pathTiles.Add(instantiatedTiles[pathPiece.x, pathPiece.y] as PathTile);
                            }
                            pathSet.Add(pathTiles);
                        }
                        StructureFactory.SpawnFort(
                            structureSpawnInfo.playerSlot, 
                            instantiatedTiles[structureSpawnInfo.row, structureSpawnInfo.col].transform.position,
                            tiles,
                            pathSet);
                    }
                    break;
            }
        }
    }
}