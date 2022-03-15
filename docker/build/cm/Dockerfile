# escape=`

ARG BASE_IMAGE
ARG SOLUTION_IMAGE
ARG TOOLING_IMAGE
ARG SMS_IMAGE
ARG HEADLESS_SERVICES_IMAGE
ARG SPE_IMAGE

FROM ${SOLUTION_IMAGE} as solution
FROM ${TOOLING_IMAGE} as tooling
FROM ${SMS_IMAGE} AS sms
FROM ${HEADLESS_SERVICES_IMAGE} as headless_services
FROM ${SPE_IMAGE} as spe
FROM ${BASE_IMAGE}

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR C:\inetpub\wwwroot

# Add developer tools and entrypoint
COPY --from=tooling C:\tools C:\tools

# Add the SMS module
COPY --from=sms  C:\module\cm\content ./

# Add the SPE module
COPY --from=spe /module/cm/content ./

# Copy and init the Headless Services module
COPY --from=headless_services C:\module\cm\content C:\inetpub\wwwroot
COPY --from=headless_services C:\module\tools C:\module\tools
RUN C:\module\tools\Initialize-Content.ps1 -TargetPath C:\inetpub\wwwroot; `
    Remove-Item -Path C:\module -Recurse -Force;

# Add solution website files
COPY --from=solution /artifacts/sitecore/ ./