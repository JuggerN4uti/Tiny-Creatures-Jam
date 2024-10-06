using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHUD : MonoBehaviour
{
    [Header("Scripts")]
    public Colony ColonyScript;
    public Leafcutters LeafcuttersScript;
    public Bullets BulletsScript;
    public Upgrades[] RoomUpgrades;

    [Header("UI")]
    public GameObject SummonWorkersBar;
    public GameObject[] ButUpgradeObject;
    public Button[] BuyUpgradeButton;
    public TMPro.TextMeshProUGUI[] CostText;
    public TMPro.TextMeshProUGUI[] UpgradeEffect;

    [Header("Stats")]
    public int currentSelected;
    public int[] currentCosts;

    void Start()
    {
        SelectRoomToUpgrade(0);
    }

    void Update()
    {
        if (ColonyScript.leaves >= currentCosts[0])
            BuyUpgradeButton[0].interactable = true;
        else BuyUpgradeButton[0].interactable = false;

        if (ColonyScript.meat >= currentCosts[1])
            BuyUpgradeButton[1].interactable = true;
        else BuyUpgradeButton[1].interactable = false;

        if (ColonyScript.skillPoints >= currentCosts[2])
            BuyUpgradeButton[2].interactable = true;
        else BuyUpgradeButton[2].interactable = false;
    }

    public void SelectRoomToUpgrade(int room)
    {
        currentSelected = room;

        if (RoomUpgrades[currentSelected].upgradesBought1 < RoomUpgrades[currentSelected].upgradesCount1)
        {
            ButUpgradeObject[0].SetActive(true);
            currentCosts[0] = RoomUpgrades[currentSelected].upgradeCost1[RoomUpgrades[currentSelected].upgradesBought1];
            CostText[0].text = currentCosts[0].ToString();
            UpgradeEffect[0].text = RoomUpgrades[currentSelected].upgradeText1[RoomUpgrades[currentSelected].upgradesBought1];
        }
        else
        {
            ButUpgradeObject[0].SetActive(false);
            UpgradeEffect[0].text = "Maxed";
        }

        if (RoomUpgrades[currentSelected].upgradesBought2 < RoomUpgrades[currentSelected].upgradesCount2)
        {
            ButUpgradeObject[1].SetActive(true);
            currentCosts[1] = RoomUpgrades[currentSelected].upgradeCost2[RoomUpgrades[currentSelected].upgradesBought2];
            CostText[1].text = currentCosts[1].ToString();
            UpgradeEffect[1].text = RoomUpgrades[currentSelected].upgradeText2[RoomUpgrades[currentSelected].upgradesBought2];
        }
        else
        {
            ButUpgradeObject[1].SetActive(false);
            UpgradeEffect[1].text = "Maxed";
        }

        if (RoomUpgrades[currentSelected].upgradesBought3 < RoomUpgrades[currentSelected].upgradesCount3)
        {
            ButUpgradeObject[2].SetActive(true);
            currentCosts[2] = RoomUpgrades[currentSelected].upgradeCost3[RoomUpgrades[currentSelected].upgradesBought3];
            CostText[2].text = currentCosts[2].ToString();
            UpgradeEffect[2].text = RoomUpgrades[currentSelected].upgradeText3[RoomUpgrades[currentSelected].upgradesBought3];
        }
        else
        {
            ButUpgradeObject[2].SetActive(false);
            UpgradeEffect[2].text = "Maxed";
        }
    }

    public void Upgrade1()
    {
        ColonyScript.SpendLeaves(currentCosts[0]);
        switch (currentSelected, RoomUpgrades[currentSelected].upgradesBought1)
        {
            case (0, 0):
                SummonWorkersBar.SetActive(true);
                ColonyScript.autoSpawn = true;
                ColonyScript.timeToSpawn = 3.6f;
                break;
            case (0, 1):
                ColonyScript.timeToSpawn = 2.5f;
                break;
            case (0, 2):
                ColonyScript.timeToSpawn = 1.8f;
                break;
            case (0, 3):
                ColonyScript.timeToSpawn = 1.35f;
                break;
            case (0, 4):
                ColonyScript.timeToSpawn = 1f;
                break;
            case (0, 5):
                ColonyScript.timeToSpawn = 0.75f;
                break;
            case (1, 0):
                LeafcuttersScript.timeToSpawn = 6.2f;
                break;
            case (1, 1):
                LeafcuttersScript.timeToSpawn = 5.3f;
                break;
            case (1, 2):
                LeafcuttersScript.timeToSpawn = 4.5f;
                break;
            case (1, 3):
                LeafcuttersScript.timeToSpawn = 3.8f;
                break;
            case (1, 4):
                LeafcuttersScript.timeToSpawn = 3.2f;
                break;
            case (2, 0):
                BulletsScript.timeToSpawn = 8.1f;
                break;
            case (2, 1):
                BulletsScript.timeToSpawn = 6.9f;
                break;
            case (2, 2):
                BulletsScript.timeToSpawn = 5.8f;
                break;
            case (2, 3):
                BulletsScript.timeToSpawn = 4.8f;
                break;
            case (2, 4):
                BulletsScript.timeToSpawn = 3.9f;
                break;
        }
        RoomUpgrades[currentSelected].upgradesBought1++;
        SelectRoomToUpgrade(currentSelected);
    }

    public void Upgrade2()
    {
        ColonyScript.SpendMeat(currentCosts[1]);
        switch (currentSelected, RoomUpgrades[currentSelected].upgradesBought2)
        {
            case (0, 0):
                ColonyScript.workerPower = 2;
                break;
            case (0, 1):
                ColonyScript.workerPower = 3;
                break;
            case (0, 2):
                ColonyScript.workerPower = 4;
                break;
            case (0, 3):
                ColonyScript.workerPower = 5;
                break;
            case (0, 4):
                ColonyScript.workerPower = 6;
                break;
            case (1, 0):
                LeafcuttersScript.LeavesCollected[0] = 2;
                LeafcuttersScript.LeavesCollected[1] = 3;
                break;
            case (1, 1):
                LeafcuttersScript.LeavesCollected[0] = 3;
                LeafcuttersScript.LeavesCollected[1] = 5;
                break;
            case (1, 2):
                LeafcuttersScript.LeavesCollected[0] = 5;
                LeafcuttersScript.LeavesCollected[1] = 8;
                break;
            case (1, 3):
                LeafcuttersScript.LeavesCollected[0] = 7;
                LeafcuttersScript.LeavesCollected[1] = 12;
                break;
            case (2, 0):
                BulletsScript.bulletDamage[0] = 13;
                BulletsScript.bulletDamage[1] = 20;
                break;
            case (2, 1):
                BulletsScript.bulletDamage[0] = 18;
                BulletsScript.bulletDamage[1] = 28;
                break;
            case (2, 2):
                BulletsScript.bulletDamage[0] = 26;
                BulletsScript.bulletDamage[1] = 37;
                break;
            case (2, 3):
                BulletsScript.bulletDamage[0] = 33;
                BulletsScript.bulletDamage[1] = 49;
                break;
        }
        RoomUpgrades[currentSelected].upgradesBought2++;
        SelectRoomToUpgrade(currentSelected);
    }

    public void Upgrade3()
    {
        ColonyScript.SpendSP(1);
        switch (currentSelected, RoomUpgrades[currentSelected].upgradesBought3)
        {
            case (0, 0):
                ColonyScript.summonMultiplyer += 0.14f;
                break;
            case (0, 1):
                ColonyScript.summonMultiplyer += 0.18f;
                break;
            case (0, 2):
                ColonyScript.summonMultiplyer += 0.22f;
                break;
            case (0, 3):
                ColonyScript.summonMultiplyer += 0.26f;
                break;
            case (1, 0):
                LeafcuttersScript.bonus = 4;
                break;
            case (1, 1):
                LeafcuttersScript.bonus = 3;
                break;
            case (1, 2):
                LeafcuttersScript.bonus = 2;
                break;
            case (1, 3):
                LeafcuttersScript.bonus = 1;
                break;
            case (2, 0):
                BulletsScript.bonus = 13;
                break;
            case (2, 1):
                BulletsScript.bonus = 31;
                break;
            case (2, 2):
                BulletsScript.bonus = 54;
                break;
            case (2, 3):
                BulletsScript.bonus = 82;
                break;
        }
        RoomUpgrades[currentSelected].upgradesBought3++;
        SelectRoomToUpgrade(currentSelected);
    }
}
