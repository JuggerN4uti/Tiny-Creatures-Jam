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
    public bool secondFloor;
    public float progress, timeToSpawn;
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
        progress += value;
        while (progress >= timeToSpawn)
        {
            progress -= timeToSpawn;
            Spawn();
            if (secondFloor)
                Invoke("Spawn", 0.25f);
        }
        ProgressBar.fillAmount = progress / timeToSpawn;
    }

    public void Spawn()
    {
        roll = Random.Range(bulletDamage[0], bulletDamage[1] + 1);

        if (ColonyScript.Perk[4])
            roll++;
        if (ColonyScript.Perk[8])
            roll += ColonyScript.level / 2;

        ColonyScript.TakeDamage(roll);
    }
}
