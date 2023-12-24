using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    [SerializeField] private Transform _ballTransform;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, _ballTransform.position.y, transform.position.z);
    }
}
