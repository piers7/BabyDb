<Query Kind="Program" />

void Main()
{
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");

	var addresses = MyExtensions.ReadCSV(Path.Combine(baseDir, "Address.csv"))
	;
	var stateProvince = MyExtensions.ReadCSV(Path.Combine(baseDir, "StateProvince.csv"))
		.Where (a => a.CountryRegionCode == "CA")
	;
	
	GetCanadianAddresses(
		stateProvince, 
		addresses,
		inner => inner.StateProvinceID,
		outer => outer.StateProvinceID,
		(inner,outer) => new { outer.AddressLine1, outer.City, StateName = inner.Name}
	)
	.Dump();
}

public static IEnumerable<object> GetCanadianAddresses<TKey>(
	IEnumerable<dynamic> inner, 
	IEnumerable<dynamic> outer,
	Func<dynamic, TKey> innerKeySelector,
	Func<dynamic, TKey> outerKeySelector,
	Func<dynamic, dynamic, dynamic> resultSelector
	){
	
	var index = new Dictionary<TKey,object>();
	foreach(var innerItem in inner){
		var key = innerKeySelector(innerItem);
		index.Add(key, innerItem);
	}
	
	foreach(var outerItem in outer){
		dynamic innerItem;
		var key = outerKeySelector(outerItem);
		if(index.TryGetValue(key, out innerItem)){
			yield return resultSelector(innerItem, outerItem);
		}
	}
}