using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutScreen : MonoBehaviour
{
    public eImplant ChosenImplant;
    public List<FeralArt> AllFeralArts;
    public List<FeralArt> ChosenFeralArts;

    public Animator FAChoiceAnim;
    public Animator ImplantChoiceAnim;

    //fade out faChoiceBox/implantChoicebox
    //fade in faChoiceBox wenn fa slot selected
    //fade in implantChoicebox wenn implant slot selected
    //füll slot mit ausgewählter fa/implant bei A, fade out extra window
    //fade out und slot nicht ausfüllen bei B
    //start nur möglich wenn alle slots gefüllt sind
    //keine doppelten fas, button disablen wenn ausgewählt wurde
    //fa button enablend, wenn fa überschrieben wurde 
}
