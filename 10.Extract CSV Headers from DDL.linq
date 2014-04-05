<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

// Because AdventureWorks CSV files don't have a header row,
// we need to extract the schema from somewhere.
// Regex'ing the installation DDL is relatively simple
//var basedir = @"C:\Users\Piers\Downloads\AdventureWorks 2012 OLTP Script";
var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
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