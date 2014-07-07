BabyDb
======

BabyDb is a scratchpad I put together to explain how RDBMS join operators work, using simple, working C# code samples.

Many developers I've worked regard interpreting RDBMS query plans as a black art they don't have the time to get into. And yet, the 3 basic strategies databases use to access and join data are something I believe nearly all developers would understand, if they took the time to. My theory was if I showed them the code, the penny would drop.

This repo consists of the sample code I demonstrated in my talk at the @PerthDotNet user group, including:

 * An extension method to load a CSV file (plus metadata) into an Enumerable<dynamic>
 * LinqPad scripts demonstrating how the three join strategies (Hash Join, Merge Join, Nested Loops) work (roughly)
 * Some code for generating a file-based 'index' structure, required for the Nested Loops demo

You will need to download the AdventureWorks sample database from CodePlex to follow the included samples, but you can of course provide your own CSV files and experiment as much you like.
