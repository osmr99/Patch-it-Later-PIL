using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPlacer : MonoBehaviour, IObserver
{
    [SerializeField]
    ObjectManager manager;

    public void Notify(Object o)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        //manager.AddObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;

        var mousePos = Mouse.current.position.ReadValue();
        var changed = Camera.main.ScreenToWorldPoint(mousePos);
        changed.z = 0;
        transform.position = changed;
    }
}
