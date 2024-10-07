create database DBCrudBlazor

use DBCrudBlazor

CREATE TABLE Departamento(
IdDepartamento int primary key identity (1,1),
Nombre varchar(50) not null
)

CREATE TABLE Empleado(
IdEmpleado int primary key identity (1,1),
NombreCompleto varchar(50) not null,
IdDepartamento int references Departamento(IdDepartamento) not null,
Sueldo int not null,
FechaContrato date not null
)

insert into Departamento(nombre) values
('Administracion'),
('Marketing'),
('Ventas'),
('Comercio')



insert into Empleado(nombreCompleto,idDepartamento,sueldo,fechaContrato)values
('Franco Hernandez',1,1400,getdate())

select * from Empleado
select * from Departamento