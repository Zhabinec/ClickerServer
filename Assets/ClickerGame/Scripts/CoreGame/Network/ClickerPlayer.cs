using Game.Clicker.Domain;
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

        public static event System.Action<ClickerState> OnLocalStateChanged;
        public static event System.Action<string> OnLocalError;

        public override void OnStartServer()
        {
            _balance = 0;
            _multiplier = 1;
            _upgradeLevel = 0;
            _nextTapTime = 0.0;
        }

        public override void OnStartLocalPlayer()
        {
            CmdRequestState();
        }

        //Вызываются Client API
        public void Tap()
        {
            if (!isLocalPlayer) return;
            CmdTap();
        }

        public void BuyUpgrade()
        {
            if (!isLocalPlayer) return;
            CmdBuyUpgrade();
        }

        // Command - это клиент -> сервер
        [Command]
        private void CmdRequestState()
        {
            TargetStateChanged(connectionToClient, MakeState());
        }

        [Command]
        private void CmdTap()
        {
            if (NetworkTime.time < _nextTapTime) return;
            _nextTapTime = NetworkTime.time + _tapCooldownSeconds;

            _balance += _multiplier;
            TargetStateChanged(connectionToClient, MakeState());
        }

        [Command]
        private void CmdBuyUpgrade()
        {
            int price = GetUpgradePrice(_upgradeLevel);
            if (_balance < price)
            {
                TargetError(connectionToClient, "not_enough_currency");
                return;
            }

            _balance -= price;
            _upgradeLevel += 1;
            _multiplier += 1;

            TargetStateChanged(connectionToClient, MakeState());
        }

        // Это наоборот Server -> Client (только этому игроку)
        [TargetRpc]
        private void TargetStateChanged(NetworkConnectionToClient connection, ClickerState state)
        {
            OnLocalStateChanged?.Invoke(state);
        }

        [TargetRpc]
        private void TargetError(NetworkConnectionToClient connection, string code)
        {
            OnLocalError?.Invoke(code);
        }

        private ClickerState MakeState()
        {
            return new ClickerState(_balance, _multiplier, _upgradeLevel);
        }

        private int GetUpgradePrice(int level)
        {
            return 10 + level * 15;
        }
    }

}
