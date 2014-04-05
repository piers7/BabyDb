<Query Kind="Program" />

void Main()
{
	var basePath = @"C:\Users\Piers\Downloads\AdventureWorks 2012 OLTP Script";
	var people = ReadCSV(Path.Combine(basePath, "Person.csv"), "+|");

	people
		.Take(100)
		.Dump();
}

public static IEnumerable<dynamic> ReadCSV(string path, string delimiter = "\t"){
	// First, read in the header info
	var headerPath = Path.ChangeExtension(path, ".header");
	var header = File.ReadAllLines(headerPath);

	// Now read in the body, and line by line
	// map the columns to properties on a dynamic object based on the header names
	var rows = File.ReadLines(path)
		.Select (line => line.Split(new[]{delimiter}, StringSplitOptions.None));

	foreach(var rowData in rows){
		var output = (IDictionary<string,object>)new System.Dynamic.ExpandoObject();
		for (var i = 0; i < header.Length; i++)
		{
			var columnName = header[i];
			var columnValue = rowData[i];
			output[columnName] = columnValue;
		}
		yield return output;
	}
}
