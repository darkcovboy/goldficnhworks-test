using UnityEngine;

namespace Game.Scripts.Utilities.Wire
{
    [RequireComponent(typeof(LineRenderer))]
    public class WireLineRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
        }
#endif
        
        private void Update()
        {
            if (_startPoint == null || _endPoint == null)
                return;

            _lineRenderer.SetPosition(0, _startPoint.position);
            _lineRenderer.SetPosition(1, _endPoint.position);
        }

    }
}