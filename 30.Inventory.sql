select * from Production.Product p
inner join Production.ProductInventory pi
on (p.ProductID = pi.ProductID)