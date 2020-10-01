﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMReporting.Data.Results {
	public class MultipleChoiceResult : StringResult {
		public List<string> Options;
		public MultipleChoiceResult(string name, List<string> options) : base(name, options[0]) {
			Options = options;
		}
	}
}
