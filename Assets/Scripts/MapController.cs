using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const int _TILE_ROWS = 7;
    private const int _TILE_COLS = 7;
    private const float _TILE_SPACING = 1.025f;

    private Tile[,] _tileMatrix;

    private List<Monster> _monsterList = new List<Monster>();
    public List<Monster> MonsterList => _monsterList;

    [SerializeField] private GameObject _tiles;
    public GameObject Tiles => _tiles;

    [SerializeField] private GameObject _castles;
    public GameObject Castles => _castles;

    [SerializeField] private GameObject _towers;
    public GameObject Towers => _towers;

    [SerializeField] private GameObject _projectiles;
    public GameObject Projectiles => _projectiles;

    [SerializeField] private GameObject _monsters;
    public GameObject Monsters => _monsters;

    // Note: x = row, y = col when using Vector2Int here

    private struct LevelData
    {
        public LevelData(Vector2Int[] player1CastlesRowCols, Vector2Int[] player2CastlesRowCols, List<Vector2Int[]> paths)
        {
            Player1CastlesRowCols = player1CastlesRowCols;
            Player2CastlesRowCols = player2CastlesRowCols;
            Paths = paths;
        }
        public Vector2Int[] Player1CastlesRowCols;
        public Vector2Int[] Player2CastlesRowCols;
        public List<Vector2Int[]> Paths;
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

    private void Start()
    {
        _tileMatrix = new Tile[_TILE_ROWS, _TILE_COLS];
        SpawnTiles(level1Data);
    }

    private void Update()
    {

    }

    private void SpawnTiles(LevelData levelData)
    {
        Vector3 topLeft = gameObject.transform.position;
        topLeft.x -= (_TILE_COLS - 1) * _TILE_SPACING * 0.5f;
        topLeft.z += (_TILE_ROWS - 1) * _TILE_SPACING * 0.5f;
        topLeft.y = 0.1f;

        bool[,] tileIsPathTile = new bool[_TILE_ROWS, _TILE_COLS];
        foreach (Vector2Int[] path in levelData.Paths)
        {
            foreach (Vector2Int point in path)
            {
                tileIsPathTile[point.x, point.y] = true;
            }
        }

        Vector3 tilePosition = topLeft;
        for (int i = 0; i < _TILE_ROWS; i++)
        {
            for (int j = 0; j < _TILE_COLS; j++)
            {
                if (tileIsPathTile[i, j])
                {
                    _tileMatrix[i, j] = TileFactory.SpawnPathTile("Tile_" + i + "_" + j, tilePosition);
                }
                else
                {
                    _tileMatrix[i, j] = TileFactory.SpawnTile("Tile_" + i + "_" + j, tilePosition);
                }
                tilePosition.x += _TILE_SPACING;
            }
            tilePosition.z -= _TILE_SPACING;
            tilePosition.x = topLeft.x;
        }

        foreach (Vector2Int point in levelData.Player1CastlesRowCols)
        {
            Vector3 castlePosition = new Vector3(topLeft.x + (_TILE_SPACING * point.y), topLeft.y, topLeft.z - (_TILE_SPACING * point.x));
            List<List<PathTile>> paths = new List<List<PathTile>>();
            foreach (Vector2Int[] path in levelData.Paths)
            {
                if (path.Length > 0 && path[0] == point)
                {
                    List<PathTile> pathTiles = new List<PathTile>();
                    foreach (Vector2Int pathPos in path)
                    {
                        if (_tileMatrix[pathPos.x, pathPos.y] is PathTile pathTile)
                        {
                            pathTiles.Add(pathTile);
                        }
                    }
                    paths.Add(pathTiles);
                }
            }
            StructureFactory.SpawnCastle(PlayerController.PlayerNumber.One, castlePosition, paths, this);
        }

        foreach (Vector2Int point in levelData.Player2CastlesRowCols)
        {
            Vector3 castlePosition = new Vector3(topLeft.x + (_TILE_SPACING * point.y), topLeft.y, topLeft.z - (_TILE_SPACING * point.x));
            List<List<PathTile>> paths = new List<List<PathTile>>();
            foreach (Vector2Int[] path in levelData.Paths)
            {
                if (path.Length > 0 && path[0] == point)
                {
                    List<PathTile> pathTiles = new List<PathTile>();
                    foreach (Vector2Int pathPos in path)
                    {
                        if (_tileMatrix[pathPos.x, pathPos.y] is PathTile pathTile)
                        {
                            pathTiles.Add(pathTile);
                        }
                    }
                    paths.Add(pathTiles);
                }
            }
            StructureFactory.SpawnCastle(PlayerController.PlayerNumber.Two, castlePosition, paths, this);
        }
    }

    public void AddMonsterToMap(Monster monster)
    {
        _monsterList.Add(monster);
    }

    public void RemoveMonsterFromMap(Monster monster)
    {
        _monsterList.Remove(monster);
    }
}
