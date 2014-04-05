<Query Kind="Program" />

void Main()
{
	GetCanadianAddressesAgain().Dump();
}

public static IEnumerable<object> GetCanadianAddressesAgain(){
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	//var addresses = MyExtensions.ReadCSV(Path.Combine(basePath, "Address.csv"));
	var stateProvince = MyExtensions.ReadCSV(Path.Combine(baseDir, "StateProvince.csv"))
		.Where (state => state.CountryRegionCode == "CA")
		//.Dump()
	;

	foreach(var state in stateProvince){
		var headerPath = Path.Combine(baseDir, "Address.header"); 
		var indexPath = Path.Combine(baseDir, "Address_Indexes", "StateProvinceID", string.Format("{0}.csv", state.StateProvinceID));
		var matchingAddressesFromIndex = MyExtensions.ReadCSV(indexPath, headerPath:headerPath);
		foreach(var address in matchingAddressesFromIndex)
			yield return new { address.AddressLine1, address.City, StateName = state.Name };
	}
}