<Query Kind="Program" />

void Main()
{
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");

	var addresses = MyExtensions.ReadCSV(Path.Combine(baseDir, "Address.csv"))
	;
	var stateProvince = MyExtensions.ReadCSV(Path.Combine(baseDir, "StateProvince.csv"))
		.Where (a => a.CountryRegionCode == "CA")
	;

	// This is what the generalized version looks like
	// ...and yes, this bears a striking resemblance to Enumerable.Join
	MergeJoin(
		stateProvince, 
		addresses,
		inner => inner.StateProvinceID,
		outer => outer.StateProvinceID,
		(inner,outer) => new { outer.AddressLine1, outer.City, StateName = inner.Name}
	)
	.Dump();
	
	// Hash joins are a great default strategy, 
	// because you don't need to know *anything* about the data, except some way to derive a common key.
	// So they always work
	// (sample implementation here is constrained by virtual memory)
}

public static IEnumerable<object> MergeJoin<TKey>(
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