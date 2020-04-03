using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldandThrow : MonoBehaviour
{
    public float Throwspeed = 10;
    public bool canHold = true;
    public GameObject item;
    public Transform guide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
           
            if (Vector3.Distance(gameObject.transform.position,item.transform.position) < 10 && canHold && Input.GetMouseButton(0))
            {
                Pickup();
            }
        }else if (Input.GetMouseButtonDown(1))
        {
            if (!canHold)
            {
                Throw();
            }
        }

        if (!canHold && item)
        {
            item.transform.position = guide.position;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 20))
            {
            Debug.Log("hit");
            if (hit.rigidbody.gameObject.tag == "Item")
            {
                if (!item && canHold)
                {
                    item = hit.rigidbody.gameObject;
                }
            }
            if (hit.rigidbody.gameObject.tag == "Item")
            {
                if (canHold&&Input.GetMouseButtonDown(0))
                {
                    item = null;
                }
            }

        }
        
    }

    

    void Pickup()
    {
        if (!item)
        {
            return;
        }

        item.transform.SetParent(guide);

        item.GetComponent<Rigidbody>().useGravity = false;

        item.transform.localRotation = transform.rotation;

        item.transform.position = guide.position;

        canHold = false;
    }


    void Throw()
    {
        if (!item)
        {
            return;
        }

        item.GetComponent<Rigidbody>().useGravity = true;

        item = null;

        guide.GetChild(0).gameObject.GetComponent<Rigidbody>().velocity = transform.forward * Throwspeed;

        guide.GetChild(0).parent = null;
        canHold = true;
    }
}
