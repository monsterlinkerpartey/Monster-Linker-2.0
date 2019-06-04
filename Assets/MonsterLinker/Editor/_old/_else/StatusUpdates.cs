using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpdates : MonoBehaviour
{
    //    public ArenaManager AM;

    //    [SerializeField] bool HPIncreaseLerp;
    //    [SerializeField] bool HPDecreaseLerp;


    //    private void OnEnable()
    //    {
    //        AM = ArenaManager.Instance;
    //        UpdateHP();
    //    }

    //    void Update()
    //    {
    //        if (HPDecreaseLerp)
    //        {
    //                AM.PlayerHealthBarTick.fillAmount = Mathf.Lerp(AM.PlayerHealthBarTick.fillAmount, AM.curPlayerHealth, Time.deltaTime * 3);
    //                AM.EnemyHealthBarTick.fillAmount = Mathf.Lerp(AM.EnemyHealthBarTick.fillAmount, AM.curEnemyHealth, Time.deltaTime * 3);

    //        }
    //    }

    //    public void UpdateHP()
    //    {
    //        AM.curPlayerHealth = (float)AM.PlayerHealth / (float)AM.PlayerMaxHealth;
    //        AM.curEnemyHealth = (float)AM.EnemyHealth / (float)AM.EnemyMaxHealth;

    //        if (AM.curPlayerHealth < 0.0f)
    //            AM.curPlayerHealth = 0.0f;

    //        if (AM.curEnemyHealth < 0.0f)
    //            AM.curEnemyHealth = 0.0f;

    //        AM.PlayerLifeCounter.text = "" + AM.PlayerHealth;
    //        AM.EnemyLifeCounter.text = "" + AM.EnemyHealth;

    //        AM.PlayerHealthBarFinal.fillAmount = AM.curPlayerHealth; //Mathf.Lerp(AM.PlayerHealthBarFinal.fillAmount, AM.curPlayerHealth, Time.deltaTime * 3);
    //        AM.EnemyHealthBarFinal.fillAmount = AM.curEnemyHealth; //Mathf.Lerp(AM.EnemyHealthBarFinal.fillAmount, AM.curEnemyHealth, Time.deltaTime * 3);

    //        HPDecreaseLerp = true;
    //    }
}
