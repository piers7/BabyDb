<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");

// First we need to copy all the AdventureWorks CSV files into Data subdir
Util.Cmd("xcopy", String.Format(@"""C:\Users\Piers\Downloads\AdventureWorks 2012 OLTP Script\*"" ""{0}"" /S /I /Q /Y", baseDir));

// Because AdventureWorks CSV files don't have a header row,
// we need some extra metadata files to make all this work
// We can create these by Regex'ing the DDL's (CREATE TABLE) in the setup script
var ddlpath = Path.Combine(baseDir, "instawdb.sql");
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
		Path.Combine(baseDir, match.name + ".header").Dump(),
		match.columns
	);
}