using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullets : MonoBehaviour
{
    [Header("Scripts")]
    public Colony ColonyScript;

    [Header("Stats")]
    public bool built;
    public int bonusFloors;
    public float progress, timeToSpawn;
    public int bonus;
    public int[] bulletDamage;
    int roll;

    [Header("UI")]
    public Image ProgressBar;

    void Update()
    {
        if (built)
            Progress(Time.deltaTime * ColonyScript.summonMultiplyer);
    }

    public void Progress(float value)
    {
        value *= 1f + ColonyScript.Perk[4] * 0.12f;
        progress += value;
        while (progress >= timeToSpawn)
        {
            progress -= timeToSpawn;
            Spawn(1 + bonusFloors);
        }
        ProgressBar.fillAmount = progress / timeToSpawn;
    }

    public void Spawn(int amount, bool fiedAmount = false)
    {
        if (!fiedAmount)
            amount += ColonyScript.bonus;

        roll = Random.Range(bulletDamage[0], bulletDamage[1] + 1);

        roll += ColonyScript.Perk[1];

        roll += (ColonyScript.level * ColonyScript.Perk[9]) / 2;

        roll += bonus;

        roll *= amount;

        if (ColonyScript.Perk[4] > 0)
            roll += Mathf.FloorToInt(roll * 0.12f * ColonyScript.Perk[4]);

        ColonyScript.TakeDamage(roll);
    }
}
