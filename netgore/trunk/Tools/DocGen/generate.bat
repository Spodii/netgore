REM Intended for NetGore code releasers only
REM Fully generates the documentation then packs it up into a zip file

doxygen Doxyfile.doxy
move docs_tmp docs
DoxyPacker\bin\DoxyPacker.exe docs\html
cd docs\html
optipng  -o7 docs\html\*.png
7z a -tzip -mx9 ..\..\html.zip .
cd ..\..\
rmdir /S /Q docs