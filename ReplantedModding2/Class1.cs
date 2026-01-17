using System.Runtime.CompilerServices;
using HarmonyLib;
using Il2CppJetBrains.Annotations;
using Il2CppReloaded.Data;
using Il2CppReloaded.Gameplay;
using Il2CppReloaded.Services;
using MelonLoader;
using ReplantAPI.Core;
using ReplantAPI.Extensions;
using ReplantedModding2;
using UnityEngine;
using static MelonLoader.MelonLogger;
using static UnityEngine.U2D.ClipperOffset2D;
using API = ReplantAPI.Core.ReplantAPI;




namespace ReplantedModding2
{

	public class ReplantedQoL : MelonMod
	{
		public override void OnInitializeMelon()
		{
			LoggerInstance.Msg("Custom Mod Initialized!");
			LoggerInstance.Msg("Using ReplantAPI - Made by HenHen!");
			LoggerInstance.Msg("--------------------------------");
			LoggerInstance.Msg("S - Activate this mod!");
			LoggerInstance.Msg("A - Set sun to 9999!");
			LoggerInstance.Msg("LeftArrow - Set speed to 0.5!");
			LoggerInstance.Msg("RightArrow - Set speed to 5!");
			LoggerInstance.Msg("Z - Decrease speed by 0.5 (down to 0,5x)!");
			LoggerInstance.Msg("X - Increase speed by 0.5 (up to 5x)!");
			LoggerInstance.Msg("C - Instant Cooldown!");
			LoggerInstance.Msg("L - Play Loonboon Soundtrack!");
			LoggerInstance.Msg("L - Play Brainiac Maniac Soundtrack!");
			LoggerInstance.Msg("L - Play Conveyer Soundtrack!");
			LoggerInstance.Msg("L - Play Loon Skirmish Soundtrack! [currently in beta]");
			LoggerInstance.Msg("--------------------------------");
			LoggerInstance.Msg("Thank you for using ^_^");
		}


		public float speed = 1f;
		public bool IsInstantCooldown = false;
		public bool IsCustomSoundtrack = false;
		public bool IsModActive = false;

		private bool IsPlayerPlaying = false;
		private bool IsPausedByMenu = false;
		



		System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.LoonSkirmishHigh);


		//todo: fix custom music playing outside of pause menu :/
		public override void OnUpdate()
		{
			//replantapi is really ahh rn so there's nothing i can do :/

			

			if (IsCustomSoundtrack)
			{
				if (Time.timeScale <= 0f && IsPlayerPlaying && !IsPausedByMenu)
				{
					player.Stop();
					IsPlayerPlaying = false;
					IsPausedByMenu = true;
				}
				
				else if (Time.timeScale > 0f && IsPausedByMenu)
				{
					player.PlayLooping();
					IsPlayerPlaying = true;
					IsPausedByMenu = false;
				}
				

			}

			//toggle mod
			if (Input.GetKeyDown(KeyCode.S))
			{
				if (IsModActive == true)
				{
					IsModActive = false;
					LoggerInstance.Msg("Mod Disabled!");
				}
				else
				{
					IsModActive = true;
					LoggerInstance.Msg("Mod Enabled!");
				}
			}

			// Press K to set sun to 9999
			if (Input.GetKeyDown(KeyCode.A))
			{
				if (!API.IsGameActive) return;
				if (!IsModActive) return;
				API.Player.SetSun(9999);
				LoggerInstance.Msg("Set sun to 9999");
			}

			// speed control 
			// Press LeftArrow to set speed scale to 0.5
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (!API.IsGameActive) return;
				if (!IsModActive) return;
				LoggerInstance.Msg("Game speed set to 0.5");
				speed = 0.5f;
				Time.timeScale = speed;
			}

			// Press RightArrow to set speed scale to 2
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (!API.IsGameActive) return;
				if (!IsModActive) return;
				LoggerInstance.Msg("Game speed set to 5");
				speed = 5f;
				Time.timeScale = speed;
			}

			// Press UpArrow to set speed scale to 0.5
			if (Input.GetKeyDown(KeyCode.Z))
			{
				if (!API.IsGameActive) return;
				if (!IsModActive) return;
				if (speed == 0.5f)
				{
					LoggerInstance.Msg("0.5x speed minimum! Can't go any further!");
					return;
				}
				LoggerInstance.Msg("Game speed decreased by 0.5");
				speed -= 0.5f;
				LoggerInstance.Msg("Current speed:" + speed);
				Time.timeScale = speed;
			}

			// Press DownArrow to set speed scale to -0.5
			if (Input.GetKeyDown(KeyCode.X))
			{
				if (!API.IsGameActive) return;
				if (!IsModActive) return;
				if (speed == 5f)
				{
					LoggerInstance.Msg("5x speed maximum! Can't go any further!");
					return;
				}
				LoggerInstance.Msg("Game speed increased by 0.5");
				speed += 0.5f;
				LoggerInstance.Msg("Current speed:" + speed);
				Time.timeScale = speed;
			}

			// Press C to toggle instant cooldown
			if (API.IsGameActive && Input.GetKeyDown(KeyCode.C))
			{
				if (!IsModActive) return;
				IsInstantCooldown = !IsInstantCooldown;
			}
			if (API.IsGameActive && IsInstantCooldown)
			{
				if (!IsModActive) return;
				API.Player.EnableInstantCooldown();
			}

			
			


			// Press L to play Loonboon soundtrack
			if (API.IsGameActive && (Input.GetKeyDown(KeyCode.L)))
			{
				if (!IsModActive) return;
				// stop custom music if playing
				if (IsCustomSoundtrack)
				{
					player.Stop();
					IsCustomSoundtrack = false;

					IsPlayerPlaying = false;
					IsPausedByMenu = false;
				}
				API.AudioService.PlayMusic(MusicTune.MinigameLoonboon);
				LoggerInstance.Msg("Playing Loonboon!");
			}

			// Press K to play Brainiac Maniac soundtrack
			if (API.IsGameActive && (Input.GetKeyDown(KeyCode.K)))
			{
				if (!IsModActive) return;
				// stop custom music if playing
				if (IsCustomSoundtrack)
				{
					player.Stop();
					IsCustomSoundtrack = false;

					IsPlayerPlaying = false;
					IsPausedByMenu = false;
				}
				API.AudioService.PlayMusic(MusicTune.FinalBossBrainiacManiac);
				LoggerInstance.Msg("Playing Brainiac Maniac!");
			}

			// Press J to play Brainiac Maniac soundtrack
			if (API.IsGameActive && (Input.GetKeyDown(KeyCode.J)))
			{
				if (!IsModActive) return;
				// stop custom music if playing
				if (IsCustomSoundtrack)
				{
					player.Stop();
					IsCustomSoundtrack = false;

					IsPlayerPlaying = false;
					IsPausedByMenu = false;
				}
				API.AudioService.PlayMusic(MusicTune.Conveyer);
				LoggerInstance.Msg("Playing Conveyer!");
			}

			// Press H to play custom soundtrack
			if (API.IsGameActive && (Input.GetKeyDown(KeyCode.H)))
			{
				if (!IsModActive) return;

				IsCustomSoundtrack = true;

				IsPausedByMenu = false;
				IsPlayerPlaying = true;

				// turn off other music and play custom music
				API.AudioService.PlayMusic(MusicTune.None);
				player.PlayLooping();
				LoggerInstance.Msg("Playing Loon Skirmish soundtrack! [currently in beta]");

			}

			// Press Y to stop the fucking custom soundtrack
			if (API.IsGameActive && (Input.GetKeyDown(KeyCode.Y)))
			{
				if (!IsModActive) return;

				// turn off other music and play custom music
				
				player.Stop();
				API.AudioService.PlayMusic(MusicTune.NumMusicTunes);


			}


		}
	}	
}