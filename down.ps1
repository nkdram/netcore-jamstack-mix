$dockerCompose = "docker-compose -f docker-compose.yml -f docker-compose.override.yml"
$renderingHostsCompose = ''
Get-ChildItem -filter 'docker-compose.rh.*.yml' | ForEach-Object { $renderingHostsCompose += (" -f "+ $_.Name) }
if($renderingHostsCompose -ne ''){
    $dockerCompose += $renderingHostsCompose
}

Write-Host "Stopping docker containers..." -ForegroundColor Green
Write-Host "$dockerCompose down"
Invoke-Expression "$dockerCompose down"