using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FeralArtCheck : MonoBehaviour
{
    [Header("FA Chains, drag n drop")]
    public List<FAChain> Chain5;
    public List<FAChain> Chain6;

    //public FAChain Chain5_1;
    //public FAChain Chain6_1;

    [Header("----- No touchies from this point on -----")]
    //FAs the player has chosen for this fight
    public List<FeralArt> LoadedFeralArts;
    //BAs the player has chosen for this round
    public List<BaseAttack> curBAlist;
    //List in which the actual attacks are written for readout during the fight
    public List<Attack> AttackList;

    public List<int> BAsToDelete;
    public List<int> BAsToColor;

    public bool superFAused;

    public InputBarHandler inputbarhandler;
    public ArenaUIHandler arenaui;
    public BAEffectsHandler baeffectshandler;

    [SerializeField]
    int FANo = 0;
    [SerializeField]
    int InputNo = 0;
    [SerializeField]
    int Pos = 0;
    [SerializeField]
    int RPCostSum;

    public void ResetLists()
    {
        curBAlist.Clear();
        AttackList.Clear();
        BAsToDelete.Clear();
        FANo = 0;
        InputNo = 0;
        Pos = 0;
    }

    public void SaveBAlist(BaseAttack attack)
    {
        curBAlist.Add(attack);
        AttackList.Add(attack);
    }

    //Checking for chained FAs
    public void CheckForChain()
    {
        //if player has not enough RP for the cheapest FA
        if (baeffectshandler.curPlayerRP < GameStateSwitch.Instance.curProfile.lowestFAcost)
        {
            print("not enough RP for FAs");
            return;
        }

        print("checking for chainable FAs");
        switch (inputbarhandler.maxBaseAttackInputSlots)
        {
            case 5:
                foreach (FAChain chain5 in Chain5)
                {
                    if (curBAlist.SequenceEqual(chain5.ChainInputList))
                    {
                        //evaluate costs of the chain
                        foreach (Attack attack in chain5.NeededFeralArts)
                        {
                            RPCostSum += attack.RPCost;
                        }

                        //see if player has enough RP
                        if (baeffectshandler.curPlayerRP >= RPCostSum)
                        {
                                for (int i = 0; i < 5; i++)
                            {
                                BAsToDelete.Add(i);
                                print("i: " + i);
                            }

                            AttackList.Clear();
                            AttackList = chain5.NeededFeralArts;
                            arenaui.VisializeFAs(BAsToDelete, Color.yellow);
                        }

                    }
                    else
                    {
                        print("not a chain, checking next");
                    }
                }
                print("no chains found");
                CompareLists();

                break;
            case 6:
                foreach (FAChain chain6 in Chain6)
                {
                    if (curBAlist.SequenceEqual(chain6.ChainInputList))
                    {
                        //evaluate costs of the chain
                        foreach (Attack attack in chain6.NeededFeralArts)
                        {
                            RPCostSum += attack.RPCost;
                        }

                        //see if player has enough RP
                        if (baeffectshandler.curPlayerRP >= RPCostSum)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                BAsToDelete.Add(i);
                                print("i: " + i);
                            }

                            AttackList.Clear();
                            AttackList = chain6.NeededFeralArts;
                            arenaui.VisializeFAs(BAsToDelete, Color.yellow);
                        }
                    }
                    else if (!curBAlist.SequenceEqual(chain6.ChainInputList))
                    {
                        foreach (FAChain chain5 in Chain5)
                        {
                            List<BaseAttack> check1 = curBAlist.GetRange(0, 5);
                            List<BaseAttack> check2 = curBAlist.GetRange(1, 5);

                            if (check1.SequenceEqual(chain5.ChainInputList))
                            {
                                //evaluate costs of the chain
                                foreach (Attack attack in chain6.NeededFeralArts)
                                {
                                    RPCostSum += attack.RPCost;
                                }

                                //see if player has enough RP
                                if (baeffectshandler.curPlayerRP >= RPCostSum)
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        BAsToDelete.Add(i);
                                        print("i: " + i);
                                    }

                                    arenaui.VisializeFAs(BAsToDelete, Color.yellow);
                                }
                            }
                            else if (check2.SequenceEqual(chain5.ChainInputList))
                            {
                                //evaluate costs of the chain
                                foreach (Attack attack in chain6.NeededFeralArts)
                                {
                                    RPCostSum += attack.RPCost;
                                }

                                //see if player has enough RP
                                if (baeffectshandler.curPlayerRP >= RPCostSum)
                                {
                                    for (int i = 1; i < 6; i++)
                                    {
                                        BAsToDelete.Add(i);
                                        print("i: " + i);
                                    }

                                    arenaui.VisializeFAs(BAsToDelete, Color.yellow);
                                }
                            }
                            else
                            {
                                print("not a chain, checking next");
                            }
                        }
                        print("no chains found");
                        CompareLists();
                    }
                }
                
                break;
            default:
                Debug.LogError("Wrong no of attack slots! Must be 5 or 6");
                CompareLists();
                break;
        }
    }

    //Checking for unchained FA
    public void CompareLists()
    {
    startFAloop:
        while (FANo < LoadedFeralArts.Count)
        {
            print("cur FA: " + LoadedFeralArts[FANo].name);

            //skip FA if there is not enough slots for it to happen
            if (LoadedFeralArts[FANo].FeralArtInput.Count > (AttackList.Count - Pos)) //inputbarhandler.maxBaseAttackInputSlots - Pos
            {
                FANo += 1;
                goto startFAloop;
            }

            for (InputNo = 0; InputNo < LoadedFeralArts[FANo].FeralArtInput.Count; InputNo++)
            {
                print("checking fa "+ LoadedFeralArts[FANo]);

                if (LoadedFeralArts[FANo].FeralArtInput[InputNo] == AttackList[InputNo + Pos]) //curBAlist[InputNo + Pos])
                {
                    //check next input
                    print("positive, checking next input");
                    //save BA pos in list -> int list
                    BAsToDelete.Add(InputNo + Pos);

                }
                else
                {
                    //check next FA
                    print("negative, checking next FA");
                    BAsToDelete.Clear();
                    InputNo = 0;
                    FANo += 1;
                    goto startFAloop;
                }
            }

            //If it is a feral art
            if (BAsToDelete.Count == LoadedFeralArts[FANo].FeralArtInput.Count)
            {
                //checking if player has enough RP
                if (baeffectshandler.curPlayerRP >= LoadedFeralArts[FANo].RPCost)
                {
                    print("FA found, adding " + LoadedFeralArts[FANo].name + " to list");
                    print("BAs to delete: " + BAsToDelete[0] +BAsToDelete[1] +BAsToDelete[2]);

                    for (int i = BAsToDelete.Count; i > 0; i--)
                        AttackList.RemoveAt(BAsToDelete[i - 1]);

                    AttackList.Insert(BAsToDelete[0], LoadedFeralArts[FANo]);

                    //check if it is the super fa which can only be used once
                    if (LoadedFeralArts[FANo] == GameStateSwitch.Instance.curProfile.SuperDuperFA)
                    {
                        superFAused = true;
                    }

                    //horrible hack um bei 2 3er FAs und 6 Input Slots beide farblich korrekt zu markieren
                        if (AttackList.Count == 2)
                    {
                        BAsToDelete.Clear();
                        BAsToDelete.Add(0);
                        BAsToDelete.Add(1);
                        BAsToDelete.Add(2);
                        BAsToDelete.Add(3);
                        BAsToDelete.Add(4);
                        BAsToDelete.Add(5);

                        arenaui.VisializeFAs(BAsToDelete, Color.magenta);
                    }
                    else
                        arenaui.VisializeFAs(BAsToDelete, Color.magenta); 

                    Pos += 1;   //BAsToDelete.Count;
                    FANo = 0;
                }
                else
                {
                    BAsToDelete.Clear();
                    print("no FA found, next FA?");
                    FANo += 1;
                }
            }
            else
            {
                BAsToDelete.Clear();
                print("no FA found, next FA?");
                FANo += 1;
            }
        }

        if (inputbarhandler.maxBaseAttackInputSlots == 5 && Pos < 3 || inputbarhandler.maxBaseAttackInputSlots == 6 && Pos < 4) // && FANo < LoadedFeralArts.Count
        {
            print("5 slots, pos = " + Pos + " , keep running FA check");            
            FANo = 0;
            Pos += 1;
            goto startFAloop;
        }
        else
        {
            print("do not run FA check anymore");
            return;
        }
    }    

    ////loop through LoadedFAlist
    //startFAloop:  for (int FANo = 0; FANo <= LoadedFeralArts.Count; FANo++)
    //{
    //    print("start loop through FA, cur at: "+ LoadedFeralArts[FANo]);
    //    //loop through BA Input of each FA
    //    for (int InputNo = 0; InputNo <= LoadedFeralArts[FANo].FeralArtInput.Count; InputNo++)
    //    {
    //        print("looping through curFA, cur at Input no: "+ InputNo);

    //        if (LoadedFeralArts[FANo].FeralArtInput[InputNo] == curBAlist[InputNo])
    //        {
    //            //check next input
    //            print("positive, checking next input");
    //            BAsToDelete.Add(InputNo);
    //            //save BA no -> int list
    //        }
    //        else
    //        {
    //            //check next FA
    //            print("negative, checking next FA");
    //            BAsToDelete.Clear();
    //            InputNo = 0;
    //            goto startFAloop;
    //        }
    //    }

    //[Tooltip("Feral arts available in this fight")]
    //[SerializeField] List<FeralArt> OriginalFALoadout = new List<FeralArt>();
    //public List<FeralArt> PossibleFeralArtList = new List<FeralArt>();
    //[Tooltip("Extra FA available at <25% HP for recoverability")]
    //public FeralArt RecoveryFA;
    ////[Tooltip("base attacks the player is putting in")]
    ////public List<BaseAttack> CurPlayerInput = new List<BaseAttack>();
    //public List<Attack> AttackOutput = new List<Attack>();

    ////Sets startpoint of the loop if a feral art has been input
    //[SerializeField] int StartPos = 0;
    ////[SerializeField] int curOutputAttackCount = 0;
    //[SerializeField] BaseAttack curPlayerInput;
    //[SerializeField] BaseAttack curFeralArtAttack;

    ////gets input from the loadout screen at the beginning of the fight, where player chooses his feral arts
    //public void FeralArtLoadout(List<FeralArt> LoadedFeralArts)
    //{
    //    OriginalFALoadout = LoadedFeralArts;
    //    LoadedFeralArts.Add(RecoveryFA);
    //    ResetFACounters();
    //}

    ////called if player input is resetted
    //public void ResetAll()
    //{
    //    StartPos = 0;
    //    ResetFACounters();
    //    //CurPlayerInput.Clear();
    //    AttackOutput.Clear();
    //    //curOutputAttackCount = 0;
    //}

    ////resets all counters
    //public void ResetFACounters()
    //{
    //    PossibleFeralArtList = OriginalFALoadout;

    //    foreach (FeralArt feralart in PossibleFeralArtList)
    //    {
    //        feralart.Counter = 0;
    //    }
    //}

    ////saves attack output exchanging BAs for FAs
    //public void SaveOutput(Attack attack)
    //{
    //    AttackOutput.Add(attack);

    //}

    //public void CheckForFeralArt(List<BaseAttack> CurPlayerInput)
    //{      

    //    #region everything that sucks
    //    //if (CurPlayerInput.Count < 2)
    //    //    return;

    //    ////while (StartPos < CurPlayerInput.Count)
    //    ////{
    //    ////Loop as many times as there are BAs in the input bar
    //    //for (int i = 0; i < CurPlayerInput.Count - StartPos; i++)
    //    //{
    //    //    //print("starting BA input loop, i: " + i);

    //    //    curPlayerInput = CurPlayerInput[i + StartPos];

    //    //    //loop through all FA for round i
    //    //    for (int n = 0; n < PossibleFeralArtList.Count; n++)
    //    //    {
    //    //        //print("check " + PossibleFeralArtList[n].name + "?");

    //    //        //If FA is the same or smaller as the input
    //    //        if (PossibleFeralArtList[n].FeralArtInput.Count <= CurPlayerInput.Count - StartPos)
    //    //        {
    //    //            print("checking " + PossibleFeralArtList[n].name);
    //    //            curFeralArtAttack = PossibleFeralArtList[n].FeralArtInput[i]; // - StartPos

    //    //            //compare the BAs
    //    //            CompareBAs(n);

    //    //            //check if counter is full => FA
    //    //            if (FACheck(n))
    //    //                return;

    //    //            //Check if current FA is not possible
    //    //            if (PossibleFeralArtList[n].Counter == 0)
    //    //            {
    //    //                //... and remove it from the list
    //    //                print("removing " + PossibleFeralArtList[n]);
    //    //                PossibleFeralArtList.Remove(PossibleFeralArtList[n]);
    //    //            }

    //    //            //Check if there are no possible FAs left
    //    //            if (PossibleFeralArtList.Count == 0)
    //    //            {
    //    //                //...save curAttack and set StartPos +1
    //    //                SaveOutput(curPlayerInput);
    //    //                StartPos += 1;
    //    //                PossibleFeralArtList = OriginalFALoadout;
    //    //            }
    //    //        }
    //    //        print("n: " + n + "; i: " + i);
    //    //    }
    //    //    //}
    //    //    //StartPos += 1;
    //    //}
    //    #endregion
    //}

    //void FAPossibiltyCheck()
    //{

    //}

    //void CompareBAs(int i)
    //{
    //    print("Feral Art Input: " + curFeralArtAttack + " Player Input: " + curPlayerInput);

    //    if (curPlayerInput == curFeralArtAttack)    //if they are the same, increase FA counter
    //    {
    //        PossibleFeralArtList[i].Counter += 1;
    //    }
    //    else                                        //if not, reset FA counter to zero
    //    {
    //        print("removing: " + PossibleFeralArtList[i].name);
    //        PossibleFeralArtList.Remove(PossibleFeralArtList[i]);
    //    }

    //}

    //bool FACheck(int n)
    //{
    //    if (PossibleFeralArtList[n].Counter == PossibleFeralArtList[n].FeralArtInput.Count)
    //    {
    //        print("!!! Player has input the art: " + PossibleFeralArtList[n].name);

    //        SaveOutput(PossibleFeralArtList[n]);
    //        StartPos += PossibleFeralArtList[n].Counter - 1;
    //        PossibleFeralArtList = OriginalFALoadout;
    //        ResetFACounters();
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}



    //#region lots of bullshit
    ////public void SaveFeralArtLoadout(List<FeralArt> LoadedFeralArts)
    ////{
    ////foreach (FeralArt feralart in LoadedFeralArts)
    ////{
    ////    switch (feralart.FeralArtInput.Count)
    ////    {
    ////        case 3:
    ////            ThreeSlotFAs.Add(feralart);
    ////            break;
    ////        case 4:
    ////            FourSlotFA.Add(feralart);
    ////            break;
    ////        case 5:
    ////            FiveSlotFA.Add(feralart);
    ////            break;
    ////        case 6:
    ////            SixSlotFA.Add(feralart);
    ////            break;
    ////    }
    ////}

    ////foreach (FeralArt feralart in ThreeSlotFAs)
    ////    print("3er:" + feralart.name);

    ////foreach (FeralArt feralart in FourSlotFA)
    ////    print("3er:" + feralart.name);

    ////foreach (FeralArt feralart in FiveSlotFA)
    ////    print("3er:" + feralart.name);

    ////foreach (FeralArt feralart in SixSlotFA)
    ////    print("3er:" + feralart.name);

    ////}

    ////call this function once there are >= 3 BA in the input bar -> retrieve current input
    ////public void CheckForFeralArt2(List<BaseAttack> CurPlayerInput)
    ////{
    ////int BAinputs = curPlayerInput.Count;
    //////List<BaseAttack> curFeralArtInput = new List<BaseAttack>();
    ////curPlayerInput = CurPlayerInput;

    //////bool sequenceFound = false;
    //////BaseAttack[] PlayerInput = curPlayerInput.ToArray();
    //////FeralArt[] LoadedFAs = LoadedFeralArts.ToArray();
    //////int i = BAinputs-1;
    //////bool feralTreedown;

    ////switch (BAinputs)
    ////{
    ////    case 3:

    ////        if (ThreeSlotFAs == null)
    ////        {
    ////            print("no 3er feral arts loaded");
    ////            return;
    ////        }

    ////        foreach (FeralArt feralart in ThreeSlotFAs)
    ////        {
    ////            print("looping through feral art list");
    ////            for (loop1 = 0; loop1 < feralart.FeralArtInput.Count; loop1++)
    ////            {
    ////                curFeralArtInput.Add(feralart.FeralArtInput[loop1]);

    ////                print("checking: " + feralart.name);

    ////                for (loop2 = 0; loop2 < curFeralArtInput.Count; loop2++)
    ////                {
    ////                    if (curPlayerInput[loop2] != curFeralArtInput[loop2])
    ////                    {
    ////                        print(feralart.name+" is no match, checking next");
    ////                        break;
    ////                    }
    ////                    else if (curPlayerInput[curFeralArtInput.Count-1] == curFeralArtInput[curFeralArtInput.Count - 1])
    ////                    {
    ////                        print(feralart.name + " is equal to the player input!");
    ////                        break;
    ////                    }
    ////                }



    ////                for (loop2 = 0; loop2 < curPlayerInput.Count - curFeralArtInput.Count; loop2++)
    ////                {
    ////                    if (curPlayerInput.Skip(loop2).Take(curFeralArtInput.Count).SequenceEqual(curFeralArtInput))
    ////                    {
    ////                        print(feralart.name + " is equal to the player input!");
    ////                        break;
    ////                    }
    ////                    else
    ////                    {
    ////                        print(feralart.name + " is no match, checking next feralart");
    ////                        break;
    ////                    }
    ////                }

    ////            }


    ////        }

    ////        //---------------------------------------------

    ////        //    for (int i = 0; i < curPlayerInput.Count - feralArt.FeralArtInput.Count; i++)
    ////        //    {
    ////        //        if (curPlayerInput.Skip(i).Take(feralArt.FeralArtInput.Count).SequenceEqual(LoadedFeralArts))
    ////        //        {
    ////        //            sequenceFound = true;
    ////        //            break;
    ////        //        }
    ////        //    }



    ////        //if (sequenceFound)
    ////        //{
    ////        //    //sequence found
    ////        //}
    ////        //else
    ////        //{
    ////        //    //sequence not found
    ////        //}

    ////        //---------------------------------------

    ////        //---------------------------------------
    ////        //print("looping through the feral art list");
    ////        ////check every feral art
    ////        //foreach (FeralArt feralArt in LoadedFeralArts)
    ////        //{
    ////        //    feralTreedown = true;

    ////        //    if (feralArt.FeralArtInput.Count > BAinputs)
    ////        //    {
    ////        //        print("feral art " + feralArt.name + " is longer than " + BAinputs);
    ////        //        break;
    ////        //    }

    ////        //    while (feralTreedown == true)
    ////        //    {
    ////        //        if (feralArt.FeralArtInput[i] == curPlayerInput[i])
    ////        //        {
    ////        //            if (i < 0)
    ////        //            {
    ////        //                print("player input = feral art " + feralArt.name);
    ////        //                return;
    ////        //            }
    ////        //            else
    ////        //            {
    ////        //                i--;
    ////        //                print("playerinput: " + curPlayerInput[i].name + " = Feral Art " + feralArt.name + " input");
    ////        //                feralTreedown = true;
    ////        //            }
    ////        //        }
    ////        //        else
    ////        //        {
    ////        //            print("this is not a feral art");
    ////        //            feralTreedown = false;
    ////        //            break;
    ////        //        }
    ////        //        return;
    ////        //    }             
    ////        //}                
    ////        //---------------------------------------

    ////        break;
    ////    case 4:
    ////        break;
    ////    case 5:
    ////        break;
    ////    case 6:
    ////        break;
    ////}

    ////}
    //#endregion

}
