using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArrayGrid
{
    
    int width;
    int height;
    private float cellsize;
    int[,] gridArray;
    TextMesh[,] debugTextArray;

    public ArrayGrid(int width, int height, float cellsize)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x=0; x<gridArray.GetLength(0); x++)
        {
            for(int y = 0; y<gridArray.GetLength(1); y++)
            {
                //debugTextArray[x, y] = CreateWorldText(null, gridArray[x,y].ToString(), GetWorldPosition(x, y) + new Vector3 (cellsize, cellsize) * .5f, 40, Color.white, TextAnchor.MiddleCenter, 0);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        SetValue(2, 1, 56);
    }

    private TextMesh CreateWorldText(Transform parent, string text, Vector3 localposition, int fontSize, Color color, TextAnchor textAnchor, int sortingorder)
    {
        GameObject gameObject = new GameObject("World_text", typeof(TextMesh));
        Transform transfrom = gameObject.transform;
        transfrom.SetParent(parent, false);
        transfrom.localPosition = localposition + new Vector3(0, -5, 5);
        transfrom.Rotate(new Vector3(90, 0, 0));
        TextMesh textMesh = gameObject.GetComponent <TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingorder;
        return textMesh;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellsize, 0, y * cellsize);
    }


    public void GetXY(Vector3 worldposition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldposition.x / cellsize);
        y = Mathf.FloorToInt(worldposition.z / cellsize);
    }

    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;

            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
        else
        {
            Debug.Log("was zum fick " + x + y);
        }
    }

    public void SetValue(Vector3 worldposition, int value)
    {
        int x, y;
        GetXY(worldposition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return gridArray[x, y];
        else
            return 0;
    }

    public int GetValue(Vector3 worldposition)
    {
        int x, y;
        GetXY(worldposition, out x, out y);
        return GetValue(x, y);
    }

    public int[,] GetGridArray()
    {
        return gridArray;
    }
}

