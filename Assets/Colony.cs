using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony : MonoBehaviour
{
    [Header("Dig")]
    public int diggingProgress;
    public int[] diggingRequirement;
    public int roomsDug;

    [Header("Dig UI")]
    public GameObject[] UnlockObject;
    public TMPro.TextMeshProUGUI[] UnlockProgressText;

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
        diggingProgress -= diggingRequirement[roomsDug];
        roomsDug++;
        UnlockObject[roomsDug].SetActive(true);
    }
}
