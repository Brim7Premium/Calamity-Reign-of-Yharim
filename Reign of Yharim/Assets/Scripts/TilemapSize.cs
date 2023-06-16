using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSize : MonoBehaviour
{
    private Vector3 size;

    private Camera mainCamera;
    private Tilemap tilemap;
    private BoundsInt tilemapBounds;
    private Vector2Int chunkSize;

    void Start()
    {
        GetComponent<Tilemap>().CompressBounds();
        size = GetComponent<Tilemap>().size;
        Debug.Log("Size = " + size);
    }
    private void Awake()
    {
        mainCamera = Camera.main; //maincamera variable equals the main camera
        tilemap = GetComponent<Tilemap>(); //tilemap equals tilemap component
        tilemapBounds = tilemap.cellBounds; //tilemapbounds equals the cellbounds of the tilemap
        chunkSize = new Vector2Int(10, 10); //the chunksize is 10x10
    }

    private void Update()
    {
        foreach (Vector3Int chunkPosition in GetVisibleChunks())
        {
            ProcessChunk(chunkPosition);
        }
    }

    private void ProcessChunk(Vector3Int chunkPosition)
    {
        BoundsInt chunkBounds = new BoundsInt(
            chunkPosition.x * chunkSize.x,
            chunkPosition.y * chunkSize.y,
            tilemapBounds.zMin,
            chunkSize.x,
            chunkSize.y,
            tilemapBounds.size.z
        );

        foreach (Vector3Int position in chunkBounds.allPositionsWithin)
        {
            Vector3 tileWorldPos = tilemap.CellToWorld(position);

            // Check if the tile is within the camera's view frustum
            bool isVisible = mainCamera.WorldToViewportPoint(tileWorldPos).z > 0;

            // Set the tile's visibility
            tilemap.SetTileFlags(position, isVisible ? TileFlags.None : TileFlags.LockColor);
            tilemap.SetColor(position, isVisible ? Color.white : Color.clear);
        }
    }

    private Vector3Int[] GetVisibleChunks()
    {
        Vector3Int cameraChunkPosition = new Vector3Int(
            Mathf.FloorToInt(mainCamera.transform.position.x / chunkSize.x),
            Mathf.FloorToInt(mainCamera.transform.position.y / chunkSize.y),
            0
        );

        int visibleChunkCountX = Mathf.CeilToInt(mainCamera.orthographicSize * mainCamera.aspect / chunkSize.x) * 2 + 1;
        int visibleChunkCountY = Mathf.CeilToInt(mainCamera.orthographicSize / chunkSize.y) * 2 + 1;

        Vector3Int[] visibleChunks = new Vector3Int[visibleChunkCountX * visibleChunkCountY];

        int chunkIndex = 0;
        for (int y = 0; y < visibleChunkCountY; y++)
        {
            for (int x = 0; x < visibleChunkCountX; x++)
            {
                visibleChunks[chunkIndex] = new Vector3Int(
                    cameraChunkPosition.x - visibleChunkCountX / 2 + x,
                    cameraChunkPosition.y - visibleChunkCountY / 2 + y,
                    0
                );
                chunkIndex++;
            }
        }

        return visibleChunks;
    }

}
