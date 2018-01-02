namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoassignSPandUDF : DbMigration
    {
        public override void Up()
        {
            Sql(_createFunctionScript);

            // Create a new store procedure
            CreateStoredProcedure("dbo.AutoAssignUsersToSessions"
            // These are stored procedure parameters
            , c => new {
                
            },
            // Here is the stored procedure body
            @"
                SET NOCOUNT ON;

                DECLARE @SessionId INT
	            DECLARE @UserId VARCHAR(128)
	            DECLARE @Msg VARCHAR(1000)
	            DECLARE @UnableToAssign TABLE(SessionId INT)
 
	            SELECT @SessionId = s.Id FROM dbo.Sessions s
	            WHERE s.SessionType IN ('General Session', 'Static Session', 'Pre-Compiler')  
	            AND isnull(s.VolunteersRequired,1) > (SELECT count(*) FROM dbo.SessionUsers su WHERE su.Session_Id = s.Id)
	            AND s.Id NOT IN (SELECT uta.SessionId FROM @UnableToAssign uta)
	            ORDER BY s.SessionStartTime DESC

	            WHILE @SessionId IS NOT NULL
	            BEGIN
	                SET @UserId = NULL 
	                SELECT TOP 1 @UserId = anu.Id
		            FROM dbo.AspNetUsers anu
		            INNER JOIN dbo.AspNetUserRoles anur
			            ON anur.UserId = anu.Id
		            INNER JOIN dbo.AspNetRoles anr
			            ON anr.Id = anur.RoleId
		            LEFT JOIN dbo.SessionUsers su
			            ON su.User_Id = anu.Id
		            LEFT JOIN dbo.Sessions s
			            ON s.Id = su.Session_Id
		            WHERE anr.Name = 'Volunteers'
		            AND dbo.HasCollision(@SessionId, anu.Id) = 0
		            GROUP BY anu.Id
		            ORDER BY sum(isnull(datediff(SECOND, s.SessionStartTime, s.SessionEndTime),0))

	
		            IF @UserId IS NOT NULL
		            BEGIN
			            INSERT INTO dbo.SessionUsers
			            (
				            Session_Id
			                , User_Id
			            )
			            VALUES
			            (
				            @SessionId   -- Session_Id - int
			                , @UserId -- User_Id - nvarchar(128)
			            )	
			            SET @Msg = 'Assigned session ' + CAST(@SessionId AS VARCHAR(20)) + ' to user ' + @UserId
			            RAISERROR (@Msg, 0, 0)
		            END
		            ELSE
		            BEGIN
			            INSERT INTO  @UnableToAssign
			            (
				            SessionId
			            )
			            VALUES
			            (
				            @SessionId -- SessionId - int
			            )
			            SET @Msg = 'Could not assign session ' + CAST(@SessionId AS VARCHAR(20)) + ' to any user.'
			            RAISERROR (@Msg, 0, 0)
		            END
		            SET @SessionId = NULL 
		            SELECT @SessionId = s.Id FROM dbo.Sessions s
			            WHERE s.SessionType IN ('General Session', 'Static Session', 'Pre-Compiler')  
			            AND isnull(s.VolunteersRequired,1) > (SELECT count(*) FROM dbo.SessionUsers su WHERE su.Session_Id = s.Id)
			            AND s.Id NOT IN (SELECT uta.SessionId FROM @UnableToAssign uta)
			            ORDER BY s.SessionStartTime DESC
	            END
                ");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.AutoAssignUsersToSessions");
            Sql(DropFunctionScript);
        }

        private const string DropFunctionScript = "DROP FUNCTION [dbo].[HasCollision]";

        private readonly string _createFunctionScript = @"
                    CREATE FUNCTION [dbo].[HasCollision] 
                    (
	                    -- Add the parameters for the function here
	                    @SessionId INT,
	                    @UserId VARCHAR(128)
                    )
                    RETURNS bit
                    AS
                    BEGIN
	                    -- Declare the return variable here
	                    DECLARE @Result BIT = 0

	                    -- Add the T-SQL statements to compute the return value here
	                    DECLARE @CollisionCount INT = 0

	                    SELECT @CollisionCount = count(*) FROM
	                    (SELECT s.Id , s.SessionStartTime, s.SessionEndTime FROM dbo.SessionUsers su 
		                    INNER JOIN dbo.Sessions s 
			                    ON s.Id = su.Session_Id
		                    WHERE su.User_Id = @UserId) a,
	
	                    (SELECT * FROM dbo.Sessions s 
		                    WHERE s.Id = @SessionId) b
		                    WHERE a.SessionStartTime BETWEEN b.SessionStartTime AND b.SessionEndTime

	                    IF @CollisionCount > 0
	                    BEGIN
		                    SET @Result = 1
	                    END

	                    -- Return the result of the function
	                    RETURN @Result

                    END
                    ";
    }
}
