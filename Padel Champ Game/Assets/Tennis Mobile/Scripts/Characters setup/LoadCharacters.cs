using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadCharacters : MonoBehaviour
{
    public Transform playerPosition;
    public Transform opponentPosition;
    public Transform teammatePosition;
    public Transform opponent2Position;
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
    GameObject teammatePrefab;
    GameObject opponent2Prefab;

    void Awake()
    {
        playerPrefab = Resources.Load<GameObject>("Character prefabs/Player base prefab");
        opponentPrefab = Resources.Load<GameObject>("Character prefabs/Opponent base prefab");
        teammatePrefab = Resources.Load<GameObject>("Character prefabs/Teammate base prefab");
        opponent2Prefab = Resources.Load<GameObject>("Character prefabs/Opponent2 base prefab");

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

                GameObject newTeammate = Instantiate(teammatePrefab, teammatePosition.position, teammatePosition.rotation);
                Teammate teammate = newTeammate.GetComponent<Teammate>();
                manager.teammate = teammate;
                teammate.player = newPlayer.transform;
                teammate.lookAt = newOpponent.transform; // Teammate looks at the opponent
                teammate.followBall = true;
                teammate.randomPitch = teammate.GetComponent<RandomPitch>();

                GameObject newOpponent2 = Instantiate(opponent2Prefab, opponent2Position.position, opponent2Position.rotation);
                Opponent opponent2 = newOpponent2.GetComponent<Opponent>();
                manager.opponent2 = opponent2;
                opponent2.player = newPlayer.transform;
                opponent2.lookAt = newPlayer.transform;
                player.opponent2 = opponent2.transform;
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
