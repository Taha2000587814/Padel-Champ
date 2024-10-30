using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadCharacters : MonoBehaviour
{
    public Transform playerPosition;
    public Transform opponentPosition;
    public Transform teammatePosition; // New
    public Transform opponent2Position; // New
    public Animator comboLabel;
    public Text comboNumberLabel;
    public Animator swipeLabel;
    public Animator shootTip;
    public Animator startPanel;
    public GameObject gamePanel;
    public GameObject scoreTexts;
    public GameObject matchLabel;
    public CameraMovement cameraMovement;
    public bool playerOnly;

    GameObject playerPrefab;
    GameObject opponentPrefab;
    GameObject teammatePrefab; // New
    GameObject opponent2Prefab; // New

    void Awake()
    {
        playerPrefab = Resources.Load<GameObject>("Character prefabs/Player base prefab");
        opponentPrefab = Resources.Load<GameObject>("Character prefabs/Opponent base prefab");
        teammatePrefab = Resources.Load<GameObject>("Character prefabs/Teammate base prefab"); // New
        opponent2Prefab = Resources.Load<GameObject>("Character prefabs/Opponent2 base prefab"); // New

        if (playerPrefab == null || opponentPrefab == null || teammatePrefab == null || opponent2Prefab == null)
        {
            Debug.LogWarning("One or more character prefabs not found in resources");
        }
        else
        {
            GameObject newPlayer = Instantiate(playerPrefab, playerPosition.position, playerPosition.rotation);
            Player player = newPlayer.GetComponent<Player>();
            GameManager manager = FindObjectOfType<GameManager>();
            manager.player = player;

            if (!playerOnly)
            {
                GameObject newOpponent = Instantiate(opponentPrefab, opponentPosition.position, opponentPosition.rotation);
                Opponent opponent = newOpponent.GetComponent<Opponent>();
                manager.opponent = opponent;
                opponent.player = newPlayer.transform;
                opponent.lookAt = newPlayer.transform;
                player.opponent = opponent.transform;

                GameObject newTeammate = Instantiate(teammatePrefab, teammatePosition.position, teammatePosition.rotation); // New
                Teammate teammate = newTeammate.GetComponent<Teammate>(); // Assuming you have a Teammate class
                manager.teammate = teammate; // Assuming you have a teammate field in your GameManager
                teammate.player = newPlayer.transform;

                GameObject newOpponent2 = Instantiate(opponent2Prefab, opponent2Position.position, opponent2Position.rotation); // New
                Opponent opponent2 = newOpponent2.GetComponent<Opponent>();
                manager.opponent2 = opponent2; // Assuming you have an opponent2 field in your GameManager
                opponent2.player = newPlayer.transform;
                opponent2.lookAt = newPlayer.transform;
                player.opponent2 = opponent2.transform; // Assuming player has an opponent2 field
            }

            Opponent op = FindObjectOfType<Opponent>();
            Transform opponentTransform = op.transform;
            player.lookAt = opponentTransform;

            if (playerOnly)
            {
                player.opponent = opponentTransform;
                op.lookAt = player.transform;
                op.player = player.transform;
            }

            cameraMovement.camTarget = player.transform;
            AssignPlayerReferences(player);
        }
    }

    void AssignPlayerReferences(Player player)
    {
        player.comboLabel = comboLabel;
        player.comboNumberLabel = comboNumberLabel;
        player.swipeLabel = swipeLabel;
        player.shootTip = shootTip;
        player.startPanel = startPanel;
        player.gamePanel = gamePanel;
        player.scoreTexts = scoreTexts;
        player.matchLabel = matchLabel;
        player.cameraMovement = cameraMovement;
    }
}
