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

    [Header("Resources")]
    public int ants;
    public int leaves, meat, level, experience, experienceReq;

    [Header("Resources UI")]
    public TMPro.TextMeshProUGUI AntsCountText;
    public TMPro.TextMeshProUGUI LeavesCountText, MeatCountText;
    public Image ExperienceBar;
    public TMPro.TextMeshProUGUI LevelText, ExperienceText;

    [Header("Dig")]
    public int[] diggingProgress;
    public int[] diggingRequirement;

    [Header("Dig UI")]
    public GameObject[] UnlockObject;
    public TMPro.TextMeshProUGUI[] UnlockProgressText;

    [Header("Rooms")]
    public GameObject[] RoomObject;

    [Header("Mobile")]
    public int encounter;
    public int MaxHealth, HitPoints;
    public Image HealthBar;
    public TMPro.TextMeshProUGUI EncounterText, HealthText;

    [Header("Skill Tree")]
    public int skillPoints;
    public TMPro.TextMeshProUGUI SPText;
    public GameObject SkillTreeObject;
    public bool[] Perk;
    int bonusClick, multiclassAnt;

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
            SpawnAnt(workerPower);
        }
        ProgressBar.fillAmount = progress / timeToSpawn;
    }

    public void SelectRoom(int room)
    {
        if (room == 0 && ants > 0)
            WorkerCombat();
        else if (ants > 0)
            Dig(room);
    }

    public void QueenClicked()
    {
        if (Perk[6])
        {
            bonusClick++;
            if (bonusClick >= 4)
            {
                bonusClick -= 4;
                QueenClicked();
            }
        }
        SpawnAnt(workerPower);
        if (Perk[3])
        {
            if (autoSpawn)
                Progress(0.15f * summonMultiplyer);
            if (LeafcuttersScript.built)
                LeafcuttersScript.Progress(0.15f * summonMultiplyer);
            if (BulletsScript.built)
                BulletsScript.Progress(0.15f * summonMultiplyer);
        }
    }

    public void SpawnAnt(int amount)
    {
        ants += amount;
        AntsCountText.text = ants.ToString();
        if (Perk[10])
        {
            multiclassAnt += amount;
            while (amount >= 30)
            {
                if (LeafcuttersScript.built)
                    LeafcuttersScript.Spawn();
                if (BulletsScript.built)
                    BulletsScript.Spawn();
            }
        }
    }

    void WorkerCombat()
    {
        if (Perk[4])
            roll = Random.Range((workerDamage[0] + 1) * ants, (workerDamage[1] + 1) * ants + 1);
        else roll = Random.Range(workerDamage[0] * ants, workerDamage[1] * ants + 1);

        TakeDamage(roll);

        if (Perk[1])
        {
            temp = Random.Range(0f, 0.2f);
            temp *= ants * 1f;
            ants = Mathf.FloorToInt(temp);
        }
        else ants = 0;
        AntsCountText.text = ants.ToString();
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
        HealthText.text = HitPoints.ToString() + "/" + MaxHealth.ToString();
    }

    void Dig(int room)
    {
        if (ants + diggingProgress[room - 1] >= diggingRequirement[room - 1])
        {
            ants -= (diggingRequirement[room - 1] - diggingProgress[room - 1]);
            AntsCountText.text = ants.ToString();
            RoomDug(room - 1);
        }
        else
        {
            diggingProgress[room - 1] += ants;
            ants = 0;
            AntsCountText.text = ants.ToString();
            UnlockProgressText[room - 1].text = diggingProgress[room - 1].ToString() + "/" + diggingRequirement[room - 1].ToString();
        }
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
    }

    public void GainLeaves(int amount)
    {
        leaves += amount;
        if (Perk[2])
            GainExperience(amount * 3);
        else GainExperience(amount * 2);
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
        ExperienceText.text = experience.ToString() + "/" + experienceReq.ToString();
    }

    void LevelUp()
    {
        experience -= experienceReq;
        level++;
        LevelText.text = level.ToString();
        experienceReq = NextLevelExpReq();
        GainExperience(0);
        skillPoints++;
        if (level % 5 == 0)
            skillPoints++;
        SPText.text = skillPoints.ToString();
        if (Perk[8])
            summonMultiplyer += 0.02f;
    }

    // Checks
    int NextLevelExpReq()
    {
        return 20 + level * 20 + level * (level + 1) * 5;
    }

    void SetEncounter()
    {
        temp = (50f + encounter * 2.1f) * (1f + encounter * 0.0155f);
        MaxHealth = Mathf.FloorToInt(temp);
        HitPoints += MaxHealth;
        if (HitPoints < 0)
            TakeDamage(1);
        EncounterText.text = encounter.ToString();
        HealthBar.fillAmount = 1f;
        HealthText.text = HitPoints.ToString() + "/" + MaxHealth.ToString();
    }

    public void SkillTree()
    {
        SkillTreeObject.SetActive(!SkillTreeObject.activeSelf);
    }

    public void BuyPerk(int which)
    {
        Perk[which] = true;
        switch (which)
        {
            case 0:
                summonMultiplyer += 0.2f;
                break;
            case 6:
                bonusClick++;
                break;
            case 8:
                summonMultiplyer += 0.02f * level;
                break;
            case 11:
                Invoke("AutoClick", 0.8f);
                break;
        }
    }

    void AutoClick()
    {
        QueenClicked();
        Invoke("AutoClick", 0.8f);
    }
}
