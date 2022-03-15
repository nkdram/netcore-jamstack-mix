# escape=`

# This Dockerfile will contain the resources that are needed for the custom Sitecore modules that are part of this Sitecore platform.
# It does not produce a runnable image itself, it will be consumed by other images

ARG BASE_IMAGE
ARG BUILD_IMAGE

FROM ${BUILD_IMAGE} as builder

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install latest PackageProvider & Sitecore.Courier
RUN Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force; `
	Install-Module -Name Sitecore.Courier -Repository PSGallery -Force -Confirm:$False  -RequiredVersion 1.4.3; `
	Import-Module Sitecore.Courier -Force -Verbose;

# Copy module resources from host
COPY /modules/ /temp/

# Collect all serialized items for the core database, master database and the files
RUN $moduleFolders = [System.Collections.ArrayList]@(); `
	$coreItemsFolders = [System.Collections.ArrayList]@(); `
	$masterItemsFolders = [System.Collections.ArrayList]@(); `
	$filesFolders = [System.Collections.ArrayList]@(); `
	$moduleFolders.AddRange(@(Get-ChildItem -Path '.\temp' -Directory)); `
	$moduleFolders | ForEach-Object { $coreItemsFolders.AddRange(@(Get-ChildItem -Path $_.FullName -Directory -Filter core)) }; `
	$coreItemsFolders | ForEach-Object { robocopy $_.FullName C:/temp/coreitems /s /ndl /njh /njs * }; `
	$moduleFolders | ForEach-Object { $masterItemsFolders.AddRange(@(Get-ChildItem -Path $_.FullName -Directory -Filter master)) }; `
	$masterItemsFolders | ForEach-Object { robocopy $_.FullName C:/temp/masteritems /s /ndl /njh /njs * }; `
	$moduleFolders | ForEach-Object { $filesFolders.AddRange(@(Get-ChildItem -Path $_.FullName -Directory -Filter files)) }; `
	$filesFolders | ForEach-Object { robocopy $_.FullName C:/artifacts/files /s /ndl /njh /njs * };

# Create folder for db artifacts
RUN New-Item -Path 'C:\\artifacts\\db' -ItemType 'Directory' -Force | Out-Null;

# Create dacpac for core items and copy to artifacts folder
RUN New-CourierPackage -Target "C:/temp/coreitems" -Output "C:/temp/dacpac" -SerializationProvider "Rainbow" -IncludeFiles $false -EnsureRevision $true -DacPac $true; `
	Rename-Item -Path "C:/temp/dacpac/.sql" -NewName "Sitecore.core.sql"; `
	Rename-Item -Path "C:/temp/dacpac/.dacpac" -NewName "Sitecore.core.dacpac"; `
	Copy-Item -Path 'C:\\temp\\dacpac\\Sitecore.core.dacpac' -Destination 'C:\\artifacts\\db';

# Create dacpac for master items and copy to artifacts folder
RUN New-CourierPackage -Target "C:/temp/masteritems" -Output "C:/temp/dacpac" -SerializationProvider "Rainbow" -IncludeFiles $false -EnsureRevision $true -DacPac $true; `
	Rename-Item -Path "C:/temp/dacpac/.sql" -NewName "Sitecore.master.sql"; `
	Rename-Item -Path "C:/temp/dacpac/.dacpac" -NewName "Sitecore.master.dacpac"; `
	Copy-Item -Path 'C:\\temp\\dacpac\\Sitecore.master.dacpac' -Destination 'C:\\artifacts\\db';
	
FROM ${BASE_IMAGE}
WORKDIR /artifacts

# Copy in all resources for the modules
COPY --from=builder /artifacts ./