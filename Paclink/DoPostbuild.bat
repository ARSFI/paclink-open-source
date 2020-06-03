rem cd
rem cd "..\Help Source\"
rem cd
rem del /f "Paclink.chm"
rem Echo copy /y "Paclink.hsc" "Paclink tmp.hsc"
rem copy /y "Paclink.hsc" "Paclink tmp.hsc"
rem echo attrib -R "Paclink tmp.hsc"
rem attrib -R "Paclink tmp.hsc"
rem "C:\Program Files (x86)\JGsoft\HelpScribble\HelpScr.exe" "Paclink tmp.hsc" /c /q
rem Echo ren "Paclink tmp.chm" "Paclink.chm"
rem ren "Paclink tmp.chm" "Paclink.chm"
rem del /f "..\Support Source\Paclink.chm"
rem copy /y "Paclink.chm" "..\Support Source\"
rem del /f "Paclink tmp.hsc"

cd
rem For direct execution out of \Paclink\Paclink, change the following line to cd "..\Support Source\"
cd "..\..\..\Support Source\"
del /f "PaclinkSupport.zip"
"C:\program files\7-zip\7z.exe" a -tzip "PaclinkSupport.zip" *.dll
"C:\program files\7-zip\7z.exe" a -tzip "PaclinkSupport.zip" *.txt
"C:\program files\7-zip\7z.exe" a -tzip "PaclinkSupport.zip" *.aps
"C:\program files\7-zip\7z.exe" a -tzip "PaclinkSupport.zip" *.chm
cd "..\Paclink\"