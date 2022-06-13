using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] TMP_Text damageTextPrefab;


    public void CreateDmgText(float damage, Vector3 targetPos, Vector3 shooterPos)
    {
        var popup = ObjectPool.Instance.GetObjFromPool();
        popup.transform.position = targetPos;
        popup.transform.rotation = Quaternion.LookRotation((targetPos - shooterPos), Vector3.up);
        popup.gameObject.GetComponent<TMP_Text>().SetText(damage.ToString());
        popup.SetActive(true);
        StartCoroutine(Hide(popup));
    }

    IEnumerator Hide(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }

}
