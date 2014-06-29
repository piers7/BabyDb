<Query Kind="Program" />

void Main()
{
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	
	CreateIndexes(baseDir, "Address.csv", row => row.StateProvinceID, "StateProvinceID");
}
	
public static void CreateIndexes(string baseDir, string csvName, Func<dynamic,int> keySelector, string indexName){

	var csvPath = Path.Combine(baseDir, csvName);
	var entityName = Path.GetFileNameWithoutExtension(csvName);
	
	var indexDir = Path.Combine(baseDir, "Indexes", "Address", indexName);
	Directory.CreateDirectory(indexDir);		
	
	var partitions = MyExtensions.ReadCSV(csvPath)
		.GroupBy (
			row => keySelector(row),
			row => (string)row.__rawLine
		)
		;
	foreach(var partition in partitions){
		var partitionPath = Path.Combine(indexDir, partition.Key + ".csv");
		LINQPad.Extensions.Dump(partitionPath);
		File.WriteAllLines(partitionPath, partition);
	}
	
	// also copy over the metadata, or we'll have to hack our other functions
	var metadataSource = Path.ChangeExtension(csvPath, "header");
	var metadataDest = Path.Combine(indexDir, Path.GetFileName(metadataSource));
	File.Copy(metadataSource, metadataDest);
}