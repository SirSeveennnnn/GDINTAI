using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool pregame = true;

    [SerializeField]
    private GameObject pregameWindow;

    [SerializeField]
    private GameObject gameOverWindow;

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    [SerializeField]
    private GameObject pauseWindow;

    [SerializeField]
    private GameObject agentDeadlist;

    [SerializeField]
    private GameObject playerDeadlist;

    [SerializeField]
    private AI_Agent agent;

    [Space(10)]

    public Board board;
    public PieceManager pieceManager;

    // Start is called before the first frame update
    void Start()
    {
        board.Create();
        pieceManager.Setup(board);
        pieceManager.DisableBlackPieceInteractive();
    }

    void Update()
    {
        HandleInput();
    }

    public void StartGame()
    {
        pregameWindow.SetActive(false);
        agentDeadlist.SetActive(true);
        playerDeadlist.SetActive(true);

        pregame = false;

        pieceManager.SwitchSides(Color.black);
    }

    public void EndGame(Color color)
    {
        if (!gameOverWindow.activeSelf)
        {
            gameOverWindow.SetActive(true);
        }

        if (color == Color.black)
        {
            gameOverText.text = "The AI Defeated You";
        }
        else if (color == Color.white)
        {
            gameOverText.text = "You Defeated the AI!";
        }

        pregame = true;

        pieceManager.SwitchSides(Color.clear);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
            Debug.Log("pause");
        }
    }

    public void togglePause()
    {
        pauseWindow.SetActive(!pauseWindow.activeSelf);
    }

   
}
