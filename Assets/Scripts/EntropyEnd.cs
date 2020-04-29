using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntropyEnd : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5;
    public Material darkness;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LightsOut");
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.transform.LookAt(player.transform.position);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

    }


    private  IEnumerator LightsOut()
    {
        yield return new WaitForSecondsRealtime(10);
        RenderSettings.skybox = darkness;
    }
}
