ECHO OFF
REM This is just a simple command-line tool for renaming the content assets to .xnb for backwards support
REM This tool should only need to be run by the Client in the pre or post build actions
for /r %x in (*.bmp;*.jpg;*.jpeg;*.dds;*.psd;*.png;*.gif;*.tga;*.hdr;*.ttf;*.mp3;*.wav;*.ogg;*.wma) do ren "%x" *.xnb