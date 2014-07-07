<Query Kind="Program" />

void Main()
{
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	GetCanadianAddressesAgain(baseDir).Dump();
}

public static IEnumerable<object> GetCanadianAddressesAgain(string baseDir){

	var stateProvince = MyExtensions.ReadCSV(Path.Combine(baseDir, "StateProvince.csv"))
		.Where (state => state.CountryRegionCode == "CA")
	;

	// Loop through the StateProvince table (the small one)
	foreach(var state in stateProvince){		
		// and for each row, spool the matching rows from Addresses (the big table)
		// via the appropriate index
		var matchingAddresses = ReadCSVFromIndex(baseDir, "Address.csv", "StateProvinceID", state.StateProvinceID);
		foreach(var address in matchingAddresses)
			yield return new { address.AddressLine1, address.City, StateName = state.Name };
	}
	
	// This is Nested Loops:
	// for each row in one table
	//  for each row in other table (or more likely, index portion)
	//   return output
}

public static IEnumerable<dynamic> ReadCSVFromIndex(
	string baseDir,
	string csvName,
	string indexName,
	object key
){
	var headerPath = Path.Combine(baseDir, Path.ChangeExtension(csvName, "header"));
	var indexPath = Path.Combine(baseDir, "Indexes", Path.GetFileNameWithoutExtension(csvName), indexName, String.Format("{0}.csv", key));
	//Console.WriteLine("Read from index {0}", indexPath);
	return MyExtensions.ReadCSV(indexPath, headerPath:headerPath);
}