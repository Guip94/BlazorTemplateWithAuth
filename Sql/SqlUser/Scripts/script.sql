CREATE DATABASE DbUserApiPFF
GO

USE DbUserApiPFF
GO

--#region : Security

    --#region : Schemas
        CREATE SCHEMA [DbUserStandard]
        GO
    --#endregion

    --#region : DB Role
        CREATE ROLE [DbUserRole]
        GO

        GRANT EXECUTE ON SCHEMA :: [DbUserStandard] TO [DbUserRole]
        GO
    --#endregion

--#endregion

--#region Création des Tables

    --#region : Role
        CREATE TABLE [DbUserStandard].[Role]
        (
            [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
            [Fonction] NVARCHAR(20) NOT NULL UNIQUE
        )
        GO
    --#endregion
    --#region : Adress
        CREATE TABLE [DbUserStandard].[Adress]
        (
            [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
            [Country] NVARCHAR(50) NULL, 
            [Zipcode] INT NULL, 
            [City] NVARCHAR(50) NULL, 
            [Street] NVARCHAR(200) NULL, 
        )
        GO

    --#endregion
    --#region : User
        CREATE TABLE [DbUserStandard].[User]
        (
            [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
            [Mail] NVARCHAR(384) NOT NULL UNIQUE, 
            [Pwd] BINARY(64) NOT NULL, 
            [RoleId] INT NOT NULL DEFAULT 3,
            [Lastname] NVARCHAR(30) NOT NULL, 
            [Firstname] NVARCHAR(30) NOT NULL, 
            [AdressId] INT NULL , 
            [RefreshToken] NVARCHAR(128) NULL, 
            CONSTRAINT [FK_User_ToRole]  FOREIGN KEY (RoleId) REFERENCES [DbUserStandard].[Role]([Id]), 
            CONSTRAINT [FK_User_ToAdress] FOREIGN KEY (AdressId) REFERENCES [DbUserStandard].[Adress](Id) ON DELETE CASCADE
        )
        GO
    --#endregion
    --#region : Company
        CREATE TABLE [DbUserStandard].[Company]
        (
            [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
            [Name] NVARCHAR(100) NOT NULL, 
            [Tva] NVARCHAR(50) NOT NULL, 
            [NumEntr] NVARCHAR(10) NOT NULL, 
            [LegalForm] NVARCHAR(30) NOT NULL, 
            [AdressId] INT NOT NULL, 
            [RoleId] INT NOT NULL,
            CONSTRAINT [FK_Company_ToAdress] FOREIGN KEY (AdressId) REFERENCES [DbUserStandard].Adress (Id),
            CONSTRAINT [FK_Company_ToRole] FOREIGN KEY (RoleId) REFERENCES [DbUserStandard].[Role] (Id)

        )
        GO
    --#endregion

--#endregion

--#region : Fonctions
    CREATE FUNCTION [DbUserStandard].[HashPwd]
    (
        @pwd NCHAR(60) 
    )
    RETURNS BINARY(64)
    AS
    BEGIN
        DECLARE @PreSalt NCHAR(2048) = N'peALmr3S@BuYyKc-^E_enw*KVTFfU8D+7XQaTL2S-6?pryv&f#wPg9^u=avQP2qE+=bepTUqfn-ZmWUkbursn4JzfF#9J6s#AxgSE&Nz+S6rYm?Z_s5W4?zkJ3tf^Q&pu_@8J@n%p_#4kQE424Lp=qN!#FMe9MamfMEruF=$*kMK&J^jMzjD*NV2v*_Z5q^__MKkw%daVpBK^&X?**5LCy5%C_N95vZuX_2qgj5pxWHtsJqg!BT6+RKkkZBmSFX7zDhahFkjr^whp?W6Nr-vW_NdHeKdH4!u!VLS_H$ce*?=+x4r3bac+xe#Cd2G&KR6daCXw4V5APNb2MdFXj*2jAH5Q$yFrg=rXTY_Nb=te5Fyt$^E4gNwdvz6na64@MBz$pbnVFB6DPgWCsN^7!GkPZA4C8*-JsN+z&CTYWtLE^nJu=3dk!8Rt#Jz9RNzyUVMkd7KA&r@z6?KUTa!HWagSv_CqCSc^Th@H=LzwV58@HE8p5!DV6e4zKr45B-!gDhfZXM4GeGZnBJRbM3RH^ekjqSrQ5&BA3crz23epVGwNAG=Y2Rpng%aUtXcnSJUTq%7MkThY^&g5j%*Z4f&3=-E&657HcfX6bTW6h@KM+mCR^5%Y*tFV8Ha-@j7*6yA^=rZ3!=%dyx#sYrLZdW@PqanN=r4S4zE$MSNT+tYMPtR?dqy4WSk&uAhU_%6JeqnAq?SAd%TM_a%G68wj^t-*dH8XJ_GGLEyypqQ@@RafdgmWPTpk2L2y&Y9=$MkzWFF5b!JVY7z_X^7xqDC-Myf%AvYnj$B9h3HE_sY$rD^+MJjZz7BVbxeGpg=nPQauLMEwNA&k&dD=W-9SG9M$p%3d!YA&p-z3TUm5&9azjQ$aFT3uB3K^9vD2W9Y?J?4rwWh-qPv5-Yp!8z!fVxQ4nYK=#Y-V&P9JSB6@EKzQ849Vww&r=dJE=3BphkXGnr3KbPzRc+TS@QhD4cZgD*#7fxh7mZMtbrR8AZ+a=&j5U8+M3qqM3$#*LLHdDHyEP=&XyP7e^+^q3-a%WtqW9_HU$2Hg5kSrKySHNv-a9+F85v2rfSqG%WuC!fA-HzD$_%Hzma?m&!7WaMXsgqtF35UVr%9@mg7cCNPccu4K3?6b&K3r$pEY+XTQN8Tn6+D_HxBBB2=7ns2Yfcq3WBbP=@tQse6WW3N8T4w-W9YD?HAsLt!eZj#$B9!Nb%su2z-UxVej+Rwq3@+b6ERNuTeJNmCX4^CSp%nzGH^&P_@r$+mLDJhT3-R$WtsA%U&fjS78yX3Lwm_BgBQDv83t#L-F_6M^pEN4Nh=@Qpj2!!sSNzJvZGk8v5VHVzFgcL-KM!hnYYjJYpmg!E*K^PRSgSs#q%rFFfNF5Kux?zeVAJLw9fEwLpE73Hkcq526nkM&u5aNH@WDqnX-cyDXadVxjCv4e=9q_2@Ech$kHcCbd=+BzWd=X&JTquj+h2NndA#T%B$j*rjhVh#j$Eqyn-jkrSgVEFRV!K=%h=$mvG8Df9GJc3t3UfYFKGANGpBmYnGysgk9yBD4$wj2NwaS$&K%q4bu_DcBTPd_8JLa--=ypc9M8B7KwweGXH#5E?-BkFD26*Knq6q-D?HP8bD4mRe$pLV!R624K8&hkB6MuX$b3%uFFmCFeuC&wxRn7ss99aaEJSmVwv%hy&#CeErj59@9jvUTh8GYpf+z!FHWq8F@k*u3*6SMSV&35M$SKNBXrA3f4&u*K76d#uQ&qM_Fny!Tu8_XSER?2fL^+n%R_xgF5+k?gcy6jXcDw?K+b+y?Ena#XyWHVUn9pf$UcEJ2C4#j8BhVPLxEUa&Bn22s9bVYN65Be!_t_9ju&C6P6%RMv!g9k9gJKc6wVR%$h-^d%bM%&amGGY4BGY&ber+%b*MQTX_G-du592fpaVrx!W3sqUE93k@Dt@GFZKdKVH!ybR2bRBHFgap=u8BRPPAJng$NE8kG@xyRmqPeJk^Fm6***yrjXtJmmpFVhRTf#U%xRhFR64z+qsRak&V*UUKD43%fB7m5ud88^n*wnK=$N3Cr^G$XJFA*Ju^&JgfW+=#';
        DECLARE @PostSalt NCHAR(2048) = N'*e-mE&Ey7Fz=w^cTujjjDc3sDV*7B@2xN3UtVGmxrXt2CdvBkLPrj3=9Ws&#ms_4js!+ADzE%5QKTm8*$wBfu%ftPW@GZk*zk&A4g?J8GdNKa@CZ7v=E9XW!?sx@_Nb&Qn?EDkXt_8&LAaL_GuXVYb%r3tgxPkj9A3*X?C-_sgL4#uHhJT-x5%X?WpusDkbYnFBU=@dEzL6Xr%--rxLQBg3nZBYQAYVfFqcxnN26e@7c&RKTu8xVuVgkvs@#*X^F!JmFp4YUq-tWqCFwjky&VPj2yw3eJd7?pqVDK+3kBjq65jrD^=%z*FV^aJR@QYdwQXCS%pByUcc4bbRRq4^ZaV9x9%t+DN^!E&V4!zGuA5qQfqe^v7TPL%&H4$SGPFhDYXHruaD=9Cgydqp8a+3AZ#=hwc#C?+LYGL%EM3W#cNH%LK*vLRUBgNC6f?sCAb=BcgVC4?xk9D#3ks&RByTLDQq5h+q4*M=yhBsB92&QtVUgv#t&D9!sYh%RQK6+Rq4@TEk_Qaw8#9t-aYRg?yx6S8DkHALhk8?ePrE8y=*FLBaWb+qSVa4jTm_@*M=L-MpUheB2P%ZQ9Q_8c=#4WW2cyNFTxXfuwkQfb_ULrGff=n+=@4mMuj?S+&DVV-a?Cg#%GB!wHP*u6#%PspDKs+HnUrKxnkEnauzEsc-FAN*DT@57SrsVUjqSWq%vtNXhRQM6Vg2_?cP59J%SPmWv=tTFwWcA==C*9se@JjQ?AEnmhEzCphQzW7_zk-@b$XT45Yuyfz$4Sbzd6*N-Ch!Z#aqZ5j6t+75?s$7ZS!?-E6!kQ+d5ggZg^pGBRt7RvJ2$B7qCB@RrNbsaMn+X-Dd_b3ZL#CYMxBTteUd?EWJ+FaMKRbwp-unBG*xFvRwBwwqrfE69*zQsegZMwUePJT3E7X?B_2C+W-yTT2md7vgsWAMP36gVQ%@$AX3dP=HAzQfGm!szZRWH2KPBL+tbkTN?GuC+$dd=^s?Xk!&Qvj#+$dHPRT#9GZ2p&H-wK@WngNd?9qTv#@rDFEvap?dXjeCuxqV^vFcW2_PaK-pqYk&WPVrA-P?gS6qrwq^EtrY3g@+Kfx*Vj=dcd-T-mzFVv%TzX+st5Pxp*@V#3mjK^=FYbvxx$KFVk7ke5F7GqD9MZhLmaALHZ?3#5pVZpqt-b-zNz$%&_G_y=fh_jSQ*#7By+6?t8pKC&mhM_LPgUZn^%c%N!qWq%UUebwHn7^4%55=mF!SnvvRzc*&6zuFLYKD9FuV?Hnc-P65g!jNcYPmWsA+sjsja36&@=BwE3vSwvRdxsB$-f5PhkJB6ZxMJ7KqZg6jAEd3YVDN278aCtqyH6KCuKwzkStRc-BS%$58*GYKS-xqhN6*Mv+=$EYqtbMqw#HCJ2sN+w!mX$e8Ksj#Y-UPtaG=#3L3h?cn4s5J?w@ZhwGaNFyAE^PwmDN^erxBvLSQ-tUq69LG^cfVs&gM%kCqrc#LYFxXamAf3$^$D!*YS+6DpeGhZqJ*BE*m4E5?w9N$g_Zg4m=!8EuMJfVh%xsBn#m6ZyEMF%ChT%7dp4Wm&d!DV5eL35?vQ==DFzD=^*3hJ3gK6JqDF=y6N3HLDp%kHf&JspV3hcQ&$=?r5_9u*_-!e&H2aV=R7=bJUPvXeh7@P&eYD@es+P53sH+yfxV-h+rm_WfJGxxv*zDWh=yGCrmj-+xYNEQJV4nK$dU6Kwp$kA9wmc#jtBp47Q*Nf%cxeUPq$U6cHS7ze%M_g*4$*#_DKT8^jJ2Y-nz*EU4r@n$Esg!NL6X$$&=ZLvDZw=9JPMUZ-N+ZD34ZKHE2wpPX$6xR%mxZ6@M_mVXS$+e5pnP=h-L5vp#cb5JAMRhtATc942?KQ?DXw*_L#WUSEWHzrHVuKfBcSrf%wWk#MtutAk6Q-hrwcnqjeR+ZK-8Y2@?PVAAxMGZ+NjP5NSa&jnHGNqTVeG2!GV@*ZKNQqnzwC&!tRJmsWJu&%^5#tdWnMcyZZuVFgNLyvD39cYyTqa6muB29B_Df@?5Dp=Ezef$J57-&PT_S8rj&WU5S@gYztqF3FeMhB';

        DECLARE @x INT = 0;
        DECLARE @Cypher BINARY(64) = HASHBYTES('SHA2_512', CONCAT(@PreSalt, @pwd, @PostSalt));

        WHILE @x < 4
        BEGIN
            SET @Cypher = HASHBYTES('SHA2_512', CONCAT(@PreSalt, @Cypher, @PostSalt));
            SET @x = @x + 1;
        END

        RETURN @Cypher;
    END
    GO
--#endregion

--#region : Création des procédures stockées

--#region : Roles
    --#region : AddRole
            CREATE PROCEDURE [DbUserStandard].[AddRole]
                @fonction NVARCHAR(20)
            AS

            BEGIN
                INSERT INTO Role (Fonction)
                VALUES (@fonction)

            END
            GO
    --#endregion
    --#region : GetAllRoles
        CREATE PROCEDURE [DbUserStandard].[GetAllRoles]

        AS

        BEGIN
            SELECT Id , Fonction FROM [DbUserStandard].[Role]
        END
        GO

    --#endregion
    --#region : GetRoleById

        CREATE PROCEDURE [DbUserStandard].[GetRoleById]
        @id INT
        AS

        BEGIN
            SELECT Fonction FROM [Role] WHERE Id=@id
        END
        GO

    --#endregion
--#endregion
--#region : Adress
    --#region : AddAdress
        CREATE PROCEDURE [DbUserStandard].[AddAdress]
            @country NVARCHAR(100),
            @zipcode INT,
            @city NVARCHAR(50),
            @street NVARCHAR(200),
            @userid INT
        AS

        BEGIN
            BEGIN TRY
            IF LEN(TRIM(@country)) = 0
                BEGIN
                    RAISERROR(N'Invalid format for @country', 17, 1)
                END
            IF LEN(TRIM(@city)) = 0
                BEGIN
                    RAISERROR (N'Invalid format for @city', 17, 1)
                END
            IF LEN(TRIM(@street)) = 0
                BEGIN
                    RAISERROR (N'Invalid format for @street', 17, 1)
                END
            





            
            IF  EXISTS (SELECT 1 FROM [DbUserStandard].[User] WHERE Id = @userid AND AdressId IS NULL )

            BEGIN
                
                DECLARE @insertrowsaffected INT = @@ROWCOUNT;

                INSERT INTO Adress (Country, Zipcode, City, Street)
                VALUES(@country, @zipcode, @city, @street)



                DECLARE @adressid INT = SCOPE_IDENTITY();

                DECLARE @updaterowsaffected INT = @@ROWCOUNT;

                UPDATE [DbUserStandard].[User] SET AdressId = @adressid WHERE Id = @userid


                RETURN  @updaterowsaffected + @insertrowsaffected;

            END

            END TRY


            BEGIN CATCH
            DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
            DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
            DECLARE @ErrorState INT = ERROR_STATE();



            RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)

            RETURN ;


            END CATCH


        END
        GO
    --#endregion
    --#region : GetAdressByUserId
        CREATE PROCEDURE [DbUserStandard].[GetAdressByUserId]
            @userId INT
        AS


        IF EXISTS (SELECT 1 FROM [DbUserStandard].[User] WHERE Id = @userid)
        BEGIN
            SELECT a.Id, a.Country, a.City, a.Zipcode, a.Street FROM [DbUserStandard].[User] as u JOIN [DbUserStandard].[Adress] as a ON u.AdressId = a.Id
            WHERE u.Id = @userId
        END
        GO
    --#endregion
    --#region : UpdateAdress
        CREATE PROCEDURE [DbUserStandard].[UpdateAdress]
            @country NVARCHAR(50),
            @street NVARCHAR(200),
            @city NVARCHAR(50),
            @zipcode INT,
            @userid INT
        AS



        BEGIN
            BEGIN TRY
            IF LEN(TRIM(@country)) = 0
                BEGIN
                    RAISERROR(N'Invalid format for @country', 17, 1)
                END
            IF LEN(TRIM(@city)) = 0
                BEGIN
                    RAISERROR (N'Invalid format for @city', 17, 1)
                END
            IF LEN(TRIM(@street)) = 0
                BEGIN
                    RAISERROR (N'Invalid format for @street', 17, 1)
                END
            IF @zipcode = 0
                BEGIN
                    RAISERROR(N'Invalid format for @street',17,1)
                END
            

            IF EXISTS (SELECT u.AdressId FROM [DbUserStandard].[User] as u JOIN [DbUserStandard].[Adress] as a ON u.AdressId = a.Id WHERE u.Id = @userid)
            BEGIN
                

                UPDATE [Adress] SET Country=@country, Street=@street, City=@city, Zipcode=@zipcode WHERE Id =@userid

                
            END

            END TRY


            BEGIN CATCH


            DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
            DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
            DECLARE @ErrorState INT = ERROR_STATE();



            RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)

            RETURN


            END CATCH

        END
        GO
    --#endregion
--#endregion
--#region : User
    --#region : AddUser
        CREATE PROCEDURE [DbUserStandard].[AddUser]
            @mail NVARCHAR (384),
            @pwd NCHAR (60),
            @lastname NVARCHAR(30),
            @firstname NVARCHAR(30)


        AS

        BEGIN
            BEGIN TRY
                IF LEN(TRIM(@mail)) = 0
                    BEGIN
                        RAISERROR(N'Invalid format for @mail',17,1)
                        RETURN;
                    END

                IF EXISTS(SELECT 1 FROM [User] WHERE Mail=@mail)
                    BEGIN
                        RAISERROR(N'this mail is already registered', 17 , 1)
                        RETURN;
                    END

                    IF LEN(TRIM(@lastname)) = 0
                    BEGIN
                        RAISERROR(N'Invalid format for @mail',17,1)
                        RETURN;
                    END
                        IF LEN(TRIM(@firstname)) = 0
                    BEGIN
                        RAISERROR(N'Invalid format for @mail',17,1)
                        RETURN;
                    END


                
                INSERT INTO [User] (Mail, Pwd, Lastname, Firstname)
                VALUES (@mail, [DbUserStandard].[HashPwd](@pwd), @lastname, @firstname)
                


            END TRY


            BEGIN CATCH

                DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
                DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
                DECLARE @ErrorState INT = ERROR_STATE();



                RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
                

                RETURN

            END CATCH
        END
        GO
    --#endregion
    --#region : GetAllUser
        CREATE PROCEDURE [DbUserStandard].[GetAllUser]
            
        AS

        BEGIN
            SELECT Id, Lastname, Firstname, AdressId FROM [DbUserStandard].[User]

        END
        GO
    --#endregion
    --#region : GetUser
        CREATE PROCEDURE [DbUserStandard].[GetUser]
            @mail NVARCHAR(384),
            @pwd NVARCHAR(60)
        AS

        BEGIN

            SELECT Id, Mail, Lastname, Firstname FROM [User] WHERE Mail=@mail AND Pwd = [DbUserStandard].HashPwd(@pwd)

        END
        GO

    --#endregion
    --#region : GetUserById
        CREATE PROCEDURE [DbUserStandard].[GetUserById]

            @id INT
            
        AS

        BEGIN
            SELECT Mail, Lastname, Firstname, AdressId FROM [DbUserStandard].[User] WHERE Id=@id
        END
        GO
    --#endregion
    --#region : GetUserByIdWithToken
        CREATE PROCEDURE [DbUserStandard].[GetUserByIdWithToken]
            @id INT
        AS

        BEGIN
            SELECT Id, Mail, Lastname, Firstname, RefreshToken  FROM [DbUserStandard].[User] WHERE Id = @id
        END
        GO


    --#endregion
    --#region : GetUserFromRToken
        CREATE PROCEDURE [DbUserStandard].[GetUserFromRToken]
            @refreshToken NVARCHAR(128)
        AS

        BEGIN
            SELECT r.Fonction as 'Fonction' FROM [DbUserStandard].[User] as u 
            JOIN [DbUserStandard].[Role] as r 
            ON u.RoleId = r.Id 
            WHERE u.RefreshToken = @refreshToken
        END
        GO

    --#endregion
    --#region : GetUserInfosById
        CREATE PROCEDURE [DbUserStandard].[GetUserInfosById]
            @id INT
        AS

        BEGIN

            SELECT u.Mail, u.Lastname, u.Firstname, a.Country, a.Zipcode, a.City, a.Street FROM [DbUserStandard].[User] as u
            JOIN [DbUserStandard].[Adress] as a ON u.AdressId = a.Id
            WHERE u.Id=@id



        END
        GO



    --#endregion
    --#region : GetUserRole
        CREATE PROCEDURE [DbUserStandard].[GetUserRole]
            @id int
        AS

        BEGIN
            SELECT r.Fonction as 'Fonction' FROM [DbUserStandard].[User] as u JOIN [DbUserStandard].[Role] as r ON u.RoleId = r.Id WHERE u.Id = @id
        END
        GO
    --#endregion
    --#region : InsertRTokenToUser
        CREATE PROCEDURE [DbUserStandard].[InsertRTokenToUser]
            @id INT,
            @refreshToken NVARCHAR(128)
        AS
            
        BEGIN
            UPDATE [User] SET RefreshToken=@refreshToken WHERE Id=@id
        END
        GO
    --#endregion
    --#region : UpdateUserMail
        CREATE PROCEDURE [DbUserStandard].[UpdateUserMail]
            @pwd NVARCHAR(60),
            @mail NVARCHAR(384)
        AS
            @id INT,
            @pwd NVARCHAR(60),
            @mail NVARCHAR(384)
        BEGIN


            BEGIN TRY
                IF LEN(TRIM(@mail)) = 0
                    BEGIN
                        RAISERROR(N'Invalid format for @mail',17,1)
                        RETURN;
                    END

                IF EXISTS(SELECT 1 FROM [User] WHERE Mail=@mail)
                    BEGIN
                        RAISERROR(N'this mail is already registered', 17 , 1)
                        RETURN;
                    END

                UPDATE [User] SET Mail=@mail WHERE Pwd=[HashPwd](@pwd) AND Id=@id

            END TRY

            BEGIN CATCH

                DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
                DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
                DECLARE @ErrorState INT = ERROR_STATE();


                RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)

            END CATCH


        END
        GO

    --#endregion

    
--#endregion


--#endregion

--#region : Script Post déploiement

        INSERT INTO [DbUserStandard].[Role] (Fonction)
        SELECT 'Admin' WHERE NOT EXISTS (SELECT 1 FROM [DbUserStandard].[Role] WHERE Fonction = 'Admin')
        UNION ALL
        SELECT 'Entr' WHERE NOT EXISTS (SELECT 1 FROM [DbUserStandard].[Role] WHERE Fonction = 'Entr')
        UNION ALL
        SELECT 'User' WHERE NOT EXISTS (SELECT 1 FROM [DbUserStandard].[Role] WHERE Fonction = 'User');

        GO



        INSERT INTO [DbUserStandard].[User] (Mail, Pwd, Lastname, Firstname)
        VALUES ('test@mail.com', [DbUserStandard].HashPwd('test123'), 'Jean', 'Louis')
        UPDATE [DbUserStandard].[User] SET RoleId = 1 

        GO


        INSERT INTO [DbUserStandard].[User] (Mail, Pwd, Lastname, Firstname)
        VALUES ('test1@mail.com', [DbUserStandard].HashPwd('test123'), 'test1', 'test1')

        GO



        INSERT INTO [DbUserStandard].[Adress] (Country, City, Zipcode, Street)
        VALUES ('Belgium', 'Namur', 1111, 'Rue des braves')
        GO

        UPDATE [DbUserStandard].[User] SET AdressId = 1 WHERE Id=1
        GO




        IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE [name] = N'DbUserLogin')

        BEGIN
            CREATE LOGIN DbUserLogin WITH PASSWORD=N'test123='
        END

        GO


        IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE [name] = N'DbUserUser')
        BEGIN
            
            CREATE USER DbUserUser FOR LOGIN DbUserLogin;
            
            ALTER ROLE DbUserRole ADD MEMBER DbUserUser;
            ALTER AUTHORIZATION ON SCHEMA::[DbUserStandard] TO [DbUserUser];
            
            
        END

        GO


--#endregion













