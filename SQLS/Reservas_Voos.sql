-- CREATE SCHEMA VOOS AUTHORIZATION "VicenteCosta"
	CREATE Table Reservas_de_Voos_Airport(
		city			Varchar(256)		not null,
		[state]			Varchar(256)		not null,
		[name]			Varchar(256)		not null,
		Airport_code			int			not null		IDENTITY(1,1),		
		Primary key (Airport_code)

	);

	CREATE Table Reservas_de_Voos_Flight(
		number			int					not null		IDENTITY(1,1),
		airline			Varchar(256)		not null,
		Weekdays		Varchar(16)			not null,
	
		Primary key(number)
	);

	CREATE Table Reservas_de_Voos_Fare(
		code					int			not null		IDENTITY(1,1),
		Flight_Number			int			not null,
		Restrictions		  Varchar		not null,
		Amount			Decimal(10,2)		not null,
		Primary key (code , Flight_number),
		CONSTRAINT FK_Fare_Flight Foreign key(Flight_Number) REFERENCES Reservas_de_Voos_Flight(number),
		check(amount >= 0)
	);


	CREATE Table Reservas_de_Voos_Flight_leg(
		Flight_Number			int			not null,
		leg_no					int			not null		IDENTITY(1,1),
		Airport1				int			not null,
		Airport2				int			not null,
		Scheduled_d1			date		not null,
		Scheduled_d2			date		not null,

		Primary key (Flight_Number, leg_no),
		CONSTRAINT FK_Flight_leg_Flight Foreign key(Flight_Number) REFERENCES Reservas_de_Voos_Flight(number),
		CONSTRAINT FK_Flight_leg_Airport Foreign key(Airport1) REFERENCES Reservas_de_Voos_Airport(Airport_code),
		CONSTRAINT FK_Flight_leg_Airport Foreign key(Airport2) REFERENCES Reservas_de_Voos_Airport(Airport_code)
	);


;
	CREATE Table Reservas_de_Voos_Leg_instance(
		[date]				date			not null,
		flight_number		int				not null,
		flight_leg_no		int				not null,
		no_of_avail_seats	int				not null,
		arr_time			time			not null,
		Dep_time			time			not null,

		primary key([date], flight_number, flight_leg_no),
		CONSTRAINT FK_Leg_instance_Flight_leg foreign key(flight_number)	REFERENCES Reservas_de_Voos_Flight_leg (Flight_Number),
		CONSTRAINT FK_Leg_instance_Flight_leg foreign key(flight_leg_no)	REFERENCES Reservas_de_Voos_Flight_leg (leg_no),
		check (no_of_avail_seats >= 0)

	
	
	);


	CREATE table Reservas_de_Voos_Airplane_type(
		typeName			varchar(256)	not null,
		max_seats					int		not null,
		company				varchar(256)	not null,
		primary key	(typeName),
		check(max_seats > 0)
	);


	CREATE table Reservas_de_Voos_Can_land(
		Airport_Code				int		not null,
		typeName			varchar(256)	not null,
		primary key (typename, Airport_code),
		CONSTRAINT FK_can_land_Airplane_type foreign key (typename) References Reservas_de_Voos_Airplane_type(typename),
		CONSTRAINT FK_can_land_Airport foreign key (Airport_Code) References Reservas_de_Voos_Airport(Airport_code),

	);


;
	CREATE table Reservas_de_Voos_Airplane(
		Airplane_id					int		not null			IDENTITY(1,1),
		AiT_type_name		varchar(256)	not null,
		Total_no_of_seats			int		not null,
		primary key(Airplane_id),
		CONSTRAINT FK_Airplane_Airplane_type foreign key (AiT_type_name) References Reservas_de_Voos_Airplane_type (typename),
		check(Total_no_of_seats > 0)
	);



	CREATE table Reservas_de_Voos_Seat(
		Seat_no						int		not null			IDENTITY(1,1),
		Leg_instance_date			date	not null,
		Flight_number				int		not null,
		flight_leg_num				int		not null,
		primary key (Seat_no, Leg_instance_date, Flight_number, flight_leg_num),
		CONSTRAINT FK_Seat_leg_instance foreign key (Leg_instance_date) references Reservas_de_Voos_Leg_instance([date]),
		CONSTRAINT FK_Seat_leg_instance foreign key (Flight_number) references Reservas_de_Voos_Leg_instance(Flight_number),
		CONSTRAINT FK_Seat_leg_instance foreign key (flight_leg_num) references Reservas_de_Voos_Leg_instance(flight_leg_num),

	);
