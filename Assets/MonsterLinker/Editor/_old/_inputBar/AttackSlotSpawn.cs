using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Spawns Players attack slots according to his available number of attacks
/// Sits on PlayerOutput
/// </summary>
public class AttackSlotSpawn : MonoBehaviour
{
    [SerializeField]
    private RectTransform PanelTransform;
    public GameObject Slot;
    public GameObject Frame;
    [SerializeField]
    private GridLayoutGroup grid;
    private int NumberOfAttackSlots;

    [SerializeField]
    private float PanelHeight;
    [SerializeField]
    private float PanelWidth;

    private void Awake()
    {
        PanelTransform = gameObject.GetComponent<RectTransform>();
        NumberOfAttackSlots = ArenaManager.Instance.MaxPlayerSlots; //TODO get attack number from arena manager
        grid = gameObject.GetComponent<GridLayoutGroup>();

        SpawnSlots();        
    }

    public void SpawnSlots()
    {
        //Checking grid size and adjusting panel size to it
        float cellsize = grid.cellSize.x;
        float spacing = grid.spacing.x;
        if (spacing == 0)
            spacing = 1;

        PanelWidth = ((NumberOfAttackSlots * cellsize) + (NumberOfAttackSlots + 1) * (spacing));
        PanelHeight = 2 * spacing + cellsize;
        PanelTransform.sizeDelta = new Vector2(PanelWidth, PanelHeight);

        int i = NumberOfAttackSlots;

        //Instantiting slots parented to the panel
        while (i > 0)
        {
            GameObject slot = GameObject.Instantiate(Slot, transform.position, transform.rotation) as GameObject;
            //HACK: Set Parent method???
            slot.transform.parent = this.gameObject.transform;
            slot.transform.localScale = new Vector3(2, 2, 1);
            i -= 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}


