using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMove : MonoBehaviour
{


    int randomNum1;
    int randomNum2;
    int randomNum3;
    // Start is called before the first frame update
    void Start()
    {
        randomNum1 = Random.Range(-20, 20);
        randomNum2 = Random.Range(-20, 20);
        randomNum3 = Random.Range(-20, 20);
        gameObject.GetComponent<Rigidbody>().AddForce(randomNum1*10, randomNum2*10, randomNum3*10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
