using SDG.Provider;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Mythicals
{
    public class MythCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "myth";
        public string Help => "This command will allow you to equip any community market hat!";
        public string Syntax => "/myth <name/id>";
        public List<string> Aliases => new List<string>() { "mythical" };
        public List<string> Permissions => new List<string>() { "myth" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, Syntax);
                return;
            }

            UnturnedPlayer client = caller as UnturnedPlayer;
            List<UnturnedEconInfo> list = TempSteamworksEconomy.econInfo;

            if (int.TryParse(command[0], out int hatId))
            {
                if(hatId == 0)
                {
                    Mythicals.Instance.RegisterPlayer(client.CSteamID.m_SteamID, hatId);
                    UnturnedChat.Say(caller, "Success! Reconnect to apply changes.");
                    return;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    if(list[i].itemdefid == hatId)
                    {
                        if(list[i].type.ToLower().Contains("hat"))
                        {
                            Mythicals.Instance.RegisterPlayer(client.CSteamID.m_SteamID, hatId);
                            UnturnedChat.Say(caller, "Successfully applied " + list[i].name + ". Reconnect to apply changes.");
                            return;
                        }
                        else
                        {
                            UnturnedChat.Say(caller, list[i].name + " is not a hat.");
                            return;
                        }
                    }
                }
                
                UnturnedChat.Say(caller, "The hat id you specified does not exist.");
            }
            else 
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].name.ToLower().Contains(command[0].ToLower()))
                    {
                        if (list[i].type.ToLower().Contains("hat"))
                        {
                            Mythicals.Instance.RegisterPlayer(client.CSteamID.m_SteamID, hatId);
                            UnturnedChat.Say(caller, "Successfully applied " + list[i].name + ". Reconnect to apply changes.");
                            return;
                        }
                        else
                        {
                            UnturnedChat.Say(caller, list[i].name + " is not a hat.");
                            return;
                        }
                    }
                }

                UnturnedChat.Say(caller, "The hat name you specified does not exist.");
            }
        }
    }
}
