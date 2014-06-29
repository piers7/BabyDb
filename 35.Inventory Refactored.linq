<Query Kind="Program" />

void Main()
{
	// But maybe we know more about the data
	// Maybe we know both tables are laid out the same way?
	// Can't we just scan though both at the same time?
	
	var baseDir = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Data");
	var products = MyExtensions.ReadCSV(Path.Combine(baseDir, "Product.csv"));
	var productInventory = MyExtensions.ReadCSV(Path.Combine(baseDir, "ProductInventory.csv"));

	ShowProductsAndInventory(
		products, 
		productInventory,
		p => p.ProductID,
		i => i.ProductID,
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

private IEnumerable<TResult> ShowProductsAndInventory<TLeft,TRight,TResult>(
	IEnumerable<TLeft> left,
	IEnumerable<TRight> right,
	Func<TLeft,int> leftKeySelector,
	Func<TRight,int> rightKeySelector,
	Func<TLeft, TRight, TResult> resultSelector
){
	// assumes both inputs sorted by ProductID
	var rightIterator = right.GetEnumerator();
	if(!rightIterator.MoveNext())
		yield break;
	var rightItem = rightIterator.Current;
	
	foreach(var leftItem in left){
		var key = leftKeySelector(leftItem);

		// ok, so this still needs integer keys just now
		while(rightKeySelector(rightItem) <= key){
			if(rightKeySelector(rightItem) == key){
				yield return resultSelector(leftItem, rightItem);
			}
			
			if(!rightIterator.MoveNext())
				yield break;
			rightItem = rightIterator.Current;
		}
	}
}