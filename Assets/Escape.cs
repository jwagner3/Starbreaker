using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escape : MonoBehaviour
{
    public GameObject Entropy;
    public GameObject Player;
    public bool entropyActive;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        entropyActive = Entropy.GetComponent<Entropy>().active;
    }


    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player" && entropyActive == true)
        {
            SceneManager.LoadScene("Victory");
        }
    }
}
