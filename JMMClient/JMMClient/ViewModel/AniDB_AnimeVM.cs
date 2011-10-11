﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using JMMClient.ViewModel;
using System.ComponentModel;
using NLog;

namespace JMMClient
{
	public class AniDB_AnimeVM :  INotifyPropertyChanged
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public int AnimeID { get; set; }
		public int EpisodeCount { get; set; }
		public DateTime? AirDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string URL { get; set; }
		public string Picname { get; set; }
		public int BeginYear { get; set; }
		public int EndYear { get; set; }
		public int AnimeType { get; set; }
		public string MainTitle { get; set; }
		public string FormattedTitle { get; set; }
		public string AllTitles { get; set; }
		public string AllCategories { get; set; }
		public string AllTags { get; set; }
		public string Description { get; set; }
		public int EpisodeCountNormal { get; set; }
		public int EpisodeCountSpecial { get; set; }
		public int Rating { get; set; }
		public int VoteCount { get; set; }
		public int TempRating { get; set; }
		public int TempVoteCount { get; set; }
		public int AvgReviewRating { get; set; }
		public int ReviewCount { get; set; }
		public DateTime DateTimeUpdated { get; set; }
		public DateTime DateTimeDescUpdated { get; set; }
		public int ImageEnabled { get; set; }
		public string AwardList { get; set; }
		public int Restricted { get; set; }
		public int? AnimePlanetID { get; set; }
		public int? ANNID { get; set; }
		public int? AllCinemaID { get; set; }
		public int? AnimeNfo { get; set; }
		public int? LatestEpisodeNumber { get; set; }

		public AniDB_Anime_DefaultImageVM DefaultPoster { get; set; }
		public AniDB_Anime_DefaultImageVM DefaultFanart { get; set; }
		public AniDB_Anime_DefaultImageVM DefaultWideBanner { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName)
		{
			if (PropertyChanged != null)
			{
				var args = new PropertyChangedEventArgs(propertyName);
				PropertyChanged(this, args);
			}
		}

		public AniDB_AnimeVM()
		{
		}

		public AniDB_AnimeVM(JMMServerBinary.Contract_AniDBAnime contract)
		{
			this.AirDate = contract.AirDate;
			this.AllCategories = contract.AllCategories;
			this.AllCinemaID = contract.AllCinemaID;
			this.AllTags = contract.AllTags;
			this.AllTitles = contract.AllTitles;
			this.AnimeID = contract.AnimeID;
			this.AnimeNfo = contract.AnimeNfo;
			this.AnimePlanetID = contract.AnimePlanetID;
			this.AnimeType = contract.AnimeType;
			this.ANNID = contract.ANNID;
			this.AvgReviewRating = contract.AvgReviewRating;
			this.AwardList = contract.AwardList;
			this.BeginYear = contract.BeginYear;
			this.Description = contract.Description;
			this.DateTimeDescUpdated = contract.DateTimeDescUpdated;
			this.DateTimeUpdated = contract.DateTimeUpdated;
			this.EndDate = contract.EndDate;
			this.EndYear = contract.EndYear;
			this.EpisodeCount = contract.EpisodeCount;
			this.EpisodeCountNormal = contract.EpisodeCountNormal;
			this.EpisodeCountSpecial = contract.EpisodeCountSpecial;
			this.ImageEnabled = contract.ImageEnabled;
			this.LatestEpisodeNumber = contract.LatestEpisodeNumber;
			this.MainTitle = contract.MainTitle;
			this.Picname = contract.Picname;
			this.Rating = contract.Rating;
			this.Restricted = contract.Restricted;
			this.ReviewCount = contract.ReviewCount;
			this.TempRating = contract.TempRating;
			this.TempVoteCount = contract.TempVoteCount;
			this.URL = contract.URL;
			this.VoteCount = contract.VoteCount;
			this.FormattedTitle = contract.FormattedTitle;

			IsImageEnabled = ImageEnabled == 1;
			IsImageDisabled = ImageEnabled != 1;

			if (contract.DefaultImagePoster != null)
				DefaultPoster = new AniDB_Anime_DefaultImageVM(contract.DefaultImagePoster);
			else
				DefaultPoster = null;

			if (contract.DefaultImageFanart != null)
				DefaultFanart = new AniDB_Anime_DefaultImageVM(contract.DefaultImageFanart);
			else
				DefaultFanart = null;

			if (contract.DefaultImageWideBanner != null)
				DefaultWideBanner = new AniDB_Anime_DefaultImageVM(contract.DefaultImageWideBanner);
			else
				DefaultWideBanner = null;

			bool isDefault = false;
			if (DefaultPoster != null && DefaultPoster.ImageParentType == (int)ImageEntityType.AniDB_Cover)
				isDefault = true;

			IsImageDefault = isDefault;
			IsImageNotDefault = !isDefault;
		}

		private AniDB_AnimeCrossRefsVM aniDB_AnimeCrossRefs = null;
		public AniDB_AnimeCrossRefsVM AniDB_AnimeCrossRefs
		{
			get
			{
				if (aniDB_AnimeCrossRefs != null) return aniDB_AnimeCrossRefs;
				RefreshAnimeCrossRefs();
				return aniDB_AnimeCrossRefs;
			}
		}

		public void RefreshAnimeCrossRefs()
		{
			JMMServerBinary.Contract_AniDB_AnimeCrossRefs xrefDetails = JMMServerVM.Instance.clientBinaryHTTP.GetCrossRefDetails(this.AnimeID);
			if (xrefDetails == null) return;

			aniDB_AnimeCrossRefs = new AniDB_AnimeCrossRefsVM();
			aniDB_AnimeCrossRefs.Populate(xrefDetails);
		}

		private bool isImageEnabled = false;
		public bool IsImageEnabled
		{
			get { return isImageEnabled; }
			set
			{
				isImageEnabled = value;
				NotifyPropertyChanged("IsImageEnabled");
			}
		}

		private bool isImageDisabled = false;
		public bool IsImageDisabled
		{
			get { return isImageDisabled; }
			set
			{
				isImageDisabled = value;
				NotifyPropertyChanged("IsImageDisabled");
			}
		}

		private bool isImageDefault = false;
		public bool IsImageDefault
		{
			get { return isImageDefault; }
			set
			{
				isImageDefault = value;
				NotifyPropertyChanged("IsImageDefault");
			}
		}

		private bool isImageNotDefault = false;
		public bool IsImageNotDefault
		{
			get { return isImageNotDefault; }
			set
			{
				isImageNotDefault = value;
				NotifyPropertyChanged("IsImageNotDefault");
			}
		}

		public string PosterPathNoDefault
		{
			get
			{
				string fileName = Path.Combine(Utils.GetAniDBImagePath(AnimeID), Picname);
				return fileName;
			}
		}

		public string PosterPath
		{
			get
			{
				string fileName = Path.Combine(Utils.GetAniDBImagePath(AnimeID), Picname);

				if (!File.Exists(fileName))
				{
					string packUriBlank = string.Format("pack://application:,,,/{0};component/Images/blankposter.png", Constants.AssemblyName);
					return packUriBlank;
				}
				return fileName;
			}
		}

		public string FullImagePath
		{
			get
			{
				return PosterPath;
			}
		}

		public string DefaultPosterPath
		{
			get
			{
				if (DefaultPoster == null)
					return PosterPath;
				else
				{
					ImageEntityType imageType = (ImageEntityType)DefaultPoster.ImageParentType;

					switch (imageType)
					{
						case ImageEntityType.AniDB_Cover:
							return this.PosterPath;

						case ImageEntityType.TvDB_Cover:
							if (DefaultPoster.TVPoster != null)
								return DefaultPoster.TVPoster.FullImagePath;
							else
								return this.PosterPath;

						case ImageEntityType.Trakt_Poster:
							if (DefaultPoster.TraktPoster != null)
								return DefaultPoster.TraktPoster.FullImagePath;
							else
								return this.PosterPath;

						case ImageEntityType.MovieDB_Poster:
							if (DefaultPoster.MoviePoster != null)
								return DefaultPoster.MoviePoster.FullImagePath;
							else
								return this.PosterPath;
					}
				}

				return PosterPath;
			}
		}

		public string AirDateAsString
		{
			get
			{
				if (AirDate.HasValue)
					return AirDate.Value.ToString("dd MMM yyyy", Globals.Culture);
				else
					return "";
			}
		}

		public bool FinishedAiring
		{
			get
			{
				if (!EndDate.HasValue) return false; // ongoing

				// all series have finished airing and the user has all the episodes
				if (EndDate.Value < DateTime.Now) return true;

				return false;
			}
		}

		public bool FanartExists
		{
			get
			{
				if (AniDB_AnimeCrossRefs == null) return false;

				if (AniDB_AnimeCrossRefs.AllFanarts.Count > 0)
					return true;
				else
					return false;

			}
		}

		public bool FanartMissing
		{
			get
			{
				return !FanartExists;
			}
		}

		public string FanartPath
		{
			get
			{
				string packUriBlank = string.Format("pack://application:,,,/{0};component/Images/blankposter.png", Constants.AssemblyName);

				// this should be randomised or use the default 
				if (DefaultFanart != null)
					return DefaultFanart.FullImagePath;

				if (AniDB_AnimeCrossRefs == null)
					return packUriBlank;

				if (AniDB_AnimeCrossRefs.AllFanarts.Count == 0)
					return packUriBlank;

				if (File.Exists(AniDB_AnimeCrossRefs.AllFanarts[0].FullImagePath))
					return AniDB_AnimeCrossRefs.AllFanarts[0].FullImagePath;

				return packUriBlank;
			}
		}

		public string FanartThumbnailPath
		{
			get
			{
				string packUriBlank = string.Format("pack://application:,,,/{0};component/Images/blankposter.png", Constants.AssemblyName);

				// this should be randomised or use the default 
				if (DefaultFanart != null)
					return DefaultFanart.FullThumbnailPath;

				if (AniDB_AnimeCrossRefs == null)
					return packUriBlank;

				if (AniDB_AnimeCrossRefs.AllFanarts.Count == 0)
					return packUriBlank;

				if (File.Exists(AniDB_AnimeCrossRefs.AllFanarts[0].FullThumbnailPath))
					return AniDB_AnimeCrossRefs.AllFanarts[0].FullImagePath;

				return packUriBlank;
			}
		}

		public string EndDateAsString
		{
			get
			{
				if (EndDate.HasValue)
					return EndDate.Value.ToString("dd MMM yyyy", Globals.Culture);
				else
					return JMMClient.Properties.Resources.Ongoing;
			}
		}

		public string AirDateAndEndDate
		{
			get
			{
				return string.Format("{0}  {1}  {2}", AirDateAsString, JMMClient.Properties.Resources.To, EndDateAsString);
			}
		}

		public string BeginYearAndEndYear
		{
			get
			{
				if (BeginYear == EndYear) return BeginYear.ToString();
				else
					return string.Format("{0} - {1}", BeginYear, EndYear);
			}
		}

		public string AniDB_SiteURL
		{
			get
			{
				return string.Format(Constants.URLS.AniDB_Series, AnimeID);

			}
		}

		public string AnimeID_Friendly
		{
			get
			{
				return string.Format("AniDB: {0}", AnimeID);
			}
		}

		public enAnimeType AnimeTypeEnum
		{
			get
			{
				if (AnimeType > 5) return enAnimeType.Other;
				return (enAnimeType)AnimeType;
			}
		}

		public string AnimeTypeDescription
		{
			get
			{
				switch (AnimeTypeEnum)
				{
					case enAnimeType.Movie: return JMMClient.Properties.Resources.AnimeType_Movie;
					case enAnimeType.Other: return JMMClient.Properties.Resources.AnimeType_Other;
					case enAnimeType.OVA: return JMMClient.Properties.Resources.AnimeType_OVA;
					case enAnimeType.TVSeries: return JMMClient.Properties.Resources.AnimeType_TVSeries;
					case enAnimeType.TVSpecial: return JMMClient.Properties.Resources.AnimeType_TVSpecial;
					case enAnimeType.Web: return JMMClient.Properties.Resources.AnimeType_Web;
					default: return JMMClient.Properties.Resources.AnimeType_Other;
						
				}
			}
		}

		public decimal AniDBTotalRating
		{
			get
			{
				try
				{
					decimal totalRating = 0;
					totalRating += ((decimal)Rating * VoteCount);
					totalRating += ((decimal)TempRating * TempVoteCount);

					return totalRating;
				}
				catch (Exception ex)
				{
					return 0;
				}
			}
		}

		public int AniDBTotalVotes
		{
			get
			{
				try
				{
					return TempVoteCount + VoteCount;
				}
				catch (Exception ex)
				{
					return 0;
				}
			}
		}

		public decimal AniDBRating
		{
			get
			{
				try
				{
					if (AniDBTotalVotes == 0)
						return 0;
					else
						return AniDBTotalRating / (decimal)AniDBTotalVotes / (decimal)100;

				}
				catch (Exception ex)
				{
					return 0;
				}
			}
		}

		public string AniDBRatingFormatted
		{
			get
			{
				return string.Format("{0} ({1} {2})", Utils.FormatAniDBRating((double)AniDBRating),
					AniDBTotalVotes, JMMClient.Properties.Resources.Votes);
			}
		}

		

		private AniDB_AnimeDetailedVM detail = null;
		public AniDB_AnimeDetailedVM Detail
		{
			get
			{
				if (detail == null)
				{
					try
					{
						JMMServerBinary.Contract_AniDB_AnimeDetailed contract = JMMServerVM.Instance.clientBinaryHTTP.GetAnimeDetailed(this.AnimeID);
						detail = new AniDB_AnimeDetailedVM();
						detail.Populate(contract, this.AnimeID);
					}
					catch (Exception ex)
					{
						Utils.ShowErrorMessage(ex);
					}
				}
				return detail;

				/*if (MainListHelperVM.Instance.AllAnimeDetailedDictionary.ContainsKey(this.AnimeID))
					return MainListHelperVM.Instance.AllAnimeDetailedDictionary[this.AnimeID];
				else
					return null;*/
			}
		}

		private CrossRef_AniDB_TvDBVM crossRefTvDB = null;
		public CrossRef_AniDB_TvDBVM CrossRefTvDB
		{
			get
			{
				if (crossRefTvDB == null)
				{
					try
					{
						JMMServerBinary.Contract_CrossRef_AniDB_TvDB contract = JMMServerVM.Instance.clientBinaryHTTP.GetTVDBCrossRef(this.AnimeID);
						if (contract != null)
							crossRefTvDB = new CrossRef_AniDB_TvDBVM(contract);
					}
					catch (Exception ex)
					{
						Utils.ShowErrorMessage(ex);
					}
				}
				return crossRefTvDB;
			}
		}

		private Dictionary<int, TvDB_EpisodeVM> dictTvDBEpisodes = null;
		public Dictionary<int, TvDB_EpisodeVM> DictTvDBEpisodes
		{
			get
			{
				if (dictTvDBEpisodes == null)
				{
					try
					{
						if (TvDBEpisodes != null)
						{
							DateTime start = DateTime.Now;

							dictTvDBEpisodes = new Dictionary<int,TvDB_EpisodeVM>();
							// create a dictionary of absolute episode numbers for tvdb episodes
							// sort by season and episode number
							// ignore season 0, which is used for specials
							List<TvDB_EpisodeVM> eps = TvDBEpisodes;
							

							int i = 1;
							foreach (TvDB_EpisodeVM ep in eps)
							{
								//if (ep.SeasonNumber > 0)
								//{
									dictTvDBEpisodes[i] = ep;
									i++;
								//}

							}
							TimeSpan ts = DateTime.Now - start;
							logger.Trace("Got TvDB Episodes in {0} ms", ts.TotalMilliseconds);
						}
					}
					catch (Exception ex)
					{
						Utils.ShowErrorMessage(ex);
					}
				}
				return dictTvDBEpisodes;
			}
		}

		private Dictionary<int, int> dictTvDBSeasons = null;
		public Dictionary<int, int> DictTvDBSeasons
		{
			get
			{
				if (dictTvDBSeasons == null)
				{
					try
					{
						if (TvDBEpisodes != null)
						{
							DateTime start = DateTime.Now;

							dictTvDBSeasons = new Dictionary<int,int>();
							// create a dictionary of season numbers and the first episode for that season
							
							List<TvDB_EpisodeVM> eps = TvDBEpisodes;
							int i = 1;
							int lastSeason = -999;
							foreach (TvDB_EpisodeVM ep in eps)
							{
								if (ep.SeasonNumber != lastSeason)
									dictTvDBSeasons[ep.SeasonNumber] = i;

								lastSeason = ep.SeasonNumber;
								i++;

							}
							TimeSpan ts = DateTime.Now - start;
							logger.Trace("Got TvDB Seasons in {0} ms", ts.TotalMilliseconds);
						}
					}
					catch (Exception ex)
					{
						Utils.ShowErrorMessage(ex);
					}
				}
				return dictTvDBSeasons;
			}
		}

		private Dictionary<int, int> dictTvDBSeasonsSpecials = null;
		public Dictionary<int, int> DictTvDBSeasonsSpecials
		{
			get
			{
				if (dictTvDBSeasonsSpecials == null)
				{
					try
					{
						if (TvDBEpisodes != null)
						{
							DateTime start = DateTime.Now;

							dictTvDBSeasonsSpecials = new Dictionary<int, int>();
							// create a dictionary of season numbers and the first episode for that season

							List<TvDB_EpisodeVM> eps = TvDBEpisodes;
							int i = 1;
							int lastSeason = -999;
							foreach (TvDB_EpisodeVM ep in eps)
							{
								if (ep.SeasonNumber > 0) continue;

								int thisSeason = 0;
								if (ep.AirsBeforeSeason.HasValue) thisSeason = ep.AirsBeforeSeason.Value;
								if (ep.AirsAfterSeason.HasValue) thisSeason = ep.AirsAfterSeason.Value;

								if (thisSeason != lastSeason)
									dictTvDBSeasonsSpecials[thisSeason] = i;

								lastSeason = thisSeason;
								i++;

							}
							TimeSpan ts = DateTime.Now - start;
							logger.Trace("Got TvDB Seasons in {0} ms", ts.TotalMilliseconds);
						}
					}
					catch (Exception ex)
					{
						Utils.ShowErrorMessage(ex);
					}
				}
				return dictTvDBSeasonsSpecials;
			}
		}

		private List<TvDB_EpisodeVM> tvDBEpisodes = null;
		public List<TvDB_EpisodeVM> TvDBEpisodes
		{
			get
			{
				if (tvDBEpisodes == null)
				{
					try
					{
						if (CrossRefTvDB != null)
						{
							List<JMMServerBinary.Contract_TvDB_Episode> eps = JMMServerVM.Instance.clientBinaryHTTP.GetAllTvDBEpisodes(CrossRefTvDB.TvDBID);
							tvDBEpisodes = new List<TvDB_EpisodeVM>();
							foreach (JMMServerBinary.Contract_TvDB_Episode episode in eps)
								tvDBEpisodes.Add(new TvDB_EpisodeVM(episode));

							List<SortPropOrFieldAndDirection> sortCriteria = new List<SortPropOrFieldAndDirection>();
							sortCriteria.Add(new SortPropOrFieldAndDirection("SeasonNumber", false, SortType.eInteger));
							sortCriteria.Add(new SortPropOrFieldAndDirection("EpisodeNumber", false, SortType.eInteger));
							tvDBEpisodes = Sorting.MultiSort<TvDB_EpisodeVM>(tvDBEpisodes, sortCriteria);
						}
					}
					catch (Exception ex)
					{
						Utils.ShowErrorMessage(ex);
					}
				}
				return tvDBEpisodes;
			}
		}

		public void ClearTvDBData()
		{
			tvDBEpisodes = null;
			dictTvDBSeasonsSpecials = null;
			dictTvDBSeasons = null;
			dictTvDBEpisodes = null;
		}

		
	}
}