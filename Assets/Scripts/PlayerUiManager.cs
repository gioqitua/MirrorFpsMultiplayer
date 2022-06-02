using UnityEngine;
using TMPro;

public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] TMP_Text damageTextPrefab;
    float textActiveTime = 1f;

    public void CreateDmgText(float damage, Vector3 targetPos, Vector3 shooterPos)
    {
        var dmgText = Instantiate(damageTextPrefab, targetPos, Quaternion.LookRotation((targetPos - shooterPos), Vector3.up));
        dmgText.SetText(damage.ToString());
    }
}
