using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colony : MonoBehaviour
{
    [Header("Scripts")]
    public Leafcutters LeafcuttersScript;

    [Header("Stats")]
    public int workerPower;
    public float summonMultiplyer;
    public bool autoSpawn;
    public float progress, timeToSpawn;

    [Header("UI")]
    public Image ProgressBar;

    [Header("Resources")]
    public int leaves;
    public int level, experience, experienceReq;

    [Header("Resources UI")]
    public TMPro.TextMeshProUGUI LeavesCountText;
    public Image ExperienceBar;

    [Header("Dig")]
    public int diggingProgress;
    public int[] diggingRequirement;
    public int roomsDug;

    [Header("Dig UI")]
    public GameObject[] UnlockObject;
    public TMPro.TextMeshProUGUI[] UnlockProgressText;

    [Header("Rooms")]
    public GameObject[] RoomObject;

    void Start()
    {
        level = 1;
        experienceReq = NextLevelExpReq();
    }

    void Update()
    {
        if (autoSpawn)
            Progress(Time.deltaTime * summonMultiplyer);
    }

    public void Progress(float value)
    {
        progress += value;
        while (progress >= timeToSpawn)
        {
            progress -= timeToSpawn;
            Dig(workerPower);
        }
        ProgressBar.fillAmount = progress / timeToSpawn;
    }

    public void QueenClicked()
    {
        Dig(workerPower);
    }

    void Dig(int amount)
    {
        diggingProgress += amount;
        if (diggingProgress >= diggingRequirement[roomsDug])
            RoomDug();
        UnlockProgressText[roomsDug].text = diggingProgress.ToString() + "/" + diggingRequirement[roomsDug].ToString();
    }

    void RoomDug()
    {
        UnlockObject[roomsDug].SetActive(false);
        RoomObject[roomsDug].SetActive(true);
        switch (roomsDug)
        {
            case 0:
                LeafcuttersScript.built = true;
                break;
        }
        diggingProgress -= diggingRequirement[roomsDug];
        roomsDug++;
        UnlockObject[roomsDug].SetActive(true);
    }

    public void GainLeaves(int amount)
    {
        leaves += amount;
        GainExperience(amount * 2);
        LeavesCountText.text = leaves.ToString();
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        if (experience >= experienceReq)
            LevelUp();
        ExperienceBar.fillAmount = (experience * 1f) / (experienceReq * 1f);
    }

    void LevelUp()
    {
        experience -= experienceReq;
        level++;
        experienceReq = NextLevelExpReq();
    }

    // Checks
    int NextLevelExpReq()
    {
        return 20 + level * 20 + level * (level + 1) * 5;
    }
}
