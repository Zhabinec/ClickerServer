using UnityEngine.Rendering.Universal;

namespace Game.Clicker.Domain
{
    public struct ClickerState
    {
        public int Balance;
        public int Multiplier;
        public int UpgradeLevel;

        public ClickerState(int balance, int multipliet, int upgradeLevel)
        {
            Balance = balance;
            Multiplier = multipliet;
            UpgradeLevel = upgradeLevel;
        }
    }
}

