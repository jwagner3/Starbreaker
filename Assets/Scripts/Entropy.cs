using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entropy : MonoBehaviour
{

    public GameObject player;
    public float moveSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(player.transform.position);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
