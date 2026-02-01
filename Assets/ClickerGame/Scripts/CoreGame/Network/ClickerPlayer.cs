using Mirror;
using UnityEngine;

namespace Game.Clicker.Network
{
    [DisallowMultipleComponent]
    public class ClickerPlayer : NetworkBehaviour
    {
        [Header("Server Rules")]
        [SerializeField] private float _tapCooldownSeconds = 0.08f;

        [SyncVar] private int _balance;
        [SyncVar] private int _multiplier = 1;
        [SyncVar] private int _upgradeLevel;

        private double _nextTapTime;

        public override void OnStartServer()
        {
            _balance = 0;
            _multiplier = 1;
            _upgradeLevel = 0;
            _nextTapTime = 0.0;
        }
    }

}
