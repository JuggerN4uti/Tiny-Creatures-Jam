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
    public bool secondFloor;
    public float progress, timeToSpawn;
    public int[] LeavesCollected;
    int roll;

    [Header("UI")]
    public Image ProgressBar;
    public GameObject CollectedPrefab;
    public Transform Origin;
    public Rigidbody2D Body;
    Display Displayed;

    void Update()
    {
        if (built)
            Progress(Time.deltaTime * ColonyScript.summonMultiplyer);
    }

    public void Progress(float value)
    {
        if (ColonyScript.Perk[7])
            value *= 1.25f;
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
        roll = Random.Range(LeavesCollected[0], LeavesCollected[1] + 1);

        if (ColonyScript.Perk[5])
            roll += 2;
        
        ColonyScript.GainLeaves(roll);

        if (ColonyScript.Perk[7])
            ColonyScript.SpawnAnt(1);

        Origin.rotation = Quaternion.Euler(Origin.rotation.x, Origin.rotation.y, Body.rotation + Random.Range(-5f, 5f));
        GameObject display = Instantiate(CollectedPrefab, Origin.position, transform.rotation);
        Displayed = display.GetComponent(typeof(Display)) as Display;
        Displayed.DisplayThis(roll);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin.up * Random.Range(1.2f, 1.6f), ForceMode2D.Impulse);
    }
}
