<Query Kind="Program" />

void Main()
{
	// But maybe we know more about the data
	// Maybe we know both tables are laid out the same way?
	// Can't we just scan though both *at the same time*?
	
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	var products = MyExtensions.ReadCSV(Path.Combine(baseDir, "Product.csv"));
	var inventory = MyExtensions.ReadCSV(Path.Combine(baseDir, "ProductInventory.csv"));

	ShowProductsAndInventory(products, inventory).Dump();
}

private IEnumerable<dynamic> ShowProductsAndInventory(
	IEnumerable<dynamic> products,
	IEnumerable<dynamic> inventory
){
	// Assuming both inputs sorted by ProductID
	// how to we spit out the results?
	
	
	#region spoilers
	/*
	var inventoryIterator = inventory.GetEnumerator();
	if(!inventoryIterator.MoveNext())
		yield break;
	var currentInventory = inventoryIterator.Current;
	
	foreach(var product in products){
		var productID = product.ProductID;

		// this next bit only works because we coverted the IDs to ints
		while(currentInventory.ProductID <= productID){
			if(currentInventory.ProductID == productID){
				yield return new { 
					product.ProductID,
					product.Name,
					product.ProductNumber,
					//InventoryProductID = currentInventory.ProductID,
					Location = String.Format("{0}/{1}/{2}", currentInventory.LocationID, currentInventory.Shelf, currentInventory.Bin),
					currentInventory.Quantity
				 }; 
			}
			
			if(!inventoryIterator.MoveNext())
				yield break;
			currentInventory = inventoryIterator.Current;
		}
	}
	*/
	#endregion
}