param(
    [string]$ApiPort = "8080"
)

$ErrorActionPreference = "Stop"

Write-Host "[1/4] Levantando servicios con Docker Compose..." -ForegroundColor Cyan
docker compose up -d --build

Write-Host "`n[2/4] Estado de contenedores:" -ForegroundColor Cyan
docker compose ps

Write-Host "`n[3/4] Logs recientes de PostgreSQL y API:" -ForegroundColor Cyan
$postgresLogs = docker compose logs postgres --tail 120

$apiLogs = @()
for ($i = 0; $i -lt 15; $i++) {
    $apiLogs = docker compose logs api --tail 120
    if ($apiLogs | Select-String -Pattern "Conexion a PostgreSQL verificada." -SimpleMatch) {
        break
    }

    Start-Sleep -Seconds 2
}

$postgresLogs | Select-String -Pattern "ready to accept connections" -SimpleMatch | ForEach-Object {
    Write-Host "PostgreSQL listo: $($_.Line)" -ForegroundColor Green
}

$apiLogs | Select-String -Pattern "Conexion a PostgreSQL verificada." -SimpleMatch | ForEach-Object {
    Write-Host "API conectada a DB: $($_.Line)" -ForegroundColor Green
}

if (-not ($postgresLogs | Select-String -Pattern "ready to accept connections" -SimpleMatch)) {
    Write-Host "No se encontro confirmacion de PostgreSQL en logs. Revisa: docker compose logs postgres" -ForegroundColor Yellow
}

if (-not ($apiLogs | Select-String -Pattern "Conexion a PostgreSQL verificada." -SimpleMatch)) {
    Write-Host "No se encontro confirmacion de conexion DB en logs de API. Revisa: docker compose logs api" -ForegroundColor Yellow
}

Write-Host "`n[4/4] Todo listo para pruebas." -ForegroundColor Cyan
Write-Host "API disponible en: http://localhost:$ApiPort" -ForegroundColor Green
