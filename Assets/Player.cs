using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Team Team;

    [SerializeField] private TypeInput _input;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ConfigurableJoint _configurableJoint;
    [SerializeField] private ParticleSystem _chargeEffect;
    [SerializeField] private Transform _toDirection;
    [SerializeField] private Transform _chargedHitToPoint;
    [SerializeField] private float _torque;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _bounceCoeff;

    private bool _grounded;
    private bool _emmiting;
    private KeyCode _jump;
    private KeyCode _right;
    private KeyCode _left;
    private KeyCode _chargedHit;
    private void Start()
    {
        _rigidbody.maxAngularVelocity = 999f;
        if (_input == TypeInput.wasd)
        {
            _jump = KeyCode.W;
            _right = KeyCode.D;
            _left = KeyCode.A;
            _chargedHit = KeyCode.S;
        }
        else
        {
            _jump = KeyCode.UpArrow;
            _right = KeyCode.RightArrow;
            _left = KeyCode.LeftArrow;
            _chargedHit = KeyCode.DownArrow;
        }
    }


    private void FixedUpdate()
    {
        if (Input.GetKey(_left))
        {
            _rigidbody.AddTorque(Vector3.forward * _torque * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        if (Input.GetKey(_right))
        {
            _rigidbody.AddTorque(-Vector3.forward * _torque * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        

    }

    private void Update()
    {
        if (Input.GetKeyDown(_jump) && _grounded)
        {
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.VelocityChange);
        }

        if (Team.Charged)
        {
            if (!_emmiting)
            {
                _emmiting = true;
                _chargeEffect.Play();
            }
        }
        else
        {
            _chargeEffect.Stop();
            _emmiting = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Team.Touch)
        {
            if (collision.collider.TryGetComponent(out Ball ball))
            {
                Team.Touch = true;
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.AddForce((_toDirection.position - collision.contacts[0].point).normalized * _bounceCoeff, ForceMode.VelocityChange);
                Team.Bounce();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Vector3.Angle(collision.contacts[0].normal, Vector3.up) < 45)
        {
            _grounded = true;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _grounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Team.Charged)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.AddForce((_chargedHitToPoint.position - other.transform.position).normalized * _bounceCoeff * 1.75f, ForceMode.VelocityChange);
                ball.SuperShot(Team);
                Team.Charged = false;
                Team.RefreshCharge();
            }
            Team.Touch = true;
        }
    }
}
public enum TypeInput
{
    wasd,
    arrows
}
