<Query Kind="Program" />

void Main()
{
	// But maybe we know more about the data
	// Maybe we know both tables are laid out the same way?
	// Can't we just scan though both at the same time?
	
	var baseDir = @"C:\Users\Piers\Downloads\AdventureWorks 2012 OLTP Script";
	var products = MyExtensions.ReadCSV(Path.Combine(baseDir, "Product.csv"));
	var productInventory = MyExtensions.ReadCSV(Path.Combine(baseDir, "ProductInventory.csv"));

	ShowProductsAndInventory(
		products, 
		productInventory,
		(product,inventory) => new { 
			product.ProductID,
			product.Name,
			product.ProductNumber,
			//InventoryProductID = currentInventory.ProductID,
			Location = String.Format("{0}/{1}/{2}", inventory.LocationID, inventory.Shelf, inventory.Bin),
			inventory.Quantity
		}
	).Dump();
}

private IEnumerable<dynamic> ShowProductsAndInventory(
	IEnumerable<dynamic> products,
	IEnumerable<dynamic> inventory,
	Func<dynamic, dynamic, dynamic> resultSelector
){
	// assumes both inputs sorted by ProductID
	var inventoryIterator = inventory.GetEnumerator();
	if(!inventoryIterator.MoveNext())
		yield break;
	var currentInventory = inventoryIterator.Current;
	
	foreach(var product in products){
		var productID = product.ProductID;

		// this next bit only works because we coverted the IDs to ints
		while(currentInventory.ProductID <= productID){
			if(currentInventory.ProductID == productID){
				yield return resultSelector(product, currentInventory);
			}
			
			if(!inventoryIterator.MoveNext())
				yield break;
			currentInventory = inventoryIterator.Current;
		}
	}
}