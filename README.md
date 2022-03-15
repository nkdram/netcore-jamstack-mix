## Contributors

- Ramkumar Nambhi Krishnan Dhinakaran
- Elakkuvan Rajamani

## Description

**JamStack** - We know this buzzword for quite sometime and it's popular among only the Front Ender community. Currently there are no handy modules available for the Sitecore Backenders to play around this term "JamStacking". 

Here we are unveiling a whole new approach of JamStacking a Sitecore website in a simpler way. Now the BE's can have same fun as FE's.

## Pre-requisites

- Docker for Windows
- Visual Studio 2019
- .NET Core (>= v 3.1) and .NET Framework 4.8
- Netlify Account (To create & host Site)
- Valid Sitecore license
- Approx 40gb HD space

## Dependencies

- **Statiq.Framework** - Custom Sitecore Page generation Pipeline - [Statiq Framework](https://www.statiq.dev/framework)
- **NetlifySharp** - Deployment to netfliy Site - 
- **StackExchangeRedis** - Store & retrieve values in Redis InMemory Cache
- **AngleSharp** - HTMLDocument processing

## Installation instructions

### Docker Setup

- run ./init.ps1 -LicenseXmlPath 'Provide-your-Sitecore-Path-Along-with-License.xml-file'
- run ./up.ps1 (for the first time to build and start containers)
- run ./up.ps1 -SkipBuild (Just to run containers by skipping build)
- It will prompt for the login to sitecore. Login and accept the device verification
- Wait for the script to complete and open the CM, CD and rendering host sites
- To Stop the environment again 
- run ./down.ps1 to stop containers

## Configuration

In 'Appsettings.json' provide below values under Foundation:GrapheNetor section,

| Configuration Category | Configuration | Comment                                                           |
| :--------------------- | :------------ | :---------------------------------------------------------------- |
| GraphQL                | UrlLive       | ExperienceEdge Endpoint                                           |
| GraphQL                | UrlEdit       | ExperienceEdge Endpoint for editing mode                          |
| Pipelines              | Language      | Site language                                                     |
| Pipelines              | Site          | Site Name                                                         |
| Pipelines              | Path          | Site Root path                                                    |
| Netlify                | Token         | Netlify Token xxxxxxxxxxx                                         |
| Netlify                | SourcePath    | Full Path of output folder(Do not change while running in Docker) |
| Netlify                | SiteId        | Netlify Site ID xxxxx-xxxxxx-xxxxxx                               |

## Usage instructions

After updating the configuration, run the site using ./up.ps1 -Skipbuild command.

- Add new or update contents in Sugcon Site
- Upon publishing - Pages that are updated gets generated automatically and it's published to Netlify Site 

🏆 [Live Site](https://adoring-bhabha-59d79c.netlify.app/en/home) - Check  this Site

## Working Behaviour

The following diagram depicts the working behaviour of the approach

![Working Behaviour](docs/images/Working_Behaviour.png?raw=true "Working Behaviour")

1. As you are seeing, the Content Authors will be editing the pages in Experience Editor which is supported by Rendering Host JSS Renderer 
1. When there is a publish, the new updated content will be available for access via the Experience Edge Endpoint
1. The publish end will trigger the GeneratePage API call. This GeneratePages end point is written with set of predefined pipelines that gets executed one after another. The following are the two important pipelines with this approach,
    1. CopyAssets Pipeline
    1. Pages Pipeline

The following diagrams showes these pipelines and its different modulelists and modules in details.

![Pipelines](docs/images/Pipelines.png?raw=true "Pipelines")

These pipelines are working based on the behaviour of the Statiq framework pipelines and modules. You can find more info about them from here,

[Statiq Pipelines and Modules](https://www.statiq.dev/guide/pipelines/)

## Pipelines in details

### 👉 CopyAssests Pipeline
1. The primary functionality of this pipeline is to copy the different statiq assests of the website
1. Statiq assest like Favourite Icons, Jss, Css etc.,

![Copy Assests](docs/images/Copy_Assests_Pipeline.png?raw=true "Copy Assests")

### 👉 Pages Pipelines

This is the core pipelines of our approach and it has following different Modules Lists,

#### ↪️ Input Modules Lists

This is the first module list in the pages pipeline and has the following modules,

##### ➡️ Input Pages Module
1. It queries all the pages of the given site
1. It has its own GraphQLSitePageService
1. Retrives only the Id, Path and Language of the page

##### ➡️ Process Pages Module
1. It processes the pages one by one
1. It makes GraphQL Layout Request to the Experience Edge Endpoint to fetch the Layout Response of the given page
1. It takes only the Id and Language of the page as input params

It looks like below,

![Input Pages Module List](docs/images/Input_Pages_ModuleList.png?raw=true "Input Pages Module List")

### ✅ ProcessPage Module Lists

The ProcessPage module list is having below set of modules,

#### ➡️ RenderRazor Module
1. This binds the LayoutResponse of the page to the SitecoreRenderingContext
1. It uses custom ViewRenderer to render the page into a html document

#### ➡️ Images Module
1. It processes the images in the html document
1. It replaces the path of the images to the local output folder path

#### ➡️ SitecoreDownload Images Module
1. It downloads all the images 
1. Both Sitecore images and external images

This ProcessPage module list looks like below,

![Process Pages Module List](docs/images/Process_Pages_ModuleLists.png?raw=true "Process Pages Module List")

### ✅ Output Module Lists

This is the final module lists of this Pages pipeline and has the follwing module,

#### ➡️ WriteFiles Module
1. It writes the generated Html document into the local output folder.

![Output Module List](docs/images/Output_ModuleList.png?raw=true "Output Module List")

## Performance 

The following are the Lighthouse report comparion of the Sitcore SUGCON site versus Static Netlify Site.

### SUGCON Site

![Sugcon Site](docs/images/Performance_Sugcon.png?raw=true "Sugcon Site")

### Static Netlify Site

![Netlify Site](docs/images/Performance_Netlify.png?raw=true "Netlify Site")

## Result

Here it is....!!!

![Result](docs/images/D4e.gif?raw=true "Result")