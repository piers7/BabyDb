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
	var canadianStates = new Dictionary<int,object>();
	foreach(var state in stateProvince){
		if(state.CountryRegionCode == "CA")
			canadianStates.Add(state.StateProvinceID, state);
	}
	
	// Then loop through all the addresses
	// and return those that have a StateProvinceID that matches one of the states we cached above
	var canadianAddresses = new List<object>();
	foreach(var address in addresses){
		dynamic state;
		if(canadianStates.TryGetValue(address.StateProvinceID, out state)){
			yield return new { address.AddressLine1, address.City, StateName = state.Name };
		}
	}
	
	// This is a hash join.
	
	// It has three parts:
	// - building the index on one of the sequences, based on some key in the sequence
	// - enumerating the other sequence, probing the index based on a key in *that* sequence that should match
	// - projecting some kind of result sequence
	// So we can generalize the above.
}