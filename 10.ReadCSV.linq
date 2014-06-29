<Query Kind="Program" />

void Main()
{
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	var people = ReadCSV(Path.Combine(baseDir, "Person.csv"), "+|");

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
	foreach(var line in File.ReadLines(path)){
		var rowData = line.Split(new[]{delimiter}, StringSplitOptions.None);
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