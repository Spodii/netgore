@echo off
echo This file is to generate the db.sql dump file
echo You will probably need to change the database connection strings within the .bat file.
@echo on
mysqldump demogame --user=root --password= --host=localhost --port=3306 --all-tables --routines --create-options > db.sql