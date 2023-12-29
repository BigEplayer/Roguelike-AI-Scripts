using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    [SerializeField] List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(var tileData in tileDatas)
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    //  btw, may be good to check if a tile is null and return if it is 
    //  before using the dictonary (as it may prevent error)

    //  can also use dictionary.TryGetValue()
}
