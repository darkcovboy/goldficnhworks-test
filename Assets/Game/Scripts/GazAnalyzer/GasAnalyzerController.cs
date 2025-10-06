using Game.Scripts.GazAnalyzer.Button;
using Game.Scripts.GazAnalyzer.Display;
using UnityEngine;
using Zenject;

namespace Game.Scripts.GazAnalyzer
{
    public class GasAnalyzerController : ITickable
    {
        private const float MaxDetectDistance = 10f;

        private readonly GasAnalyzerDisplay _displayView;
        private readonly DangerZoneService _dangerService;

        private bool _isOn;

        public GasAnalyzerController(GasAnalyzerDisplay displayView, DangerZoneService dangerService)
        {
            _displayView = displayView;
            _dangerService = dangerService;
        }

        public void TogglePower()
        {
            _isOn = !_isOn;

            if (_isOn)
                _displayView.Show();
            else
                _displayView.Hide();
        }

        public void Tick()
        {
            if (!_isOn) return;

            float distance = _dangerService.GetClosestDistance();
            if (distance < 0) return;

            _displayView.SetDistance(distance);

            float gasLevel = Mathf.Clamp01(1f - distance / MaxDetectDistance);
            _displayView.SetGasLevel(gasLevel);
        }
    }
}