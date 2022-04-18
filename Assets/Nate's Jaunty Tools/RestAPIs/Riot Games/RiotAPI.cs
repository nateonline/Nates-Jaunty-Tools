using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NatesJauntyTools.RestAPIs;

namespace NatesJauntyTools.RiotGames
{
	[CreateAssetMenu(menuName = "Nate's Jaunty Tools/Riot Games/Riot API")]
	public class RiotAPI : RestAPI
	{
		public string apiKey;

		Dictionary<string, string> Headers => new Dictionary<string, string>() { { "X-Riot-Token", apiKey } };


		public async void GetSummoner(string summonerName, Action<Summoner> callback)
		{
			Summoner summoner = await GetSummonerAsync(summonerName);
			callback?.Invoke(summoner);
		}

		public async Task<Summoner> GetSummonerAsync(string summonerName)
		{
			string url = $"https://na1.api.riotgames.com/lol/summoner/v4/summoners/by-name/{summonerName.Replace(" ", "%20")}";
			return await Get<Summoner>(url, Headers);
		}

		public void GetSummoners(List<string> summonerNames, Action<List<Summoner>> callback)
		{
			List<Summoner> summoners = new List<Summoner>();

			foreach (string summonerName in summonerNames) { GetSummoner(summonerName, CheckCallback); }


			void CheckCallback(Summoner summoner)
			{
				summoners.Add(summoner);
				if (summoners.Count == summonerNames.Count) { callback(summoners); }
			}
		}

		public async void GetMatch(string matchID, Action<Match> callback)
		{
			string url = $"https://americas.api.riotgames.com/lol/match/v5/matches/{matchID.ToUpper()}";
			await Get<Match>(url, Headers);
		}

		public async void GetMatches(List<string> matchIDs, Action<List<Match>> callback, Action<float> progressCallback = null)
		{
			List<Match> matches = new List<Match>();

			foreach (string matchID in matchIDs)
			{
				GetMatch(matchID, CheckCallback);
				await Task.Delay(1000);
			}


			void CheckCallback(Match match)
			{
				matches.Add(match);
				if (progressCallback != null) { progressCallback((float)matches.Count / (float)matchIDs.Count); }
				if (matches.Count == matchIDs.Count) { callback(matches); }
			}
		}

		public async void GetMatchHistory(Summoner summoner, int count, Action<List<string>> callback)
		{
			string url = $"https://americas.api.riotgames.com/lol/match/v5/matches/by-puuid/{summoner.puuid}/ids?&count={count}";
			await Get<List<string>>(url, Headers);
		}
	}
}
