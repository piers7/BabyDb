<Query Kind="Program" />

void Main()
{
	// Back to Canada
	// Scanning the whole Addresses table still seems inefficient
	// What if we had a magic way of locating Addresses by StateProvinceID?
	
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	GetCanadianAddressesAgain(baseDir).Dump();
}

public static IEnumerable<object> GetCanadianAddressesAgain(string baseDir){

	var stateProvince = MyExtensions.ReadCSV(Path.Combine(baseDir, "StateProvince.csv"))
		.Where (state => state.CountryRegionCode == "CA")
	;

	throw new NotImplementedException();
}

#region spoilers
/*
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
*/
#endregion