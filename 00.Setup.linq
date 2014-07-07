<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.FileSystem.dll</Reference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Security.AccessControl</Namespace>
</Query>

// This script sets up the data structures for the demo
// But first **YOU** will have to:
// - Download 'Adventure Works 2012 OLTP Script.zip' from http://msftdbprodsamples.codeplex.com/releases/view/55330
// - Extract it to the location specified by 'download.Dir' below
// - Make sure extracting didn't give you an 'extra level' in the output path
// I tried to automate this for you, but !@#$ https://twitter.com/MrPiers/status/486139685098385409

// == You may wish to change the following:
var download = new {
	// This is what **YOU** need to download
	// It's the 2012 Schema+CSV version of AdventureWorks
	// listed as 'Adventure Works 2012 OLTP Script' at http://msftdbprodsamples.codeplex.com/releases/view/55330
	Source = @"http://msftdbprodsamples.codeplex.com/downloads/get/353146",
	
	// This is where I'm going to look for the extracted AdventureWorks zip file
	Dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
	FileName = @"AdventureWorks 2012 OLTP Script",
};

// Check extracted zip exists
var extractedDir = Path.Combine(download.Dir, download.FileName);
if(!Directory.Exists(download.Dir)) Directory.CreateDirectory(download.Dir);
if(!Directory.Exists(extractedDir)){
	throw new FileNotFoundException(string.Format("I didn't find {0}. Did you follow the download instructions?", extractedDir));
}

var dataDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
foreach(var csv in Directory.EnumerateFiles(extractedDir, "*.csv")){
	File.Copy(csv, Path.Combine(dataDir, Path.GetFileName(csv)).Dump(), true);
}

// Because AdventureWorks CSV files don't have a header row,
// we need some extra metadata files to make all this work
// We can create these by Regex'ing the DDL's (CREATE TABLE) in the setup script
var ddlpath = Path.Combine(extractedDir, "instawdb.sql");
var text = File.ReadAllText(ddlpath);
var tablePattern = @"CREATE TABLE\s+(\[([^\]]+)\]\.\[([^\]]+)\])(.+?);";
var columnPattern = @"^\s*\[([^\]]+)\]";
var matches = 
	Regex.Matches(text, tablePattern, RegexOptions.Singleline)
		.Cast<Match>()
		.Select (m => new { 
			name = m.Groups[3].Value,  
			columns = Regex.Matches(m.Groups[4].Value, columnPattern, RegexOptions.Multiline)
				.Cast<Match>()
				.Select (ma => ma.Groups[1].Value)
	});
foreach(var match in matches){
	File.WriteAllLines(
		Path.Combine(dataDir, match.name + ".header").Dump(),
		match.columns
	);
}

Console.WriteLine("Done");