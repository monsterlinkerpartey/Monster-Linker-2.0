using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeralArtCheck : MonoBehaviour
{
    //List with Feral Arts (Scriptable objects) - only the ones available at this fight (get it from the loadout)
    public List<FeralArt> LoadedFeralArts = new List<FeralArt>();


    //call this function once there are >= 3 BA in the input bar -> retrieve current input
    public void CheckForFeralArt(List<BaseAttack> curPlayerInput)
    {
        int BAinputs = curPlayerInput.Count;
        int i = BAinputs-1;
        bool feralTreedown;

        switch (BAinputs)
        {
            case 3:
                
                print("looping through the feral art list");
                //check every feral art
                foreach (FeralArt feralArt in LoadedFeralArts)
                {
                    feralTreedown = true;

                    if (feralArt.FeralArtInput.Count > BAinputs)
                    {
                        print("feral art " + feralArt.name + " is longer than " + BAinputs);
                        break;
                    }

                    while (feralTreedown == true)
                    {
                        if (feralArt.FeralArtInput[i] == curPlayerInput[i])
                        {
                            if (i < 0)
                            {
                                print("player input = feral art " + feralArt.name);
                                return;
                            }
                            else
                            {
                                i--;
                                print("playerinput: " + curPlayerInput[i].name + " = Feral Art " + feralArt.name + " input");
                                feralTreedown = true;
                            }
                        }
                        else
                        {
                            print("this is not a feral art");
                            feralTreedown = false;
                            break;
                        }
                        return;
                    }                  

                }                


                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
        
    }
    
}
