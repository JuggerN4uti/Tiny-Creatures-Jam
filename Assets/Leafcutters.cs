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
        roll = Random.Range(LeavesCollected[0], LeavesCollected[1] + 1);
        ColonyScript.GainLeaves(roll);

        Origin.rotation = Quaternion.Euler(Origin.rotation.x, Origin.rotation.y, Body.rotation + Random.Range(-5f, 5f));
        GameObject display = Instantiate(CollectedPrefab, Origin.position, transform.rotation);
        Displayed = display.GetComponent(typeof(Display)) as Display;
        Displayed.DisplayThis(roll);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin.up * Random.Range(1.2f, 1.6f), ForceMode2D.Impulse);
    }
}
