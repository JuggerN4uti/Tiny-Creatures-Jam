using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leafcutters : MonoBehaviour
{
    [Header("Scripts")]
    public Colony ColonyScript;

    [Header("Stats")]
    public bool built;
    public float progress, timeToSpawn;
    public int LeavesCollected;

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
        }
        ProgressBar.fillAmount = progress / timeToSpawn;
    }

    void Spawn()
    {
        ColonyScript.GainLeaves(LeavesCollected);
    }
}
