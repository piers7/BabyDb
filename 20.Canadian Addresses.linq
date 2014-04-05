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

}