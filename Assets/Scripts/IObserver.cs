using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    // Start is called before the first frame update
    public void Notify(Object o);
}
