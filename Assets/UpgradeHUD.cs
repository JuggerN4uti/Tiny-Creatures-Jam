using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHUD : MonoBehaviour
{
    [Header("Scripts")]
    public Colony ColonyScript;
    public Leafcutters LeafcuttersScript;
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

        if (ColonyScript.leaves >= currentCosts[1])
            BuyUpgradeButton[1].interactable = true;
        else BuyUpgradeButton[1].interactable = false;
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
    }

    public void Upgrade1()
    {
        switch (currentSelected, RoomUpgrades[currentSelected].upgradesBought1)
        {
            case (0, 0):
                SummonWorkersBar.SetActive(true);
                ColonyScript.autoSpawn = true;
                ColonyScript.timeToSpawn = 4f;
                break;
            case (0, 1):
                ColonyScript.timeToSpawn = 2.8f;
                break;
            case (0, 2):
                ColonyScript.timeToSpawn = 2f;
                break;
            case (0, 3):
                ColonyScript.timeToSpawn = 1.5f;
                break;
            case (1, 0):
                LeafcuttersScript.timeToSpawn = 7f;
                break;
            case (2, 1):
                LeafcuttersScript.timeToSpawn = 6.1f;
                break;
            case (3, 2):
                LeafcuttersScript.timeToSpawn = 5.3f;
                break;
            case (4, 3):
                LeafcuttersScript.timeToSpawn = 4.6f;
                break;
        }
        RoomUpgrades[currentSelected].upgradesBought1++;
        SelectRoomToUpgrade(currentSelected);
    }

    public void Upgrade2()
    {
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
            case (1, 0):
                LeafcuttersScript.LeavesCollected[0] = 2;
                LeafcuttersScript.LeavesCollected[1] = 3;
                break;
            case (2, 1):
                LeafcuttersScript.LeavesCollected[0] = 3;
                LeafcuttersScript.LeavesCollected[1] = 5;
                break;
            case (3, 2):
                LeafcuttersScript.LeavesCollected[0] = 5;
                LeafcuttersScript.LeavesCollected[1] = 7;
                break;
            case (4, 3):
                LeafcuttersScript.LeavesCollected[0] = 7;
                LeafcuttersScript.LeavesCollected[1] = 10;
                break;
        }
        RoomUpgrades[currentSelected].upgradesBought2++;
        SelectRoomToUpgrade(currentSelected);
    }
}
