using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.GazAnalyzer.Display
{
    public class GasAnalyzerDisplay : MonoBehaviour
    {
        [Header("UI Elements")] [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _gasValueText;
        [SerializeField] private Image _gasFillImage;

        [Header("Colors")] [SerializeField] private Color _safeColor = Color.green;
        [SerializeField] private Color _warningColor = Color.yellow;
        [SerializeField] private Color _dangerColor = Color.red;

        [Header("Animation")] [SerializeField] private float _fadeDuration = 0.5f;

        private Coroutine _fadeRoutine;

        public void Show()
        {
            if (_fadeRoutine != null)
                StopCoroutine(_fadeRoutine);

            _fadeRoutine = StartCoroutine(FadeCanvas(1f));
        }

        public void Hide()
        {
            if (_fadeRoutine != null)
                StopCoroutine(_fadeRoutine);

            _fadeRoutine = StartCoroutine(FadeCanvas(0f));
        }

        public void SetDistance(float distance)
        {
            _distanceText.text = $"Дистанция: {distance:F2} m";
        }

        public void SetGasLevel(float level)
        {
            _gasFillImage.fillAmount = level;
            _gasValueText.text = $"{Mathf.RoundToInt(level * 100)}%";

            _gasFillImage.color = level switch
            {
                <= 0.4f => _safeColor,
                <= 0.7f => _warningColor,
                _ => _dangerColor
            };
        }

        private IEnumerator FadeCanvas(float targetAlpha)
        {
            float start = _canvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(start, targetAlpha, elapsed / _fadeDuration);
                yield return null;
            }

            _canvasGroup.alpha = targetAlpha;
        }
    }
}