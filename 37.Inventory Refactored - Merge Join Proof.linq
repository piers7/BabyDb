<Query Kind="Program" />

void Main()
{
	var left = new[]{ 1,2,4,5,7,10 };
	var right = new[]{ 2,3,4,7,8,10 };
	
	MergeJoin(
		left, 
		right,
		l => l,
		r => r,
		(l,r) => new { l, r } 
	).Dump();
}

private IEnumerable<TResult> MergeJoin<TLeft,TRight,TResult>(
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