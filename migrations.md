## When adding a new table 

Add a `DbSet` to `GoalsDbContext`

From the package manager console run 

`Add-Migration -Context GoalsDbContext`

`Update-Database -Context GoalsDbContext`