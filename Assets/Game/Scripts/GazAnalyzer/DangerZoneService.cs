using System.Linq;
using UnityEngine;

namespace Game.Scripts.GazAnalyzer
{
    public class DangerZoneService
    {
        private readonly Transform _probe;
        private readonly string _dangerTag;

        public DangerZoneService(Transform probe, string dangerTag)
        {
            _probe = probe;
            _dangerTag = dangerTag;
        }

        public float GetClosestDistance()
        {
            var zones = GameObject.FindGameObjectsWithTag(_dangerTag);
            if (zones == null || zones.Length == 0)
                return -1f;

            return zones.Min(z => Vector3.Distance(_probe.position, z.transform.position));
        }
    }
}