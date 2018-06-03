# InsideTradeRegistryApi

Provides a .NET Core library that facilitates the retrieval 
of PDMR transactional data from Finansinspektionen (the Swedish Financial 
Supervisory Authority).


All data is provided by [Finansinspektionen][Registry].

The library is available as a NuGet Package: [NorthernLight.InsideTradeRegistry.Api][Nuget]


--------
### Code Sample

```C#
var api = new InsideTradeRegistryApi();

var transactions = await api.GetInsideTradeTransactionsAsync(
	new SearchQuery
	{
		Issuer = "Investor",
        	PDMRPerson = "Johan Forssell",                
        	PublicationDateFrom = DateTime.Parse("2018-01-01"),
        	PublicationDateTo = DateTime.Parse("2018-06-01"),
        	TransactionDateFrom = DateTime.Parse("2017-12-01"),
        	TransactionDateTo = DateTime.Now
	});
```

[Registry]: https://marknadssok.fi.se/publiceringsklient/en-GB/Search/Start/Insyn
[Nuget]: https://www.nuget.org/packages/NorthernLight.InsideTradeRegistry.Api/







