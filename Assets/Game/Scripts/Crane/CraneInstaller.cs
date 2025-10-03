using UnityEngine;
using Zenject;
using SignalBus =Game.Scripts.Crane.Signals.SignalBus;

namespace Game.Scripts.Crane
{
    public class CraneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SignalBus>().AsSingle();
        }
    }
}