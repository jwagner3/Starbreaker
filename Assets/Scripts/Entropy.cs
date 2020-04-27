using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Entropy : MonoBehaviour
{

    public GameObject player;
    public float moveSpeed = 5;
    public GameObject[] lights;
    public bool active = false;
    public float playerHP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerHP = player.GetComponent<PlayerMovement>().hp;
        if (playerHP <= 0)
        {
            PlayerDeadRelocate();
        }
        lights = GameObject.FindGameObjectsWithTag("Light");
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.transform.LookAt(player.transform.position);

        if (Vector3.Distance(player.transform.position, transform.position) < 100)
        {
            active = true;
           
        }

        if (active && playerHP > 0)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        for (int i = 0; i < lights.Length; i++)
        {
            float lightDistance = Vector3.Distance(transform.position, lights[i].transform.position);

            if (lightDistance < 15)
            {
                lights[i].gameObject.GetComponent<Light>().intensity -= 5;
            }


        }
        
    }

   

    private void PlayerDeadRelocate()
    {
        gameObject.transform.position = new Vector3(23.14f, 37.38f, 1998.76f);
    }
}
