# escape=`

# This Dockerfile will build the Sitecore solution and save the build artifacts for use in
# other images, such as 'cm' and 'rendering'. It does not produce a runnable image itself.

ARG BASE_IMAGE
ARG BUILD_IMAGE

# In a separate image (as to not affect layer cache), gather all NuGet-related solution assets, so that
# we have what we need to run a cached NuGet restore in the next layer:
# https://stackoverflow.com/questions/51372791/is-there-a-more-elegant-way-to-copy-specific-files-using-docker-copy-to-the-work/61332002#61332002
# This technique is described here:
# https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1#the-dockerfile-1
FROM ${BUILD_IMAGE} AS nuget-prep
ARG SOLUTION_NAME
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
# Gather only artifacts necessary for NuGet restore, retaining directory structure
COPY "${SOLUTION_NAME}.sln" nuget.config /nuget/
COPY src/ /temp/
RUN Invoke-Expression 'robocopy C:/temp C:/nuget/src /s /ndl /njh /njs *.csproj *.scproj packages.config'

FROM ${BUILD_IMAGE} AS builder
ARG BUILD_CONFIGURATION
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Create an empty working directory
WORKDIR /build

# Copy prepped NuGet artifacts, and restore as distinct layer to take better advantage of caching
COPY --from=nuget-prep ./nuget ./

# Restore NuGet packages -- Not needed to restore nuget as it is restored in line 40 and it conflicts with multiple solution
# Restore NuGet packages
RUN dotnet restore

# Copy remaining source code
COPY src/ ./src/

# Build and publish the environment projects
RUN $csprojFiles = [System.Collections.ArrayList]@(); `
    $csprojFiles.AddRange(@(Get-ChildItem -Path '.\src\Environment' -Filter *.csproj -Recurse)); `
    $csprojFiles | ForEach-Object { msbuild $_.FullName /p:Configuration=$env:BUILD_CONFIGURATION /restore /m /p:DeployOnBuild=true /p:PublishProfile=Local };

# Start new Stage
FROM ${BASE_IMAGE}
WORKDIR /artifacts

# Copy in all artifacts for the specific environments
COPY --from=builder /build/docker/deploy ./