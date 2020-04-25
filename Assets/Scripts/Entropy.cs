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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lights = GameObject.FindGameObjectsWithTag("Light");
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.transform.LookAt(player.transform.position);

        if (Vector3.Distance(player.transform.position, transform.position) < 100)
        {
            active = true;
           
        }

        if (active)
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            SceneManager.LoadScene("Defeat");
        }
    }
}
