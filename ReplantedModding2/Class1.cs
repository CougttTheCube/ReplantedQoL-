using System;
using System.Media;
using System.Runtime.CompilerServices;
using YourApp.Audio;
using HarmonyLib;
using Il2CppJetBrains.Annotations;
using Il2CppReloaded.Data;
using Il2CppReloaded.Gameplay;
using Il2CppReloaded.Services;
using Il2CppReloaded.TreeStateActivities;
using Il2CppSource.Binders;
using Il2CppSource.DataModels;
using MelonLoader;
using MelonLoader.Utils;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using ReplantAPI.Core;
using ReplantAPI.Extensions;
using ReplantedModding2;
using UnityEngine;
using static MelonLoader.MelonLogger;
using static UnityEngine.U2D.ClipperOffset2D;
using API = ReplantAPI.Core.ReplantAPI;
using System.Linq.Expressions;
/*
TODO: - Add more QoL features: 
      - Replace wav files with mp3 files to reduce mod size
      - support mp3 audio outside



*/
namespace ReplantedModding2
{
	
	public class ReplantedQoL : ReplantMod
	{
		
			public override void OnInitializeMelon()
			{
				LoggerInstance.Msg("ReplantedQoL Initialized - Made by Cougtt!");
				LoggerInstance.Msg("Using ReplantAPI v1.0.2 - Made by HenHen!");
				LoggerInstance.Msg("--------------------------------");
				LoggerInstance.Msg("S - Toggle this mod!");
				LoggerInstance.Msg("A - Set sun to 9999!");
				LoggerInstance.Msg("C - Toggle Instant Cooldown!");
				LoggerInstance.Msg("--------------------------------");
				LoggerInstance.Msg("GAME SPEED");
				LoggerInstance.Msg("LeftArrow - Set speed to 0.5!");
				LoggerInstance.Msg("RightArrow - Set speed to 5!");
				LoggerInstance.Msg("Z - Decrease speed by 0.5 (down to 0,5x)!");
				LoggerInstance.Msg("X - Increase speed by 0.5 (up to 5x)!");
				LoggerInstance.Msg("--------------------------------");
				LoggerInstance.Msg("MUSIC");
				LoggerInstance.Msg("L - Play Loonboon Soundtrack!");
				LoggerInstance.Msg("K - Play Brainiac Maniac Soundtrack!");
				LoggerInstance.Msg("J - Play Conveyer Soundtrack!");
				LoggerInstance.Msg("P - Play Custom Soundtrack! [currently in beta]");
				LoggerInstance.Msg("O - Toggle Pause Custom Soundtrack! [currently in beta]");
				LoggerInstance.Msg("Y - Stop Custom Soundtrack! [currently in beta]");
				LoggerInstance.Msg("Using Custom Soundtrack: Grasswalk RIP");
				LoggerInstance.Msg("--------------------------------");
				LoggerInstance.Msg("Thank you for using ^_^");

				//Setting up custom audio player
				player = new AudioPlayer();
				player.LoadMp3FromResource(Properties.Resources.grasswalk);
				player.Loop = true;

			}


			public float speed = 1f;

			public bool IsInstantCooldown = false;
			public bool IsCustomSoundtrack = false;

			public bool IsModActive = false;

			private bool IsPlayerPlaying = false;
			private bool IsPausedByMenu = false;

			public bool TogglePause = false;



			private AudioPlayer player;

			public override void OnUpdate()

			{
				try
				{
					//Ensure player is valid and game is active

					if (!API.IsGameActive) return;


					//Stop custom track when in pause menu
					if (IsCustomSoundtrack)
					{
						if (Time.timeScale <= 0f && IsPlayerPlaying && !IsPausedByMenu) //if i was playing, then pause game then stop audio
						{
							player.Pause();

							IsPlayerPlaying = false;
							IsPausedByMenu = true;
						}
						else if (Time.timeScale > 0f && IsPausedByMenu) //if paused by menu and is in gameplay, then continue audio
						{
							player.Play();

							IsPlayerPlaying = true;
							IsPausedByMenu = false;
						}
					}


					//Toggle the mod
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



					// ------------------------------GAME SPEED-------------------------------
					// Press LeftArrow to set speed scale to 0.5
					if (Input.GetKeyDown(KeyCode.LeftArrow))
					{
						if (!API.IsGameActive) return;
						if (!IsModActive) return;
						LoggerInstance.Msg("Game speed set to 0.5");
						speed = 0.5f;
						Time.timeScale = speed;
					}

					// Press RightArrow to set speed scale to 5
					if (Input.GetKeyDown(KeyCode.RightArrow))
					{
						if (!API.IsGameActive) return;
						if (!IsModActive) return;
						LoggerInstance.Msg("Game speed set to 5");
						speed = 5f;
						Time.timeScale = speed;
					}

					// Press Z to decrease game speed by 0.5
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

					// Press X to decrease game speed by 0.5
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










					// ------------------------------MUSIC-------------------------------
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

					// Press J to play Conveyer soundtrack
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


					// Press Y to stop the fucking custom soundtrack
					if (API.IsGameActive && (Input.GetKeyDown(KeyCode.Y)))
					{
						if (!IsModActive) return;

						// turn off all music

						player.Stop();
						API.AudioService.PlayMusic(MusicTune.NumMusicTunes);
					}

					//Stop custom music when one side won in versus mode
					if (API.GameplayActivity != null && API.GameplayActivity.VersusMode != null && API.GameplayActivity.VersusMode.m_winInitialized)
					{
						if (IsCustomSoundtrack)
						{
							player.Stop();
							IsCustomSoundtrack = false;
							IsPlayerPlaying = false;
							IsPausedByMenu = false;
						}
					}

					//Press P to play Custom Grasswalk RIP track
					if (API.IsGameActive && (Input.GetKeyDown(KeyCode.P)))
					{
						if (!IsModActive) return;

						// turn off all music
						IsCustomSoundtrack = true;

						IsPausedByMenu = false;
						IsPlayerPlaying = true;


						API.AudioService.PlayMusic(MusicTune.NumMusicTunes);
						player.Play();
						LoggerInstance.Msg("Playing Grasswalk RIP track!");
					}
					//Press O to toggle pause custom track
					if (API.IsGameActive && (Input.GetKeyDown(KeyCode.O)))
					{
						if (!IsModActive) return;

						// turn off all music
						TogglePause = !TogglePause;
						if (TogglePause)
						{
							player.Pause();
						}
						else
						{
							player.Play();
						}

					}




					if (API.IsGameActive && (Input.GetKeyDown(KeyCode.M)))
					{
						if (!IsModActive) return;

						//bool asdf = API.GameplayActivity.CanShowAlmanac();
						//bool asdff = API.GameplayActivity.GameplayService.WasCollected;
						//API.GameplayActivity.CheckForGameEnd();
						//API.GameplayActivity.EndLevel();
						//API.GameplayActivity.FadeOutCutScene();

					}




				}
				catch (Exception ex)
				{
					LoggerInstance.Error(ex.ToString());
				}
			}

			//stop custom music when going back to main menu
			public override void OnSceneWasLoaded(int buildIndex, string sceneName)
			{
				base.OnSceneWasLoaded(buildIndex, sceneName);
				LoggerInstance.Msg("Scene Loaded: " + sceneName);
				if (sceneName == "Frontend")
				{				
					if (IsCustomSoundtrack)
					{
						player.Stop();
						IsCustomSoundtrack = false;
						IsPlayerPlaying = false;
						IsPausedByMenu = false;
					}
					speed = 1f;
					Time.timeScale = speed;
				}
			}


		

	}


	

	//Patch: Being able to use Flower Pot and Plantern in Versus Mode
	[HarmonyPatch(typeof(SeedChooserScreen), nameof(SeedChooserScreen.SeedNotAllowedToPick))]
	internal static class SeedChooserDataModelPatch
	{
		[HarmonyPrefix]
		private static bool Prefix(SeedChooserScreen __instance, ref bool __result, SeedType theSeedType)
		{
			if (__instance.mApp.IsVersusMode() && theSeedType == SeedType.Flowerpot)
			{
				__result = false;
				return false;
			}
			if (__instance.mApp.IsVersusMode() && theSeedType == SeedType.Plantern)
			{
				__result = false;
				return false;
			}
			return true;
		}
	}
}