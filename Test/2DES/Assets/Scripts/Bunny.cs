using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bunny : MonoBehaviour
{
    public Tilemap tilemap;

    public GameObject BunnyGO;

    public Tile tlGrass;
    public Tile tlWater;
    public Tile tlFood;
    public Tile tlRand;
    public Tile tlFox;
    public Tile tlBunny;


    public Simulationmanager SM;

    public bool ispregnant;
    
    float time;

    public int Nahrungswert = 30;


    
    public float Zeitversatz = 1f;
    public int Vermehrungswert = 30;
    public int Fortplanzungsessentransfer = 10;
    public int sensorydistance = 3;


    public int pregnancyduration = 5;

    public float babytimeoffset = 2.5f;
    public int babysensorydistance = 1;



    public int currentpregnancyvalue = 0;

    public int growuptime = 0;


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

        transform.position = new Vector3 (SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z + 5);

    }

    // Update is called once per frame
    void Update()
    {
        int x, y;
        SM.grid.GetXY(transform.position, out x, out y);

        if (tilemap.GetTile(new Vector3Int(x, y, 0)) == tlGrass)
        {
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            Destroy(gameObject);

            return;
        }

        float varZeitversatz;

        if (growuptime > 0)
        {
            varZeitversatz = babytimeoffset;
        }
        else
        {
            transform.localScale = new Vector3(10, 10, 0);
            varZeitversatz = Zeitversatz;
        }

        

        if (Time.time > time + varZeitversatz)
        {
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
        Zeitversatz = timeoffsetspeed;
    }
    public void SetFoodcount(int newfoodcount)
    {
        Nahrungswert = newfoodcount;
    }
    public void SetProcreationvalue(int newprocreationvalue)
    {
        Vermehrungswert = newprocreationvalue;
    }
    public void SetProcreationfoodtransfer(int newprocreationfoodtransfer)
    {
        Fortplanzungsessentransfer = newprocreationfoodtransfer;
    }
    public void SetSensoryDistance(int newsensorydistance)
    {
        sensorydistance = newsensorydistance;
    }
    public void SetPregnancyduration(int newpregnancyduration)
    {
        pregnancyduration = newpregnancyduration;
    }
    public void SetBabytimeoffset(float newbabytimeoffset)
    {
        babytimeoffset = newbabytimeoffset;
    }
    public void SetBabysensorydistance(int newbabysensorydistance)
    {
        babysensorydistance = newbabysensorydistance;
    }
    public bool ifTierAlone(int i, int j)
    {
        GameObject[] gameObjects;
        int x, y;
        gameObjects = GameObject.FindGameObjectsWithTag("Bunny");
        foreach (var obj in gameObjects)
        {
            if(obj != gameObject)
            {
                SM.grid.GetXY(obj.transform.position, out x, out y);

                if (x == i && y == j) return false;
            }

        }
        return true;
    }

    void Movement()
    {
        Nahrungswert--;

        if(growuptime > 0)
        {
            growuptime--;
        }
        if (currentpregnancyvalue > 0)
        {
            /*
            if(currentpregnancyvalue % 2 == 1)
            {
                Nahrungswert--;
            }
            */

            currentpregnancyvalue--;
        }




        if (currentpregnancyvalue <= 0 && ispregnant)
        {
            ispregnant = false;

            GameObject varBunny = Instantiate(BunnyGO, this.transform.position, transform.rotation);

            // Get Genesvalue +- randomisierter Zahl
            float varZeitversatz = Zeitversatz + Random.Range(-.1f, .1f);
            int varFortplanzungsessentransfer = Fortplanzungsessentransfer + Random.Range(-2, 3);
            int varVermehrungswert = Vermehrungswert + Random.Range(-2, 3);
            int varSensorydistance = sensorydistance + Random.Range(-1, 2);
            int varpregnancyduration = pregnancyduration + Random.Range(-1, 2);
            int varbabysensorydistance = babysensorydistance + Random.Range(-1, 2);
            float varbabytimeoffset = babytimeoffset + Random.Range(-.2f, .2f);

            if(varbabysensorydistance < 0) varbabysensorydistance = 0;
            if (varSensorydistance < 0) varSensorydistance = 0;

            varBunny.gameObject.SetActive(true);

            // Set Genes
            varBunny.GetComponent<Bunny>().SetSensoryDistance(varSensorydistance);
            varBunny.GetComponent<Bunny>().SetSpeed(varZeitversatz);
            varBunny.GetComponent<Bunny>().SetProcreationfoodtransfer(varFortplanzungsessentransfer);
            varBunny.GetComponent<Bunny>().SetProcreationvalue(varVermehrungswert);
            varBunny.GetComponent<Bunny>().SetFoodcount(Fortplanzungsessentransfer);
            varBunny.GetComponent<Bunny>().SetPregnancyduration(varpregnancyduration);
            varBunny.GetComponent<Bunny>().SetBabysensorydistance(varbabysensorydistance);
            varBunny.GetComponent<Bunny>().SetBabytimeoffset(varbabytimeoffset);
            varBunny.GetComponent<Bunny>().growuptime = 10 - pregnancyduration;


            varBunny.transform.localScale = new Vector3(5, 5, 0);

            Nahrungswert = Nahrungswert - Fortplanzungsessentransfer - varSensorydistance - varbabysensorydistance;
        }


        if (Nahrungswert > Vermehrungswert && growuptime <= 0 && ispregnant == false)
        {
            ispregnant = true;
            currentpregnancyvalue = pregnancyduration;
        }


        int x, y;

        SM.grid.GetXY(gameObject.transform.position, out x, out y);




        if (Nahrungswert <= 0)
        {
            if(ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            Destroy(gameObject);

            return;
        }





        if (tilemap.GetTile(new Vector3Int(x, y + 1, 0)) == tlFood)
        {
            tilemap.SetTile(new Vector3Int(x, y + 1, 0), tlGrass);
            Nahrungswert += 10;
            return;
        }
        if (tilemap.GetTile(new Vector3Int(x, y - 1, 0)) == tlFood)
        {
            tilemap.SetTile(new Vector3Int(x, y - 1, 0), tlGrass);
            Nahrungswert += 10;
            return;
        }
        if (tilemap.GetTile(new Vector3Int(x + 1, y, 0)) == tlFood)
        {
            tilemap.SetTile(new Vector3Int(x + 1, y, 0), tlGrass);
            Nahrungswert += 10;
            return;
        }
        if (tilemap.GetTile(new Vector3Int(x - 1, y, 0)) == tlFood)
        {
            tilemap.SetTile(new Vector3Int(x - 1, y, 0), tlGrass);
            Nahrungswert += 10;
            return;
        }


        int varsensorydistance;

        if(growuptime > 0)
        {
            varsensorydistance = babysensorydistance;
        }
        else
        {
            varsensorydistance = sensorydistance;
        }



        for (int i = 1; i <= varsensorydistance; i++)
        {
            if (FoxOben(x, y, i))
            {
                if (tilemap.GetTile(new Vector3Int(x, y - 1, 0)) == tlRand) break;

                tilemap.SetTile(new Vector3Int(x, y - 1, 0), tlBunny);
                if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z - 5);
                return;
            }
            if (FoxRechts(x, y, i))
            {
                if (tilemap.GetTile(new Vector3Int(x - 1, y, 0)) == tlRand) break;

                tilemap.SetTile(new Vector3Int(x - 1, y, 0), tlBunny);
                if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x - 5, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                return;
            }
            if (FoxLinks(x, y, i))
            {
                if (tilemap.GetTile(new Vector3Int(x + 1, y, 0)) == tlRand) break;

                tilemap.SetTile(new Vector3Int(x + 1, y, 0), tlBunny);
                if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 15, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                return;
            }
            if (FoxUnten(x, y, i))
            {
                if (tilemap.GetTile(new Vector3Int(x, y + 1, 0)) == tlRand) break;

                tilemap.SetTile(new Vector3Int(x, y + 1, 0), tlBunny);
                if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z + 15);
                return;
            }
        }




        int randomint = Random.Range(1, 5);

        Debug.Log("randomint = " + randomint);

        if(randomint == 1)
        {
            for(int i = 1; i <= varsensorydistance; i++)
            {
                if (FoodOben(x, y, i))
                {
                    tilemap.SetTile(new Vector3Int(x, y + 1, 0), tlBunny);

                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z + 15);
                    return;
                }
                if (FoodLinks(x, y, i))
                {

                    tilemap.SetTile(new Vector3Int(x - 1, y, 0), tlBunny);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x - 5, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                    return;
                }
                if (FoodRechts(x, y, i))
                {

                    tilemap.SetTile(new Vector3Int(x + 1, y, 0), tlBunny);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 15, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                    return;
                }

                if (FoodUnten(x, y, i))
                {

                    tilemap.SetTile(new Vector3Int(x, y - 1, 0), tlBunny);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z - 5);
                    return;
                }
            } 


        }
        if (randomint == 2)
        {
            for (int i = 1; i <= varsensorydistance; i++)
            {
                if (FoodUnten(x, y, i))
                {

                    tilemap.SetTile(new Vector3Int(x, y - 1, 0), tlBunny);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z - 5);
                    return;
                }
                if (FoodRechts(x, y, i))
                {

                    tilemap.SetTile(new Vector3Int(x + 1, y, 0), tlBunny);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 15, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                    return;
                }
                if (FoodLinks(x, y, i))
                {

                    tilemap.SetTile(new Vector3Int(x - 1, y, 0), tlBunny);
                    if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
                    transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x - 5, 0, SM.grid.GetWorldPosition(x, y).z + 5);
                    return;
                }
                if (FoodOben(x, y, i))
                {

                    tilemap.SetTile(new Vector3Int(x, y + 1, 0), tlBunny);
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

            tilemap.SetTile(new Vector3Int(x, y + 1, 0), tlBunny);
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            if (tilemap.GetTile(new Vector3Int(x, y + 1, 0)) == tlWater) return;
            transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 5, 0, SM.grid.GetWorldPosition(x, y).z + 15);
        }
        else if (random <= mvar1 + mvar2)
        {

            tilemap.SetTile(new Vector3Int(x + 1, y, 0), tlBunny);
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            if (tilemap.GetTile(new Vector3Int(x + 1, y, 0)) == tlWater) return;
            transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x + 15, 0, SM.grid.GetWorldPosition(x, y).z + 5);
        }
        else if (random <= mvar1 + mvar2 + mvar3)
        {

            tilemap.SetTile(new Vector3Int(x - 1, y, 0), tlBunny);
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            if (tilemap.GetTile(new Vector3Int(x - 1, y, 0)) == tlWater) return;
            transform.position = new Vector3(SM.grid.GetWorldPosition(x, y).x - 5, 0, SM.grid.GetWorldPosition(x, y).z + 5);
        }
        else if (random <= mvar1 + mvar2 + mvar3 + mvar4)
        {

            tilemap.SetTile(new Vector3Int(x, y - 1, 0), tlBunny);
            if (ifTierAlone(x, y)) tilemap.SetTile(new Vector3Int(x, y, 0), tlGrass);
            if (tilemap.GetTile(new Vector3Int(x, y - 1, 0)) == tlWater) return;
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

        if(varsensorydistance >= 1)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 2, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 1, 0)) == tlFood) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 1, 0)) == tlFood) return true;
        }


        if(varsensorydistance >= 2)
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


    bool FoxOben(int i, int j, int varsensorydistance)
    {

        if(varsensorydistance == 0)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 1, 0)) == tlFox) return true;
        }

        if (varsensorydistance >= 1)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 1, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 2)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 2, 0)) == tlFox) return true;
        }



        if (varsensorydistance >= 3)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 2, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 4)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 3, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 5)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 6, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j + 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j + 3, 0)) == tlFox) return true;
        }

        if (varsensorydistance >= 6)
        {
            if (tilemap.GetTile(new Vector3Int(i, j + 7, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 6, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 6, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j + 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j + 4, 0)) == tlFox) return true;
        }


        return false;
    }
    bool FoxRechts(int i, int j, int varsensorydistance)
    {

        if (varsensorydistance == 0)
        {
            if (tilemap.GetTile(new Vector3Int(i + 1, j, 0)) == tlFox) return true;
        }

        if (varsensorydistance >= 1)
        {
            if (tilemap.GetTile(new Vector3Int(i + 2, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j + 1, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 2)
        {
            if (tilemap.GetTile(new Vector3Int(i + 3, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 1, 0)) == tlFox) return true;
        }



        if (varsensorydistance >= 3)
        {
            if (tilemap.GetTile(new Vector3Int(i + 4, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j - 1, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 4)
        {
            if (tilemap.GetTile(new Vector3Int(i + 5, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j - 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j - 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j + 2, 0)) == tlFox) return true;
        }

        if (varsensorydistance >= 5)
        {
            if (tilemap.GetTile(new Vector3Int(i + 6, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 5, j - 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 5, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j - 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j + 2, 0)) == tlFox) return true;
        }

        if (varsensorydistance >= 6)
        {
            if (tilemap.GetTile(new Vector3Int(i + 7, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 6, j - 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 6, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 5, j - 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 5, j + 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j - 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 4, j + 3, 0)) == tlFox) return true;
        }



        return false;
    }
    bool FoxLinks(int i, int j, int varsensorydistance)
    {

        if (varsensorydistance == 0)
        {
            if (tilemap.GetTile(new Vector3Int(i - 1, j, 0)) == tlFox) return true;
        }

        if (varsensorydistance >= 1)
        {
            if (tilemap.GetTile(new Vector3Int(i - 2, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j + 1, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 2)
        {
            if (tilemap.GetTile(new Vector3Int(i - 3, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 1, 0)) == tlFox) return true;
        }



        if (varsensorydistance >= 3)
        {
            if (tilemap.GetTile(new Vector3Int(i - 4, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j - 1, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 4)
        {
            if (tilemap.GetTile(new Vector3Int(i - 5, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j - 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j - 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j + 2, 0)) == tlFox) return true;
        }

        if (varsensorydistance >= 5)
        {
            if (tilemap.GetTile(new Vector3Int(i - 6, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 5, j - 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 5, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j - 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j + 2, 0)) == tlFox) return true;
        }

        if (varsensorydistance >= 6)
        {
            if (tilemap.GetTile(new Vector3Int(i - 7, j, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 6, j - 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 6, j + 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 5, j - 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 5, j + 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j - 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 4, j + 3, 0)) == tlFox) return true;
        }

        return false;
    }
    bool FoxUnten(int i, int j, int varsensorydistance)
    {
        if (varsensorydistance == 0)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 1, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 1)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 1, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 1, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 2)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 2, 0)) == tlFox) return true;
        }



        if (varsensorydistance >= 3)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 2, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 2, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 4)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 3, 0)) == tlFox) return true;
        }


        if (varsensorydistance >= 5)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 6, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j - 3, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j - 3, 0)) == tlFox) return true;
        }

        if (varsensorydistance >= 6)
        {
            if (tilemap.GetTile(new Vector3Int(i, j - 7, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 1, j - 6, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 1, j - 6, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 2, j - 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 2, j - 5, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i - 3, j - 4, 0)) == tlFox) return true;
            if (tilemap.GetTile(new Vector3Int(i + 3, j - 4, 0)) == tlFox) return true;
        }
        return false;
    }

}
