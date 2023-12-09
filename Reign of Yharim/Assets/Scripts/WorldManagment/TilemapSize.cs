using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSize : MonoBehaviour
{
    private Vector3 size;

    void Start()
    {
        GetComponent<Tilemap>().CompressBounds();
        size = GetComponent<Tilemap>().size;
        Debug.Log("Size = " + size);
    }
}
