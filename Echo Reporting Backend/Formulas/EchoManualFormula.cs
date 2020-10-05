﻿using DICOMReporting.Data;

namespace DICOMReporting.Formulas {
	public class EchoManualFormula : IFormula {
		private Constants constants;
		public string ReportAnomaly(double measurement) {
			double ZScore = GetZScore(measurement);
			int bracket;
			if (ZScore < -2) {
				bracket = 0;
			}
			else if (ZScore <= 2) {
				bracket = 1;
			}
			else {
				bracket = 2;
			}

			if (constants.AnomalyPrefix) {
				return constants.Anomalies[bracket] + constants.MeasurementName;
			}
			else {
				return constants.Anomalies[bracket];
			}
		}
		public double GetZScore(double measurement) {
			return (measurement - constants.Average) / constants.SD;
		}
		public bool ZScoreable() => constants.Pd.PatientAge.Value >= 3;
		public static EchoManualFormula MVDecelTime(PatientData pd, string name) {
			double[] averages = { 145, 157, 172 };
			double[] sds = { 18, 19, 22 };
			return new EchoManualFormula(new Constants(averages, sds, pd, name, new[] { "Shortened", "Normal", "Prolonged" }));
		}
		public static EchoManualFormula LVIVRT(PatientData pd, string name) {
			double[] averages = { 62, 67, 74 };
			double[] sds = { 10, 10, 13 };
			return new EchoManualFormula(new Constants(averages, sds, pd, name, new[] { "Shortened", "Normal", "Prolonged" }));
		}
		private EchoManualFormula(Constants constants) {
			this.constants = constants;
		}
		private struct Constants {
			public PatientData Pd {
				get; private set;
			}
			public string MeasurementName {
				get; set;
			}
			public string[] Anomalies {
				get; private set;
			}
			public bool AnomalyPrefix {
				get; private set;
			}
			public double Average {
				get; private set;
			}
			public double SD {
				get; private set;
			}
			public Constants(double[] averages, double[] sds, PatientData pd, string name, string[] anomalies, bool prefix = true) {
				AnomalyPrefix = prefix;
				MeasurementName = name;
				Anomalies = anomalies;
				Pd = pd;
				double age = pd.PatientAge.Value;
				int bracket;
				if (age <= 8) {
					bracket = 0;
				}
				else if (age <= 12) {
					bracket = 1;
				}
				else {
					bracket = 2;
				}
				Average = averages[bracket];
				SD = sds[bracket];
			}
		}
	}
}
