using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;


public class GetTilePositions : MonoBehaviour
{
    //list of tiles for giving shapes
    List<Vector3Int> tilepositions;
    Vector3Int pos;
    Vector3 tileSize = new Vector3(0.6f, 0.6f, 1.0f);

    public Tilemap tileMap = null;

    public List<Vector3> availablePlaces;

    // Start is called before the first frame update
    void Start()
    {

        tileMap = transform.GetComponentInParent<Tilemap>();
        availablePlaces = new List<Vector3>();


        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                Vector3 place = tileMap.CellToWorld(localPlace);
                if (tileMap.HasTile(localPlace))
                {
                    //found tile
                    //adjusted place recenters the cubes to the grid. adjusts with tileSize
                    var adjustedPlace = new Vector3(place.x + 0.3f, place.y + 0.3f, place.z);
                    availablePlaces.Add(adjustedPlace);
                }
                else
                {
                    //No tile at "place"
                }
            }
        }

        //adds cubes that cast shadows only and attaches shadowcaster to them for cooperation with Light 2D
        foreach (Vector3 position in availablePlaces)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.AddComponent<ShadowCaster2D>();
            cube.GetComponent<ShadowCaster2D>().selfShadows = true;
            cube.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            cube.transform.localScale = tileSize;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
