using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopularityResolveManager : GameplayModule
{
    [Header("Game Over Variables")]
    [SerializeField] private int maxUnpopularDaysAllowed = 2;
    private Dictionary<Faction, int> unpopularFaction = new Dictionary<Faction, int>();

    private void Start()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(Faction)).Length; i++)
        {
            unpopularFaction.Add((Faction)i, 0);
        }
    }

    public override void Activate()
    {
        if(PlayerManager.Instance.Resources[ResourceType.Gold] == 0 || PlayerManager.Instance.Resources[ResourceType.Provision] == 0)
        {
            GameManager.Instance.GameOver();
            return;
        }

        //Popularity Resolve

        for (int i = 0; i < unpopularFaction.Count; i++)
        {
            Faction faction = unpopularFaction.ElementAt(i).Key;

            if (PlayerManager.Instance.FactionPopularity[faction] <= 0)
            {
                unpopularFaction[faction]++;
            }
            else
            {
                if (unpopularFaction[faction] > 0)
                    unpopularFaction[faction]--;
            }

            if (unpopularFaction[faction] >= maxUnpopularDaysAllowed)
            {
                GameManager.Instance.GameOver(faction);
                return;
            }
        }

        GameManager.Instance.NextState();
    }
}
