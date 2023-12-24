using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform StartPosition;
    [SerializeField] private Collider _hasToIgnore;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ParticleSystem _burningBallEffect;
    [SerializeField] private float _speedLowEnoughtToStopEffect;
    [SerializeField] private Animator _goalAnimator;
    [SerializeField] private Frame _frame;
    [SerializeField] private ParticleSystem RightRays;
    [SerializeField] private ParticleSystem RightConfetti;
    [SerializeField] private ParticleSystem LeftRays;
    [SerializeField] private ParticleSystem LeftConfetti;
    [SerializeField] private Team _teamRight;
    [SerializeField] private Team _teamLeft;

    private bool _burns;
    private Team _teamWhoShot;


    private void Start()
    {
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), _hasToIgnore);
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool ignore = false;
        if (_burns)
        {
            if (collision.collider.attachedRigidbody != null && collision.collider.attachedRigidbody.TryGetComponent(out Player p))
            {
                if (p.Team == _teamWhoShot)
                {
                    ignore = true;
                }
            }
        }
        if (collision.collider.attachedRigidbody != null && collision.collider.attachedRigidbody.TryGetComponent(out Player __))
        {
            if (!ignore)
            {
                _rigidbody.velocity = Vector3.zero;
                if (_burns)
                {
                    _burns = false;
                    _burningBallEffect.Stop();
                }
            }
            else if (_rigidbody.velocity.magnitude < _speedLowEnoughtToStopEffect)
            {
                if (_burns)
                {
                    _burns = false;
                    _burningBallEffect.Stop();
                }
            }
        }
        
        if (collision.collider.gameObject.TryGetComponent(out Ground _) ||
            collision.collider.gameObject.TryGetComponent(out Bounds _) ||
            collision.collider.gameObject.TryGetComponent(out StaticBounds _))
        {
            if (_burns)
            {
                _burns = false;
                _burningBallEffect.Stop();
            }
            if (collision.collider.gameObject.TryGetComponent(out Ground _))
            {
                StartCoroutine(Goal());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.attachedRigidbody != null && collision.collider.attachedRigidbody.TryGetComponent(out Player p))
        {
            p.Team.Touch = false;
        }
    }

    public void SuperShot(Team teamWho)
    {
        _burns = true;
        _burningBallEffect.Play();
        _teamWhoShot = teamWho;
    }

    IEnumerator Goal()
    {
        _frame.FrameApearence();
        _goalAnimator.SetTrigger("goal");
        if (transform.position.x > 0)
        {
            _teamLeft.Goal();
            LeftConfetti.Play();
            LeftRays.Play();
        } else
        {
            RightConfetti.Play();
            RightRays.Play();
            _teamRight.Goal();
        }
        transform.position = StartPosition.position;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        yield  return new WaitForSeconds(3);
        _rigidbody.isKinematic = false;

        int direction;
        while (true)
        {
            direction = Random.Range(-1, 2);
            if (direction != 0)
            {
                break;
            }
        }
        _rigidbody.AddForce(Vector3.right * direction * 4f, ForceMode.VelocityChange);
    }
}
