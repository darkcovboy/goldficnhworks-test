using System;
using System.Collections;
using Game.Scripts.Test;
using UnityEngine;
using Zenject;

namespace Game.Scripts.GazAnalyzer.Button
{
    public class GasAnalyzerButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private float _holdTime = 3f;
        [SerializeField] private Renderer _indicatorRenderer;
        [SerializeField] private Color _idleColor = Color.gray;
        [SerializeField] private Color _progressColor = Color.green;

        private GasAnalyzerController _controller;
        private Coroutine _holdRoutine;
        private Material _indicatorMaterial;
        private float _progress;

        [Inject]
        public void Construct(GasAnalyzerController controller)
        {
            _controller = controller;
        }

        private void Awake()
        {
            if (_indicatorRenderer != null)
                _indicatorMaterial = _indicatorRenderer.material;
        }

        public void Interact()
        {
            _holdRoutine ??= StartCoroutine(HoldRoutine());
        }

        public void Release()
        {
            if (_holdRoutine != null)
            {
                StopCoroutine(_holdRoutine);
                _holdRoutine = null;
            }

            ResetIndicator();
        }

        private IEnumerator HoldRoutine()
        {
            _progress = 0f;

            while (_progress < _holdTime)
            {
                _progress += Time.deltaTime;
                UpdateIndicator(_progress / _holdTime);
                yield return null;
            }

            _controller.TogglePower();
            ResetIndicator();
            _holdRoutine = null;
        }

        private void UpdateIndicator(float t)
        {
            if (_indicatorMaterial != null)
                _indicatorMaterial.color = Color.Lerp(_idleColor, _progressColor, t);
        }

        private void ResetIndicator()
        {
            _progress = 0f;
            if (_indicatorMaterial != null)
                _indicatorMaterial.color = _idleColor;
        }
    }
}