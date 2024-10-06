using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colony : MonoBehaviour
{
    [Header("Scripts")]
    public Leafcutters LeafcuttersScript;
    public Bullets BulletsScript;

    [Header("Stats")]
    public int workerPower;
    public int[] workerDamage;
    public float summonMultiplyer;
    public bool autoSpawn;
    public float progress, timeToSpawn;
    int roll;
    float temp;

    [Header("UI")]
    public GameObject LeafcutterUpgrade;
    public GameObject BulletUpgrade;
    public Image ProgressBar;
    public Image HealthBar;

    [Header("Resources")]
    public int leaves;
    public int meat, level, experience, experienceReq;

    [Header("Resources UI")]
    public TMPro.TextMeshProUGUI LeavesCountText;
    public TMPro.TextMeshProUGUI MeatCountText;
    public Image ExperienceBar;

    [Header("Dig")]
    public int[] diggingProgress;
    public int[] diggingRequirement;
    public int roomSelected;

    [Header("Dig UI")]
    public GameObject[] UnlockObject;
    public TMPro.TextMeshProUGUI[] UnlockProgressText;

    [Header("Rooms")]
    public GameObject[] RoomObject;

    [Header("Mobile")]
    public int encounter;
    public int MaxHealth, HitPoints;

    void Start()
    {
        level = 1;
        experienceReq = NextLevelExpReq();
        encounter = 1;
        SetEncounter();
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
            SpawnAnt();
        }
        ProgressBar.fillAmount = progress / timeToSpawn;
    }

    public void SelectRoom(int room)
    {
        roomSelected = room;
    }

    public void QueenClicked()
    {
        SpawnAnt();
    }

    void SpawnAnt()
    {
        if (roomSelected == 0)
            WorkerCombat();
        else Dig(workerPower);
    }

    void WorkerCombat()
    {
        roll = Random.Range(workerDamage[0], workerDamage[1] + 1);
        roll *= workerPower;

        TakeDamage(roll);
    }

    public void TakeDamage(int amount)
    {
        HitPoints -= amount;
        if (HitPoints <= 0)
        {
            GainMeat(6 + encounter / 3);
            GainExperience(21 + encounter);
            encounter++;
            SetEncounter();
        }
        HealthBar.fillAmount = (HitPoints * 1f) / (MaxHealth * 1f);
    }

    void Dig(int amount)
    {
        diggingProgress[roomSelected - 1] += amount;
        UnlockProgressText[roomSelected - 1].text = diggingProgress[roomSelected - 1].ToString() + "/" + diggingRequirement[roomSelected - 1].ToString();
        if (diggingProgress[roomSelected - 1] >= diggingRequirement[roomSelected - 1])
            RoomDug(roomSelected - 1);
    }

    void RoomDug(int which)
    {
        UnlockObject[which].SetActive(false);
        switch (which)
        {
            case 0:
                RoomObject[which].SetActive(true);
                LeafcuttersScript.built = true;
                LeafcutterUpgrade.SetActive(true);
                break;
            case 1:
                RoomObject[which].SetActive(true);
                BulletsScript.built = true;
                BulletUpgrade.SetActive(true);
                break;
            case 2:
                LeafcuttersScript.secondFloor = true;
                break;
            case 3:
                BulletsScript.secondFloor = true;
                break;
        }
        SelectRoom(0);
    }

    public void GainLeaves(int amount)
    {
        leaves += amount;
        GainExperience(amount * 2);
        LeavesCountText.text = leaves.ToString();
    }

    public void SpendLeaves(int amount)
    {
        leaves -= amount;
        LeavesCountText.text = leaves.ToString();
    }

    void GainMeat(int amount)
    {
        meat += amount;
        MeatCountText.text = meat.ToString();
    }

    public void SpendMeat(int amount)
    {
        meat -= amount;
        MeatCountText.text = meat.ToString();
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

    void SetEncounter()
    {
        temp = (50f + encounter * 2.2f) * (1f + encounter * 0.016f);
        MaxHealth = Mathf.FloorToInt(temp);
        HitPoints = MaxHealth;
        HealthBar.fillAmount = 1f;
    }
}
