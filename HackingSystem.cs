
using System.Collections;
using System.Collections.Generic;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Steamworks;

namespace Foogle.NumberGame
{
    public static class HackingSystem
    {
        private static readonly Dictionary<CSteamID, List<string>> PlayerPatterns = new();
        private static readonly Dictionary<CSteamID, int> PlayerProgress = new();

        public static void Show(
            UnturnedPlayer player,
            int patternLength,
            float patternSpeed,
            ushort effectId,
            short effectKey)
        {
            EffectManager.sendUIEffect(effectId, effectKey, player.CSteamID, true);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

            var tileNames = GetAllTileNames();
            string whiteUrl = "https://images.ctfassets.net/nnkxuzam4k38/72FI1uoNvyy66AU9lmkxD0/29545557828129fd31864cdd486457f4/white-radial-gradient.jpg";
            foreach (var tile in tileNames)
                EffectManager.sendUIEffectImageURL(effectKey, player.CSteamID, true, tile, whiteUrl);

            player.Player.StartCoroutine(ShowPattern(player, tileNames, patternLength, patternSpeed, effectKey));
        }

        private static List<string> GetAllTileNames()
        {
            var names = new List<string>();
            foreach (var row in new[] { "A", "B", "C" })
                for (int i = 1; i <= 9; i++)
                    names.Add($"B{row}{i}");
            return names;
        }

        private static IEnumerator ShowPattern(UnturnedPlayer player, List<string> tileNames, int patternLength, float patternSpeed, short effectKey)
        {
            yield return new WaitForSeconds(1f);

            var random = new System.Random();
            var pattern = new List<string>();
            for (int i = 0; i < patternLength; i++)
                pattern.Add(tileNames[random.Next(tileNames.Count)]);

            PlayerPatterns[player.CSteamID] = new List<string>(pattern);
            PlayerProgress[player.CSteamID] = 0;

            string redUrl = "https://htmlcolorcodes.com/assets/images/colors/red-color-solid-background-1920x1080.png";
            string whiteUrl = "https://images.ctfassets.net/nnkxuzam4k38/72FI1uoNvyy66AU9lmkxD0/29545557828129fd31864cdd486457f4/white-radial-gradient.jpg";

            string prevTile = null;
            foreach (var tile in pattern)
            {
                if (prevTile != null)
                    EffectManager.sendUIEffectImageURL(effectKey, player.CSteamID, true, prevTile, whiteUrl);

                EffectManager.sendUIEffectImageURL(effectKey, player.CSteamID, true, tile, redUrl);
                prevTile = tile;
                yield return new WaitForSeconds(patternSpeed);
            }
            if (prevTile != null)
                EffectManager.sendUIEffectImageURL(effectKey, player.CSteamID, true, prevTile, whiteUrl);
        }

        public static void OnEffectButtonClicked(UnturnedPlayer player, string buttonName, short effectKey, ushort effectId)
        {
            int progress = 0;
            PlayerProgress.TryGetValue(player.CSteamID, out progress);

            var steamID = player.CSteamID;
            if (!PlayerPatterns.ContainsKey(steamID)) return;

            var pattern = PlayerPatterns[steamID];
            if (progress < pattern.Count && buttonName == pattern[progress])
            {
                PlayerProgress[steamID] = progress + 1;
                if (PlayerProgress[steamID] >= pattern.Count)
                {
                    player.Player.StartCoroutine(ShowSuccessAnimation(player, effectKey));
                    player.Player.StartCoroutine(CloseEffectAfterDelay(player, 2.1f, effectId, effectKey));
                    PlayerPatterns.Remove(steamID);
                    PlayerProgress.Remove(steamID);
                }
            }
            else
            {
                CloseAndReset(player, effectId, effectKey);
            }
        }

        private static void CloseAndReset(UnturnedPlayer player, ushort effectId, short effectKey)
        {
            EffectManager.askEffectClearByID(effectId, player.CSteamID);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
            PlayerPatterns.Remove(player.CSteamID);
            PlayerProgress.Remove(player.CSteamID);
        }

        private static IEnumerator ShowSuccessAnimation(UnturnedPlayer player, short effectKey)
        {
            string greenUrl = "https://static.wikia.nocookie.net/colors/images/c/cd/00ff00.png/revision/latest?cb=20231102223543.png";
            var correctPat = new List<string>();
            for (int i = 1; i <= 9; i++) correctPat.Add($"BA{i}");
            for (int i = 9; i >= 1; i--) correctPat.Add($"BB{i}");
            for (int i = 1; i <= 9; i++) correctPat.Add($"BC{i}");

            foreach (var tile in correctPat)
            {
                EffectManager.sendUIEffectImageURL(effectKey, player.CSteamID, true, tile, greenUrl);
                yield return new WaitForSeconds(0.07f);
            }
        }

        private static IEnumerator CloseEffectAfterDelay(UnturnedPlayer player, float delay, ushort effectId, short effectKey)
        {
            yield return new WaitForSeconds(delay);
            CloseAndReset(player, effectId, effectKey);
        }
    }
}