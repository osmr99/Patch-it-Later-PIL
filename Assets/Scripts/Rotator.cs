using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : InteractableObject
{
    bool init = false;

    public new float rotSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            StartMovement();
        }
    }

    public void StartMovement()
    {
        if (transform.parent == null)
            return;

        transform.parent.gameObject.GetComponent<InteractableObject>().SetRotation(rotSpeed);

        init = true;
    }
}
