using Rocket.Core.Plugins;
using Rocket.Unturned.Permissions;
using SDG.Unturned;
using Steamworks;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Logger = Rocket.Core.Logging.Logger;

namespace Mythicals
{
    class Mythicals : RocketPlugin
    {
        public const string JSON_FILE = "Plugins/Mythicals/players_myths.json";
        public static Mythicals Instance { get; private set; }
        public List<JsonInfo> players { get; set; }

        protected override void Load()
        {
            Instance = this;
            LoadCFG();
            UnturnedPermissions.OnJoinRequested += new UnturnedPermissions.JoinRequested(OnPlayerConnect);
        }

        public void LoadCFG()
        {
            try
            {
                if (File.Exists(JSON_FILE))
                {
                    string json = File.ReadAllText(JSON_FILE);
                    players = JsonConvert.DeserializeObject<List<JsonInfo>>(json);
                }
                else
                {
                    players = new List<JsonInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load 'players_myths.json'.");
                Logger.LogException(ex);
            }
        }

        public void RegisterPlayer(ulong steamid, int hatId)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if(players[i].steamid == steamid)
                {
                    if (hatId != 0)
                        players[i].hatId = hatId;
                    else
                        players.RemoveAt(i);

                    File.WriteAllText(JSON_FILE, JsonConvert.SerializeObject(players));
                    return;
                }
            }

            players.Add(new JsonInfo { steamid = steamid, hatId = hatId });
            File.WriteAllText(JSON_FILE, JsonConvert.SerializeObject(players));
        }

        protected override void Unload()
        {
            Instance = null;
            UnturnedPermissions.OnJoinRequested -= new UnturnedPermissions.JoinRequested(OnPlayerConnect);
        }

        public void OnPlayerConnect(CSteamID Player, ref ESteamRejection? rejection)
        {
            foreach (SteamPending Players in Provider.pending)
            {
                if (Players.playerID.steamID == Player)
                {
                    JsonInfo info = players.FirstOrDefault(x => x.steamid == Player.m_SteamID);
                    if (info != null)
                    {
                        Players.hatItem = info.hatId;
                    }
                }
            }
        }
    }

    class JsonInfo
    {
        public ulong steamid;
        public int hatId;
    }
}
