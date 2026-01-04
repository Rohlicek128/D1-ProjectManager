-- Tables
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
    project_id int not null FOREIGN KEY REFERENCES Projects(id),
    assigned_user_id int not null FOREIGN KEY REFERENCES Users(id),
    title varchar(30) not null,
    description text,
    status varchar(20) not null check (status in ('todo', 'in_progress', 'done')),
    priority varchar(20) not null check (priority in ('low', 'medium', 'high')),
    completed bit not null default 0
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


-- Views
create view view_users_workload as
select u.username, count(t.assigned_user_id) as assignments
from Users u left join Tasks t
on t.assigned_user_id = u.id
group by u.username;

create view view_projects_contributors_amount as
select p.title, count(up.user_id) as contributors
from Projects p left join UsersProjects up
on up.project_id = p.id
group by p.title;


-- Transaction
begin transaction;
update Tasks
set assigned_user_id = 3
where id = 1;
if @@rowcount = 0
    begin
        rollback transaction;
        print 'Task not found, rollback transaction';
    end
else
    begin
        commit transaction;
        print 'Task found, commit transaction';
    end