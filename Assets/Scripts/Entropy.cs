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
    public ParticleSystem body;
    public ParticleSystem excessParticles;
    public ParticleSystem lightningBeam;

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

        if (active)
        {
            //body.transform.lossyScale += new Vector3(.01f, .01f, .01f);
            //excessParticles.transform.localScale += new Vector3(.01f, .01f, .01f);
            //lightningBeam.transform.localScale += new Vector3(.01f, .01f, .01f);
            
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
