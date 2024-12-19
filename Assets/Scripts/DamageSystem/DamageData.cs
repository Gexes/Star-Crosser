using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageData", menuName = "DamageVariable/DamageData", order = 1)]
public class DamageData : ScriptableObject
{
    [Header("Damage Settings")]
    public float damageAmount; // The amount of damage this object deals
    public string damageType = "";  // "Default leave Blank", we can possibly implement Physical, Fire, stuff for damage categorization

    [Header("Fire")]
    public float fireTickDamage = 0f; // Amount of damage dealt per fire tick
    public float fireDuration = 0f;    // Duration for fire damage
    public float fireInterval = 1f;    // Interval between fire damage ticks











}
