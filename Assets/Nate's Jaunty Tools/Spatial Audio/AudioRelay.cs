using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NatesJauntyTools.SpatialAudio
{
	public enum AudibleSegment { Inaudible, VolumeScaled, VolumeMax, SurroundScaled, SurroundMax }

	public class AudioRelay : Singleton<AudioRelay>
	{
		AudioListener listener;
		AudioSource directSource;

		public bool DebugInWorld;
		public bool DebugInaudible;

		public GameObject connectionPrefab;

		public List<AudioConnection> connections = new List<AudioConnection>();
		List<AudioTransmitter> transmitters = new List<AudioTransmitter>();
		List<AudioReceiver> receivers = new List<AudioReceiver>();


		#region Setup & Debug

		protected override void PostInitialize()
		{
			listener = ForceInitializeComponent<AudioListener>();
			directSource = ForceInitializeComponent<AudioSource>();

			if (connectionPrefab == null) { Debug.LogWarning("Connection prefab not set in the inspector!"); }
		}

		#endregion


		#region Control

		public void PauseSpatialAudio()
		{
			foreach (AudioConnection connection in connections) { connection.source.Pause(); }
		}

		public void ResumeSpatialAudio()
		{
			foreach (AudioConnection connection in connections) { connection.source.UnPause(); }
		}

		public void DirectPlayOnce(AudioClip clip, float volume = 0.5f)
		{
			directSource.PlayOneShot(clip, volume);
		}

		public void DirectPlayLoop(AudioClip clip, float volume = 0.5f)
		{
			directSource.clip = clip;
			directSource.loop = true;
			directSource.volume = volume;
			directSource.Play();
		}

		#endregion


		#region Management

		public void AddTransmitter(AudioTransmitter transmitter)
		{
			transmitters.Add(transmitter);

			foreach (AudioReceiver receiver in receivers)
			{
				connections.Add(NewAudioConnection(transmitter, receiver));
			}
		}

		public void AddReceiver(AudioReceiver receiver)
		{
			receivers.Add(receiver);

			foreach (AudioTransmitter transmitter in transmitters)
			{
				connections.Add(NewAudioConnection(transmitter, receiver));
			}
		}

		AudioConnection NewAudioConnection(AudioTransmitter transmitter, AudioReceiver receiver)
		{
			GameObject newConnectionObject = Instantiate(connectionPrefab, transform);
			AudioConnection newConnection = newConnectionObject.GetComponent<AudioConnection>();
			AudioSource newConnectionSource = newConnectionObject.GetComponentInChildren<AudioSource>();
			newConnection.Initialize(transmitter, receiver, newConnectionSource);
			return newConnection;
		}

		public void RemoveTransmitter(AudioTransmitter transmitter)
		{
			transmitters.Remove(transmitter);

			List<AudioConnection> connectionsToRemove = new List<AudioConnection>();
			foreach (AudioConnection connection in connections)
			{
				if (connection.transmitter == transmitter)
				{
					connectionsToRemove.Add(connection);
				}
			}

			for (int i = 0; i < connectionsToRemove.Count; i++)
			{
				connections.Remove(connectionsToRemove[i]);
				if (connectionsToRemove[i] != null)
				{
					Destroy(connectionsToRemove[i].gameObject);
				}
			}
		}

		public void RemoveReceiver(AudioReceiver receiver)
		{
			receivers.Remove(receiver);

			List<AudioConnection> connectionsToRemove = new List<AudioConnection>();
			foreach (AudioConnection connection in connections)
			{
				if (connection.receiver == receiver)
				{
					connectionsToRemove.Add(connection);
				}
			}

			for (int i = 0; i < connectionsToRemove.Count; i++)
			{
				connections.Remove(connectionsToRemove[i]);
				if (connectionsToRemove[i] != null)
				{
					Destroy(connectionsToRemove[i].gameObject);
				}
			}
		}

		#endregion
	}
}
