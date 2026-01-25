using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject tilePrefab;

    private int tileRows = 7;
    private int tileCols = 7;
    private float tileSpacing = 1.025f;

    void Start()
    {
        SpawnTiles();
    }

    void Update()
    {
        
    }

    void SpawnTiles()
    {
        Vector3 topLeft = gameObject.transform.position;
        topLeft.x -= (tileCols - 1) * tileSpacing / 2f;
        topLeft.z += (tileRows - 1) * tileSpacing / 2f;

        Vector3 tilePosition = topLeft;
        tilePosition.y = 0.1f;
        for (int i = 0; i < tileCols; i++)
        {
            for (int j = 0; j < tileRows; j++)
            {
                GameObject instance = Instantiate(tilePrefab, tilePosition, tilePrefab.transform.rotation);
                instance.name = tilePrefab.name + "_" + i + "_" + j;
                tilePosition.x += tileSpacing;
            }
            tilePosition.z -= tileSpacing;
            tilePosition.x = topLeft.x;
        }
    }
}
