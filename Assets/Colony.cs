using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public int bonus;
    int roll;
    float temp;

    [Header("UI")]
    public GameObject LeafcutterUpgrade;
    public GameObject BulletUpgrade, ThirdUpgrades, BulletLair, ResearchLair;
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
    public int[] BonusLeavesFloorsReq, BonusBulletFloorsReq;
    public GameObject[] RoomSprteObject;

    [Header("Animations")]
    public bool animationOff;
    public GameObject[] AnimationObject;
    public Animator[] Animators;

    [Header("Mobile")]
    public int encounter;
    public int MaxHealth, HitPoints;
    public Image HealthBar, MobImage;
    public Sprite[] MobSprite;
    public TMPro.TextMeshProUGUI EncounterText, HealthText;
    public GameObject DealtPrefab;
    public Transform Origin;
    public Rigidbody2D Body;
    Display Displayed;

    [Header("Skill Tree")]
    public int skillPoints;
    public int pointsSpent;
    public TMPro.TextMeshProUGUI SPText;
    public GameObject SkillTreeObject;
    public Button[] PerkButton;
    public GameObject[] LockImage;
    public TMPro.TextMeshProUGUI[] PerksText;
    public bool[] aviableToBuy;
    public int[] Perk, perkCost, perkMax;
    int bonusClick, multiclassAnt;

    void Start()
    {
        level = 1;
        experienceReq = NextLevelExpReq();
        ExperienceBar.fillAmount = (experience * 1f) / (experienceReq * 1f);
        ExperienceText.text = experience.ToString() + "/" + experienceReq.ToString();
        encounter = 1;
        SetEncounter();
        CheckAviablePerks();
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
        if (Perk[6] > 0)
        {
            bonusClick++;
            if (bonusClick >= 7)
            {
                bonusClick -= 7;
                QueenClicked();
                QueenClicked();
            }
        }
        SpawnAnt(workerPower + Perk[3]);
        if (Perk[3] > 0)
        {
            if (autoSpawn)
                Progress(0.04f * Perk[3] * summonMultiplyer);
            if (LeafcuttersScript.built)
                LeafcuttersScript.Progress(0.04f * Perk[3] * summonMultiplyer);
            if (BulletsScript.built)
                BulletsScript.Progress(0.04f * Perk[3] * summonMultiplyer);
        }
    }

    public void SpawnAnt(int amount, bool fixedAmount = false)
    {
        if (!fixedAmount)
        {
            amount += Perk[7] * 2;
            amount += bonus;
        }
        ants += amount;
        AntsCountText.text = ants.ToString();
        if (Perk[7] > 0)
        {
            multiclassAnt += amount;
            while (multiclassAnt >= 75)
            {
                multiclassAnt -= 75;
                if (LeafcuttersScript.built)
                    LeafcuttersScript.Spawn(Perk[7] * 2, true);
                if (BulletsScript.built)
                    BulletsScript.Spawn(Perk[7] * 2, true);
            }
        }
    }

    void WorkerCombat()
    {
        roll = Random.Range((workerDamage[0] + Perk[1]) * ants, (workerDamage[1] + Perk[1]) * ants + 1);

        TakeDamage(roll);

        ants = 0;
        AntsCountText.text = ants.ToString();
    }

    public void TakeDamage(int amount)
    {
        HitPoints -= amount;

        if (amount != 1)
        {
            Origin.rotation = Quaternion.Euler(Origin.rotation.x, Origin.rotation.y, Body.rotation + Random.Range(-15f, 15f));
            GameObject display = Instantiate(DealtPrefab, Origin.position, transform.rotation);
            Displayed = display.GetComponent(typeof(Display)) as Display;
            Displayed.DisplayThis(amount);
            Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
            display_body.AddForce(Origin.up * Random.Range(1.3f, 1.8f), ForceMode2D.Impulse);
        }

        if (HitPoints <= 0)
        {
            GainMeat(8 + (encounter * 4) / 9);
            GainExperience(22 + encounter);
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
            UnlockProgressText[room - 1].text = (diggingRequirement[room - 1] - diggingProgress[room - 1]).ToString();
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
                BulletLair.SetActive(true);
                RoomSprteObject[0].SetActive(true);
                AnimationObject[0].SetActive(false);
                AnimationObject[1].SetActive(true);
                break;
            case 1:
                RoomObject[which].SetActive(true);
                BulletsScript.built = true;
                BulletUpgrade.SetActive(true);
                RoomSprteObject[1].SetActive(true);
                AnimationObject[1].SetActive(false);
                AnimationObject[2].SetActive(true);
                break;
            case 2:
                LeafcuttersScript.bonusFloors++;
                if (LeafcuttersScript.bonusFloors < BonusLeavesFloorsReq.Length)
                {
                    UnlockObject[which].SetActive(true);
                    diggingProgress[which] = 0;
                    diggingRequirement[which] = BonusLeavesFloorsReq[LeafcuttersScript.bonusFloors];
                    UnlockProgressText[which].text = (diggingRequirement[which] - diggingProgress[which]).ToString();
                }
                break;
            case 3:
                BulletsScript.bonusFloors++;
                if (BulletsScript.bonusFloors < BonusBulletFloorsReq.Length)
                {
                    UnlockObject[which].SetActive(true);
                    diggingProgress[which] = 0;
                    diggingRequirement[which] = BonusBulletFloorsReq[BulletsScript.bonusFloors];
                    UnlockProgressText[which].text = (diggingRequirement[which] - diggingProgress[which]).ToString();
                }
                break;
            case 4:
                ThirdUpgrades.SetActive(true);
                RoomSprteObject[2].SetActive(true);
                break;
        }
    }

    public void GainLeaves(int amount)
    {
        leaves += amount;
        GainExperience(amount * (2 + Perk[2]));
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

    public void GainSP()
    {
        skillPoints++;
        SPText.text = skillPoints.ToString();
        CheckAviablePerks();
    }

    public void SpendSP(int amount)
    {
        skillPoints -= amount;
        SPText.text = skillPoints.ToString();
        CheckAviablePerks();
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
        if (level == 25)
            ResearchLair.SetActive(true);
        LevelText.text = level.ToString();
        experienceReq = NextLevelExpReq();
        GainExperience(0);
        GainSP();
       // if (level % 5 == 0)
       //     skillPoints++;
        summonMultiplyer += 0.01f * Perk[8];
    }

    // Checks
    int NextLevelExpReq()
    {
        return 80 + level * 36 + level * (level + 1) * 6;
    }

    void SetEncounter()
    {
        MobImage.sprite = MobSprite[Random.Range(0, MobSprite.Length)];
        temp = (50f + encounter * 2.3f) * (1f + encounter * 0.0165f);
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
        SpendSP(perkCost[which]);
        //pointsSpent++;
        pointsSpent += perkCost[which];
        Perk[which]++;
        if (Perk[which] == perkMax[which])
            aviableToBuy[which] = false;
        PerksText[which].text = Perk[which].ToString() + "/" + perkMax[which].ToString();
        switch (which)
        {
            case 0:
                summonMultiplyer += 0.11f;
                break;
            case 6:
                bonusClick += 2;
                break;
            case 8:
                summonMultiplyer += 0.01f * level;
                break;
            case 11:
                Invoke("AutoClick", 0.66f);
                break;
        }
        if (pointsSpent >= 2)
        {
            for (int i = 3; i < 6; i++)
            {
                if (Perk[i] < perkMax[i] && Perk[i - 3] > 0)
                    UnlockPerk(i);
            }
        }
        if (pointsSpent >= 5)
        {
            if (Perk[6] == 0)
                UnlockPerk(6);
            if (Perk[7] == 0)
                UnlockPerk(7);
        }
        if (pointsSpent >= 9)
        {
            for (int i = 8; i < 11; i++)
            {
                if (Perk[i] < perkMax[i] && Perk[i - 5] > 0)
                    UnlockPerk(i);
            }
        }
        if (pointsSpent >= 14 && Perk[11] == 0)
            UnlockPerk(11);
        CheckAviablePerks();
    }

    void CheckAviablePerks()
    {
        for (int i = 0; i < 12; i++)
        {
            if (aviableToBuy[i] && skillPoints >= perkCost[i])
                PerkButton[i].interactable = true;
            else PerkButton[i].interactable = false;
        }
    }

    void UnlockPerk(int which)
    {
        aviableToBuy[which] = true;
        LockImage[which].SetActive(false);
    }

    void AutoClick()
    {
        QueenClicked();
        Invoke("AutoClick", 0.66f);
    }

    public void AnimationChange()
    {
        if (!animationOff)
            animationOff = true;
        else animationOff = false;

        for (int i = 0; i < Animators.Length; i++)
        {
            Animators[i].enabled = !animationOff;
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
