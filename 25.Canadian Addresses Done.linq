<Query Kind="Program" />

void Main()
{
	var baseDir = @"C:\Users\Piers\Downloads\AdventureWorks 2012 OLTP Script";
	
	GetCanadianAddresses(baseDir).Dump();
}

public static IEnumerable<object> GetCanadianAddresses(string basePath){
	var addresses = MyExtensions.ReadCSV(Path.Combine(basePath, "Address.csv"));
	var stateProvince = MyExtensions.ReadCSV(Path.Combine(basePath, "StateProvince.csv"));
	
	// How do we get all the addresses in Canada?
	var canadianStates = new Dictionary<int,object>();
	foreach(var state in stateProvince){
		if(state.CountryRegionCode == "CA")
			canadianStates.Add(state.StateProvinceID, state);
	}
	
	var canadianAddresses = new List<object>();
	foreach(var address in addresses){
		dynamic state;
		if(canadianStates.TryGetValue(address.StateProvinceID, out state)){
			yield return new { address.AddressLine1, address.City, StateName = state.Name };
		}
	}
}