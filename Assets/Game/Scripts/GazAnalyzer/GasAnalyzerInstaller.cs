using Game.Scripts.GazAnalyzer.Display;
using UnityEngine;
using Zenject;

namespace Game.Scripts.GazAnalyzer
{
    public class GasAnalyzerInstaller : MonoInstaller
    {
        [SerializeField] private GasAnalyzerDisplay _displayView;
        [SerializeField] private Transform _probe;

        public override void InstallBindings()
        {
            Container.BindInstance(_displayView).AsSingle();
            Container.Bind<DangerZoneService>()
                .AsSingle()
                .WithArguments(_probe, "DangerZone");
            Container.BindInterfacesAndSelfTo<GasAnalyzerController>().AsSingle().NonLazy();
        }
    }
}