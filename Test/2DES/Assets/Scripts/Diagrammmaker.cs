using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Diagrammmaker : MonoBehaviour
{
    float time;
    GameObject[] gameObjects;

    public int population = 0;

    public int Dpopulation = 0;

    public float DZeitversatz = 0;

    public float DVermehrungswert = 0;

    public float DFortplanzungsessentransfer = 0;

    public float Dsensorydistance = 0;

    public float Dpregnancyduration = 0;

    public float Dbabytimeoffset = 0;

    public float Dbabysensorydistance = 0;


    public int Cpopulation = 0;

    public float CZeitversatz = 0;

    public float CRuheZeitversatz = 0;

    public float CVermehrungswert = 0;

    public float CFortplanzungsessentransfer = 0;

    public float Csensorydistance = 0;

  



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > time + 60)
        {
            time = Time.time;
            gameObjects = GameObject.FindGameObjectsWithTag("Bunny");
            population = gameObjects.Length;

            Dpopulation = population;

            float zDZeitversatz = 0;
            foreach(var obj in gameObjects)
            {
                zDZeitversatz += obj.GetComponent<Bunny>().Zeitversatz;
            }
            DZeitversatz = zDZeitversatz / (float)population;

            float zDVermehrungswert = 0;
            foreach (var obj in gameObjects)
            {
                zDVermehrungswert += obj.GetComponent<Bunny>().Vermehrungswert;
            }
            DVermehrungswert = zDVermehrungswert / (float)population;

            float zDFortplanzungsessentransfer = 0;
            foreach (var obj in gameObjects)
            {
                zDFortplanzungsessentransfer += obj.GetComponent<Bunny>().Fortplanzungsessentransfer;
            }
            DFortplanzungsessentransfer = zDFortplanzungsessentransfer / (float)population;

            float zDsensorydistance = 0;
            foreach (var obj in gameObjects)
            {
                zDsensorydistance += obj.GetComponent<Bunny>().sensorydistance;
            }
            Dsensorydistance = zDsensorydistance / (float)population;

            float zDpregnancyduration = 0;
            foreach (var obj in gameObjects)
            {
                zDpregnancyduration += obj.GetComponent<Bunny>().pregnancyduration;
            }
            Dpregnancyduration = zDpregnancyduration / (float)population;

            float zDbabytimeoffset = 0;
            foreach (var obj in gameObjects)
            {
                zDbabytimeoffset += obj.GetComponent<Bunny>().babytimeoffset;
            }
            Dbabytimeoffset = zDbabytimeoffset / (float)population;

            float zDbabysensorydistance = 0;
            foreach (var obj in gameObjects)
            {
                zDbabysensorydistance += obj.GetComponent<Bunny>().babysensorydistance;
            }
            Dbabysensorydistance = zDbabysensorydistance / (float)population;






            gameObjects = GameObject.FindGameObjectsWithTag("Fox");
            population = gameObjects.Length;
            Cpopulation = population;

            float zCZeitversatz = 0;
            foreach (var obj in gameObjects)
            {
                zCZeitversatz += obj.GetComponent<Fox>().timeoffset;
            }
            CZeitversatz = zCZeitversatz / (float)population;

            float zCRuheZeitversatz = 0;
            foreach (var obj in gameObjects)
            {
                zCRuheZeitversatz += obj.GetComponent<Fox>().Ruhetimeoffset;
            }
            CRuheZeitversatz = zCRuheZeitversatz / (float)population;

            float zCVermehrungswert = 0;
            foreach (var obj in gameObjects)
            {
                zCVermehrungswert += obj.GetComponent<Fox>().procreationvalue;
            }
            CVermehrungswert = zCVermehrungswert / (float)population;

            float zCFortplanzungsessentransfer = 0;
            foreach (var obj in gameObjects)
            {
                zCFortplanzungsessentransfer += obj.GetComponent<Fox>().procreationfoodtransfer;
            }
            CFortplanzungsessentransfer = zCFortplanzungsessentransfer / (float)population;

            float zCsensorydistance = 0;
            foreach (var obj in gameObjects)
            {
                zCsensorydistance += obj.GetComponent<Fox>().sensorydistance;
            }
            Csensorydistance = zCsensorydistance / (float)population;

        }
    }
}
