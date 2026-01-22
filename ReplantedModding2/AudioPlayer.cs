using System;
using System.IO;
using NAudio.Wave;

namespace YourApp.Audio
{
	public class AudioPlayer : IDisposable
	{
		private WaveOutEvent outputDevice;
		private WaveStream waveStream;
		private bool isStopping;

		public bool Loop { get; set; }

		public PlaybackState PlaybackState =>
			outputDevice?.PlaybackState ?? PlaybackState.Stopped;

		// ---------- DISK ----------
		public void Load(string filePath)
		{
			Cleanup();
			waveStream = new AudioFileReader(filePath);
			InitOutput();
		}

		// ---------- RESOURCE (MP3) ----------
		public void LoadMp3FromResource(byte[] resourceData)
		{
			Cleanup();

			var ms = new MemoryStream(resourceData);
			var mp3 = new Mp3FileReader(ms);
			waveStream = WaveFormatConversionStream.CreatePcmStream(mp3);

			InitOutput();
		}

		private void InitOutput()
		{
			outputDevice = new WaveOutEvent();
			outputDevice.Init(waveStream);
			outputDevice.PlaybackStopped += OnPlaybackStopped;
		}

		public void Play()
		{
			if (outputDevice == null)
				return;

			isStopping = false;
			outputDevice.Play();
		}

		public void Pause()
		{
			if (PlaybackState == PlaybackState.Playing)
				outputDevice.Pause();
		}

		public void Stop()
		{
			if (outputDevice == null)
				return;

			isStopping = true;
			outputDevice.Stop();
			waveStream.Position = 0;
		}

		private void OnPlaybackStopped(object sender, StoppedEventArgs e)
		{
			if (Loop && !isStopping && waveStream != null)
			{
				waveStream.Position = 0;
				outputDevice.Play();
			}
		}

		private void Cleanup()
		{
			if (outputDevice != null)
			{
				outputDevice.PlaybackStopped -= OnPlaybackStopped;
				outputDevice.Stop();
				outputDevice.Dispose();
				outputDevice = null;
			}

			waveStream?.Dispose();
			waveStream = null;
		}

		public void Dispose() => Cleanup();
		public void LoadWavFromResource(Stream resourceStream)
		{
			Cleanup();

			var ms = new MemoryStream();
			resourceStream.CopyTo(ms);
			ms.Position = 0;

			waveStream = new WaveFileReader(ms);
			InitOutput();
		}
		public void LoadMp3FromResource(Stream resourceStream)
		{
			Cleanup();

			// Important: copy to MemoryStream because Mp3FileReader needs seekable stream
			var ms = new MemoryStream();
			resourceStream.CopyTo(ms);
			ms.Position = 0;

			var mp3 = new Mp3FileReader(ms);
			waveStream = WaveFormatConversionStream.CreatePcmStream(mp3);

			InitOutput();
		}

	}
}