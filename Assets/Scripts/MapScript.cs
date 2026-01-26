using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject pathTilePrefab;

    public GameObject castlePrefab;

    private const int TileRows = 7;
    private const int TileCols = 7;
    private const float TileSpacing = 1.025f;

    private GameObject[,] tiles;

    // Note: x = row, y = col when using Vector2Int here

    private struct LevelData
    {
        public LevelData(Vector2Int[] player1CastlesRowCols, Vector2Int[] player2CastlesRowCols, List<Vector2Int[]> paths)
        {
            this.player1CastlesRowCols = player1CastlesRowCols;
            this.player2CastlesRowCols = player2CastlesRowCols;
            this.paths = paths;
        }
        public Vector2Int[] player1CastlesRowCols;
        public Vector2Int[] player2CastlesRowCols;
        public List<Vector2Int[]> paths;
    };

    private LevelData level1Data = new LevelData(
        new Vector2Int[]{ 
            new Vector2Int(3, 0)
        },
        new Vector2Int[]{
            new Vector2Int(3, 6)
        },
        new List<Vector2Int[]>{ 
            new Vector2Int[]{ // Path 1
                new Vector2Int(3, 0),
                new Vector2Int(4, 0), 
                new Vector2Int(4, 1), 
                new Vector2Int(4, 2), 
                new Vector2Int(4, 3), 
                new Vector2Int(4, 4), 
                new Vector2Int(4, 5), 
                new Vector2Int(4, 6),
                new Vector2Int(3, 6)
            },
            new Vector2Int[]{ // Path 2
                new Vector2Int(3, 6),
                new Vector2Int(2, 6),
                new Vector2Int(2, 5),
                new Vector2Int(2, 4),
                new Vector2Int(2, 3),
                new Vector2Int(2, 2),
                new Vector2Int(2, 1),
                new Vector2Int(2, 0),
                new Vector2Int(3, 0)
            }
        }
    );

    void Start()
    {
        tiles = new GameObject[TileRows, TileCols];
        SpawnTiles(level1Data);
    }

    void Update()
    {
        
    }

    void SpawnTiles(LevelData levelData)
    {
        Vector3 topLeft = gameObject.transform.position;
        topLeft.x -= (TileCols - 1) * TileSpacing / 2f;
        topLeft.z += (TileRows - 1) * TileSpacing / 2f;
        topLeft.y = 0.1f;

        bool[,] tileIsPathTile = new bool[TileRows, TileCols];
        foreach (Vector2Int[] path in levelData.paths)
        {
            foreach (Vector2Int point in path)
            {
                tileIsPathTile[point.x, point.y] = true;
            }
        }

        Vector3 tilePosition = topLeft;
        for (int i = 0; i < TileRows; i++)
        {
            for (int j = 0; j < TileCols; j++)
            {
                GameObject instance;
                if (tileIsPathTile[i, j])
                {
                    instance = Instantiate(pathTilePrefab, tilePosition, tilePrefab.transform.rotation);
                }
                else
                {
                    instance = Instantiate(tilePrefab, tilePosition, tilePrefab.transform.rotation);
                }
                instance.name = tilePrefab.name + "_" + i + "_" + j;
                tiles[j, i] = instance;
                tilePosition.x += TileSpacing;
            }
            tilePosition.z -= TileSpacing;
            tilePosition.x = topLeft.x;
        }

        foreach (Vector2Int point in levelData.player1CastlesRowCols)
        {
            Vector3 castlePosition = new Vector3(topLeft.x + (TileSpacing * point.y), topLeft.y, topLeft.z - (TileSpacing * point.x));
            GameObject castleInstance = Instantiate(castlePrefab, castlePosition, tilePrefab.transform.rotation);
            CastleScript castle = castleInstance.GetComponent<CastleScript>();
            foreach (Vector2Int[] path in levelData.paths)
            {
                if (path.Length > 0 && path[0] == point)
                {
                    List<GameObject> pathTiles = new List<GameObject>();
                    foreach (Vector2Int pathPos in path)
                    {
                        pathTiles.Add(tiles[pathPos.y, pathPos.x]);
                    }
                    castle.AddPath(pathTiles);
                }
            }
        }

        foreach (Vector2Int point in levelData.player2CastlesRowCols)
        {
            Vector3 castlePosition = new Vector3(topLeft.x + (TileSpacing * point.y), topLeft.y, topLeft.z - (TileSpacing * point.x));
            GameObject castleInstance = Instantiate(castlePrefab, castlePosition, tilePrefab.transform.rotation);
            CastleScript castle = castleInstance.GetComponent<CastleScript>();
            foreach (Vector2Int[] path in levelData.paths)
            {
                if (path.Length > 0 && path[0] == point)
                {
                    List<GameObject> pathTiles = new List<GameObject>();
                    foreach (Vector2Int pathPos in path)
                    {
                        pathTiles.Add(tiles[pathPos.y, pathPos.x]);
                    }
                    castle.AddPath(pathTiles);
                }
            }
        }
    }
}
