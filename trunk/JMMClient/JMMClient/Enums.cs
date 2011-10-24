﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMMClient
{
	public enum ImageEntityType
	{
		AniDB_Cover = 1, // use AnimeID
		AniDB_Character = 2, // use CharID
		AniDB_Creator = 3, // use CreatorID
		TvDB_Banner = 4, // use TvDB Banner ID
		TvDB_Cover = 5, // use TvDB Cover ID
		TvDB_Episode = 6, // use TvDB Episode ID
		TvDB_FanArt = 7, // use TvDB FanArt ID
		MovieDB_FanArt = 8,
		MovieDB_Poster = 9,
		Trakt_Poster = 10,
		Trakt_Fanart = 11,
		Trakt_Episode = 12
	}

	public enum ImageDownloadEventType
	{
		Started = 1,
		Complete = 2
	}

	public enum EpisodeType
	{
		Episode = 1,
		Credits = 2,
		Special = 3,
		Trailer = 4,
		Parody = 5,
		Other = 6
	}

	public enum enAnimeType
	{
		Movie = 0,
		OVA = 1,
		TVSeries = 2,
		TVSpecial = 3,
		Web = 4,
		Other = 5
	}

	public enum WatchedStatus
	{
		All = 1,
		Unwatched = 2,
		Watched = 3
	}

	public enum CrossrefSource
	{
		AniDB = 1,
		User = 2
	}

	public enum AniDBVoteType
	{
		Anime = 1,
		AnimeTemp = 2,
		Group = 3,
		Episode = 4
	}

	public class WatchedStatusContainer
	{
		public WatchedStatus WatchedStatus { get; set; }
		public string WatchedStatusDescription { get; set; }

		public WatchedStatusContainer()
		{
		}

		public WatchedStatusContainer(WatchedStatus status, string desc)
		{
			WatchedStatus = status;
			WatchedStatusDescription = desc;
		}

		public static List<WatchedStatusContainer> GetAll()
		{
			List<WatchedStatusContainer> statuses = new List<WatchedStatusContainer>();
			statuses.Add(new WatchedStatusContainer(WatchedStatus.All, JMMClient.Properties.Resources.Episodes_Watched_All));
			statuses.Add(new WatchedStatusContainer(WatchedStatus.Unwatched, JMMClient.Properties.Resources.Episodes_Watched_Unwatched));
			statuses.Add(new WatchedStatusContainer(WatchedStatus.Watched, JMMClient.Properties.Resources.Episodes_Watched_Watched));
			return statuses;
		}
	}

	public enum AvailableEpisodeType
	{
		All = 1,
		Available = 2,
		NoFiles = 3
	}

	public enum AnimeGroupSortMethod 
	{ 
		SortName = 0, 
		IsFave = 1 
	}

	public enum SortDirection
	{
		Ascending = 1,
		Descending = 2
	}

	public enum GroupFilterConditionType
	{
		CompletedSeries = 1,
		MissingEpisodes = 2,
		HasUnwatchedEpisodes = 3,
		AllEpisodesWatched = 4,
		UserVoted = 5,
		Category = 6,
		AirDate = 7,
		Studio = 8,
		AssignedTvDBInfo = 9,
		ReleaseGroup = 11, 
		AnimeType = 12,
		VideoQuality = 13,
		Favourite = 14,
		AnimeGroup = 15,
		AniDBRating = 16,
		UserRating = 17,
		SeriesCreatedDate = 18,
		EpisodeAddedDate = 19,
		EpisodeWatchedDate = 20,
		FinishedAiring = 21,
		MissingEpisodesCollecting = 22,
		AudioLanguage = 23,
		SubtitleLanguage = 24,
		AssignedTvDBOrMovieDBInfo = 25,
		AssignedMovieDBInfo = 26,
		UserVotedAny = 27
	}

	public enum GroupFilterOperator
	{
		Include = 1,
		Exclude = 2,
		GreaterThan = 3,
		LessThan = 4,
		Equals = 5,
		NotEquals = 6,
		In = 7,
		NotIn = 8,
		LastXDays = 9,
		InAllEpisodes = 10,
		NotInAllEpisodes = 11
	}

	public enum GroupFilterSorting
	{
		SeriesAddedDate = 1,
		EpisodeAddedDate = 2,
		EpisodeAirDate = 3,
		EpisodeWatchedDate = 4,
		GroupName = 5,
		Year = 6,
		SeriesCount = 7,
		UnwatchedEpisodeCount = 8,
		MissingEpisodeCount = 9,
		UserRating = 10,
		AniDBRating = 11,
		SortName = 12
	}

	public enum GroupFilterSortDirection
	{
		Asc = 1,
		Desc = 2
	}

	public enum GroupFilterBaseCondition
	{
		Include = 1,
		Exclude = 2
	}

	public enum EpisodeDisplayStyle
	{
		Always = 1,
		InExpanded = 2,
		Never = 3
	}

	public class AvailableEpisodeTypeContainer
	{
		public AvailableEpisodeType AvailableEpisodeType { get; set; }
		public string AvailableEpisodeTypeDescription { get; set; }

		public AvailableEpisodeTypeContainer()
		{
		}

		public AvailableEpisodeTypeContainer(AvailableEpisodeType eptype, string desc)
		{
			AvailableEpisodeType = eptype;
			AvailableEpisodeTypeDescription = desc;
		}

		public static List<AvailableEpisodeTypeContainer> GetAll()
		{
			List<AvailableEpisodeTypeContainer> eptypes = new List<AvailableEpisodeTypeContainer>();
			eptypes.Add(new AvailableEpisodeTypeContainer(AvailableEpisodeType.All, JMMClient.Properties.Resources.Episodes_AvAll));
			eptypes.Add(new AvailableEpisodeTypeContainer(AvailableEpisodeType.Available, JMMClient.Properties.Resources.Episodes_AvOnly));
			eptypes.Add(new AvailableEpisodeTypeContainer(AvailableEpisodeType.NoFiles, JMMClient.Properties.Resources.Episodes_AvMissing));
			return eptypes;
		}
	}

	public enum CrossRefType
	{
		MovieDB = 1,
		MyAnimeList = 2,
		AnimePlanet = 3,
		BakaBT = 4,
		TraktTV = 5,
		AnimeNano = 6,
		CrunchRoll = 7,
		Konachan = 8
	}

	public enum ImageSizeType
	{
		Poster = 1,
		Fanart = 2,
		WideBanner = 3
	}

	public enum SeriesWidgets
	{
		Categories = 1,
		Titles = 2,
		FileSummary = 3,
		TvDBLinks = 4,
		PlayNextEpisode = 5,
		Tags = 6
	}

	public enum DashboardWidgets
	{
		WatchNextEpisode = 1,
		SeriesMissingEpisodes = 2,
		MiniCalendar = 3,
		RecommendationsWatch = 4,
		RecommendationsDownload = 5
	}

	public enum DashWatchNextStyle
	{
		Simple = 1,
		Detailed = 2
	}

	public enum ScheduledUpdateFrequency
	{
		Never = 1,
		HoursSix = 2,
		HoursTwelve = 3,
		Daily = 4
	}
}