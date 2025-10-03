using System;
using UniRx;

namespace Game.Scripts.Crane.Signals
{
    public class SignalBus
    {
        private readonly Subject<MoveSignal> _moveStream = new();
        
        public IObservable<MoveSignal> OnMove => _moveStream;
        
        public void Fire(MoveSignal signal) => _moveStream.OnNext(signal);
    }
}