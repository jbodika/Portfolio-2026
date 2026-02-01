1. Open the AssignmentDB.SQL file in SSMS
2. Execute the script.
3. Change the ConnectionString in appsettings.config
 "MyConnection": "Server=cssql.cegep-heritage.qc.ca;Database=NEW_DB_NAME;User id=USERID;Password=PASSWORD;",
 	3.1 - jbH60Store proj
	3.2 - jbH60Customer proj
	3.3 - jbH60Services proj
4. Change the Identity DB Connection String in appsettings.config to your credentials (for User id and Password):
	4.1 jbH60Customer proj =>     "jbH60CustomerContextConnection": "Server=cssql.cegep-heritage.qc.ca;Database=H60Assignment3DBIdentity_M2_jb;User id=JBODIKA;Password=password;"

	4.2  jbH60Store proj => 
    "jbH60StoreContextConnection": "Server=cssql.cegep-heritage.qc.ca;Database=NEW_DB_NAME;User id=USERID;Password=PASSWORD;"   
				
4. You can use the Testing Accounts.txt file to log into a Manager, Clerk and Customer roles.

