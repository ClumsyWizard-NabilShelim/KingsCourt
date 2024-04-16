using System.Collections;
using System.Collections.Generic;
using ClumsyWizard.Utilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Summon Data")]
public class SummonSO : ScriptableObject
{
    [field: SerializeField] public Sprite FactionSprite { get; private set; }
    [field: SerializeField, TextArea(5, 5)] public string Demand { get; private set; }
    [field: SerializeField] public string AgreeLabel { get; private set; }
    [field: SerializeField] public string DeclintLabel { get; private set; }

    [Header("Agree Result")]
    [SerializeField] private ClumsyDictionary<ResourceType, int> agreeResult;
    [SerializeField] private ClumsyDictionary<Faction, int> agreeFactionPopularityResult;

    [Header("Decline Result")]
    [SerializeField] private ClumsyDictionary<Faction, int> declineFactionPopularityResult;

    public string GetAgreeResultInfo()
    {
        string info = "";

        if (agreeResult.Count != 0)
        {
            foreach (ResourceType type in agreeResult.Keys)
            {
                string icon = "";
                switch (type)
                {
                    case ResourceType.Gold:
                        icon = " <sprite=2>";
                        break;
                    case ResourceType.Provision:
                        icon = " <sprite=0>";
                        break;
                    case ResourceType.Men:
                        icon = " <sprite=1>";
                        break;
                    default:
                        icon = "";
                        break;
                }
                info += (agreeResult[type] > 0 ? "+" : "") + agreeResult[type].ToString() + icon + "\n";
            }
        }

        return info;
    }

    public bool CanAgreeToDemand()
    {
        bool canAgree = true;

        if (agreeResult.Count != 0)
        {
            foreach (ResourceType type in agreeResult.Keys)
            {
                if (agreeResult[type] > 0)
                    continue;

                if (PlayerManager.Instance.Resources[type] < Mathf.Abs(agreeResult[type]))
                    return false;
            }
        }

        return canAgree;
    }

    public void OnAgree()
    {
        if (agreeResult.Count != 0)
        {
            foreach (ResourceType type in agreeResult.Keys)
            {
                if (agreeResult[type] > 0)
                {
                    PlayerManager.Instance.AddResource(type, agreeResult[type]);
                }
                else
                {
                    if(!PlayerManager.Instance.UseResource(type, agreeResult[type]))
                    {
                        OnDecline();
                        return;
                    }
                }
            }
        }

        if(agreeFactionPopularityResult.Count != 0)
        {
            for (int i = 0; i < PlayerManager.MAX_FACTION_COUNT; i++)
            {
                Faction faction = (Faction)i;

                if (!agreeFactionPopularityResult.ContainsKey(faction))
                {
                    PlayerManager.Instance.IncreasePopularity(faction, 0);
                    continue;
                }

                if (agreeFactionPopularityResult[faction] > 0)
                {
                    PlayerManager.Instance.IncreasePopularity(faction, agreeFactionPopularityResult[faction]);
                }
                else
                {
                    PlayerManager.Instance.DecreasePopularity(faction, agreeFactionPopularityResult[faction]);
                }
            }
        }
    }

    public void OnDecline()
    {
        if (declineFactionPopularityResult.Count != 0)
        {
            for (int i = 0; i < PlayerManager.MAX_FACTION_COUNT; i++)
            {
                Faction faction = (Faction)i;

                if (!declineFactionPopularityResult.ContainsKey(faction))
                {
                    PlayerManager.Instance.IncreasePopularity(faction, 0);
                    continue;
                }

                if (declineFactionPopularityResult[faction] > 0)
                {
                    PlayerManager.Instance.IncreasePopularity(faction, declineFactionPopularityResult[faction]);
                }
                else
                {
                    PlayerManager.Instance.DecreasePopularity(faction, declineFactionPopularityResult[faction]);
                }
            }

        }
    }
}
