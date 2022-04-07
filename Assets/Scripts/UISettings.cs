using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UISettings : MonoBehaviour
{
    [SerializeField] private float seconds;

    [SerializeField] private Image spriteRendererPaddle;
    [SerializeField] private Image spriteRendererBall;

    private Coroutine _paddleCoroutine;
    private Coroutine _ballCoroutine;

    private void Start()
    {
        Color[] colors = PersistenceManager.Instance.Colors;
        spriteRendererPaddle.color = colors[0];
        spriteRendererBall.color = colors[1];
    }

    public void PaddleColorChange()
    {
        if (_paddleCoroutine != null)
        {
            StopCoroutine(_paddleCoroutine);
        }
        _paddleCoroutine = StartCoroutine(ChangeColor(spriteRendererPaddle));
    }

    public void BallColorChange()
    {
        if (_ballCoroutine != null)
        {
            StopCoroutine(_ballCoroutine);
        }
        _ballCoroutine = StartCoroutine(ChangeColor(spriteRendererBall));
    }

    private IEnumerator ChangeColor(Graphic spriteRenderer)
    {
        var randomColor = new Color(Random.value, Random.value, Random.value);
        float time = 0f;
        while (time <= seconds)
        {
            var newColor = Color.Lerp(spriteRenderer.color, randomColor, time / seconds);
            spriteRenderer.color = newColor;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            print(time);
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
        PersistenceManager.Instance.SetColors(new[]{spriteRendererPaddle.color, spriteRendererBall.color});
    }
}