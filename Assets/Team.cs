using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team : MonoBehaviour
{
    public float Score;
    public float MaxScore;
    public int Charge;
    public int MaxCharge;
    public bool Charged;
    public bool Touch;
    [SerializeField] private TypeInput _input;
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _conter;
    private KeyCode _chargedHit;

    private void Start()
    {
        if (_input == TypeInput.arrows)
        {
            _chargedHit = KeyCode.DownArrow;
        } else
        {
            _chargedHit = KeyCode.S;
        }
    }

    public void Bounce()
    {
        if (Charge < MaxCharge)
        {
            StartCoroutine(SmoothSlider(Charge, ++Charge));
        }
    }

    public void Goal()
    {
        if (Score < MaxScore)
        {
            Score++;
            _conter.text = Score.ToString();
        }
    }

    private void Update()
    {
        if (Charge == MaxCharge)
        {
            if (Input.GetKeyDown(_chargedHit))
            {
                if (Charged == false)
                {
                    Charged = true;
                }
                else
                {
                    Charged = false;
                }
            }
        }
    }

    public void RefreshCharge()
    {
        Charge = 0;
        StartCoroutine(SmoothSlider(MaxCharge, 0));
    }

    private IEnumerator SmoothSlider(float from, float to)
    {
        if (from < to)
        {
            for (float f = from; f < to; f += Time.deltaTime * 3)
            {
                _slider.value = f;
                yield return null;
            }
        } else
        {
            for (float f = from; f > to; f -= Time.deltaTime * 3)
            {
                _slider.value = f;
                yield return null;
            }
        }
        _slider.value = to;
    }
}
