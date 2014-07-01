select AddressID
from person.Address a
inner join person.StateProvince s on (s.StateProvinceID = a.StateProvinceID)
where s.StateProvinceID = 1