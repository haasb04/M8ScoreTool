using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace M8ScoreLibrary {
	[Serializable()]
	public class MatchSettings {
		[DisplayName("Delta Multiplier")]
		[DefaultValue(3)]
		public int WinMultiplier { get; set; }
		[DefaultValue(325)]
		[DisplayName("Bonus/Penalty Limit")]
		public int OverUnderLimit { get; set; }
		[DefaultValue(5)]
		[DisplayName("Penalty Multiplier")]
		public int PenaltyMultiplier { get; set; }
		[DisplayName("Non-Rated Player Rate")]
		[DefaultValue(50)]
		public int NonRatedPlayerRate { get; set; }
		[DefaultValue(true)]
		[DisplayName("Regular Season Match")]
		public bool IsRegularSeason { get; set; }

		public MatchSettings() {
			WinMultiplier = 3;
			OverUnderLimit = 325;
			PenaltyMultiplier = 5;
			NonRatedPlayerRate = 50;
			IsRegularSeason = true;
		}

		public void Save(string path) {
			try {
				XmlSerializer serializer = new XmlSerializer(typeof(MatchSettings));
				using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write)) {
					serializer.Serialize(fs, this);
				}
			} catch(Exception) {
				//todo: log. 
				throw;
			}
		}

		public static MatchSettings LoadFromXml(string path) {
			try {
				XmlSerializer serializer = new XmlSerializer(typeof(MatchSettings));
				using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
					return (MatchSettings)serializer.Deserialize(fs);
				}
			} catch(Exception) {
				//todo: log. 
				return null;
			}
		}

		public static string PathString() {
			return "~/App_Data/MatchSettings.Xml";
		}
	}
}
