using Il2CppJetBrains.Annotations;
using Il2CppReloaded.Data;
using Il2CppReloaded.Gameplay;
using Il2CppReloaded.Services;
using MelonLoader;
using ReplantAPI.Core;
using ReplantAPI.Extensions;
using UnityEngine;
using static UnityEngine.U2D.ClipperOffset2D;
using API = ReplantAPI.Core.ReplantAPI;
namespace ReplantedModding2
{

	public class TestMod : MelonMod
	{		
		public override void OnInitializeMelon()
		{
			LoggerInstance.Msg("Custom Mod Initialized!");
			LoggerInstance.Msg("Using ReplantAPI!");
			LoggerInstance.Msg("--------------------------------");
			LoggerInstance.Msg("K - Set sun to 9999!");
			LoggerInstance.Msg("LeftArrow - Set speed to 0.5!");
			LoggerInstance.Msg("RightArrow - Set speed to 2!");
			LoggerInstance.Msg("Z - Decrease speed by 0.5 (down to 0,5x)!");
			LoggerInstance.Msg("X - Increase speed by 0.5 (up to 5x)!");
			LoggerInstance.Msg("C - Instant Cooldown!");
			LoggerInstance.Msg("V - Play Loonboon Soundtrack!");
			LoggerInstance.Msg("--------------------------------");
			LoggerInstance.Msg("Thank you for using ^_^");
		}
			 



		public float speed = 1f;
		public override void OnUpdate()
		{
			// Press K to set sun to 9999
			if (Input.GetKeyDown(KeyCode.K))
			{
				if (!API.IsGameActive) return;
				API.Player.SetSun(9999);
				LoggerInstance.Msg("Set sun to 9999");
			}
			if (!API.IsGameActive) return;


			// Press LeftArrow to set speed scale to 0.5
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (!API.IsGameActive) return;
				LoggerInstance.Msg("Reset time scale to 0.5");
				speed = 0.5f;
				Time.timeScale = speed;
			}
			// Press RightArrow to set speed scale to 2
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (!API.IsGameActive) return;
				LoggerInstance.Msg("Reset time scale to 2");
				speed = 2f;
				Time.timeScale = speed;
			}
			// Press UpArrow to set speed scale to 0.5
			if (Input.GetKeyDown(KeyCode.Z))
			{
				if (!API.IsGameActive) return;
				if (speed == 0.5f) return;
				LoggerInstance.Msg("Decrease time scale to 0.5");
				speed -= 0.5f;
				LoggerInstance.Msg("Current speed:" + speed);
				Time.timeScale = speed;
			}
			// Press DownArrow to set speed scale to -0.5
			if (Input.GetKeyDown(KeyCode.X))
			{
				if (!API.IsGameActive) return;
				if (speed == 5f) return;
				LoggerInstance.Msg("Increase time scale to 0.5");
				speed += 0.5f;
				LoggerInstance.Msg("Current speed:" + speed);
				Time.timeScale = speed;
			}
			// Press C to enable instant cooldown
			if (API.IsGameActive && (Input.GetKeyDown(KeyCode.C)))
			{
				API.Player.EnableInstantCooldown();
				LoggerInstance.Msg("Instant Cooldown!");
			}
			if (API.IsGameActive && (Input.GetKeyDown(KeyCode.V)))
			{
				API.AudioService.PlayMusic(MusicTune.MinigameLoonboon);
				LoggerInstance.Msg("Playing Loonboon!");
			}

		}
		public void OnZombieSpawned(Zombie zombie)
		{
			if (zombie.mZombieType == ZombieType.Flag)
			{
				API.AudioService.PlayMusic(MusicTune.NightMoongrains);
				API.AudioService.StartBurst();

			}
		}


	}
	
}