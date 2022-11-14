using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizePosition : MonoBehaviour
{
    [SerializeField] float rangeX = 0.25f;
    [SerializeField] float rangeY = 0.25f;



    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(
            gameObject.transform.position.x + RandomizeX(),
            gameObject.transform.position.y + RandomizeY(),
            gameObject.transform.position.z

            );
    }
    private float RandomizeX()
    {
        return UnityEngine.Random.Range(-rangeX, rangeX);
    }
    private float RandomizeY()
    {
        return UnityEngine.Random.Range(-rangeY, rangeY);
    }

}
