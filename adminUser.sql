﻿CREATE USER [adminUser]
	WITHOUT LOGIN
	WITH DEFAULT_SCHEMA = dbo

GO

GRANT SELECT, INSERT, UPDATE, DELETE, CONNECT TO [adminUser]
