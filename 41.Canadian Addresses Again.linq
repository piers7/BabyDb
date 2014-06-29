<Query Kind="Program" />

static string dataDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");

void Main()
{
	GetCanadianAddressesAgain().Dump();
}

public static IEnumerable<object> GetCanadianAddressesAgain(){

	var stateProvince = MyExtensions.ReadCSV(Path.Combine(dataDir, "StateProvince.csv"))
		.Where (state => state.CountryRegionCode == "CA")
	;

	foreach(var state in stateProvince){		
		var matchingAddresses = ReadCSVFromIndex(dataDir, "Address.csv", "StateProvinceID", state.StateProvinceID);
		foreach(var address in matchingAddresses)
			yield return new { address.AddressLine1, address.City, StateName = state.Name };
	}
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