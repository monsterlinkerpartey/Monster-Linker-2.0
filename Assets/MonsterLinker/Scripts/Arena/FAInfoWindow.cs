using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FAInfoWindow : MonoBehaviour
{
    public List<GameObject> FASlots;
    public List<Implant> availableImplants = new List<Implant>(4);

    public GameObject SI;
    public GameObject Icon;

    public void WriteFAData()
    {
        List<FeralArt> FAs = GameStateSwitch.Instance.curProfile.FALoadout;

        for (int i = 0; i < FAs.Count; i++)
        {
            Text Name = FASlots[i].GetComponentInChildren<FaNameField>().GetComponent<Text>();
            Text Cost = FASlots[i].GetComponentInChildren<FaCostField>().GetComponent<Text>();
            Name.text = FAs[i].FAName;
            Cost.text = ""+FAs[i].RPCost;
                                 
            GameObject IconParent = FASlots[i].GetComponentInChildren<FaInputField>().gameObject;            

            //spawn input icons
            int n = 0;
            while (n < FAs[i].FeralArtInput.Count)
            {
                GameObject icon = GameObject.Instantiate(Icon, transform.position, transform.rotation) as GameObject;

                icon.transform.parent = IconParent.transform;
                icon.transform.localScale = new Vector3(1, 1, 1);

                Image iconImg = icon.GetComponent<Image>();
                iconImg.sprite = FAs[i].FeralArtInput[n].InfoPanelIcon;

                n += 1;
            }
        }
    }

    public void SetSI()
    {
        WriteSiData(GameStateSwitch.Instance.curProfile.curImplant);

        //switch (GameStateSwitch.Instance.curProfile.curImplant)
        //{
        //    case eImplant.None:
        //        Debug.LogError("No implant chosen?!");
        //        break;
        //    case eImplant.RisingRage:
        //        WriteSiData(availableImplants[0]);

        //        break;
        //    case eImplant.UnleashedMode:
        //        WriteSiData(availableImplants[1]);

        //        break;
        //    case eImplant.SuperFA:
        //        WriteSiData(availableImplants[2]);

        //        break;
        //    case eImplant.TempInputSlot:
        //        WriteSiData(availableImplants[3]);

        //        break;
        //}
    }

    public void WriteSFAData(FeralArt superFA)
    {
        Text Name = SI.GetComponentInChildren<FaNameField>().GetComponent<Text>();
        Text Cost = SI.GetComponentInChildren<FaCostField>().GetComponent<Text>();
        Name.text = superFA.FAName;

        GameObject IconParent = SI.GetComponentInChildren<FaInputField>().gameObject;

        //spawn input icons
        int n = 0;
        while (n < superFA.FeralArtInput.Count)
        {
            GameObject icon = GameObject.Instantiate(Icon, transform.position, transform.rotation) as GameObject;
            icon.transform.SetParent(IconParent.transform);
            icon.transform.localScale = new Vector3(1, 1, 1);

            Image iconImg = icon.GetComponent<Image>();
            iconImg.sprite = superFA.FeralArtInput[n].InfoPanelIcon;

            n += 1;
        }
    }

    public void WriteSiData(Implant curImplant)
    {
        Text Name = SI.GetComponentInChildren<FaNameField>().GetComponent<Text>();
        Text Cost = SI.GetComponentInChildren<FaCostField>().GetComponent<Text>();
        Name.text = curImplant.ImplantName;
        Cost.text = "" + curImplant.RPCost;

        if (curImplant.FAInput.Count > 0)
        {
            GameObject IconParent = SI.GetComponentInChildren<FaInputField>().gameObject;

            //spawn input icons
            int n = 0;
            while (n < curImplant.FAInput.Count)
            {
                GameObject icon = GameObject.Instantiate(Icon, transform.position, transform.rotation) as GameObject;
                icon.transform.SetParent(IconParent.transform);
                icon.transform.localScale = new Vector3(1, 1, 1);

                Image iconImg = icon.GetComponent<Image>();
                iconImg.sprite = curImplant.FAInput[n].InfoPanelIcon;

                n += 1;
            }
        }
    }
}