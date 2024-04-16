using System;
using System.Collections;
using System.Collections.Generic;
using ClumsyWizard.Utilities;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public Action onFactionOpinionMenuClose;

    [Header("Resources")]
    [SerializeField] private ClumsyDictionary<ResourceType, CWIconTextPair> resourceUI;

    [Header("Faction")]
    [SerializeField] private ClumsyDictionary<Faction, FactionPopularitySlotUI> factionPopularityUI;
    [SerializeField] private Animator factionUIAnimator;

    public void Initialize(Dictionary<ResourceType, int> resources, Dictionary<Faction, int> factionPopularity)
    {
        foreach (ResourceType key in resources.Keys)
        {
            UpdateResourceUI(key, resources[key]);
        }

        foreach (Faction key in factionPopularity.Keys)
        {
            UpdateFactionPopularityUI(key, factionPopularity[key], 0);
        }
    }

    //Resource UI
    public void UpdateResourceUI(ResourceType type, int amount)
    {
        resourceUI[type].UpdateText(amount.ToString());
    }

    //Faction popularity UI
    public void UpdateFactionPopularityUI(Faction type, int currentAmount, int changeAmount)
    {
        factionPopularityUI[type].UpdateUI(currentAmount, changeAmount);
    }
    public void ToggleFactionOpinionMenu(bool toggle)
    {
        factionUIAnimator.SetBool("SlideIn", toggle);

        if (toggle == false)
        {
            for (int i = 0; i < PlayerManager.MAX_FACTION_COUNT; i++)
            {
                Faction faction = (Faction)i;
                factionPopularityUI[faction].ToggleChangeText(false);
            }

            onFactionOpinionMenuClose?.Invoke();
            onFactionOpinionMenuClose = null;
        }
    }
}
