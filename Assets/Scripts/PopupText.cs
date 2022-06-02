using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    float lifeTime = 1f;
    float speed = 10f;
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }

}
