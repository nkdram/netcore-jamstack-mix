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
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
# Gather only artifacts necessary for NuGet restore, retaining directory structure
## COPYING THE SOLUTION WITH CORE & Sugcon in it
COPY SugconRH.sln nuget.config /nuget/
COPY src/ /temp/
RUN Invoke-Expression 'robocopy C:/temp C:/nuget/src /s /ndl /njh /njs *.csproj *.scproj packages.config'

FROM ${BUILD_IMAGE} AS builder
ARG BUILD_CONFIGURATION
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Create an empty working directory
WORKDIR /build-renderinghost-Sugcon

# Copy prepped NuGet artifacts, and restore as distinct layer to take better advantage of caching
COPY --from=nuget-prep ./nuget ./
# Restore NuGet packages
RUN dotnet restore

# Copy remaining source code
COPY src/ ./src/

# Publish the Rendering Host Artifacts
ARG RENDERINGHOST_PROJECT_PATH
WORKDIR "./src${RENDERINGHOST_PROJECT_PATH}"
RUN dotnet publish -c Release -o /build-renderinghost-Sugcon/Sugcon

# Start new Stage
FROM ${BASE_IMAGE}
WORKDIR /artifacts


# Copy in all artifacts for sitecore, identity & rendering host
COPY --from=builder /build-renderinghost-Sugcon/Sugcon ./renderinghost-Sugcon/