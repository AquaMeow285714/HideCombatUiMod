param(
    [string]$Configuration = "Debug"
)

# ===== 基本配置 =====
$Project = "HideCombatUiMod.csproj"
$ModName = "HideCombatUiMod"

# 自动获取 Steam 路径
$SteamPath = (Get-ItemProperty -Path "HKCU:\Software\Valve\Steam").SteamPath
$Sts2Path = Join-Path $SteamPath "steamapps\common\Slay the Spire 2"
$ModsPath = Join-Path $Sts2Path "mods\$ModName"

# ===== 构建 =====

Write-Host "===== BUILD START =====" -ForegroundColor Cyan
dotnet build $Project -c $Configuration

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# ===== 找到输出 DLL =====
$DllPath = "C:\Users\yuzhu\Documents\hide-ui\.godot\mono\temp\bin\Debug\$ModName.dll"
$JsonPath = "$ModName.json"

# ===== 创建 mods 目录 =====
if (!(Test-Path $ModsPath)) {
    Write-Host "Creating mods folder: $ModsPath"
    New-Item -ItemType Directory -Path $ModsPath | Out-Null
}

# ===== 复制文件 =====
Write-Host "===== DEPLOY =====" -ForegroundColor Cyan

Copy-Item $DllPath -Destination $ModsPath -Force
Write-Host "Copied DLL"

if (Test-Path $JsonPath) {
    Copy-Item $JsonPath -Destination $ModsPath -Force
    Write-Host "Copied JSON"
}

Write-Host "===== DONE =====" -ForegroundColor Green
Write-Host "Mod deployed to: $ModsPath"