using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public GameObject cube;
    public int cubeNumber = 25;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cubeNumber; i++)
        {
            Instantiate(cube, gameObject.transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
