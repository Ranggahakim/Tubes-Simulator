using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnbaseSystem : MonoBehaviour
{
    [Header("Karakter")]
    public CharacterCode player;
    public CharacterCode enemy;

    [Header("Fight")]
    public KGBEnum playerKGB;
    public KGBEnum enemyKGB;

    [Header("Events brow")]

    public UnityEvent ExecuteWhenStartRound;
    public UnityEvent ExecuteWhenEndRound;

    public UnityEvent ExecuteWhenWin;
    public UnityEvent ExecuteWhenLose;

    [Space]

    public float float_countdownTime = 10;
    public bool bool_isTimeToFChoose;

    void Awake()
    {
        player.StartGame();
        enemy.StartGame();
    }

    void Start()
    {
        player.tbs = this;
        enemy.tbs = this;
        StartRound();
    }

    void Update()
    {
        if (bool_isTimeToFChoose)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerKGB = KGBEnum.kertas;
                player.SetImageActive(playerKGB);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerKGB = KGBEnum.gunting;
                player.SetImageActive(playerKGB);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                playerKGB = KGBEnum.batu;
                player.SetImageActive(playerKGB);
            }

        }
    }

    public void StartRound()
    {
        // Debug.Log("Start");
        player.SetImageActive(playerKGB);
        enemy.SetImageActive(enemyKGB);

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
        if (playerKGB == KGBEnum.none)
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
            player.SetImageActive(playerKGB);

        }

        int rndEnemy = Random.Range(0, 3);

        switch (rndEnemy)
        {
            case 0:
                enemyKGB = KGBEnum.kertas;
                enemy.SetImageActive(enemyKGB);
                break;
            case 1:
                enemyKGB = KGBEnum.gunting;
                enemy.SetImageActive(enemyKGB);
                break;
            case 2:
                enemyKGB = KGBEnum.batu;
                enemy.SetImageActive(enemyKGB);
                break;
        }
        ExecuteWhenEndRound.Invoke();
        switch (DecideWinner())
        {
            case HitterCondition.neutral:
                Debug.Log("Netral");
                StartCoroutine(BetweenRound());
                break;
            case HitterCondition.player:
                player.Attack(enemy);
                // Debug.Log("Player");
                break;
            case HitterCondition.enemy:
                enemy.Attack(player);
                // Debug.Log("Enemy");
                break;
        }

    }

    public void AdaYangKalah(CharacterCode character, bool isIt)
    {
        //character adalah yang kalah
        // isIt adalah apakah ada yang kalah atw tidak... kalo true artinya ada yang kalah

        if (!isIt)
        {
            Debug.Log("Masih ada yang belum kalah");
            StartCoroutine(BetweenRound());

        }
        else
        {
            if (character == player)
            {
                ExecuteWhenLose.Invoke();
            }
            else if (character == enemy)
            {
                ExecuteWhenWin.Invoke();
            }
        }
    }

    public HitterCondition DecideWinner()
    {
        if (playerKGB != KGBEnum.none && enemyKGB != KGBEnum.none)
        {
            // Kalo player milih kertas
            if (playerKGB == KGBEnum.kertas)
            {
                switch (enemyKGB)
                {
                    case KGBEnum.kertas:
                        return HitterCondition.neutral;
                    case KGBEnum.gunting:
                        return HitterCondition.enemy;
                    case KGBEnum.batu:
                        return HitterCondition.player;
                }
            }
            // Kalo player milih gunting
            else if (playerKGB == KGBEnum.gunting)
            {
                switch (enemyKGB)
                {
                    case KGBEnum.kertas:
                        return HitterCondition.player;
                    case KGBEnum.gunting:
                        return HitterCondition.neutral;
                    case KGBEnum.batu:
                        return HitterCondition.enemy;
                }
            }
            // Kalo player milih batu
            else if (playerKGB == KGBEnum.batu)
            {
                switch (enemyKGB)
                {
                    case KGBEnum.kertas:
                        return HitterCondition.enemy;
                    case KGBEnum.gunting:
                        return HitterCondition.player;
                    case KGBEnum.batu:
                        return HitterCondition.neutral;
                }
            }
            else
            {
                return HitterCondition.neutral;
            }
        }
        else
        {
            return HitterCondition.neutral;
        }
        return HitterCondition.neutral;
    }

    public IEnumerator BetweenRound()
    {
        // Debug.Log("Ini adalah jeda");

        yield return new WaitForSeconds(1);

        // Debug.Log("Ini diantara jeda");
        playerKGB = KGBEnum.none;
        enemyKGB = KGBEnum.none;

        player.SetImageActive(playerKGB);
        enemy.SetImageActive(enemyKGB);

        yield return new WaitForSeconds(3);

        // Debug.Log("Jeda beres :DDD");

        StartRound();
    }
}

public enum HitterCondition
{
    neutral,
    player,
    enemy,
}