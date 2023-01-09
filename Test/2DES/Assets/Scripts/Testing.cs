using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Testing : MonoBehaviour
{
    public Tilemap tilemap;
    private ArrayGrid grid;

    public Tile tlSpace;
    public Tile tlRoom;

    public List<Vector3Int> possibleRoomtile;
    public List<Vector3Int> gangls;
    int gangnum = 101;
    int roomnum = 1;
    List<Vector3Int> lsgang2;

    int tlnumrooms = 1;

    public GameObject RoomLarge;
    public GameObject RoomMedium; 
    public GameObject Gang;
    public GameObject WallO;
    public GameObject WallL;
    public GameObject WallR;
    public GameObject WallU;

    void Start()
    {
        grid = new ArrayGrid(30, 30, 10f);
        possibleRoomtile = new List<Vector3Int>();
        gangls = new List<Vector3Int>();
        lsgang2 = new List<Vector3Int>();



        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                if (tilemap.GetTile(new Vector3Int(i, j, 0)) == tlSpace)
                {
                    grid.SetValue(i, j, -1);


                    if (tilemap.GetTile(new Vector3Int(i + 1, j + 1, 0)) == tlSpace && tilemap.GetTile(new Vector3Int(i + 1, j, 0)) == tlSpace && tilemap.GetTile(new Vector3Int(i, j + 1, 0)) == tlSpace)
                    {
                        possibleRoomtile.Add(new Vector3Int(i, j, 0));
                        grid.SetValue(i, j, -3);
                    }
                }
            }
        }

    }

    private void Update()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(GetMouseWorldPosition(), 22);
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(GetMouseWorldPosition()));

        }
        // increase

        if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if (tilemap.GetTile(new Vector3Int(i, j, 0)) == tlRoom)
                    {
                        if(tilemap.GetTile(new Vector3Int(i + 1, j, 0)) != tlRoom)
                        {
                            Instantiate(WallR, grid.GetWorldPosition(i + 1, j), Quaternion.identity);
                        }
                        if (tilemap.GetTile(new Vector3Int(i, j + 1, 0)) != tlRoom)
                        {
                            Instantiate(WallO, grid.GetWorldPosition(i, j + 1), Quaternion.identity);
                        }
                        if (tilemap.GetTile(new Vector3Int(i - 1, j, 0)) != tlRoom)
                        {
                            Instantiate(WallL, grid.GetWorldPosition(i - 1, j), Quaternion.identity);
                        }
                        if (tilemap.GetTile(new Vector3Int(i, j - 1, 0)) != tlRoom)
                        {
                            Instantiate(WallU, grid.GetWorldPosition(i, j - 1), Quaternion.identity);
                        }

                    }
                }
            }
        }

            if (Input.GetKeyDown(KeyCode.C))
        {
            List<Vector3Int> varlss = new List<Vector3Int>();

            //roomnum++;
            gangls.Clear();

            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if (grid.GetValue(i, j) == roomnum)
                    {
                        gangls.Add(new Vector3Int(i, j, 0));
                    }
                }
            }
            varlss.Add(new Vector3Int(1, 1, 0));

            while (varlss.Count > 0)
            {
                varlss.Clear();
                foreach (var tile in gangls)
                {

                    if (tilemap.GetTile(new Vector3Int(tile.x + 1, tile.y, 0)) == tlRoom && !gangls.Contains(new Vector3Int(tile.x + 1, tile.y, 0)))
                    {
                        varlss.Add(new Vector3Int(tile.x + 1, tile.y, 0));
                    }
                    if (tilemap.GetTile(new Vector3Int(tile.x - 1, tile.y, 0)) == tlRoom && !gangls.Contains(new Vector3Int(tile.x - 1, tile.y, 0)))
                    {
                        varlss.Add(new Vector3Int(tile.x - 1, tile.y, 0));
                    }
                    if (tilemap.GetTile(new Vector3Int(tile.x, tile.y + 1, 0)) == tlRoom && !gangls.Contains(new Vector3Int(tile.x, tile.y + 1, 0)))
                    {
                        varlss.Add(new Vector3Int(tile.x, tile.y + 1, 0));
                    }
                    if (tilemap.GetTile(new Vector3Int(tile.x, tile.y - 1, 0)) == tlRoom && !gangls.Contains(new Vector3Int(tile.x, tile.y - 1, 0)))
                    {
                        varlss.Add(new Vector3Int(tile.x, tile.y - 1, 0));
                    }
                }

                foreach (var tile in varlss)
                {

                        gangls.Add(tile);
                }
            }
        }

        //Reset
        if (Input.GetKeyDown(KeyCode.V))
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if (grid.GetValue(i, j) >= 100)
                    {
                        grid.SetValue(i, j, 0);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            gangls.Clear();

            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if(grid.GetValue(i, j) == roomnum)
                    {
                        gangls.Add(new Vector3Int(i, j, 0));
                    }
                }
            }


        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            List<Vector3Int> varls = new List<Vector3Int>();
            int tlvalue;

            foreach(var tile in gangls)
            {
                if((grid.GetValue(tile.x + 1, tile.y) == 0 || grid.GetValue(tile.x + 1, tile.y) == -3) && tilemap.GetTile(new Vector3Int(tile.x + 1, tile.y, 0)) != tlRoom && tilemap.GetTile(new Vector3Int(tile.x + 1, tile.y, 0)) == tlSpace)
                {
                    
                    grid.SetValue(tile.x + 1, tile.y, gangnum);
                    varls.Add(new Vector3Int(tile.x + 1, tile.y, 0));
                }
                else if (tilemap.GetTile(new Vector3Int(tile.x + 1, tile.y, 0)) == tlRoom && grid.GetValue(tile.x + 1, tile.y) != roomnum && grid.GetValue(tile.x + 1, tile.y) != 0)
                {



                    tlvalue = (grid.GetValue(tile.x, tile.y));
                    int tilex = tile.x;
                    int tiley = tile.y;
                    tilemap.SetTile(new Vector3Int(tile.x, tile.y, 0), tlRoom);
                    Instantiate(Gang, grid.GetWorldPosition(tilex, tiley), Quaternion.identity);

                    while (tlvalue > 100)
                    {
                        tilemap.SetTile(new Vector3Int(tilex, tiley, 0), tlRoom);

                        if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex + 1, tiley))
                        {
                            tilemap.SetTile(new Vector3Int(tilex + 1, tiley, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex + 1, tiley), Quaternion.identity);
                            tilex++;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex - 1, tiley))
                        {
                            tilemap.SetTile(new Vector3Int(tile.x - 1, tile.y, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex - 1, tiley), Quaternion.identity);
                            tilex--;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex, tiley + 1))
                        {
                            tilemap.SetTile(new Vector3Int(tilex, tiley + 1, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex, tiley + 1), Quaternion.identity);
                            tiley++;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex, tiley - 1))
                        {
                            tilemap.SetTile(new Vector3Int(tilex, tiley - 1, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex, tiley - 1), Quaternion.identity);
                            tiley--;
                        }

                        tlvalue--;
                    }

                    for (int i = 0; i < 30; i++)
                    {
                        for (int j = 0; j < 30; j++)
                        {
                            if (grid.GetValue(i, j) == grid.GetValue(tile.x + 1, tile.y))
                            {
                                grid.SetValue(i, j, 1);
                            }
                        }
                    }

                    return;

                }

                if ((grid.GetValue(tile.x - 1, tile.y) == 0 || grid.GetValue(tile.x - 1, tile.y) == -3) && tilemap.GetTile(new Vector3Int(tile.x - 1, tile.y, 0)) != tlRoom && tilemap.GetTile(new Vector3Int(tile.x - 1, tile.y, 0)) == tlSpace)
                {
                    grid.SetValue(tile.x - 1, tile.y, gangnum);
                    varls.Add(new Vector3Int(tile.x - 1, tile.y, 0));
                }
                else if (tilemap.GetTile(new Vector3Int(tile.x - 1, tile.y, 0)) == tlRoom && grid.GetValue(tile.x - 1, tile.y) != roomnum && grid.GetValue(tile.x - 1, tile.y) != 0)
                {


                    tlvalue = (grid.GetValue(tile.x, tile.y));
                    int tilex = tile.x;
                    int tiley = tile.y;
                    tilemap.SetTile(new Vector3Int(tile.x, tile.y, 0), tlRoom);
                    Instantiate(Gang, grid.GetWorldPosition(tilex, tiley), Quaternion.identity);

                    while (tlvalue > 100)
                    {
                        tilemap.SetTile(new Vector3Int(tilex, tiley, 0), tlRoom);

                        if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex + 1, tiley))
                        {
                            tilemap.SetTile(new Vector3Int(tilex + 1, tiley, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex + 1, tiley), Quaternion.identity);
                            tilex++;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex - 1, tiley))
                        {
                            tilemap.SetTile(new Vector3Int(tile.x - 1, tile.y, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex - 1, tiley), Quaternion.identity);
                            tilex--;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex, tiley + 1))
                        {
                            tilemap.SetTile(new Vector3Int(tilex, tiley + 1, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex, tiley + 1), Quaternion.identity);
                            tiley++;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex, tiley - 1))
                        {
                            tilemap.SetTile(new Vector3Int(tilex, tiley - 1, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex, tiley - 1), Quaternion.identity);
                            tiley--;
                        }

                        tlvalue--;
                    }

                    for (int i = 0; i < 30; i++)
                    {
                        for (int j = 0; j < 30; j++)
                        {
                            if (grid.GetValue(i, j) == grid.GetValue(tile.x - 1, tile.y))
                            {
                                grid.SetValue(i, j, 1);
                            }
                        }
                    }

                    return;
                }

                if ((grid.GetValue(tile.x, tile.y + 1) == 0 || grid.GetValue(tile.x , tile.y + 1) == -3) && tilemap.GetTile(new Vector3Int(tile.x, tile.y + 1, 0)) != tlRoom && tilemap.GetTile(new Vector3Int(tile.x, tile.y + 1, 0)) == tlSpace)
                {
                    grid.SetValue(tile.x, tile.y + 1, gangnum);
                    varls.Add(new Vector3Int(tile.x, tile.y + 1, 0));
                }
                else if (tilemap.GetTile(new Vector3Int(tile.x, tile.y + 1, 0)) == tlRoom && grid.GetValue(tile.x, tile.y + 1) != roomnum && grid.GetValue(tile.x, tile.y + 1) != 0)
                {


                    tlvalue = (grid.GetValue(tile.x, tile.y));
                    int tilex = tile.x;
                    int tiley = tile.y;
                    tilemap.SetTile(new Vector3Int(tile.x, tile.y, 0), tlRoom);
                    Instantiate(Gang, grid.GetWorldPosition(tilex, tiley), Quaternion.identity);

                    while (tlvalue > 100)
                    {
                        tilemap.SetTile(new Vector3Int(tilex, tiley, 0), tlRoom);

                        if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex + 1, tiley))
                        {
                            tilemap.SetTile(new Vector3Int(tilex + 1, tiley, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex + 1, tiley), Quaternion.identity);
                            tilex++;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex - 1, tiley))
                        {
                            tilemap.SetTile(new Vector3Int(tile.x - 1, tile.y, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex - 1, tiley), Quaternion.identity);
                            tilex--;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex, tiley + 1))
                        {
                            tilemap.SetTile(new Vector3Int(tilex, tiley + 1, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex, tiley + 1), Quaternion.identity);
                            tiley++;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex, tiley - 1))
                        {
                            tilemap.SetTile(new Vector3Int(tilex, tiley - 1, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex, tiley - 1), Quaternion.identity);
                            tiley--;
                        }

                        tlvalue--;
                    }

                    for (int i = 0; i < 30; i++)
                    {
                        for (int j = 0; j < 30; j++)
                        {
                            if (grid.GetValue(i, j) == grid.GetValue(tile.x, tile.y + 1))
                            {
                                grid.SetValue(i, j, 1);
                            }
                        }
                    }

                    return;
                }

                if ((grid.GetValue(tile.x, tile.y - 1) == 0 || grid.GetValue(tile.x, tile.y - 1) == -3) && tilemap.GetTile(new Vector3Int(tile.x, tile.y - 1, 0)) != tlRoom && tilemap.GetTile(new Vector3Int(tile.x, tile.y - 1, 0)) == tlSpace)
                {
                    grid.SetValue(tile.x, tile.y - 1, gangnum);
                    varls.Add(new Vector3Int(tile.x, tile.y - 1, 0));
                }
                else if (tilemap.GetTile(new Vector3Int(tile.x, tile.y - 1, 0)) == tlRoom && grid.GetValue(tile.x, tile.y - 1) != roomnum && grid.GetValue(tile.x, tile.y - 1) != 0)
                {


                    tlvalue = (grid.GetValue(tile.x, tile.y));
                    int tilex = tile.x;
                    int tiley = tile.y;
                    tilemap.SetTile(new Vector3Int(tile.x, tile.y, 0), tlRoom);
                    Instantiate(Gang, grid.GetWorldPosition(tilex, tiley), Quaternion.identity);

                    while (tlvalue > 100)
                    {
                        tilemap.SetTile(new Vector3Int(tilex, tiley, 0), tlRoom);

                        if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex + 1, tiley))
                        {
                            tilemap.SetTile(new Vector3Int(tilex + 1, tiley, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex + 1, tiley), Quaternion.identity);
                            tilex++;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex - 1, tiley))
                        {
                            tilemap.SetTile(new Vector3Int(tile.x - 1, tile.y, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex - 1, tiley), Quaternion.identity);
                            tilex--;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex, tiley + 1))
                        {
                            tilemap.SetTile(new Vector3Int(tilex, tiley + 1, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex, tiley + 1), Quaternion.identity);
                            tiley++;
                        }
                        else if (grid.GetValue(tilex, tiley) - 1 == grid.GetValue(tilex, tiley - 1))
                        {
                            tilemap.SetTile(new Vector3Int(tilex, tiley - 1, 0), tlRoom);
                            Instantiate(Gang, grid.GetWorldPosition(tilex, tiley - 1), Quaternion.identity);
                            tiley--;
                        }

                        tlvalue--;
                    }

                    for (int i = 0; i < 30; i++)
                    {
                        for (int j = 0; j < 30; j++)
                        {
                            if (grid.GetValue(i, j) == grid.GetValue(tile.x, tile.y - 1))
                            {
                                grid.SetValue(i, j,  1);
                            }
                        }
                    }

                    return;
                }
            }
            gangnum++;
            gangls = varls;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            possibleRoomtile.Clear();

            int num2  = Random.Range(0, 2);

            //small Room
            if (num2 == 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (tilemap.GetTile(new Vector3Int(i, j, 0)) == tlSpace)
                        {

                            if ( smallRoomavaiable(i,j))
                            {
                                possibleRoomtile.Add(new Vector3Int(i, j, 0));
                            }

                        }
                    }
                }

                if (possibleRoomtile.Count == 0)
                    return;

                int num = Random.Range(0, possibleRoomtile.Count);


                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x, possibleRoomtile[num].y, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x + 1, possibleRoomtile[num].y, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x + 1, possibleRoomtile[num].y + 1, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x, possibleRoomtile[num].y + 1, 0), tlRoom);

                Instantiate(RoomMedium, grid.GetWorldPosition(possibleRoomtile[num].x, possibleRoomtile[num].y), Quaternion.identity);

                SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y + 1, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y + 1, tlnumrooms);
            }


            //large Room
            else if (num2 == 1)
            {
                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (tilemap.GetTile(new Vector3Int(i, j, 0)) == tlSpace)
                        {

                            if (bigRoomavaiable(i, j))
                            {
                                possibleRoomtile.Add(new Vector3Int(i, j, 0));
                            }

                        }
                    }
                }

                if (possibleRoomtile.Count == 0)
                    return;

                int num = Random.Range(0, possibleRoomtile.Count);


                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x, possibleRoomtile[num].y, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x + 1, possibleRoomtile[num].y, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x + 1, possibleRoomtile[num].y + 1, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x, possibleRoomtile[num].y + 1, 0), tlRoom);

                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x, possibleRoomtile[num].y + 2, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x + 1, possibleRoomtile[num].y + 2, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x + 2, possibleRoomtile[num].y, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x + 2, possibleRoomtile[num].y + 1, 0), tlRoom);
                tilemap.SetTile(new Vector3Int(possibleRoomtile[num].x + 2, possibleRoomtile[num].y + 2, 0), tlRoom);

                Instantiate(RoomLarge, grid.GetWorldPosition(possibleRoomtile[num].x, possibleRoomtile[num].y), Quaternion.identity);

                SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y + 1, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y + 1, tlnumrooms);

                SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y + 2, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y + 2, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x + 2, possibleRoomtile[num].y, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x + 2, possibleRoomtile[num].y + 1, tlnumrooms);
                SetValueifnotroom(possibleRoomtile[num].x + 2, possibleRoomtile[num].y + 2, tlnumrooms);
            }


            




            /*
            SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y, tlnumrooms);
            SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y + 1, tlnumrooms);
            SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y - 1, 0);



            SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y, tlnumrooms);
            SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y + 1, tlnumrooms);
            SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y - 1, 0);



            SetValueifnotroom(possibleRoomtile[num].x - 1, possibleRoomtile[num].y, 0);
            SetValueifnotroom(possibleRoomtile[num].x - 1, possibleRoomtile[num].y - 1, 0);
            SetValueifnotroom(possibleRoomtile[num].x - 1, possibleRoomtile[num].y + 1, 0);



            SetValueifnotroom(possibleRoomtile[num].x + 2, possibleRoomtile[num].y - 2, 0);
            SetValueifnotroom(possibleRoomtile[num].x + 2, possibleRoomtile[num].y - 1, 0);
            SetValueifnotroom(possibleRoomtile[num].x + 2, possibleRoomtile[num].y, 0);
            SetValueifnotroom(possibleRoomtile[num].x + 2, possibleRoomtile[num].y + 1, 0);
            SetValueifnotroom(possibleRoomtile[num].x + 2, possibleRoomtile[num].y + 2, 0);

            SetValueifnotroom(possibleRoomtile[num].x - 2, possibleRoomtile[num].y - 2, 0);
            SetValueifnotroom(possibleRoomtile[num].x - 2, possibleRoomtile[num].y - 1, 0);
            SetValueifnotroom(possibleRoomtile[num].x - 2, possibleRoomtile[num].y, 0);
            SetValueifnotroom(possibleRoomtile[num].x - 2, possibleRoomtile[num].y + 1, 0);
            SetValueifnotroom(possibleRoomtile[num].x - 2, possibleRoomtile[num].y + 2, 0);

            SetValueifnotroom(possibleRoomtile[num].x - 1, possibleRoomtile[num].y + 2, 0);
            SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y + 2, 0);
            SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y + 2, 0);
            SetValueifnotroom(possibleRoomtile[num].x - 1, possibleRoomtile[num].y - 2, 0);
            SetValueifnotroom(possibleRoomtile[num].x, possibleRoomtile[num].y - 2, 0);
            SetValueifnotroom(possibleRoomtile[num].x + 1, possibleRoomtile[num].y - 2, 0);

            /*

            possibleRoomtile.Remove(new Vector3Int(possibleRoomtile[num].x, possibleRoomtile[num].y, 0));
            possibleRoomtile.Remove(new Vector3Int(possibleRoomtile[num].x, possibleRoomtile[num].y - 1, 0));
            possibleRoomtile.Remove(new Vector3Int(possibleRoomtile[num].x, possibleRoomtile[num].y + 1, 0));

            possibleRoomtile.Remove(new Vector3Int(possibleRoomtile[num].x + 1, possibleRoomtile[num].y, 0));
            possibleRoomtile.Remove(new Vector3Int(possibleRoomtile[num].x + 1, possibleRoomtile[num].y + 1, 0));
            possibleRoomtile.Remove(new Vector3Int(possibleRoomtile[num].x + 1, possibleRoomtile[num].y - 1, 0));


            possibleRoomtile.Remove(new Vector3Int(possibleRoomtile[num].x - 1, possibleRoomtile[num].y, 0));
            possibleRoomtile.Remove(new Vector3Int(possibleRoomtile[num].x - 1, possibleRoomtile[num].y - 1, 0));
            possibleRoomtile.Remove(new Vector3Int(possibleRoomtile[num].x - 1, possibleRoomtile[num].y + 1, 0));

            */

            Debug.Log(possibleRoomtile.Count);

            tlnumrooms++;
        }
    }

    void SetValueifnotroom(int x, int y, int value)
    {
        if(grid.GetValue(x, y) <= 0)
        {
            grid.SetValue(x, y, value);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.y = 0f;
        return vec;
    }

    bool smallRoomavaiable(int i, int j)
    {

        if (grid.GetValue(i, j) >= 0) return false;
        if (grid.GetValue(i + 1, j) >= 0) return false;
        if (grid.GetValue(i + 1, j + 1) >= 0) return false;
        if (grid.GetValue(i, j + 1) >= 0) return false;

        if (grid.GetValue(i - 1, j - 1) > 0) return false;
        if (grid.GetValue(i - 1, j) > 0) return false;
        if (grid.GetValue(i - 1, j + 1) > 0) return false;
        if (grid.GetValue(i - 1, j + 2) > 0) return false;

        if (grid.GetValue(i, j - 1) > 0) return false;
        if (grid.GetValue(i, j + 2) > 0) return false;

        if (grid.GetValue(i + 1, j - 1) > 0) return false;
        if (grid.GetValue(i + 1, j + 2) > 0) return false;

        if (grid.GetValue(i + 2, j - 1) > 0) return false;
        if (grid.GetValue(i + 2, j) > 0) return false;
        if (grid.GetValue(i + 2, j + 1) > 0) return false;
        if (grid.GetValue(i + 2, j + 2) > 0) return false;



        return true;
    }

    bool bigRoomavaiable(int i, int j)
    {
        if (grid.GetValue(i, j) >= 0) return false;
        if (grid.GetValue(i + 1, j) >= 0) return false;
        if (grid.GetValue(i + 1, j + 1) >= 0) return false;
        if (grid.GetValue(i, j + 1) >= 0) return false;

        if (grid.GetValue(i - 1, j - 1) > 0) return false;
        if (grid.GetValue(i - 1, j) > 0) return false;
        if (grid.GetValue(i - 1, j + 1) > 0) return false;
        if (grid.GetValue(i - 1, j + 2) > 0) return false;

        if (grid.GetValue(i, j - 1) > 0) return false;
        if (grid.GetValue(i, j + 2) >= 0) return false;

        if (grid.GetValue(i + 1, j - 1) > 0) return false;
        if (grid.GetValue(i + 1, j + 2) >= 0) return false;

        if (grid.GetValue(i + 2, j - 1) > 0) return false;
        if (grid.GetValue(i + 2, j) >= 0) return false;
        if (grid.GetValue(i + 2, j + 1) >= 0) return false;
        if (grid.GetValue(i + 2, j + 2) >= 0) return false;




        if (grid.GetValue(i - 1, j + 3) > 0) return false;
        if (grid.GetValue(i, j + 3) > 0) return false;
        if (grid.GetValue(i + 1, j + 3) > 0) return false;
        if (grid.GetValue(i + 2, j + 3) > 0) return false;
        if (grid.GetValue(i + 3, j + 3) > 0) return false;
        if (grid.GetValue(i + 3, j + 2) > 0) return false;
        if (grid.GetValue(i + 3, j + 1) > 0) return false;
        if (grid.GetValue(i + 3, j) > 0) return false;
        if (grid.GetValue(i + 3, j - 1) > 0) return false;


        return true;
    }

    
}
