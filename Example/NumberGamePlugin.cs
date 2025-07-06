using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace Foogle.NumberGame
{
    public class NumberGamePlugin : RocketPlugin<NumberGameConfiguration>
    {
        
        private const ushort EffectId = 2061;
        private const short EffectKey = 0;

        protected override void Load()
        {
            Logger.Log($"{Name} {Assembly.GetName().Version.ToString(3)} has been loaded!");
            EffectManager.onEffectButtonClicked += OnEffectButtonClicked;
        }

        protected override void Unload()
        {
            Logger.Log($"{Name} has been unloaded!");
            EffectManager.onEffectButtonClicked -= OnEffectButtonClicked;
        }

        private void OnEffectButtonClicked(Player player, string buttonName)
        {
            var unturnedPlayer = UnturnedPlayer.FromPlayer(player);
            HackingSystem.OnEffectButtonClicked(unturnedPlayer, buttonName, EffectKey, EffectId);
        }
    }
}