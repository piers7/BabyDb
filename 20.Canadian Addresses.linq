<Query Kind="Program" />

void Main()
{
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");

	var addresses = MyExtensions.ReadCSV(Path.Combine(baseDir, "Address.csv"));
	var stateProvince = MyExtensions.ReadCSV(Path.Combine(baseDir, "StateProvince.csv"));
	
	addresses.Take(10).Dump();
	stateProvince.Dump();
	
	
	//GetCanadianAddresses(baseDir).Dump();
}

public static IEnumerable<object> GetCanadianAddresses(string baseDir){
	var addresses = MyExtensions.ReadCSV(Path.Combine(baseDir, "Address.csv"));
	var stateProvince = MyExtensions.ReadCSV(Path.Combine(baseDir, "StateProvince.csv"));
	
	// How do we get all the addresses in Canada?
	throw new NotImplementedException();
}