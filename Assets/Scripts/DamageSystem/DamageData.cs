using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageData", menuName = "DamageVariable/DamageData", order = 1)]
public class DamageData : ScriptableObject
{
    [Header("Damage Settings")]
    public float damageAmount; // The amount of damage this object deals
    public string damageType;  // note "leave Blank", we can possibly implement Physical, Fire, stuff for damage categorization
}
