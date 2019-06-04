using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feralArtCheck : MonoBehaviour
{

    //List with Feral Arts (Scriptable objects) - only the ones available at this fight (get it from the loadout)
    public List<FeralArt> LoadedFeralArts = new List<FeralArt>();

    //call this function once there are >= 3 BA in the input bar -> retrieve current input
    public void CheckInput(List<BaseAttack> curPlayerInput)
    {
        for (int i = 3; i >= 0; i--)
        {
            
        
        }

        foreach (FeralArt feralArt in LoadedFeralArts)
        {
            for (int i = 3; i >= 0; i--)
            {
                if (feralArt.FeralArtInput[i] == curPlayerInput[i])
                {

                }

            }
        }
    }



}
