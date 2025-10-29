CREATE DATABASE testdb;
\c testdb
CREATE TABLE test (id SERIAL PRIMARY KEY, name VARCHAR(50));
INSERT INTO test (name) VALUES ('PostgreSQL çalışıyor!');
SELECT * FROM test;