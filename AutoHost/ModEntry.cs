using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

using System.Net.Http;

namespace AutoHost {
    internal sealed class ModEntry : Mod {

        private readonly HttpClient client = new HttpClient();
        private readonly string HOST = "http://localhost:9246";

        public override void Entry(IModHelper helper) {
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.ConsoleCommands.Add("ah", "Debug command of AutoHost", this.AutoHostCommand);
        }

        // ================ on events ================

        private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e) {
            if (!Context.IsWorldReady) {
                return;
            }

            TryPause();
        }

        private async void OnDayStarted(object? sender, DayStartedEventArgs e) {
            if (!Context.IsWorldReady) {
                return;
            }

            await client.GetStringAsync($"{HOST}/zoom-and-sleep");
            StartSleep();
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e) {
            if (!Context.IsWorldReady) {
                return;
            }

            GoingToSleepOnTick();
        }

        // ================ class methods ================

        private int walkToSleepCountdown = -1;

        private void StartSleep() {
            walkToSleepCountdown = 300;
        }

        private void GoingToSleepOnTick() {
            if (walkToSleepCountdown >= 0 && !isPaused) {
                this.Monitor.Log($"Walking to sleep at countdown {walkToSleepCountdown}", LogLevel.Debug);
                walkToSleepCountdown--;
                if (walkToSleepCountdown == 150) {
                    this.Monitor.Log("Walking to bed", LogLevel.Debug);
                    Game1.player.SetMovingRight(true);
                }
                if (walkToSleepCountdown == -1) {
                    FinishSleep();
                }
            }
        }

        private void FinishSleep() {
            Game1.player.SetMovingRight(false);
            client.GetStringAsync($"{HOST}/zoom-and-sleep");
        }

        private bool isPaused = false;

        private async void TryPause() {
            int onlineCount = Game1.getOnlineFarmers().Count;
            this.Monitor.Log("onlineCount = " + onlineCount, LogLevel.Debug);
            this.Monitor.Log("isPaused = " + isPaused, LogLevel.Debug);
            if (onlineCount <= 1) {
                if (!isPaused) {
                    isPaused = true;
                    await client.GetStringAsync($"{HOST}/zoom-and-unsleep");
                    Game1.chatBox.textBoxEnter("/pause");
                    Game1.chatBox.addInfoMessage($"Paused at {DateTime.Now}");
                }
            } else {
                if (isPaused) {
                    isPaused = false;
                    Game1.chatBox.textBoxEnter("/resume");
                    Game1.chatBox.addInfoMessage($"Resumed at {DateTime.Now}");
                }
            }
        }

        // ================ ah cmd ================

        private void AutoHostCommand(string command, string[] args) {
            if (args[0] == "c") {
                Game1.chatBox.textBoxEnter(args[1]);
            } else if (args[0] == "f") {
                foreach (Farmer farmer in Game1.getAllFarmers()) {
                    this.Monitor.Log(farmer.Name + " " + farmer.isActive().ToString(), LogLevel.Debug);
                }
            } else if (args[0] == "r") {
                if (args[1] == "zoom") {
                    client.GetStringAsync($"{HOST}/record/zoom");
                } else if (args[1] == "sleep") {
                    client.GetStringAsync($"{HOST}/record/sleep");
                } else if (args[1] == "unsleep") {
                    client.GetStringAsync($"{HOST}/record/unsleep");
                } else {
                    this.Monitor.Log("Invalid argument for record.", LogLevel.Debug);
                }
            } else if (args[0] == "m") {
                bool[] directions = new bool[4];
                directions[int.Parse(args[1])] = true;
                Game1.player.SetMovingUp(directions[0]);
                Game1.player.SetMovingRight(directions[1]);
                Game1.player.SetMovingDown(directions[2]);
                Game1.player.SetMovingLeft(directions[3]);
            }
        }
    }
}
