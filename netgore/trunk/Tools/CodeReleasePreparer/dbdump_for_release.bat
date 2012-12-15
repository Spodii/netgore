@echo off
SET A=password
SET B=3306
@echo on

REM Dump
mysqldump demogame --user=root --password=%A% --host=localhost --port=%B% --all-tables --routines --create-options --triggers > db.sql

REM Create temp database
mysqladmin --force --user=root --password=%A% --host=localhost --port=%B% drop demogame_tmp
mysqladmin --user=root --password=%A% --host=localhost --port=%B% create demogame_tmp

REM Import to temp database
mysql --user=root --password=%A% --host=localhost --port=%B% demogame_tmp < db.sql

REM Clean temp
mysql --user=root --password=%A% --host=localhost --port=%B% demogame_tmp < dbdump_for_release-queries.sql

REM Dump temp
mysqldump demogame_tmp --user=root --password=%A% --host=localhost --port=%B% --all-tables --routines --create-options --triggers > db-clean.sql

REM Delete temp database
mysqladmin --force --user=root --password=%A% --host=localhost --port=%B% drop demogame_tmp
