using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colony : MonoBehaviour
{
    [Header("Scripts")]
    public Leafcutters LeafcuttersScript;

    [Header("Stats")]
    public float summonMultiplyer;

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

    public void QueenClicked()
    {
        Dig(1);
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
        ExperienceBar.fillAmount = (experience * 1f) / (experienceReq * 1f);
    }

    // Checks
    int NextLevelExpReq()
    {
        return level * 40 + level * (level + 1) * 5;
    }
}
