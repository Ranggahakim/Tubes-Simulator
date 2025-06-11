using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour
{
    public UnityEvent ExecuteWhenStart;
    // Start is called before the first frame update
    void Start()
    {
        ExecuteWhenStart.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
