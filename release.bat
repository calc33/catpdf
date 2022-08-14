@ECHO OFF

REM 環境変数SIGNには sign.bat のパスをセットする
SET SIGN=C:\bin\sign.bat

SET DBG=bin\Debug\catpdf.exe
SET REL=bin\Release\catpdf.exe
IF NOT EXIST %REL% (
  ECHO "Releaseでビルドされていません"
  GOTO END
)
IF NOT EXIST %DBG% GOTO COPY
FOR %%a IN ( %DBG% ) DO SET TDBG=%%~ta
FOR %%b IN ( %REL% ) DO SET TREL=%%~tb

IF "%TDBG%" GTR "%TREL%" (
  ECHO "最後のビルドがDebugで行われています。Releaseでビルドしてください。"
  GOTO END
)

:COPY
REM MKDIR Release
CMD /C %SIGN% bin\Release\catpdf.exe
XCOPY /Y /S /I /EXCLUDE:exclude.txt bin\Release Release

:END
PAUSE