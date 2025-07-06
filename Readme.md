# Hacking plugin 

I made this quite quickly so might not be the best

## Setup
The main file is HackingSystem.cs this will allow you to set this up

Then call
```csharp
   - HackingSystem.Show(player, patternLength, patternSpeed, effectId, effectKey); to start the it
   - Hook EffectManager.onEffectButtonClicked and call HackingSystem.OnEffectButtonClicked(...) in your handler
```
Should look somthing like this:
```csharp
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
```
And main file should look like this:
```csharp
private void OnEffectButtonClicked(Player player, string buttonName)
{
    var unturnedPlayer = UnturnedPlayer.FromPlayer(player);
    HackingSystem.OnEffectButtonClicked(unturnedPlayer, buttonName, EffectKey, EffectId);
}
```

The base steam workshop item is [here](https://steamcommunity.com/sharedfiles/filedetails/?id=3518202291)

And if you want to change the Ui The unity Package is [here](https://github.com/fo0gle/F.HackingSystem/blob/main/Hacking.unitypackage)

### Do anything you want with this code if you  use it in your plugin/project please share it with me at fo0gl3 on discord also and questions Dm me
