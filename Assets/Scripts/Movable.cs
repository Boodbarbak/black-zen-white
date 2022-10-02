using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public GameObject startPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            BackToStart();
        }
    }

    public void BackToStart()
    {
        transform.position = startPoint.transform.position;
    }
}
