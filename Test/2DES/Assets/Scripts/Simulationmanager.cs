using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Simulationmanager : MonoBehaviour
{
    public Tilemap tilemap;
    public ArrayGrid grid;

    public Tile tlGrass;
    public Tile tlWater;
    public Tile tlFood;
    public Tile tlRand;

    public List<Vector3Int> lstlGrass;

    public float timeoffset = 1.5f;
    float time;


    // Start is called before the first frame update
    void Start()
    {
        grid = new ArrayGrid(50, 50, 10f);


        lstlGrass = new List<Vector3Int>();

        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                if (tilemap.GetTile(new Vector3Int(i, j, 0)) == tlGrass)
                {
                    lstlGrass.Add(new Vector3Int(i, j, 0));
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > time + timeoffset)
        {
            time = Time.time;
            int rannum = Random.Range(0, lstlGrass.Count);
            tilemap.SetTile(lstlGrass[rannum], tlFood);
        }
    }
}
