using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] public float range;//remaining range between furthest left and furthest right
    //[SerializeField] public int forceMultiplePaths;//-1 -> random multiple paths (up to 2)

    public IEnumerator Generate(GameObject eventPrefab, int remainingDepth, Transform parent, string name)//eventPrefab is already instantiated
    {
        yield return null;
        //eventPrefab.GetComponent<CircleCollider2D>().enabled = false;
        remainingDepth--;
        gameObject.GetComponent<GenerateRandomEvent>().AddConnection(eventPrefab);


//        Debug.Log(eventPrefab.transform.position.y);
        MovePrefab(eventPrefab.transform);
//        Debug.Log(eventPrefab.transform.position.y);

        eventPrefab.name = name;
        eventPrefab.SetActive(true);




        if (remainingDepth > 0)
        {
            
            GameObject obj = Instantiate(eventPrefab, parent);
            //gameObject.GetComponent<GenerateRandomEvent>().AddConnection(obj);

            StartCoroutine(eventPrefab.GetComponent<TileGenerator>().Generate(
            obj, remainingDepth, parent, name + remainingDepth.ToString()
            ));
            this.gameObject.GetComponent<RandomizePosition>().enabled = true;

        }
        else //here is last, before boss tile
        {
            eventPrefab.GetComponent<GenerateRandomEvent>().AddConnection(MapManager.Instance.bossTilePosition);
            MapManager.Instance.AfterGeneration();
        }
    }

    private void MovePrefab(Transform currentPos)
    {
//        Debug.Log(MapManager.distanceY);
        currentPos.position = new Vector3(
            currentPos.position.x,
            currentPos.position.y + MapManager.Instance.distanceY,
            currentPos.position.z);
        //Debug.Log(currentPos.transform.position.y);
    }
}
