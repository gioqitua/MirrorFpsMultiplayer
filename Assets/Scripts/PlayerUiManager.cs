using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] TMP_Text OutgoingDamageText;


    public void SetOutGoingDmgText(float dmgTxt)
    {
        OutgoingDamageText.SetText(dmgTxt.ToString());
      //  StartCoroutine(SetText(dmgTxt));
    }

   
}
