using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public GameObject entropy;
    public AudioSource musicPlayer;
    public AudioClip scaryMusic;
    public AudioClip calmMusic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(entropy.GetComponent<Entropy>().active == true && musicPlayer.clip != scaryMusic)
        {
            musicPlayer.clip = scaryMusic;
            musicPlayer.Play();
        }
        else
        {
            
        }
        
    } 
}
