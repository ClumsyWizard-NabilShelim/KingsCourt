using System;
using System.Collections;
using System.Collections.Generic;
using ClumsyWizard.Utilities;
using UnityEngine;

public enum ResourceType
{
    Gold,
    Provision,
    Men
}

public enum Faction
{
    Common_Folk,
    Nobel_Lords,
    Priests,
    Advisors,
    Army
}

public class PlayerManager : Singleton<PlayerManager>
{
    public static readonly int MAX_FACTION_COUNT = Enum.GetValues(typeof(Faction)).Length;
    public static readonly int MAX_FACTION_POPULARITY = 10;

    private PlayerUI uiManager;

    public Dictionary<ResourceType, int> Resources { get; private set; }
    public Dictionary<Faction, int> FactionPopularity { get; private set; }

    private void Start()
    {
        uiManager = GetComponent<PlayerUI>();

        Resources = new Dictionary<ResourceType, int>()
        {
            { ResourceType.Gold, 10000},
            { ResourceType.Provision, 350},
            { ResourceType.Men, 100},
        };

        FactionPopularity = new Dictionary<Faction, int>()
        {
            { Faction.Common_Folk, 8 },
            { Faction.Nobel_Lords, 8},
            { Faction.Priests, 8},
            { Faction.Advisors, 8},
            { Faction.Army, 8},
        };

        uiManager.Initialize(Resources, FactionPopularity);
    }

    //Resource Management
    public void AddResource(ResourceType type, int amount)
    {
        Resources[type] += Mathf.Abs(amount);
        uiManager.UpdateResourceUI(type, Resources[type]);
    }
    public bool UseResource(ResourceType type, int amount)
    {
        if (Resources[type] < Mathf.Abs(amount))
            return false;
        
        Resources[type] -= Mathf.Abs(amount);
        uiManager.UpdateResourceUI(type, Resources[type]);
        return true;
    }

    //Faction Management
    public void IncreasePopularity(Faction faction, int amount)
    {
        if (FactionPopularity[faction] == MAX_FACTION_POPULARITY)
            return;

        FactionPopularity[faction] += Mathf.Abs(amount);

        if (FactionPopularity[faction] > MAX_FACTION_POPULARITY)
            FactionPopularity[faction] = MAX_FACTION_POPULARITY;

        uiManager.UpdateFactionPopularityUI(faction, FactionPopularity[faction], amount);
    }
    public void DecreasePopularity(Faction faction, int amount)
    {
        if (FactionPopularity[faction] == 0)
            return;

        FactionPopularity[faction] -= Mathf.Abs(amount);

        if (FactionPopularity[faction] < 0)
            FactionPopularity[faction] = 0;

        uiManager.UpdateFactionPopularityUI(faction, FactionPopularity[faction], -Mathf.Abs(amount));
    }

    //UI
    public void ShowFactionOpinionMenu(Action closeCallback)
    {
        uiManager.onFactionOpinionMenuClose += () =>
        {
            closeCallback?.Invoke();
        };

        uiManager.ToggleFactionOpinionMenu(true);
    }

    //Clean up
    protected override void CleanUpStaticData()
    {
    }
}
