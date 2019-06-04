using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks if the entered attack is a combo, sets last attack of combo to special
/// </summary>
public class CheckForCombo : MonoBehaviour
{
    public AttackIcon low;
    public AttackIcon mid;
    public AttackIcon high;

    public Color normal;
    public Color combo;

    //TODO nasty hack to get feral rp to work
    public static bool Feral;

    private void OnEnable()
    {
        Feral = false;
        normal = ArenaManager.Instance.white;
        combo = ArenaManager.Instance.pink;
    }

    void Update()
    {
        if (ArenaManager.Instance.ArenaState == eArena.PlayerInput && ComboHandler.attacks.Count == ArenaManager.Instance.MaxPlayerSlots)
        {
            if (ComboHandler.attacks[0] == low)
            {
                if (ComboHandler.attacks[1] == mid)
                {
                    if (ComboHandler.attacks[2] == mid)
                    {
                        if (ArenaManager.Instance.PlayerRagePoints >= mid.RPCost)
                        {                            
                            ComboHandler.playerSlots[0].icon.color = combo;
                            ComboHandler.playerSlots[1].icon.color = combo;
                            ComboHandler.playerSlots[2].icon.color = combo;
                            Feral = true;
                        }
                    }
                }
            }
            else
            {
                Feral = false;
            }
        }
        else
        {
            ComboHandler.playerSlots[0].icon.color = normal;
            ComboHandler.playerSlots[1].icon.color = normal;
            ComboHandler.playerSlots[2].icon.color = normal;
        }
    }

}
