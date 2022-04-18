using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NatesJauntyTools.RiotGames
{
	[Serializable]
	public class Match
	{
		public Metadata metadata;
		public Info info;


		[Serializable]
		public class Metadata
		{
			public string dataVersion;
			public string matchId;
			public List<string> participants;
		}

		[Serializable]
		public class Info
		{
			public long gameCreation;
			public int gameDuration;
			public long gameEndTimestamp;
			public long gameId;
			public string gameMode;
			public string gameName;
			public long gameStartTimestamp;
			public string gameType;
			public string gameVersion;
			public int mapId;
			public List<Participant> participants;
			public string platformId;
			public int queueId;
			public List<Team> teams;
			public string tournamentCode;
		}

		[Serializable]
		public class Participant
		{
			public int assists;
			public int baronKills;
			public int bountyLevel;
			public Challenges challenges;
			public int champExperience;
			public int champLevel;
			public ChampionID championId;
			public string championName;
			public int championTransform;
			public int consumablesPurchased;
			public int damageDealtToBuildings;
			public int damageDealtToObjectives;
			public int damageDealtToTurrets;
			public int damageSelfMitigated;
			public int deaths;
			public int detectorWardsPlaced;
			public int doubleKills;
			public int dragonKills;
			public bool firstBloodAssist;
			public bool firstBloodKill;
			public bool firstTowerAssist;
			public bool firstTowerKill;
			public bool gameEndedInEarlySurrender;
			public bool gameEndedInSurrender;
			public int goldEarned;
			public int goldSpent;
			public string individualPosition;
			public int inhibitorKills;
			public int inhibitorTakedowns;
			public int inhibitorsLost;
			public int item0;
			public int item1;
			public int item2;
			public int item3;
			public int item4;
			public int item5;
			public int item6;
			public int itemsPurchased;
			public int killingSprees;
			public int kills;
			public string lane;
			public int largestCriticalStrike;
			public int largestKillingSpree;
			public int largestMultiKill;
			public int longestTimeSpentLiving;
			public int magicDamageDealt;
			public int magicDamageDealtToChampions;
			public int magicDamageTaken;
			public int neutralMinionsKilled;
			public int nexusKills;
			public int nexusLost;
			public int nexusTakedowns;
			public int objectivesStolen;
			public int objectivesStolenAssists;
			public int participantId;
			public int pentaKills;
			public Perks perks;
			public int physicalDamageDealt;
			public int physicalDamageDealtToChampions;
			public int physicalDamageTaken;
			public int profileIcon;
			public string puuid;
			public int quadraKills;
			public string riotIdName;
			public string riotIdTagline;
			public string role;
			public int sightWardsBoughtInGame;
			public int spell1Casts;
			public int spell2Casts;
			public int spell3Casts;
			public int spell4Casts;
			public int summoner1Casts;
			public int summoner1Id;
			public int summoner2Casts;
			public int summoner2Id;
			public string summonerId;
			public int summonerLevel;
			public string summonerName;
			public bool teamEarlySurrendered;
			public int teamId;
			public string teamPosition;
			public int timeCCingOthers;
			public int timePlayed;
			public int totalDamageDealt;
			public int totalDamageDealtToChampions;
			public int totalDamageShieldedOnTeammates;
			public int totalDamageTaken;
			public int totalHeal;
			public int totalHealsOnTeammates;
			public int totalMinionsKilled;
			public int totalTimeCCDealt;
			public int totalTimeSpentDead;
			public int totalUnitsHealed;
			public int tripleKills;
			public int trueDamageDealt;
			public int trueDamageDealtToChampions;
			public int trueDamageTaken;
			public int turretKills;
			public int turretTakedowns;
			public int turretsLost;
			public int unrealKills;
			public int visionScore;
			public int visionWardsBoughtInGame;
			public int wardsKilled;
			public int wardsPlaced;
			public bool win;
		}

		[Serializable]
		public class Challenges
		{
			public int _12AssistStreakCount;
			public int abilityUses;
			public int acesBefore15Minutes;
			public double alliedJungleMonsterKills;
			public int baronTakedowns;
			public int blastConeOppositeOpponentCount;
			public int bountyGold;
			public int buffsStolen;
			public int completeSupportQuestInTime;
			public double controlWardTimeCoverageInRiverOrEnemyHalf;
			public int controlWardsPlaced;
			public double damagePerMinute;
			public double damageTakenOnTeamPercentage;
			public int dancedWithRiftHerald;
			public int deathsByEnemyChamps;
			public int dodgeSkillShotsSmallWindow;
			public int doubleAces;
			public int dragonTakedowns;
			public double earlyLaningPhaseGoldExpAdvantage;
			public double effectiveHealAndShielding;
			public int elderDragonKillsWithOpposingSoul;
			public int elderDragonMultikills;
			public int enemyChampionImmobilizations;
			public double enemyJungleMonsterKills;
			public int epicMonsterKillsNearEnemyJungler;
			public int epicMonsterKillsWithin30SecondsOfSpawn;
			public int epicMonsterSteals;
			public int epicMonsterStolenWithoutSmite;
			public int flawlessAces;
			public int fullTeamTakedown;
			public double gameLength;
			public int getTakedownsInAllLanesEarlyJungleAsLaner;
			public double goldPerMinute;
			public int hadAfkTeammate;
			public int hadOpenNexus;
			public int immobilizeAndKillWithAlly;
			public int initialBuffCount;
			public int initialCrabCount;
			public double jungleCsBefore10Minutes;
			public int junglerKillsEarlyJungle;
			public int junglerTakedownsNearDamagedEpicMonster;
			public int kTurretsDestroyedBeforePlatesFall;
			public double kda;
			public int killAfterHiddenWithAlly;
			public double killParticipation;
			public int killedChampTookFullTeamDamageSurvived;
			public int killsNearEnemyTurret;
			public int killsOnLanersEarlyJungleAsJungler;
			public int killsOnOtherLanesEarlyJungleAsLaner;
			public int killsOnRecentlyHealedByAramPack;
			public int killsUnderOwnTurret;
			public int killsWithHelpFromEpicMonster;
			public int knockEnemyIntoTeamAndKill;
			public int landSkillShotsEarlyGame;
			public int laneMinionsFirst10Minutes;
			public double laningPhaseGoldExpAdvantage;
			public int legendaryCount;
			public int lostAnInhibitor;
			public double maxCsAdvantageOnLaneOpponent;
			public int maxKillDeficit;
			public int maxLevelLeadLaneOpponent;
			public double moreEnemyJungleThanOpponent;
			public int multiKillOneSpell;
			public int multiTurretRiftHeraldCount;
			public int multikills;
			public int multikillsAfterAggressiveFlash;
			public int outerTurretExecutesBefore10Minutes;
			public int outnumberedKills;
			public int outnumberedNexusKill;
			public int perfectDragonSoulsTaken;
			public int perfectGame;
			public int pickKillWithAlly;
			public int poroExplosions;
			public int quickCleanse;
			public int quickFirstTurret;
			public int quickSoloKills;
			public int riftHeraldTakedowns;
			public int saveAllyFromDeath;
			public int scuttleCrabKills;
			public int skillshotsDodged;
			public int skillshotsHit;
			public int snowballsHit;
			public int soloBaronKills;
			public int soloKills;
			public int soloTurretsLategame;
			public int stealthWardsPlaced;
			public int survivedSingleDigitHpCount;
			public int survivedThreeImmobilizesInFight;
			public int takedownOnFirstTurret;
			public int takedowns;
			public int takedownsAfterGainingLevelAdvantage;
			public int takedownsBeforeJungleMinionSpawn;
			public int takedownsFirst25Minutes;
			public int takedownsInAlcove;
			public int takedownsInEnemyFountain;
			public int teamBaronKills;
			public double teamDamagePercentage;
			public int teamElderDragonKills;
			public int teamRiftHeraldKills;
			public int threeWardsOneSweeperCount;
			public int tookLargeDamageSurvived;
			public int turretPlatesTaken;
			public int turretTakedowns;
			public int turretsTakenWithRiftHerald;
			public int twentyMinionsIn3SecondsCount;
			public int unseenRecalls;
			public double visionScoreAdvantageLaneOpponent;
			public double visionScorePerMinute;
			public int wardTakedowns;
			public int wardTakedownsBefore20M;
			public int wardsGuarded;
			public int? highestChampionDamage;
			public int? teleportTakedowns;
			public double? earliestDragonTakedown;
			public double? firstTurretKilledTime;
			public int? highestWardKills;
			public int? mythicItemUsed;
			public int? highestCrowdControlScore;
		}

		[Serializable]
		public class Perks
		{
			public StatPerks statPerks;
			public List<Style> styles;
		}

		[Serializable]
		public class StatPerks
		{
			public int defense;
			public int flex;
			public int offense;
		}

		[Serializable]
		public class Style
		{
			public string description;
			public List<Selection> selections;
			public int style;
		}

		[Serializable]
		public class Selection
		{
			public int perk;
			public int var1;
			public int var2;
			public int var3;
		}

		[Serializable]
		public class Team
		{
			public List<Ban> bans;
			public Objectives objectives;
			public int teamId;
			public bool win;
		}

		[Serializable]
		public class Ban
		{
			public ChampionID championId;
			public int pickTurn;
		}

		[Serializable]
		public class Objectives
		{
			public ObjectiveEntry baron;
			public ObjectiveEntry champion;
			public ObjectiveEntry dragon;
			public ObjectiveEntry inhibitor;
			public ObjectiveEntry riftHerald;
			public ObjectiveEntry tower;
		}

		[Serializable]
		public class ObjectiveEntry
		{
			public bool first;
			public int kills;
		}
	}
}