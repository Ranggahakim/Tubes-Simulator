using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorTemporaryDataScriptable", menuName = "Scriptable Objects/ActorTemporaryDataScriptable")]
public class ActorTemporaryDataScriptable : ScriptableObject
{
    public string string_nama;
    public int int_hp;
    public int int_atkDmg;

    public void SetNewData(CharacterScriptable character)
    {
        string_nama = character.string_nama;
        int_hp = character.int_hp;
        int_atkDmg = character.int_atkDmg;
    }
}
