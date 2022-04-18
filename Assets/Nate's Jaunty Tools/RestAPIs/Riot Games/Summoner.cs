using System;

namespace NatesJauntyTools.RiotGames
{
	[Serializable]
	public class Summoner
	{
		public string name;
		public string id;
		public string accountId;
		public string puuid;
		public int profileIconId;
		public long revisionDate;
		public int summonerLevel;
	}
}