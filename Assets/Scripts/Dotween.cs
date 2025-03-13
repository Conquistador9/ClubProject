using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class Dotween : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _button;
    [SerializeField] private CanvasGroup _text;

    private void Start()
    {
        _text.DOFade(1, 2f).From(0);
        _button.DOScale(4, 1.18f).From(0).SetEase(Ease.OutBounce);
    }

    public void StartGame()
    {
        _button.DOScale(0, 0.2f);
        _text.DOFade(0, 0.3f);
        StartCoroutine(GameScene());
    }

    private IEnumerator GameScene()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(1);
    }
}