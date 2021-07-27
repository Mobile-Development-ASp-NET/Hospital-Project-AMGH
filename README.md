# Hospital-Project-AMGH

## ust initalised the project.

##Adding models to database
*ADD AN App_Data in the folder found in the File Explore of the project*
1. Create Models you will be needing
2. Go to the identiy model page and add your table name there public DbSet<Questions> *Table Name* { get; set; }
3. tools->nuget package console-> add-migration *name of table*
4. tools->nuget package console-> update-database
