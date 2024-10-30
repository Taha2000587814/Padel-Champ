using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreasManager : MonoBehaviour
{
    public BoxCollider playerArea;
    public BoxCollider teammateArea;
    public BoxCollider opponentArea;
    public BoxCollider opponent2Area;

    public GameObject playerPrefab;
    public GameObject teammatePrefab;
    public GameObject opponentPrefab;
    public GameObject opponent2Prefab;

    void Update()
    {
        CheckArea(playerArea, playerPrefab, "Player prefab within player area bounds");
        CheckArea(teammateArea, teammatePrefab, "Teammate prefab within teammate area bounds");
        CheckArea(opponentArea, opponentPrefab, "Opponent prefab within opponent area bounds");
        CheckArea(opponent2Area, opponent2Prefab, "Opponent 2 prefab within opponent 2 area bounds");
    }

    void CheckArea(BoxCollider area, GameObject prefab, string logMessage)
    {
        if (area.bounds.Contains(prefab.transform.position))
        {
            Debug.Log(logMessage);
        }
    }
}
