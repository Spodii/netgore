@echo off
echo This file will import the database dump from its default location
echo You will probably need to change the database connection strings in the .bat file.
@echo on
mysql --user=root --password= --host=localhost --port=3306 demogame < ..\db.sql