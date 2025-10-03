using Game.Scripts.Crane.Signals;
using UniRx;
using UnityEngine;
using Zenject;
using SignalBus = Game.Scripts.Crane.Signals.SignalBus;

namespace Game.Scripts.Crane.Mover
{
    public class CraneMover : MonoBehaviour
    {
        [Header("Части для движения")] [SerializeField] private Transform _hook;
        [SerializeField] private Transform _basePart;
        [SerializeField] private Transform _crane;
        [SerializeField] private Transform _reel;

        [Header("Скорость движения")] [SerializeField] private float _upSpeed = 2f;
        [SerializeField] private float _downSpeed = 2f;
        [SerializeField] private float _northSpeed = 3f;
        [SerializeField] private float _southSpeed = 3f;
        [SerializeField] private float _eastSpeed = 3f;
        [SerializeField] private float _westSpeed = 3f;

        [Header("Лимиты для движения")] [SerializeField] private Vector2 _limitX = new Vector2(-5f, 5f);
        [SerializeField] private Vector2 _limitY = new Vector2(0f, 5f);
        [SerializeField] private Vector2 _limitZ = new Vector2(-5f, 5f);
        
        [Header("Настройки катушки")]
        [SerializeField] private float _reelRotationSpeed = 200f;
        [SerializeField] private AudioSource _reelAudio;

        private Vector3 _hookVelocity;
        private Vector3 _baseVelocity;
        private Vector3 _craneVelocity;
        private SignalBus _signalBus;

        private readonly CompositeDisposable _disposables = new();

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.OnMove
                .Subscribe(OnMove)
                .AddTo(_disposables);
        }

        private void OnDisable()
        {
            _disposables.Clear();
        }

        private void Update()
        {
            MoveHook();
            MoveBase();
            MoveCrane();
            RotateReel();
        }

        private void RotateReel()
        {
            if (Mathf.Abs(_hookVelocity.y) > 0.01f)
            {
                float dir = Mathf.Sign(_hookVelocity.y);
                _reel.Rotate(Vector3.right * dir * _reelRotationSpeed * Time.deltaTime);

                if (!_reelAudio.isPlaying)
                    _reelAudio.Play();
            }
            else
            {
                if (_reelAudio.isPlaying)
                    _reelAudio.Stop();
            }

        }

        private void MoveHook()
        {
            if (_hook == null) return;

            Vector3 newPos = _hook.localPosition + _hookVelocity * Time.deltaTime;
            newPos.y = Mathf.Clamp(newPos.y, _limitY.x, _limitY.y);
            _hook.localPosition = newPos;
        }

        private void MoveBase()
        {
            if (_basePart == null) return;

            Vector3 newPos = _basePart.localPosition + _baseVelocity * Time.deltaTime;
            newPos.z = Mathf.Clamp(newPos.z, _limitZ.x, _limitZ.y);
            _basePart.localPosition = newPos;
        }

        private void MoveCrane()
        {
            if (_crane == null) return;

            Vector3 newPos = _crane.localPosition + _craneVelocity * Time.deltaTime;
            newPos.x = Mathf.Clamp(newPos.x, _limitX.x, _limitX.y);
            _crane.localPosition = newPos;
        }

        private void OnMove(MoveSignal signal)
        {
            switch (signal.Direction)
            {
                case MoveDirection.Up:
                    _hookVelocity = Vector3.up * _upSpeed * signal.Intensity;
                    break;
                case MoveDirection.Down:
                    _hookVelocity = Vector3.down * _downSpeed * signal.Intensity;
                    break;
                case MoveDirection.North:
                    _baseVelocity = Vector3.forward * _northSpeed * signal.Intensity;
                    break;
                case MoveDirection.South:
                    _baseVelocity = Vector3.back * _southSpeed * signal.Intensity;
                    break;
                case MoveDirection.East:
                    _craneVelocity = Vector3.right * _eastSpeed * signal.Intensity;
                    break;
                case MoveDirection.West:
                    _craneVelocity = Vector3.left * _westSpeed * signal.Intensity;
                    break;
            }
        }
    }
}