[CmdletBinding(DefaultParameterSetName = "no-arguments")]
Param (
    [Parameter(HelpMessage = "The path to a valid Sitecore license.xml file.",
        ParameterSetName = "env-init")]
    [string]$LicenseXmlPath
)

$ErrorActionPreference = "Stop";


if (-not $LicenseXmlPath.EndsWith("license.xml")) {
    Write-Error "Sitecore license file must be named 'license.xml'."
}
if (-not (Test-Path $LicenseXmlPath)) {
    Write-Error "Could not find Sitecore license file at path '$LicenseXmlPath'."
}
# We actually want the folder that it's in for mounting
$LicenseXmlPath = (Get-Item $LicenseXmlPath).Directory.FullName
Set-EnvFileVariable "HOST_LICENSE_FOLDER" -Value $LicenseXmlPath
Write-Host "License path is set in env File!" -ForegroundColor Green
################################################
# Retrieve and import SitecoreDockerTools module
################################################

# Check for Sitecore Gallery
Import-Module PowerShellGet
$SitecoreGallery = Get-PSRepository | Where-Object { $_.SourceLocation -eq "https://sitecore.myget.org/F/sc-powershell/api/v2" }
if (-not $SitecoreGallery) {
    Write-Host "Adding Sitecore PowerShell Gallery..." -ForegroundColor Green 
    Register-PSRepository -Name SitecoreGallery -SourceLocation https://sitecore.myget.org/F/sc-powershell/api/v2 -InstallationPolicy Trusted
    $SitecoreGallery = Get-PSRepository -Name SitecoreGallery
}

#Install and Import SitecoreDockerTools 
$dockerToolsVersion = "10.0.5"
Remove-Module SitecoreDockerTools -ErrorAction SilentlyContinue
if (-not (Get-InstalledModule -Name SitecoreDockerTools -RequiredVersion $dockerToolsVersion -ErrorAction SilentlyContinue)) {
    Write-Host "Installing SitecoreDockerTools..." -ForegroundColor Green
    Install-Module SitecoreDockerTools -RequiredVersion $dockerToolsVersion -Scope CurrentUser -Repository $SitecoreGallery.Name -AllowClobber
}
Write-Host "Importing SitecoreDockerTools..." -ForegroundColor Green
Import-Module SitecoreDockerTools -RequiredVersion $dockerToolsVersion


##################################
# Configure TLS/HTTPS certificates
##################################
Push-Location docker\traefik\certs
try {
    $mkcert = ".\mkcert.exe"
    if ($null -ne (Get-Command mkcert.exe -ErrorAction SilentlyContinue)) {
        # mkcert installed in PATH
        $mkcert = "mkcert"
    } elseif (-not (Test-Path $mkcert)) {
        Write-Host "Downloading and installing mkcert certificate tool..." -ForegroundColor Green 
        Invoke-WebRequest "https://github.com/FiloSottile/mkcert/releases/download/v1.4.1/mkcert-v1.4.1-windows-amd64.exe" -UseBasicParsing -OutFile mkcert.exe
        if ((Get-FileHash mkcert.exe).Hash -ne "1BE92F598145F61CA67DD9F5C687DFEC17953548D013715FF54067B34D7C3246") {
            Remove-Item mkcert.exe -Force
            throw "Invalid mkcert.exe file"
        }
    }
    Write-Host "Generating Traefik TLS certificate..." -ForegroundColor Green
    & $mkcert -install
    & $mkcert -key-file key.pem -cert-file cert.pem "*.sc10.localhost"
}
catch {
    Write-Error "An error occurred while attempting to generate TLS certificate: $_"
}
finally {
    Pop-Location
}


Get-Content ".env" | foreach-object -begin {$settings=@{}} -process { if($_.Contains("=")){  $value = [regex]::split($_,'='); if(($value[0].CompareTo("") -ne 0) -and ($value[0].StartsWith("[") -ne $True) -and ($value[0].StartsWith("#") -ne $True) ) { if(-Not $settings.ContainsKey($value[0])) { $settings.Add($value[0], $value[1]) }  } } }

$PlatformName = $settings.COMPOSE_PROJECT_NAME

################################
# Add Windows hosts file entries
################################
$cmHost = $PlatformName.ToLower() + "-cm.sc10.localhost"
$cdHost = $PlatformName.ToLower() + "-cd.sc10.localhost" 
$idHost = $PlatformName.ToLower() + "-id.sc10.localhost"
$RE_HOST_Sugcon = "Sugcon-rh.sc10.localhost"

Write-Host "Adding Windows hosts file entries..." -ForegroundColor Green
Add-HostsEntry $cmHost
Add-HostsEntry $cdHost
Add-HostsEntry $idHost
Add-HostsEntry $RE_HOST_Sugcon

#########################################################
# Tell git to ignore changes to .env
# Note: For future upgrades this can be undone by running
# git update-index --no-assume-unchanged .env
#########################################################
git update-index --assume-unchanged .env

Write-Host "Done!" -ForegroundColor Green