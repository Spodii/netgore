echo This file is to generate the db.sql dump file
echo You will probably need to change the database connection strings

mysqldump demogame --user=root --password= --host=localhost --all-tables --routines --create-options > db.sql