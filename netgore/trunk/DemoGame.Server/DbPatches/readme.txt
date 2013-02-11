This folder contains database patches for NetGore. The purpose of these are to make it easy for existing NetGore users to get the latest database schema changes without having to manually port them over or restore from the database dump (and lose all their content as well).

Simply run Patch.bat and any missing patches will be applied.

You do not need to add your own database patches here. It is recommended you actually don't even touch anything in here if you are not developing for NetGore (in opposed to your own game created with NetGore).

There is no harm in running Patch.bat multiple times as it only applies any patches that have not been applied already.

== For Developers ==

The first directory should be just the year. The files then must start with the datetime format "yyyy-mm-dd" and end with ".sql". The rest is up to you. If there are multiple patches for the same date, they are then sorted by the part after the date. So if you have multiple patches for one day, do something like:

2012-05-05 01 First patch.sql
2012-05-05 02 Second patch.sql
...etc

Keep the names descriptive but also the full file name must be <= 255 chars.