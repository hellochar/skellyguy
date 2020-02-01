using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
