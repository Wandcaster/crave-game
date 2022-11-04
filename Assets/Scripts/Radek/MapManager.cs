using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] public GameObject startPosition;
    [SerializeField] public GameObject bossTilePosition;
    [SerializeField] public GameObject eventPrefab;
    [SerializeField] public GameObject currentTile;
    [SerializeField] public int depthLv;//number of tiles starting from start till the boss, excluding start and boss
    [SerializeField] public int seperatePaths = 2;
    [SerializeField] public float range = 9;
    [SerializeField] GameObject player;

    //line renderer colors
    private Color availablePathColor = new Color(255, 0, 0);
    private Color closedPathColor= new Color(255, 255, 255);

    public float distanceY;//wysokosc wzwyz dla kazdego tile'a

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        distanceY = bossTilePosition.transform.position.y - startPosition.transform.position.y;//radius for each tile is 0.5; distance for all tiles
//        distanceY -= (depthLv*0.5f);
        distanceY = (distanceY) / (depthLv+1);//keep in mind that if radius is increased then some may overlap; distance between 2 tiles
//        Debug.Log(distanceY);
        currentTile = startPosition;
        currentTile.GetComponent<LineRenderer>().startColor = availablePathColor;
        currentTile.GetComponent<LineRenderer>().endColor = availablePathColor;

        afterGeneration = false;
        finishedRouteCounter = 0;
        GenerateMap();
    }



    private void GenerateMap()
    {
        eventPrefab.transform.position = startPosition.transform.position;
        float leftLimit = this.transform.position.x-range/2;
        leftLimit += (range/seperatePaths)/2;//mid point of each block
        GenerateRandomEvent eventController;
        eventController = startPosition.GetComponent<GenerateRandomEvent>();

        for (int i = 0; i < seperatePaths; i++)
        {
            GameObject tmp = Instantiate(eventPrefab, this.gameObject.transform);
            tmp.transform.position = new Vector3(
                leftLimit+i*range/seperatePaths,
            tmp.transform.position.y, tmp.transform.position.z);
            
            StartCoroutine(startPosition.GetComponent<TileGenerator>().Generate(tmp, depthLv, gameObject.transform, "ABCD"));
            tmp.GetComponent<RandomizePosition>().enabled = true;

        }

    }

    private bool afterGeneration = false;
    private short finishedRouteCounter = 0;//counter, how many paths there's to generate, so after last path it'll run
    public void AfterGeneration()//this functions runs once after generation of map
    {
        //in short: only last invoke of this method will succesfully pass if
        finishedRouteCounter++;
        if (afterGeneration == false && finishedRouteCounter == seperatePaths)
        {
            player.transform.position = startPosition.transform.position;
            afterGeneration = true;
            ChangeTileState(true);
            GenerateLines(startPosition);
        }
    }

    private void GenerateLines(GameObject obj)
    {
        LineRenderer lr = obj.GetComponent<LineRenderer>();
        List<GameObject> conn = obj.GetComponent<GenerateRandomEvent>().connections;
        lr.positionCount = conn.Count*2;
        

        for(int i = 0; i < conn.Count; i++)
        {
            lr.SetPosition(i * 2, obj.transform.position);
            lr.SetPosition(i * 2+1, conn[i].transform.position);
            GenerateLines(conn[i]);
        }


        //lr.SetPosition();


    }

    private void ChangeTileState(bool state)
    {
        GenerateRandomEvent tmp = currentTile.GetComponent<GenerateRandomEvent>();
        for (int i=0; i< tmp.connections.Count; i++)
        {
            tmp.connections[i].GetComponent<CircleCollider2D>().enabled = state;
        }
    }

    public void ChangeCurrentTile(GameObject clicked)
    {
        currentTile.GetComponent<LineRenderer>().startColor = closedPathColor;
        currentTile.GetComponent<LineRenderer>().endColor = closedPathColor;
        ChangeTileState(false);
        currentTile = clicked;


        player.transform.position = clicked.transform.position;
        currentTile.GetComponent<LineRenderer>().startColor = availablePathColor;
        currentTile.GetComponent<LineRenderer>().endColor = availablePathColor;
        ChangeTileState(true);
    }

}
