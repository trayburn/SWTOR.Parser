using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
	public class Parser
	{
		public IList<LogEntry> Parse(System.IO.TextReader rdr)
		{
			if (rdr == null) throw new ArgumentNullException("rdr");
			
			string line = null;
			var list = new List<LogEntry>();
			
			while ((line = rdr.ReadLine()) != null)
			{
				var entry = new LogEntry();
				
				int start = line.IndexOf('[') + 1;
				int end = line.IndexOf(']',start);
				
				string tStamp = line.Substring(start,end-start);
				entry.Timestamp = Convert.ToDateTime(tStamp);
				
				var rest = line.Substring(end);
				
				start = rest.IndexOf('[') + 1;
				end = rest.IndexOf(']',start);
				
				var source = rest.Substring(start,end-start);
				entry.Source = new Actor();
				entry.Source.IsPlayer = source.StartsWith("@");
				entry.Source.Name = source.Replace("@","");
				
				rest = rest.Substring(end);
				
			 	list.Add(entry);
			}
			
			return list;
		}
		
		private BetweenResult Between(char startChar, char endChar, string line)
		{
			var result = new BetweenResult();
			
			int start = line.IndexOf(startChar) + 1;
			int end = line.IndexOf(endChar,start);
			
			result.FoundValue = line.Substring(start,end-start);
			result.Rest = line.Substring(end);
			
		}
		
		class BetweenResult
		{
			public string FoundValue {get;set;}
			public string Rest {get;set;}
		}
	}
	
	public class LogEntry
	{
		public DateTime Timestamp {get;set;}
		public Actor Source {get;set;}
		public Actor Target {get;set;}
	}
	
	public class Actor
	{
		public string Name {get;set;}
		public bool IsPlayer {get;set;}
	}
}

