@echo off
echo This file will import the database dump from its default location
SET /P A= Type your database password. 
echo -
SET /P B= Type your port number (Note 3306 is default mysql port). 
echo -
@echo on
mysql --user=root --password=%A% --host=localhost --port=%B% demogame < db.sql