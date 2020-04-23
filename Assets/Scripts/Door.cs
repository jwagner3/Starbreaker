using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag != "Last Door")
        {
            OpenDoor();
        }
        else
        {
            OpenBossDoor();
        }
        
    }

    public void OpenDoor()
    {
        
        
            if (Vector3.Distance(player.transform.position, transform.position) < 10)
            {
                gameObject.GetComponent<Animator>().enabled = true;
                StartCoroutine("DoorTimer");

            }
        
    }

    public void OpenBossDoor()
    {
        if(player.GetComponent<PlayerMovement>().keyCount == 3 && Vector3.Distance(player.transform.position, transform.position) < 10)
            {
            gameObject.GetComponent<Animator>().enabled = true;
            StartCoroutine("DoorTimer");
        }
    }


    public IEnumerator DoorTimer()
    {
        yield return new WaitForSecondsRealtime(1);
        gameObject.GetComponent<Animator>().enabled = false;
    }
}
