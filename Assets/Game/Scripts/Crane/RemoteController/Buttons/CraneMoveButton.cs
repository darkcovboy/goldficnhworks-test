using System;
using System.Collections;
using Game.Scripts.Crane.Signals;
using Game.Scripts.Test;
using TMPro;
using UnityEngine;
using Zenject;
using SignalBus = Game.Scripts.Crane.Signals.SignalBus;

namespace Game.Scripts.Crane.RemoteController.Buttons
{
    public class CraneMoveButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private MoveDirection _direction;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Transform _buttonVisual;
        
        private float _pressDepth = 0.001f;
        private float _pressDuration = 0.1f;
        private Vector3 _defaultLocalPos;
        private Coroutine _animationCoroutine;

        private SignalBus _bus;

        [Inject]
        public void Construct(SignalBus bus)
        {
            _bus = bus;
        }

        private void Start()
        {
            _label.text = _direction.ToString();
        }

        public void Interact()
        {
            _bus.Fire(new MoveSignal
            {
                Direction = _direction,
                Intensity = 1f
            });
            
            StartPressAnimation(true);
        }

        public void Release()
        {
            _bus.Fire(new MoveSignal
            {
                Direction = _direction,
                Intensity = 0f
            });
            
            StartPressAnimation(false);
        }
        
        private void StartPressAnimation(bool pressed)
        {
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(AnimatePress(pressed));
        }

        private IEnumerator AnimatePress(bool pressed)
        {
            Vector3 startPos = _buttonVisual.localPosition;
            Vector3 targetPos = pressed 
                ? _defaultLocalPos + Vector3.back * _pressDepth 
                : _defaultLocalPos;

            float elapsed = 0f;
            while (elapsed < _pressDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _pressDuration);
                _buttonVisual.localPosition = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }

            _buttonVisual.localPosition = targetPos;
        }

    }
}