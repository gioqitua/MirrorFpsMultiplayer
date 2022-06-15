using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] float lifeTime = 0.01f;
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }


}
