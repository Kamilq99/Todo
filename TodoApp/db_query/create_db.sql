CREATE TABLE tasks (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    IsDone BOOLEAN 
);

SELECT * FROM tasks

INSERT INTO tasks (Name, IsDone) VALUES ('Buy milk', false);
INSERT INTO tasks (Name, IsDone) VALUES ('Buy eggs', false);
INSERT INTO tasks (Name, IsDone) VALUES ('Buy bread', false);

ALTER TABLE tasks RENAME COLUMN id TO Id;
ALTER TABLE tasks RENAME COLUMN Isdone TO IsDone;
ALTER TABLE tasks RENAME COLUMN name TO Name;