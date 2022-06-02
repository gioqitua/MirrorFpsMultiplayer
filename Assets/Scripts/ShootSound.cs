using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ShootSound : NetworkBehaviour
{

    void Start()
    {
        Destroy(this.gameObject, 1f); 
    }

}
