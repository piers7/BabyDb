<Query Kind="Statements" />

var filesAndKeys = Directory.EnumerateFiles(@"C:\Users\Piers\Downloads\AdventureWorks 2012 OLTP Script", "*.header")
	.Select (f => new { 
		name = f, 
		key = File.ReadLines(f).First ()
		})
	.ToList();
		
filesAndKeys
	.OrderBy (ak => ak.key).ThenBy (ak => ak.name)
	.Where (ak => filesAndKeys.Count (f => f.key == ak.key) > 1)
	.Dump()
	;