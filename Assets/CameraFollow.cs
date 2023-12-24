using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _coeff;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _coeff);
    }

}
