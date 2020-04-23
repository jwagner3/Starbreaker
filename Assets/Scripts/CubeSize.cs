using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSize : MonoBehaviour
{
    public int rand1;
    public int rand2;
    public int rand3;
    // Start is called before the first frame update
    void Start()
    {
        rand1 = Random.Range(0, 10);
        rand2 = Random.Range(0, 10);
        rand3 = Random.Range(0, 10);

        gameObject.transform.localScale = new Vector3(rand1, rand2,rand3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
