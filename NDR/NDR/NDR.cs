using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Core.Plugins;
using Logger = Rocket.Core.Logging.Logger;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using System.Collections;

namespace NDR
{
    public class NDR : RocketPlugin<Config>
    {
        public List<Vector3> DeathPos;
        public static List<CSteamID> TasedPlayers;
        protected override void Load()
        {
            Logger.Log("NDR succefully loaded! for bug reports contact Arbusto#6794");
            DeathPos = new List<Vector3>();
            TasedPlayers = new List<CSteamID>();

            UnturnedPlayerEvents.OnPlayerDeath += PlayerDead;
        }

        public void PlayerDead(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {

         //   Logger.Log("player dead:" + player.CharacterName + " position:" + player.Position);
            EffectManager.sendEffect(125, 30, player.Position);
            DeathPos.Add(player.Position);

            StartCoroutine(removeEffect(player.Position));
            StartCoroutine(mantainEffect(player.Position, player));
        }

        IEnumerator mantainEffect(Vector3 position, UnturnedPlayer player)
        {
            

            while (DeathPos.Contains(position))
            {

                float Distance = Vector3.Distance(position, player.Position);

                if (Distance < Configuration.Instance.Radius && player.Health != 0)
                {

                    float multiply = (Configuration.Instance.Radius + Configuration.Instance.TpRadius) / Distance;

                    float oZ = (player.Position.z - position.z);
                    float oX = (player.Position.x - position.x);

                    float nX = oX * multiply;
                    float nZ = oZ * multiply;

                    float mX = nX + position.x;
                    float mZ = nZ + position.z;

                    Vector3 point = new Vector3(mX, player.Position.y, mZ);
               //     Logger.Log(point.ToString());
                    player.Teleport(point, player.Rotation);
                    UnturnedChat.Say(player, "No puedes volver a tu zona de muerte todavia! Se te ha ralentizado", Color.red);

                    if (Configuration.Instance.slow == true)
                    {
                        player.Player.movement.sendPluginSpeedMultiplier(0.4f);
                        StartCoroutine(RemoveFromSlow(player));
                    }

                }
                    yield return new WaitForSeconds(0.2f);
            }
        }

        IEnumerator removeEffect(Vector3 position)
        {
            yield return new WaitForSeconds(Configuration.Instance.ReturnAllowed);
            DeathPos.Remove(position);
        }

        IEnumerator RemoveFromSlow(UnturnedPlayer victim)
        {
            yield return new WaitForSeconds(Configuration.Instance.slowtime);
            TasedPlayers.Remove(victim.CSteamID);
            victim.Player.movement.sendPluginSpeedMultiplier(1f);
        }
    }
}
