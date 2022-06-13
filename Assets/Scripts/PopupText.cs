using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    float speed = 10f;

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }

}
