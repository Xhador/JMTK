using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fox : MonoBehaviour
{
    public Tilemap tilemap;

    public GameObject FoxGO;

    public Tile tlGrass;
    public Tile tlWater;
    public Tile tlFood;
    public Tile tlRand;
    public Tile tlFox;

    public Simulationmanager SM;

    public float currenttimeoffset;
    float time;

    public int foodcount = 30;

    public int sensorydistance = 3;
    public float timeoffset = 1f;
    public float Ruhetimeoffset = 2.5f;
    public int procreationvalue = 30;
    public int procreationfoodtransfer = 10;

    public bool ispregnant;
    public int pregnancyduration = 10;
    public int currentpregnancyvalue = 0;

    float mvar1 = 1;
    float mvar2 = 1;
    float mvar3 = 1;
    float mvar4 = 1;
    float mvarall;

    // Start is called before the first frame update
    void Start()
    {
        int x, y;

        SM.grid.GetXY(gameObject.transform.position, out x, out y);

        transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z + 5);
    }

    // Update is called once per frame
    void Update()
    {


        if (Time.time > time + currenttimeoffset)
        {
            int x, y;

            SM.grid.GetXY(gameObject.transform.position, out x, out y);

            if (FoodOben(x, y, sensorydistance)  || FoodLinks(x, y, sensorydistance) || FoodRechts(x, y, sensorydistance) || FoodUnten(x, y, sensorydistance))
            {
                currenttimeoffset = timeoffset;
            }
            else
            {
                currenttimeoffset = Ruhetimeoffset;
            }


            time = Time.time;

            mvar1 = 1;
            mvar2 = 1;
            mvar3 = 1;
            mvar4 = 1;

            Movement();


        }

    }

    public void SetSpeed(float timeoffsetspeed)
    {
        timeoffset = timeoffsetspeed;
    }
    public void SetRuheSpeed(float Ruhetimeoffsetspeed)
    {
        Ruhetimeoffset = Ruhetimeoffsetspeed;
    }
    public void SetFoodcount(int newfoodcount)
    {
        foodcount = newfoodcount;
    }
    public void SetProcreationvalue(int newprocreationvalue)
    {
        procreationvalue = newprocreationvalue;
    }
    public void SetProcreationfoodtransfer(int newprocreationfoodtransfer)
    {
        procreationfoodtransfer = newprocreationfoodtransfer;
    }
    public void SetSensoryDistance(int newsensorydistance)
    {
        sensorydistance = newsensorydistance;
    }


    public bool ifTierAlone(int i, int j)
    {
        GameObject[] gameObjects;
        int x, y;
        gameObjects = GameObject.FindGameObjectsWithTag("Fox");
        foreach (var obj in gameObjects)
        {
            if (obj != gameObject)
            {
                SM.grid.GetXY(obj.transform.position, out x, out y);

                if (x == i && y == j) return false;
            }

        }
        return true;
    }



    void Movement()
    {
        foodcount--;


        if (currentpregnancyvalue > 0)
        {
            if (currentpregnancyvalue % 2 == 1)
            {
                foodcount--;
            }


            currentpregnancyvalue--;
        }



        if (currentpregnancyvalue <= 0 && ispregnant)
        {
            ispregnant = false;

            GameObject varFox = Instantiate(FoxGO, this.transform.position, transform.rotation);

            float varoffsetspeed = timeoffset + Random.Range(-.1f, .1f);
            float varRuheoffsetspeed = Ruhetimeoffset + Random.Range(-.2f, .2f);
            int varprocreationfoodtransfer = procreationfoodtransfer + Random.Range(-3, 4);
            int varprocreationvalue = procreationvalue + Random.Range(-3, 4);
            int varSensorydistance = sensorydistance + Random.Range(-2, 3);

            varFox.gameObject.SetActive(true);

            varFox.GetComponent<Fox>().SetRuheSpeed(varRuheoffsetspeed);
            varFox.GetComponent<Fox>().SetSensoryDistance(varSensorydistance);
            varFox.GetComponent<Fox>().foodcount = procreationfoodtransfer;
            varFox.GetComponent<Fox>().SetSpeed(varoffsetspeed);
            varFox.GetComponent<Fox>().SetProcreationfoodtransfer(varprocreationfoodtransfer);
            varFox.GetComponent<Fox>().SetProcreationvalue(varprocreationvalue);


            foodcount = foodcount - procreationfoodtransfer - varSensorydistance;
        }


        if (foodcount > procreationvalue && ispregnant == false)
        {
            ispregnant = true;
            currentpregnancyvalue = pregnancyduration;
        }


        int x, y;

        SM.grid.GetXY(gameObject.transform.position, out x, out y);


        if (foodcount <= 0)
        {
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            Destroy(gameObject);

            return;
        }
        


        if (tilemap.GetTile(new Vector3Int(x, y + 1, 0)) == tlFood)
        {
            tilemap.SetTile(new Vector3Int(x, y + 1, 0), tlGrass);
            foodcount += 15;
            return;
        }
        if (tilemap.GetTile(new Vector3Int(x, y - 1, 0)) == tlFood)
        {
            tilemap.SetTile(new Vector3Int(x, y - 1, 0), tlGrass);
            foodcount += 15;
            return;
        }
        if (tilemap.GetTile(new Vector3Int(x + 1, y, 0)) == tlFood)
        {
            tilemap.SetTile(new Vector3Int(x + 1, y, 0), tlGrass);
            foodcount += 15;
            return;
        }
        if (tilemap.GetTile(new Vector3Int(x - 1, y, 0)) == tlFood)
        {
            tilemap.SetTile(new Vector3Int(x - 1, y, 0), tlGrass);
            foodcount += 15;
            return;
        }


        int randomint = Random.Range(1, 3);

        Debug.Log("randomint = " + randomint);

        if (randomint == 1)
        {
            for(int i = 1; i <= sensorydistance; i++)
            {
                if (FoodOben(x, y, i))
                {
                    tilemap.SetTile(new Vector3Int(x, y + 1, 0), tlFox);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z + 15);
                    return;
                }
                if (FoodRechts(x, y, i))
                {
                    tilemap.SetTile(new Vector3Int(x + 1, y, 0), tlFox);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 15, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                    return;
                }
                if (FoodLinks(x, y, i))
                {
                    tilemap.SetTile(new Vector3Int(x - 1, y, 0), tlFox);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x - 5, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                    return;
                }
                if (FoodUnten(x, y, i))
                {
                    tilemap.SetTile(new Vector3Int(x, y - 1, 0), tlFox);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z - 5);
                    return;
                }
            }
        }
        if (randomint == 2)
        {
            for (int i = 1; i <= sensorydistance; i++)
            {
                if (FoodUnten(x, y, i))
                {
                    tilemap.SetTile(new Vector3Int(x, y - 1, 0), tlFox);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z - 5);
                    return;
                }
                if (FoodLinks(x, y, i))
                {
                    tilemap.SetTile(new Vector3Int(x - 1, y, 0), tlFox);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x - 5, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                    return;
                }
                if (FoodRechts(x, y, i))
                {
                    tilemap.SetTile(new Vector3Int(x + 1, y, 0), tlFox);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 15, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                    return;
                }

                if (FoodOben(x, y, i))
                {
                    tilemap.SetTile(new Vector3Int(x, y + 1, 0), tlFox);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z + 15);
                    return;
                }
            }
        }








        mvarall = mvar1 + mvar2 + mvar3 + mvar4;

        if (mvarall == 0) return;

        float random = Random.Range(0, mvarall);

        if (tilemap.GetTile(new Vector3Int(x, y + 1, 0)) == tlRand || tilemap.GetTile(new Vector3Int(x, y + 1, 0)) == tlWater)
        {
            mvar1 = 0;
        }
        if (tilemap.GetTile(new Vector3Int(x, y - 1, 0)) == tlRand || tilemap.GetTile(new Vector3Int(x, y - 1, 0)) == tlWater)
        {
            mvar4 = 0;
        }
        if (tilemap.GetTile(new Vector3Int(x + 1, y, 0)) == tlRand || tilemap.GetTile(new Vector3Int(x + 1, y, 0)) == tlWater)
        {
            mvar2 = 0;
        }
        if (tilemap.GetTile(new Vector3Int(x - 1, y, 0)) == tlRand || tilemap.GetTile(new Vector3Int(x + 1, y, 0)) == tlWater)
        {
            mvar3 = 0;
        }

        if (random <= mvar1)
        {
            if (tilemap.GetTile(new Vector3Int(x, y + 1, 0)) == tlWater) return;
            tilemap.SetTile(new Vector3Int(x, y + 1, 0), tlFox);
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z + 15);
        }
        else if (random <= mvar1 + mvar2)
        {
            if (tilemap.GetTile(new Vector3Int(x + 1, y, 0)) == tlWater) return;
            tilemap.SetTile(new Vector3Int(x + 1, y, 0), tlFox);
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 15, 0, SM.grid.GetWorldPosition(x, y).z + 5);
        }
        else if (random <= mvar1 + mvar2 + mvar3)
        {
            if (tilemap.GetTile(new Vector3Int(x - 1, y, 0)) == tlWater) return;
            tilemap.SetTile(new Vector3Int(x - 1, y, 0), tlFox);
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x - 5, 0, SM.grid.GetWorldPosition(x, y).z + 5);
        }
        else if (random <= mvar1 + mvar2 + mvar3 + mvar4)
        {
            if (tilemap.GetTile(new Vector3Int(x, y - 1, 0)) == tlWater) return;
            tilemap.SetTile(new Vector3Int(x, y - 1, 0), tlFox);
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z - 5);
        }
    }











    bool FoodOben(int i, int j, int varsensorydistance)
    {


        if (varsensorydistance >= 1)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 1, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 2)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 2, 0)) == tlFood) return true;
        }



        if (varsensorydistance >= 3)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 2, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 4)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 3, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 5)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 6, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j + 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j + 3, 0)) == tlFood) return true;
        }

        if (varsensorydistance >= 6)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 7, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 6, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 6, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j + 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j + 4, 0)) == tlFood) return true;
        }


        return false;
    }
    bool FoodRechts(int i, int j, int varsensorydistance)
    {


        if (varsensorydistance >= 1)
        {
            if (tilemap.GetTile(new Vector3Int(i + 2, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 1, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 2)
        {
            if (tilemap.GetTile(new Vector3Int(i + 3, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 1, 0)) == tlFood) return true;
        }



        if (varsensorydistance >= 3)
        {
            if (tilemap.GetTile(new Vector3Int(i + 4, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j - 1, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 4)
        {
            if (tilemap.GetTile(new Vector3Int(i + 5, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j + 2, 0)) == tlFood) return true;
        }

        if (varsensorydistance >= 5)
        {
            if (tilemap.GetTile(new Vector3Int(i + 6, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 5, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 5, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j + 2, 0)) == tlFood) return true;
        }

        if (varsensorydistance >= 6)
        {
            if (tilemap.GetTile(new Vector3Int(i + 7, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 6, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 6, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 5, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 5, j + 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j - 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j + 3, 0)) == tlFood) return true;
        }



        return false;
    }
    bool FoodLinks(int i, int j, int varsensorydistance)
    {

        if (varsensorydistance >= 1)
        {
            if (tilemap.GetTile(new Vector3Int(i - 2, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 1, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 2)
        {
            if (tilemap.GetTile(new Vector3Int(i - 3, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 1, 0)) == tlFood) return true;
        }



        if (varsensorydistance >= 3)
        {
            if (tilemap.GetTile(new Vector3Int(i - 4, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j - 1, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 4)
        {
            if (tilemap.GetTile(new Vector3Int(i - 5, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j + 2, 0)) == tlFood) return true;
        }

        if (varsensorydistance >= 5)
        {
            if (tilemap.GetTile(new Vector3Int(i - 6, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 5, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 5, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j + 2, 0)) == tlFood) return true;
        }

        if (varsensorydistance >= 6)
        {
            if (tilemap.GetTile(new Vector3Int(i - 7, j, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 6, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 6, j + 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 5, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 5, j + 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j - 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j + 3, 0)) == tlFood) return true;
        }

        return false;
    }
    bool FoodUnten(int i, int j, int varsensorydistance)
    {

        if (varsensorydistance >= 1)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 1, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 2)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 2, 0)) == tlFood) return true;
        }



        if (varsensorydistance >= 3)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 2, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 4)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 3, 0)) == tlFood) return true;
        }


        if (varsensorydistance >= 5)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 6, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j - 3, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j - 3, 0)) == tlFood) return true;
        }

        if (varsensorydistance >= 6)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 7, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 6, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 6, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 5, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j - 4, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j - 4, 0)) == tlFood) return true;
        }
        return false;
    }

}
