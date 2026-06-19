@echo off
setlocal enabledelayedexpansion

:: ==========================================
:: 設定項目
:: ==========================================
set "REPO_OWNER=SharkBot-Dev"
set "REPO_NAME=SharkLauncher"
set "EXTRACT_DIR=%USERPROFILE%\Documents\%REPO_NAME%"
set "SHORTCUT_NAME=SharkLauncher"

echo ==========================================
echo  %REPO_NAME% セットアップスクリプト
echo ==========================================

echo 最新リリースの情報を取得中...
set "API_URL=https://api.github.com/repos/%REPO_OWNER%/%REPO_NAME%/releases/latest"
set "TEMP_JSON=%TEMP%\release_latest.json"

powershell -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; Invoke-WebRequest -Uri '%API_URL%' -OutFile '%TEMP_JSON%'" -UserAgent "Mozilla/5.0"

if not exist "%TEMP_JSON%" (
    echo [エラー] 最新リリースの情報取得に失敗しました。ネット接続を確認してください。
    pause
    exit /b
)

:: JSONからbrowser_download_url (zip) を抽出するPowerShellコマンド
set "GET_ZIP_URL_CMD=(Get-Content '%TEMP_JSON%' -Raw | ConvertFrom-Json).assets | Where-Object { $_.name -like '*.zip' } | Select-Object -ExpandProperty browser_download_url -First 1"

for /f "delims=" %%i in ('powershell -Command "%GET_ZIP_URL_CMD%"') do (
    set "ZIP_URL=%%i"
)

if "%ZIP_URL%"=="" (
    echo [エラー] リリース内から.zipファイルが見つかりませんでした。
    del "%TEMP_JSON%"
    pause
    exit /b
)

echo ダウンロードURL: !ZIP_URL!
echo ファイルをダウンロード中...
set "ZIP_FILE=%TEMP%\%REPO_NAME%_latest.zip"

powershell -Command "Invoke-WebRequest -Uri '!ZIP_URL!' -OutFile '%ZIP_FILE%'"

if not exist "%ZIP_FILE%" (
    echo [エラー] zipファイルのダウンロードに失敗しました。
    del "%TEMP_JSON%"
    pause
    exit /b
)

echo 解凍中... [%EXTRACT_DIR%]
if not exist "%EXTRACT_DIR%" mkdir "%EXTRACT_DIR%"

powershell -Command "Expand-Archive -Path '%ZIP_FILE%' -DestinationPath '%EXTRACT_DIR%' -Force"

set "EXE_PATH="

:: フォルダ内（サブフォルダ含む）から最初に見つかった .exe を対象とする
for /r "%EXTRACT_DIR%" %%f in (*.exe) do (
    if not defined EXE_PATH (
        set "EXE_PATH=%%f"
    )
)

if "%EXE_PATH%"=="" (
    echo [警告] 解凍先から .exe ファイルが見つかりませんでした。
    echo ショートカットの作成をスキップします。
    goto cleanup
)

echo 実行ファイルを発見: !EXE_PATH!

echo デスクトップにショートカットを作成中...
set "SHORTCUT_PATH=%USERPROFILE%\Desktop\%SHORTCUT_NAME%.lnk"
set "WSH_SCRIPT=%TEMP%\CreateShortcut.vbs"

echo Set oWS = WScript.CreateObject("WScript.Shell") > "%WSH_SCRIPT%"
echo sLinkFile = "%SHORTCUT_PATH%" >> "%WSH_SCRIPT%"
echo Set oLink = oWS.CreateShortcut(sLinkFile) >> "%WSH_SCRIPT%"
echo oLink.TargetPath = "!EXE_PATH!" >> "%WSH_SCRIPT%"
echo oLink.WorkingDirectory = "%EXTRACT_DIR%" >> "%WSH_SCRIPT%"
echo oLink.Save >> "%WSH_SCRIPT%"

cscript //nologo "%WSH_SCRIPT%"
del "%WSH_SCRIPT%"

echo [成功] ショートカットをデスクトップに作成しました: %SHORTCUT_NAME%.lnk

:cleanup
:: 一時ファイルの削除
if exist "%TEMP_JSON%" del "%TEMP_JSON%"
if exist "%ZIP_FILE%" del "%ZIP_FILE%"

echo ==========================================
echo  セットアップが完了しました。
echo ==========================================
pause