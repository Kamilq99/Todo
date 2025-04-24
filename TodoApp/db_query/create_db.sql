CREATE TABLE Tasks (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    is_done BOOLEAN 
);

SELECT * FROM Tasks

INSERT INTO Tasks (name, is_done) VALUES ('Task 1', false);

ALTER TABLE Tasks RENAME TO tasks;

SELECT * FROM tasks