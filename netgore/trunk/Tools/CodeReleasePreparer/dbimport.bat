@echo off
echo This file will import the database dump file at db.sql
echo If this window closes immediately after executing, there was likely an error
SET /P A= Type your database password. 
echo -
SET /P B= Type your port number (Note 3306 is default mysql port). 
echo -
@echo on
mysql --user=root --password=%A% --host=localhost --port=%B% demogame < db.sql