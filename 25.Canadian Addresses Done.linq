<Query Kind="Program" />

void Main()
{
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	
	GetCanadianAddresses(baseDir).Dump();
}

public static IEnumerable<object> GetCanadianAddresses(string baseDir){
	var addresses = MyExtensions.ReadCSV(Path.Combine(baseDir, "Address.csv"));
	var stateProvince = MyExtensions.ReadCSV(Path.Combine(baseDir, "StateProvince.csv"));
	
	// How do we get all the addresses in Canada?
	
	// First, pull all the Canadian states into a hashtable (eg Dictionary)
	// Q: what is the difference between a hashtable and a Dictionary<T,T1>?
	var canadianStates = new Dictionary<int,object>();
	foreach(var state in stateProvince){
		if(state.CountryRegionCode == "CA")
			canadianStates.Add(state.StateProvinceID, state);
	}
	
	// Then pull all the addresses, filter based on the hashtable, return the matches
	var canadianAddresses = new List<object>();
	foreach(var address in addresses){
		dynamic state;
		if(canadianStates.TryGetValue(address.StateProvinceID, out state)){
			yield return new { address.AddressLine1, address.City, StateName = state.Name };
		}
	}
}