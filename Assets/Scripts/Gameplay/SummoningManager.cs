using System.Collections;
using System.Collections.Generic;
using ClumsyWizard.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummoningManager : GameplayModule
{
    private AudioManager audioManager;
    private Animator animator;

    [SerializeField] private int minSummons;
    [SerializeField] private int maxSummons;
    private int currentSummons;
    private int currentSummonsTarget;

    [SerializeField] private ClumsyDictionary<Faction, List<SummonSO>> summons;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI factionName;
    [SerializeField] private TextMeshProUGUI demandText;
    [SerializeField] private TextMeshProUGUI agreeResultText;
    [SerializeField] private TextMeshProUGUI agreeButtonText;
    [SerializeField] private TextMeshProUGUI declineButtonText;
    [SerializeField] private Button agreeButton;
    [SerializeField] private Image factionGFX;
    [SerializeField] private TextMeshProUGUI summoningsText;

    private SummonSO currentSummonSO;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
    }

    public override void Activate()
    {
        currentSummonsTarget = Random.Range(minSummons, maxSummons + 1);
        currentSummons = currentSummonsTarget;
        ShowSummon();
    }

    private void ShowSummon()
    {
        currentSummons--;

        if(currentSummons < 0)
        {
            GameManager.Instance.NextState();
            return;
        }

        summoningsText.text = $"Summons: {Mathf.Abs(currentSummonsTarget - currentSummons)} - {currentSummonsTarget}";

        audioManager.Play("Footstep");
        animator.SetBool("MoveIn", true);
        Faction faction = (Faction)Random.Range(0, PlayerManager.MAX_FACTION_COUNT);
        currentSummonSO = summons[faction][Random.Range(0, summons[faction].Count)];
        UpdateUI(faction);
    }

    private void UpdateUI(Faction faction)
    {
        factionName.text = faction.ToString().Replace("_", " ") + $" <sprite={(int)faction}>";

        agreeButtonText.text = currentSummonSO.AgreeLabel;
        declineButtonText.text = currentSummonSO.DeclintLabel;

        demandText.text = currentSummonSO.Demand;
        string agreeInfo = currentSummonSO.GetAgreeResultInfo();
        if(agreeInfo == "")
        {
            agreeResultText.gameObject.SetActive(false);
        }
        else
        {
            agreeResultText.gameObject.SetActive(true);
            agreeResultText.text = agreeInfo;
        }

        agreeButton.interactable = currentSummonSO.CanAgreeToDemand();

        factionGFX.sprite = currentSummonSO.FactionSprite;
    }

    //UI
    public void Agree()
    {
        currentSummonSO.OnAgree();
        ShowPopularityResult();
    }
    public void Decline()
    {
        currentSummonSO.OnDecline();
        ShowPopularityResult();
    }

    private void ShowPopularityResult()
    {
        audioManager.Play("Footstep");
        animator.SetBool("MoveIn", false);
        PlayerManager.Instance.ShowFactionOpinionMenu(() =>
        {
            ShowSummon();
        });
    }
}
