<Query Kind="Program" />

void Main()
{
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	var csvPath = Path.Combine(baseDir, "Address.csv");
	
	var partitions = MyExtensions.ReadCSV(csvPath)
		.GroupBy (
			row => (int)row.StateProvinceID,
			row => (string)row.__rawLine
		)
		;
	foreach(var partition in partitions){
		var partitionDir = Path.Combine(baseDir, "Address_Indexes", "StateProvinceID");
		Directory.CreateDirectory(partitionDir);		
		var partitionPath = Path.Combine(partitionDir, partition.Key + ".csv").Dump();
		
		File.WriteAllLines(partitionPath, partition);
	}
}
