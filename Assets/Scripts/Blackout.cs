using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Blackout : MonoBehaviour
{
    private Image _image;
    private float _speed = 3f;

    private void Start()
    {
        _image = GetComponent<Image>();
        StartCoroutine(ScreenBlackout());
    }

    private IEnumerator ScreenBlackout()
    {
        Color color = _image.color;

        while (color.a < 1f)
        {
            color.a += _speed * Time.deltaTime;
            _image.color = color;
            yield return null;
        }
    }
}