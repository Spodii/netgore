@echo off
echo This file is to generate the db.sql dump file
SET /P A= Type your database password. 
echo -
SET /P B= Type your port number (Note 3306 is default mysql port). 
echo -
@echo on
mysqldump demogame --user=root --password=%A% --host=localhost --port=%B% --all-tables --routines --create-options --triggers > db.sql