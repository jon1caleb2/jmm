﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMMServer.Entities;
using System.IO;
using JMMServer.Repositories;
using JMMContracts;
using JMMFileHelper;
using AniDBAPI;
using System.Xml;
using JMMServer.WebCache;
using JMMServer.Commands.MAL;
using JMMServer.Commands.AniDB;

namespace JMMServer.Commands
{
	[Serializable]
	public class CommandRequest_ProcessFile : CommandRequestImplementation, ICommandRequest
	{

		public int VideoLocalID { get; set; }
		public bool ForceAniDB { get; set; }

		private VideoLocal vlocal = null;

		public CommandRequestPriority DefaultPriority
		{
			get { return CommandRequestPriority.Priority3; }
		}

		public string PrettyDescription
		{
			get
			{
				if (vlocal != null)
					return string.Format("Getting file info from UDP API: {0}", vlocal.FullServerPath);
				else
					return string.Format("Getting file info from UDP API: {0}", VideoLocalID);
			}
		}

		public CommandRequest_ProcessFile()
		{
		}

		public CommandRequest_ProcessFile(int vidLocalID, bool forceAniDB)
		{
			this.VideoLocalID = vidLocalID;
			this.ForceAniDB = forceAniDB;
			this.CommandType = (int)CommandRequestType.ProcessFile;
			this.Priority = (int)DefaultPriority;

			GenerateCommandID();
		}

		public override void ProcessCommand()
		{
			logger.Info("Processing File: {0}", VideoLocalID);

			
			try
			{
				VideoLocalRepository repVids = new VideoLocalRepository();
				vlocal = repVids.GetByID(VideoLocalID);
				if (vlocal == null) return;

				//now that we have all the has info, we can get the AniDB Info
				ProcessFile_AniDB(vlocal);
			}
			catch (Exception ex)
			{
				logger.Error("Error processing CommandRequest_ProcessFile: {0} - {1}", VideoLocalID, ex.ToString());
				return;
			}

			// TODO update stats for group and series

			// TODO check for TvDB
		}

		private void ProcessFile_AniDB(VideoLocal vidLocal)
		{
			logger.Trace("Checking for AniDB_File record for: {0} --- {1}", vidLocal.Hash, vidLocal.FilePath);
			// check if we already have this AniDB_File info in the database
			
			AniDB_FileRepository repAniFile = new AniDB_FileRepository();
			AniDB_EpisodeRepository repAniEps = new AniDB_EpisodeRepository();
			AniDB_AnimeRepository repAniAnime = new AniDB_AnimeRepository();
			AnimeSeriesRepository repSeries = new AnimeSeriesRepository();
			VideoLocalRepository repVidLocals = new VideoLocalRepository();
			AnimeEpisodeRepository repEps = new AnimeEpisodeRepository();
			CrossRef_File_EpisodeRepository repXrefFE = new CrossRef_File_EpisodeRepository();

			AniDB_File aniFile = null;

			if (!ForceAniDB)
			{
				aniFile = repAniFile.GetByHashAndFileSize(vidLocal.Hash, vlocal.FileSize);

				if (aniFile == null)
					logger.Trace("AniDB_File record not found");
			}

			int animeID = 0;

			if (aniFile == null)
			{
				// get info from AniDB
				logger.Debug("Getting AniDB_File record from AniDB....");
				Raw_AniDB_File fileInfo = JMMService.AnidbProcessor.GetFileInfo(vidLocal);
				if (fileInfo != null)
				{
					// check if we already have a record
					aniFile = repAniFile.GetByHashAndFileSize(vidLocal.Hash, vlocal.FileSize);

					if (aniFile == null)
						aniFile = new AniDB_File();

					aniFile.Populate(fileInfo);

					//overwrite with local file name
					string localFileName = Path.GetFileName(vidLocal.FilePath);
					aniFile.FileName = localFileName;

					repAniFile.Save(aniFile, false);
					aniFile.CreateLanguages();
					aniFile.CreateCrossEpisodes(localFileName);

                    if (!string.IsNullOrEmpty(fileInfo.OtherEpisodesRAW))
                    {
                        string[] epIDs = fileInfo.OtherEpisodesRAW.Split(',');
                        foreach (string epid in epIDs)
                        {
                            int id = 0;
                            if (int.TryParse(epid, out id))
                            {
                                CommandRequest_GetEpisode cmdEp = new CommandRequest_GetEpisode(id);
                                cmdEp.Save();
                            }
                        }
                    }

					animeID = aniFile.AnimeID;
				}
			}

			bool missingEpisodes = false;

			// if we still haven't got the AniDB_File Info we try the web cache or local records
			if (aniFile == null)
			{
				// check if we have any records from previous imports
				List<CrossRef_File_Episode> crossRefs = repXrefFE.GetByHash(vidLocal.Hash);
				if (crossRefs == null || crossRefs.Count == 0)
				{
					// lets see if we can find the episode/anime info from the web cache
					if (ServerSettings.WebCache_XRefFileEpisode_Get)
					{
						crossRefs = XMLService.Get_CrossRef_File_Episode(vidLocal);
						if (crossRefs == null || crossRefs.Count == 0)
						{
							logger.Debug("Cannot find AniDB_File record or get cross ref from web cache record so exiting: {0}", vidLocal.ED2KHash);
							return;
						}
						else
						{
							foreach (CrossRef_File_Episode xref in crossRefs)
							{
								// in this case we need to save the cross refs manually as AniDB did not provide them
								repXrefFE.Save(xref);
							}
						}
					}
					else
					{
						logger.Debug("Cannot get AniDB_File record so exiting: {0}", vidLocal.ED2KHash);
						return;
					}
				}

				// we assume that all episodes belong to the same anime
				foreach (CrossRef_File_Episode xref in crossRefs)
				{
					animeID = xref.AnimeID;
					
					AniDB_Episode ep = repAniEps.GetByEpisodeID(xref.EpisodeID);
					if (ep == null) missingEpisodes = true;
				}
			}
			else
			{
				// check if we have the episode info
				// if we don't, we will need to re-download the anime info (which also has episode info)

				if (aniFile.EpisodeCrossRefs.Count == 0)
				{
					animeID = aniFile.AnimeID;

					// if we have the anidb file, but no cross refs it means something has been broken
					logger.Debug("Could not find any cross ref records for: {0}", vidLocal.ED2KHash);
					missingEpisodes = true;
				}
				else
				{
					foreach (CrossRef_File_Episode xref in aniFile.EpisodeCrossRefs)
					{
						AniDB_Episode ep = repAniEps.GetByEpisodeID(xref.EpisodeID);
						if (ep == null)
							missingEpisodes = true;

						animeID = xref.AnimeID;
					}
				}
			}

			// get from DB
			AniDB_Anime anime = repAniAnime.GetByAnimeID(animeID);
			bool animeRecentlyUpdated = false;

			if (anime != null)
			{
				TimeSpan ts = DateTime.Now - anime.DateTimeUpdated;
				if (ts.TotalHours < 4) animeRecentlyUpdated = true;
			}

			// even if we are missing episode info, don't get data  more than once every 4 hours
			// this is to prevent banning
			if (missingEpisodes && !animeRecentlyUpdated)
			{
				logger.Debug("Getting Anime record from AniDB....");

				// try using the cache first
				using (var session = JMMService.SessionFactory.OpenSession())
				{
					anime = JMMService.AnidbProcessor.GetAnimeInfoHTTPFromCache(session, animeID, ServerSettings.AutoGroupSeries);
				}

				// if not in cache try from AniDB
				if (anime == null)
					anime = JMMService.AnidbProcessor.GetAnimeInfoHTTP(animeID, true, ServerSettings.AutoGroupSeries);
			}

			// create the group/series/episode records if needed
			AnimeSeries ser = null;
			if (anime != null)
			{
				logger.Debug("Creating groups, series and episodes....");
				// check if there is an AnimeSeries Record associated with this AnimeID
				ser = repSeries.GetByAnimeID(animeID);
				if (ser == null)
				{
					// create a new AnimeSeries record
					ser = anime.CreateAnimeSeriesAndGroup();
				}

				
				ser.CreateAnimeEpisodes();

				// check if we have any group status data for this associated anime
				// if not we will download it now
				AniDB_GroupStatusRepository repStatus = new AniDB_GroupStatusRepository();
				if (repStatus.GetByAnimeID(anime.AnimeID).Count == 0)
				{
					CommandRequest_GetReleaseGroupStatus cmdStatus = new CommandRequest_GetReleaseGroupStatus(anime.AnimeID, false);
					cmdStatus.Save();
				}

				// update stats
				ser.EpisodeAddedDate = DateTime.Now;
				repSeries.Save(ser);

				AnimeGroupRepository repGroups = new AnimeGroupRepository();
				foreach (AnimeGroup grp in ser.AllGroupsAbove)
				{
					grp.EpisodeAddedDate = DateTime.Now;
					repGroups.Save(grp);
				}
			}

			vidLocal.RenameIfRequired();
			vidLocal.MoveFileIfRequired();
			

			// update stats for groups and series
			if (ser != null)
			{
				// update all the groups above this series in the heirarchy
				ser.UpdateStats(true, true, true);
				StatsCache.Instance.UpdateUsingSeries(ser.AnimeSeriesID);
			}
			

			// Add this file to the users list
			if (ServerSettings.AniDB_MyList_AddFiles)
			{
				CommandRequest_AddFileToMyList cmd = new CommandRequest_AddFileToMyList(vidLocal.ED2KHash);
				cmd.Save();
			}

			// lets also try adding to the users trakt collecion by sync'ing the series
			if (ser != null)
			{
				CommandRequest_TraktSyncCollectionSeries cmdTrakt = new CommandRequest_TraktSyncCollectionSeries(ser.AnimeSeriesID, ser.GetAnime().MainTitle);
				cmdTrakt.Save();
			}

			// sync the series on MAL
			if (ser != null)
			{
				CommandRequest_MALUpdatedWatchedStatus cmdMAL = new CommandRequest_MALUpdatedWatchedStatus(ser.AniDB_ID);
				cmdMAL.Save();
			}
		}

		/// <summary>
		/// This should generate a unique key for a command
		/// It will be used to check whether the command has already been queued before adding it
		/// </summary>
		public override void GenerateCommandID()
		{
			this.CommandID = string.Format("CommandRequest_ProcessFile_{0}", this.VideoLocalID);
		}

		public override bool LoadFromDBCommand(CommandRequest cq)
		{
			this.CommandID = cq.CommandID;
			this.CommandRequestID = cq.CommandRequestID;
			this.CommandType = cq.CommandType;
			this.Priority = cq.Priority;
			this.CommandDetails = cq.CommandDetails;
			this.DateTimeUpdated = cq.DateTimeUpdated;

			// read xml to get parameters
			if (this.CommandDetails.Trim().Length > 0)
			{
				XmlDocument docCreator = new XmlDocument();
				docCreator.LoadXml(this.CommandDetails);

				// populate the fields
				this.VideoLocalID = int.Parse(TryGetProperty(docCreator, "CommandRequest_ProcessFile", "VideoLocalID"));
				this.ForceAniDB = bool.Parse(TryGetProperty(docCreator, "CommandRequest_ProcessFile", "ForceAniDB"));
			}

			return true;
		}

		public override CommandRequest ToDatabaseObject()
		{
			GenerateCommandID();

			CommandRequest cq = new CommandRequest();
			cq.CommandID = this.CommandID;
			cq.CommandType = this.CommandType;
			cq.Priority = this.Priority;
			cq.CommandDetails = this.ToXML();
			cq.DateTimeUpdated = DateTime.Now;

			return cq;
		}
	}
}
