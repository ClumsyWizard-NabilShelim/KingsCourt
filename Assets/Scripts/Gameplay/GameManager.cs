using System.Collections;
using System.Collections.Generic;
using ClumsyWizard.Utilities;
using TMPro;
using UnityEngine;

public enum GameState
{
    Initialize = -1,
    Summoning,
    KingdomResolve,
    PopularityResolve,
    DayEnd,
}

public class GameManager : Singleton<GameManager>
{
    private Animator animator;
    private AudioManager audioManager;

    public GameState State { get; private set; }
    public int DayCount { get; private set; }

    [SerializeField] private ClumsyDictionary<GameState, GameplayModule> gameplayModules;

    [Header("Game Verdict")]
    [SerializeField] private GameObject beheadingGameOverPanel;
    [SerializeField] private ClumsyDictionary<Faction, GameObject> gameOverPanel;
    [SerializeField] private TextMeshProUGUI verdictText;

    [Header("Day Count")]
    [SerializeField] private TextMeshProUGUI dayText;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();

        State = GameState.Initialize;
        NextState();
    }

    public void NextState()
    {
        switch (State)
        {
            case GameState.Initialize:
                StartCoroutine(SetState(GameState.Summoning));
                break;
            case GameState.Summoning:
                StartCoroutine(SetState(GameState.KingdomResolve));
                break;
            case GameState.KingdomResolve:
                StartCoroutine(SetState(GameState.PopularityResolve));
                break;
            case GameState.PopularityResolve:
                StartCoroutine(SetState(GameState.DayEnd));
                break;
            case GameState.DayEnd:
                StartCoroutine(SetState(GameState.Summoning));
                break;
            default:
                break;
        }
    }

    private IEnumerator SetState(GameState state)
    {
        State = state;

        if(State == GameState.DayEnd)
        {
            NextState();
        }
        else
        {
            if(State == GameState.Summoning)
            {
                DayCount++;
                dayText.text = "Day " + DayCount.ToString();
                animator.SetTrigger("ChangeDay");
                audioManager.Play("DayStart");
                yield return new WaitForSeconds(2.0f);
            }

            if (gameplayModules.ContainsKey(State))
                gameplayModules[State].Activate();
        }
    }

    //Game Verdict
    public void GameOver()
    {
        audioManager.Stop("BackgroundMusic");
        audioManager.Play("GameOver");
        beheadingGameOverPanel.SetActive(true);
        animator.SetTrigger("GameOver");

        if (PlayerManager.Instance.Resources[ResourceType.Gold] <= 0)
            verdictText.text = $"King for <color=#98EE7A>{DayCount}</color> Days. Doomed by <color=#F34953>Lack of Coffers</color>";
        else
            verdictText.text = $"King for <color=#98EE7A>{DayCount}</color> Days. Doomed by <color=#F34953>Lack of Provision</color>";
    }

    public void GameOver(Faction faction)
    {
        audioManager.Stop("BackgroundMusic");
        audioManager.Play("GameOver");
        gameOverPanel[faction].SetActive(true);
        animator.SetTrigger("GameOver");

        verdictText.text = $"King for <color=#98EE7A>{DayCount}</color> Days. Doomed by <color=#F34953>{faction} <sprite={(int)faction}></color>";
    }

    //UI
    public void Retry()
    {
        SceneManager.Instance.Load("Court");
    }

    public void MainMenu()
    {
        SceneManager.Instance.Load("MainMenu");
    }

    //Cleanup
    protected override void CleanUpStaticData()
    {
    }
}
