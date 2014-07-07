<Query Kind="Program" />

void Main()
{
	var dataDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	var people = ReadCSV(Path.Combine(dataDir, "Person.csv"), "+|");

	people
		.Take(100)
		.Dump();
}

public static IEnumerable<dynamic> ReadCSV(string path, string delimiter = "\t", string headerPath = null){
	if(headerPath==null)
		headerPath = Path.ChangeExtension(path, ".header");

	// First, read in the header info
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
			if(columnName.EndsWith("ID"))
				// hack to make ID columns numeric (ints)
				// is neccesary for Merge Joins to compare correctly
				output[columnName] = columnValue == "" ? (int?)null : Convert.ToInt32(columnValue);
			else
				// other than the above, everything else is just a string
				output[columnName] = columnValue;
		}
		output["__rawLine"] = line;
		yield return output;
	}
}
