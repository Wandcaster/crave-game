using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] public GameObject startPosition;
    [SerializeField] public GameObject bossTilePosition;
    [SerializeField] public GameObject eventPrefab;
    [SerializeField] public static GameObject currentTile;
    [SerializeField] public int depthLv;//number of tiles starting from start till the boss, excluding start and boss
    [SerializeField] public int seperatePaths = 2;
    [SerializeField] public float range = 9;


    public static float distanceY;//wysokosc wzwyz dla kazdego tile'a

    // Start is called before the first frame update
    void Start()
    {
        distanceY = bossTilePosition.transform.position.y - startPosition.transform.position.y;//radius for each tile is 0.5; distance for all tiles
//        distanceY -= (depthLv*0.5f);
        distanceY = (distanceY) / (depthLv+1);//keep in mind that if radius is increased then some may overlap; distance between 2 tiles
//        Debug.Log(distanceY);
        currentTile = startPosition;
        GenerateMap();
    }

    private void GenerateMap()
    {
        eventPrefab.transform.position = startPosition.transform.position;
        float leftLimit = this.transform.position.x-range/2;
        leftLimit += (range/seperatePaths)/2;//mid point of each block

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


}
