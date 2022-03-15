[CmdletBinding(DefaultParameterSetName = "no-arguments")]
Param (
    [Parameter(HelpMessage = "Skip Build")]
    [switch]$SkipBuild
)

# ENSURE ENV FILE EXISTS
if(-not (Test-Path .env)) {
    Write-Host "Docker Env file not found. Have you run init.ps1 first? Refer to README.md for details" -ForegroundColor Red
    exit
}
    
#Store env variables in settings - that can be used later
Get-Content ".env" | foreach-object -begin {$settings=@{}} -process { if($_.Contains("=")){  $value = [regex]::split($_,'='); if(($value[0].CompareTo("") -ne 0) -and ($value[0].StartsWith("[") -ne $True) -and ($value[0].StartsWith("#") -ne $True) ) { if(-Not $settings.ContainsKey($value[0])) { $settings.Add($value[0], $value[1]) }  } } }

#Get Platform Name from .env file
$PlatformName = $settings.COMPOSE_PROJECT_NAME


# Restore dotnet tool for sitecore login and serialization
dotnet tool restore

$dockerCompose = "docker-compose -f docker-compose.yml -f docker-compose.override.yml -f docker-compose.rh.Sugcon.yml"

if($SkipBuild -eq $false){
    # Build all containers in the Sitecore instance, forcing a pull of latest base containers
    Write-Host "BUILD the docker containers..." -ForegroundColor Green
    Write-Host "$dockerCompose build"
    Invoke-Expression "$dockerCompose build"

    if ($LASTEXITCODE -ne 0)
    {
        Write-Error "Container build failed, see errors above."
    }
}

# Run the docker containers
Write-Host "Run the docker containers..." -ForegroundColor Green
Write-Host "$dockerCompose up -d"
Invoke-Expression "$dockerCompose up -d"

# Wait for Traefik to expose CM route
Write-Host "Waiting for CM to become available..." -ForegroundColor Green
$startTime = Get-Date
do {
    Start-Sleep -Milliseconds 100
    try {
        $status = Invoke-RestMethod "http://localhost:8079/api/http/routers/cm-secure@docker"
    } catch {
        if ($_.Exception.Response.StatusCode.value__ -ne "404") {
            throw
        }
    }
	$i = $i + 1
	if($i -eq 10){
		$WaitIndicator = $WaitIndicator + "."
		Write-Host "`r$WaitIndicator" -NoNewline
		$i = 0
	}
} while ($status.status -ne "enabled" -and $startTime.AddSeconds(60) -gt (Get-Date))
Write-Host "`r$WaitIndicator"
if (-not $status.status -eq "enabled") {
    $status
    Write-Error "Timeout waiting for Sitecore CM to become available via Traefik proxy. Check CM container logs."
}

dotnet sitecore login --cm "https://$($PlatformName.ToLower())-cm.sc10.localhost/" --auth "https://$($PlatformName.ToLower())-id.sc10.localhost/" --allow-write true
if ($LASTEXITCODE -ne 0) {
    Write-Error "Unable to log into Sitecore, did the Sitecore environment start correctly? See logs above."
}

Write-Host "Pushing latest items to Sitecore..." -ForegroundColor Green

dotnet sitecore ser push
if ($LASTEXITCODE -ne 0) {
    Write-Error "Serialization push failed, see errors above."
}

dotnet sitecore publish
if ($LASTEXITCODE -ne 0) {
    Write-Error "Item publish failed, see errors above."
}

Write-Host "Opening site..." -ForegroundColor Green

Start-Process "https://$($PlatformName.ToLower())-cm.sc10.localhost/sitecore/"
Start-Process "https://$($PlatformName.ToLower())-cd.sc10.localhost/"


$RenderingHostSites = $settings.Keys | Where-Object {$_ -match "RE_HOST"} | % { $settings.Item($_) }
foreach ($RenderingHostSite in $RenderingHostSites) {
	Start-Process "https://$RenderingHostSite"
}

Write-Host "Use the following command to bring your docker environment Stop again:" -ForegroundColor Green Write-Host ".\Stop.ps1"