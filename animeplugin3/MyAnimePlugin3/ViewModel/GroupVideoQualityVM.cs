﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAnimePlugin3.ViewModel
{
	public class GroupVideoQualityVM
	{
		public string GroupName { get; set; }
		public string GroupNameShort { get; set; }
		public int Ranking { get; set; }
		public string Resolution { get; set; }
		public string VideoSource { get; set; }
		public int VideoBitDepth { get; set; }
		public int FileCountNormal { get; set; }
		public bool NormalComplete { get; set; }
		public int FileCountSpecials { get; set; }
		public bool SpecialsComplete { get; set; }

		public List<int> NormalEpisodeNumbers { get; set; }
		public string NormalEpisodeNumberSummary { get; set; }

		public bool HasAnySpecials
		{
			get
			{
				return FileCountSpecials > 0;
			}
		}

		public GroupVideoQualityVM(JMMServerBinary.Contract_GroupVideoQuality contract)
		{
			this.GroupName = contract.GroupName;
			this.GroupNameShort = contract.GroupNameShort;
			this.Ranking = contract.Ranking;
			this.Resolution = contract.Resolution;
			this.VideoSource = contract.VideoSource;
			this.FileCountNormal = contract.FileCountNormal;
			this.FileCountSpecials = contract.FileCountSpecials;
			this.NormalComplete = contract.NormalComplete;
			this.SpecialsComplete = contract.SpecialsComplete;
			this.NormalEpisodeNumbers = contract.NormalEpisodeNumbers;
			this.NormalEpisodeNumberSummary = contract.NormalEpisodeNumberSummary;

			this.VideoBitDepth = contract.VideoBitDepth;
		}

		public string PrettyDescription
		{
			get
			{
				return this.ToString();
			}
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}/{2} - {3}/{4} Files", GroupNameShort, Resolution, VideoSource, FileCountNormal, FileCountSpecials);
		}
	}
}
