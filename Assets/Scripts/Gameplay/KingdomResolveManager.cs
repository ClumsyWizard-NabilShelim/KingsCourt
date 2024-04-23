using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KingdomResolveManager : GameplayModule
{
    private Animator animator;

    [SerializeField] private List<KingdomEventSO> kingdomEventProfit;
    [SerializeField] private List<KingdomEventSO> kingdomEventLoss;
    [SerializeField] private List<KingdomEventSO> kingdomEventTrouble;

    private KingdomEventSO currentKingdomEventSO;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI eventNameText;
    [SerializeField] private TextMeshProUGUI eventDescriptionText;
    [SerializeField] private TextMeshProUGUI eventResultText;
    [SerializeField] private TextMeshProUGUI agreeButtonText;
    [SerializeField] private TextMeshProUGUI declineButtonText;
    [SerializeField] private Button agreeButton;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Activate()
    {
        float chanceForEvent = Random.Range(0.0f, 100.0f);

        if(chanceForEvent <= 40.0f)
        {
            //40% chance for no events
            GameManager.Instance.NextState();
        }
        else if(chanceForEvent > 40.0f && chanceForEvent <= 60.0f)
        {
            //20% Chance of profit event
            if (kingdomEventProfit.Count > 0)
            {
                currentKingdomEventSO = kingdomEventProfit[Random.Range(0, kingdomEventProfit.Count)];
                kingdomEventProfit.Remove(currentKingdomEventSO);
                UpdateUI();
            }
            else
            {
                GameManager.Instance.NextState();
            }
        }
        else if (chanceForEvent > 60.0f && chanceForEvent <= 80.0f)
        {
            //20% Chance of loss event
            if (kingdomEventLoss.Count > 0)
            {
                currentKingdomEventSO = kingdomEventLoss[Random.Range(0, kingdomEventLoss.Count)];
                UpdateUI();
            }
            else
            {
                GameManager.Instance.NextState();
            }
        }
        else
        {
            //20% Chance of trouble event
            if (kingdomEventTrouble.Count > 0)
            {
                currentKingdomEventSO = kingdomEventTrouble[Random.Range(0, kingdomEventTrouble.Count)];
                UpdateUI();
            }
            else
            {
                GameManager.Instance.NextState();
            }
        }
    }

    private void UpdateUI()
    {
        agreeButtonText.text = currentKingdomEventSO.AgreeLabel;
        declineButtonText.text = currentKingdomEventSO.DeclintLabel;

        eventNameText.text = currentKingdomEventSO.EventName;
        eventDescriptionText.text = currentKingdomEventSO.EventDescription;

        string agreeInfo = currentKingdomEventSO.GetEventResultTextInfo();
        if (agreeInfo == "")
        {
            eventResultText.gameObject.SetActive(false);
        }
        else
        {
            eventResultText.gameObject.SetActive(true);
            eventResultText.text = agreeInfo;
        }

        agreeButton.interactable = currentKingdomEventSO.CanAgreeToDemand();

        animator.SetBool("FadeIn", true);
    }

    public void OnAgree()
    {
        currentKingdomEventSO.OnAgree();
        ShowPopularityResult();
    }
    public void OnDecline()
    {
        currentKingdomEventSO.OnDecline();
        ShowPopularityResult();
    }

    private void ShowPopularityResult()
    {
        animator.SetBool("FadeIn", false);
        PlayerManager.Instance.ShowFactionOpinionMenu(() =>
        {
            GameManager.Instance.NextState();
        });
    }
}
