using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCode : MonoBehaviour
{
    public string str_name;
    public int int_hp;
    public int int_maxhp;
    public int int_atkDmg;
    public Animator myAnimator;

    [Space]
    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI charName;

    [Space]
    public GameObject kertasImg;
    public GameObject guntingImg;
    public GameObject batuImg;


    [Space]
    public Character[] characters;

    [System.Serializable]
    public class Character
    {
        public CharacterScriptable cs;
        public GameObject go;
        public Animator animator;
    }

    public ActorTemporaryDataScriptable atds;

    public void StartGame()
    {
        foreach (Character c in characters)
        {
            if (c.cs.string_nama == atds.string_nama)
            {
                str_name = c.cs.string_nama;
                int_hp = c.cs.int_hp;
                int_maxhp = c.cs.int_hp;
                int_atkDmg = c.cs.int_atkDmg;

                c.go.SetActive(true);
                myAnimator = c.animator;
            }
            else
            {
                c.go.SetActive(false);
            }
        }
        charName.text = str_name;
    }

    public void Attack(CharacterCode target)
    {
        Debug.Log($"{gameObject.name} nyerang {target.gameObject.name}");
        target.GetDamage(int_atkDmg);
    }

    public void GetDamage(int damage)
    {
        // Debug.Log($"animasi dimulai");
        StartCoroutine(waitAnimation(damage));
    }

    IEnumerator waitAnimation(int damage)
    {
        yield return new WaitForSeconds(.5f);
        int_hp -= damage;

        float healthValue = (float)int_hp / (float)int_maxhp;

        healthSlider.value = healthValue;
        Debug.Log($"health Slider {gameObject.name}: {healthValue}");
        Debug.Log($"{str_name} Kena pukul, damage : {damage}, sisa hp : {int_hp}");
    }

    public void SetImageActive(KGBEnum kGBEnum)
    {
        kertasImg.SetActive(false);
        guntingImg.SetActive(false);
        batuImg.SetActive(false);

        switch (kGBEnum)
        {
            case KGBEnum.kertas:
                kertasImg.SetActive(true);
                break;
            case KGBEnum.gunting:
                guntingImg.SetActive(true);
                break;
            case KGBEnum.batu:
                batuImg.SetActive(true);
                break;
        }
    }
}
