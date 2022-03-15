# escape=`

ARG BASE_IMAGE
ARG HEADLESS_SERVICES_IMAGE
ARG SPE_IMAGE

FROM ${SPE_IMAGE} as spe
FROM ${HEADLESS_SERVICES_IMAGE} as headless_services
FROM ${BASE_IMAGE}

# Add SPE module
COPY --from=spe \module\db \spe_data
RUN C:\DeployDatabases.ps1 -ResourcesDirectory C:\spe_data; `
    Remove-Item -Path C:\spe_data -Recurse -Force;

# Copy and init the Headless Services module
COPY --from=headless_services C:\module\db C:\headless_services_data
RUN C:\DeployDatabases.ps1 -ResourcesDirectory C:\headless_services_data; `
    Remove-Item -Path C:\headless_services_data -Recurse -Force;

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
