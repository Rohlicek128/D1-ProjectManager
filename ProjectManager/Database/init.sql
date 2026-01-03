use ProjectManagement;


create table Projects(
    id int primary key identity(1,1),
    title varchar(30),
    description text,
    created_date datetime default GETDATE()
);

create table Users(
    id int primary key identity(1,1),
    username varchar(30),
    email varchar(30),
    created_date datetime default GETDATE()
);

create table Roles (
    id int identity primary key,
    name varchar(20) not null
);

create table Tasks (
    id int identity primary key,
    project_id int not null,
    assigned_user_id int null,
    title varchar(30) not null,
    description text,
    status varchar(20) not null check (status in ('todo', 'in_progress', 'done')),
    priority varchar(20) not null check (priority in ('low', 'medium', 'high')),
    completed bit not null default 0,
);

create table UsersProjects (
    id int identity primary key,
    user_id int not null,
    project_id int not null,
    role_id int not null,   

    constraint fk_user foreign key (user_id) references Users(id) on delete cascade,    
    constraint fk_project foreign key (project_id)references Projects(id) on delete cascade,   
    constraint fk_up_role foreign key (role_id)references Roles(id)
);
