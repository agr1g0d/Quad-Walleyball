using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Frame : MonoBehaviour
{
    [SerializeField] private float _maxAlpha;
    [SerializeField] private Image _frame;

    IEnumerator SmootheEnterAndExit()
    {
        for (float f = 0; f < _maxAlpha; f += Time.deltaTime)
        {
            _frame.color = new Color(_frame.color.r, _frame.color.g, _frame.color.b, f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        for (float f = _maxAlpha; f > 0 + 0.001f; f = Mathf.Lerp(f, 0, Time.deltaTime * 3))
        {
            _frame.color = new Color(_frame.color.r, _frame.color.g, _frame.color.b, f);
            yield return null;
        }
    }

    public void FrameApearence()
    {
        StartCoroutine(SmootheEnterAndExit());
    }
}
