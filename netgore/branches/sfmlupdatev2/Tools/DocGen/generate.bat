REM Intended for NetGore code releasers only
REM Fully generates the documentation then packs it up into a zip file

@ECHO off

REM Generate documentation with Doxygen
doxygen Doxyfile.doxy

REM Move from temp directory to working directory
rmdir /S /Q docs
move docs_tmp docs

REM Run DoxyPacker, which performs some optimizations and the renaming
DoxyPacker\bin\DoxyPacker.exe docs\html

REM Use OptiPNG to optimize the generated PNGs
optipng docs\html\*.png

REM Use HtmlCompressor to minify all of the HTML files (removes unneeded characters characters)
FOR %%f IN (docs\html\*.html) DO (
	ECHO Minifying %%f
	java -jar htmlcompressor.jar --remove-intertag-spaces --remove-quotes -o "%%f" "%%f"
)

REM Add to zip file using 7-Zip; move to docs\html dir to make the contents at root level in the archive
cd docs\html
7z a -tzip -mx9 ..\..\html.zip .
cd ..\..\

REM Remove docs directory (since we now have html.zip)
rmdir /S /Q docs

REM Done