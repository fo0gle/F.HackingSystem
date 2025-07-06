using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Player;
using Steamworks;

namespace Foogle.NumberGame
{
    public class EffectCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "hack";
        public string Help => "Hack";
        public string Syntax => "";
        public List<string> Aliases => new();
        public List<string> Permissions => new();

        private static readonly int PatternLength = 5;
        private static readonly float PatternSpeed = 0.5f;
        private static readonly ushort EffectId = 2061;
        private static readonly short EffectKey = 0;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is not UnturnedPlayer player) return;
            HackingSystem.Show(player, PatternLength, PatternSpeed, EffectId, EffectKey);
        }
    }
}