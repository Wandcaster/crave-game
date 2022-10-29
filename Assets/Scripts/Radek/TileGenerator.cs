using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] public float range;//range between furthest left and furthest right
    [SerializeField] public int forceMultiplePaths;//-1 -> random multiple paths (up to 2)

    public IEnumerator Generate(GameObject eventPrefab, int remainingDepth, Transform parent, string name)//eventPrefab is already instantiated
    {
        yield return null;
        remainingDepth--;

//        Debug.Log(eventPrefab.transform.position.y);
        MovePrefab(eventPrefab.transform);
//        Debug.Log(eventPrefab.transform.position.y);

        eventPrefab.name = name;
        eventPrefab.SetActive(true);




        if (remainingDepth > 0) StartCoroutine(
            eventPrefab.GetComponent<TileGenerator>().Generate(
                Instantiate(eventPrefab, parent), remainingDepth, parent, name + remainingDepth.ToString()
                ));
    }

    private void MovePrefab(Transform currentPos)
    {
        Debug.Log(MapManager.distanceY);
        currentPos.position = new Vector3(
            currentPos.position.x,
            currentPos.position.y + MapManager.distanceY,
            currentPos.position.z);
        //Debug.Log(currentPos.transform.position.y);
    }
}
