using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnbaseSystem : MonoBehaviour
{
    [Header("Karakter")]
    public KGBEnum playerKGB;

    [Header("Events brow")]

    public UnityEvent ExecuteWhenStartRound;
    public UnityEvent ExecuteWhenEndRound;

    [Space]
    
    public float float_countdownTime = 10;
    public bool bool_isTimeToFChoose;

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if(bool_isTimeToFChoose)
        {
            if(Input.GetKeyDown(KeyCode.W))
        {
            playerKGB = KGBEnum.kertas;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            playerKGB = KGBEnum.gunting;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            playerKGB = KGBEnum.batu;
        }
        }
    }

    public void StartRound()
    {
        Debug.Log("Start");
        ExecuteWhenStartRound.Invoke();
        StartCoroutine(CountdownChoose());
    }

    IEnumerator CountdownChoose()
    {
        bool_isTimeToFChoose = true;

        yield return new WaitForSeconds(float_countdownTime);

        bool_isTimeToFChoose = false;
        FinishRound();
    }

    public void FinishRound()
    {
        if(playerKGB == KGBEnum.none)
        {
            int rnd = Random.Range(0, 3);

            switch (rnd)
            {
                case 0: 
            playerKGB = KGBEnum.kertas;
                break;
                case 1:
            playerKGB = KGBEnum.gunting;
                break;
                case 2:
            playerKGB = KGBEnum.batu;
                break;
            }
        }
        Debug.Log(playerKGB);

        Debug.Log("=========================");
            playerKGB = KGBEnum.none;

            ExecuteWhenEndRound.Invoke();

            StartCoroutine(BetweenRound());
    }

    IEnumerator BetweenRound()
    {
        Debug.Log("Ini adalah jeda");
        
        yield return new WaitForSeconds(1);
        
        Debug.Log("Ini diantara jeda");

        yield return new WaitForSeconds(3);
        
        Debug.Log("Jeda beres :DDD");

        StartRound();
    }
}
