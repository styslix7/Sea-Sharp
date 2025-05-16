using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Characters;
using System.Collections.Generic;

namespace AtlantisCompanion
{
    public class ModEntry : Mod
    {
        private NPC companion;
        private Vector2 companionPosition;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            if (Game1.currentLocation != null && Game1.player != null)
            {
                companionPosition = Game1.player.getTileLocation() + new Vector2(1, 0);
                if (companion == null)
                {
                    companion = new NPC(null, companionPosition * 64, Game1.currentLocation.Name, 2, "Companion", false, null, Game1.content.Load<Texture2D>("Characters\\Abigail"));
                    companion.Speed = 3;
                    Game1.currentLocation.addCharacter(companion);
                }
                else
                {
                    companion.setTilePosition((int)companionPosition.X, (int)companionPosition.Y);
                    if (!Game1.currentLocation.characters.Contains(companion))
                        Game1.currentLocation.addCharacter(companion);
                }
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (companion == null || Game1.player == null || companion.currentLocation != Game1.player.currentLocation)
                return;

            FollowPlayer();
        }

        private void FollowPlayer()
        {

            Vector2 playerTile = Game1.player.getTileLocation();
            Vector2 behindPlayerTile = playerTile;

            switch (Game1.player.FacingDirection)
            {
                case 0:
                    behindPlayerTile = playerTile + new Vector2(0, 1);
                    break;
                case 1: 
                    behindPlayerTile = playerTile + new Vector2(-1, 0);
                    break;
                case 2: 
                    behindPlayerTile = playerTile + new Vector2(0, -1);
                    break;
                case 3:
                    behindPlayerTile = playerTile + new Vector2(1, 0);
                    break;
            }

            companion.controller = new PathFindController(companion, Game1.currentLocation, behindPlayerTile, -1, null, 200);
        }
    }
}
