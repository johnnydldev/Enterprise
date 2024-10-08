
CREATE DATABASE ENTERPRISE;

GO 

USE ENTERPRISE;

CREATE TABLE Branch
(
idBranch INT PRIMARY KEY IDENTITY,
description VARCHAR(150) NOT NULL,
createdDate DATETIME DEFAULT GETDATE()
)

--End create branch table
GO

CREATE TABLE Employee
(
idEmployee INT PRIMARY KEY IDENTITY,
name VARCHAR(200) NOT NULL,
age INT NOT NULL,
sex VARCHAR(50) NOT NULL,
workDescription VARCHAR(150) NOT NULL,
idBranch INT REFERENCES Branch (idBranch),
createdDate DATETIME DEFAULT GETDATE()
);

--End create employee table
GO


--STARTING OF STORE PROCEDURE OF EMPLOYEE

CREATE PROC sp_create_employee
(
@name VARCHAR(200),
@age INT,
@sex VARCHAR(50),
@workDescription VARCHAR(150),
@idBranch INT,
@idResult INT OUTPUT,
@message VARCHAR(500) OUTPUT

)
AS
BEGIN
	
	SET @idResult = 0
	SET @message = ''

	IF NOT EXISTS(SELECT * FROM Employee WHERE name = @name and workDescription = @workDescription and age = @age and idBranch = @idBranch)
		BEGIN
			INSERT INTO Employee(name, age, sex, workDescription, idBranch)
			VALUES(@name, @age, @sex, @workDescription, @idBranch)
			SET @idResult = SCOPE_IDENTITY()
		END
	ELSE
			SET @message = 'No se puede crear un nuevo usuario que ya existe.'
		

END

--End sp create employee
GO

CREATE PROC sp_edit_employee
(
@idEmployee INT,
@name VARCHAR(200),
@age INT,
@sex VARCHAR(50),
@workDescription VARCHAR(150),
@idBranch INT,
@response BIT OUTPUT,
@message VARCHAR(500) OUTPUT

)
AS
BEGIN
	
	SET @response = 0
	SET @message =''

	IF NOT EXISTS(SELECT * FROM Employee WHERE name = @name and workDescription = @workDescription and age = @age and idBranch = @idBranch and idEmployee != @idEmployee)

		BEGIN
			UPDATE Employee SET
			name = @name,
			age = @age,
			sex = @sex,
			workDescription = @workDescription,
			idBranch = @idBranch

			WHERE idEmployee = @idEmployee

			SET @response = 1

		END

	ELSE
		
		SET @message = 'No se puede repetir el empleado, los datos ya existen.'

END

--End sp edit employee
GO

CREATE PROC sp_delete_employee
(
@idEmployee INT,
@response BIT OUTPUT,
@message VARCHAR(500) OUTPUT

)
AS
BEGIN
	
	SET @response = 0
	SET @message = ''
	
	IF EXISTS(SELECT * FROM Employee WHERE idEmployee = @idEmployee)
		
		BEGIN

			DELETE TOP(1) FROM Employee WHERE idEmployee = @idEmployee 
			SET @response = 1

		END
	
	ELSE
		
		SET @message = 'El empleado especificado no existe.'


END


GO
--End sp delete employee


--STARING STORE PROCEDURE OF BRANCH
CREATE PROC sp_create_branch
(
@description VARCHAR(150),
@idResult INT OUTPUT

)
AS 
BEGIN

	SET @idResult = 0 

	IF NOT EXISTS(SELECT * FROM Branch WHERE description = @description)

		BEGIN
			
			INSERT INTO Branch(description)
			VALUES (@description)

			SET @idResult = SCOPE_IDENTITY()

		END



END

--End sp create branch 
GO

CREATE PROC sp_edit_branch
(
@idBranch INT,
@description VARCHAR(150),
@response INT OUTPUT,
@message VARCHAR(500) OUTPUT

)
AS 
BEGIN

	SET @response = 0 
	SET @message = ''

	IF NOT EXISTS(SELECT * FROM Branch WHERE description = @description and idBranch != @idBranch)

		BEGIN
			
			UPDATE Branch SET
			description = @description
			WHERE idBranch = @idBranch

			SET @response = 1

		END

	ELSE

		SET @message = 'No se puede repetir la sucursal, ya existe.'


END

-- End sp edit branch
GO

CREATE PROC sp_delete_branch
(
@idBranch INT,
@response INT OUTPUT,
@message VARCHAR(500) OUTPUT
)
AS
BEGIN
	
	SET @response = 0
	SET @message = ''

	IF EXISTS(SELECT * FROM Branch WHERE idBranch = @idBranch)

		IF NOT EXISTS(SELECT * FROM Employee e INNER JOIN Branch b ON e.idBranch = b.idBranch WHERE b.idBranch = @idBranch)
			
			BEGIN
				DELETE TOP(1) FROM Branch WHERE idBranch = @idBranch
				SET @response = 1
			END

		ELSE
			SET @message = 'La sucursal esta vinculada a uno o mas empleados, no puede ser eliminada.'

	ELSE
		
		SET @message = 'La susursal especificada no existe.'


END

--End sp delete branch 
GO













GO

SELECT e.idEmployee, e.name, e.age, e.sex, e.workDescription, b.idBranch, b.description[enterpriseName], e.createdDate FROM Employee e INNER JOIN Branch b ON e.idBranch = b.idBranch;

GO 

USE ENTERPRISE;

GO

SELECT idBranch, description FROM Branch;

GO

EXEC sp_create_branch @description = 'Sysba', @idResult = 0, @message = ''
GO
EXEC sp_create_branch @description = 'Caja Popular', @idResult = 0, @message = ''
GO
EXEC sp_create_branch @description = 'Jumex', @idResult = 0, @message = ''
GO
EXEC sp_create_branch @description = 'Coca cola', @idResult = 0, @message = ''
GO

EXEC sp_create_employee @name ='Pamela Garcia', @age = 23, @sex ='Mujer', @workDescription ='Diseñador', @idBranch = 1,@idResult = 0, @message = ''

GO
--EXEC sp_delete_branch @idBranch = 1, @response = 0, @message = ''


EXEC sp_edit_employee @idEmployee = 2, @name ='Pamela Garcia', @age = 23, @sex ='Mujer', @workDescription ='Diseñador', @idBranch = 2, @response = 0, @message = ''

GO

EXEC sp_create_employee @name ='Miguel Rodriguez', @age = 28, @sex ='Hombre', @workDescription ='Analista de datos', @idBranch = 2,@idResult = 0, @message = ''

GO

--CHARGE ALL EMPLOYEES BY ID ENTERPRISE QUERY
SELECT e.idEmployee, e.name, e.age, e.sex, e.workDescription, e.idBranch, b.description[enterprise] 
FROM Employee e INNER JOIN Branch b ON e.idBranch = b.idBranch
WHERE b.idBranch = 2;

SELECT idBranch FROM Branch WHERE description = 'Jumex'

