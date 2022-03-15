using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetlifySharp;
using Sugcon.Foundation.Caching.Rendering;
using Sugcon.Foundation.GrapheNetor.Rendering.Extension;
using Sugcon.Foundation.GrapheNetor.Rendering.Modules;
using Sugcon.Foundation.GrapheNetor.Rendering.Pipelines;
using Sugcon.Foundation.GrapheNetor.Rendering.Services;
using Sugcon.Project.Sugcon.Rendering.Configuration;
using Sugcon.Project.Sugcon.Rendering.Models;
using Sitecore.AspNet.RenderingEngine;
using Sitecore.AspNet.RenderingEngine.Filters;
using Sitecore.LayoutService.Client.Exceptions;
using Sitecore.LayoutService.Client.Response.Model;
using Statiq.App;
using Statiq.Common;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Sugcon.Project.Sugcon.Rendering.Controllers
{
    public class DefaultController : Controller
    {
        private IBootstrapper _bootstrapper { get; set; }
        private readonly ViewRenderer _viewRenderer;
        private IServiceProvider _serviceprovider { get; set; }
        private RedisConfiguration RedisConfiguration { get; }

        public DefaultController(IServiceProvider serviceprovider, IConfiguration configuration, ViewRenderer view, IBootstrapper bootstrapper)
        {
            _serviceprovider = serviceprovider;
            _viewRenderer = view;
            RedisConfiguration = configuration.GetSection(RedisConfiguration.Key).Get<RedisConfiguration>();
            _bootstrapper = bootstrapper;
        }

        // Inject Sitecore rendering middleware for this controller action
        // (enables model binding to Sitecore objects such as Route,
        // and causes requests to the Sitecore Layout Service for controller actions)
        [UseSitecoreRendering]
        public async Task<IActionResult> Index(Route route)
        {
            var request = HttpContext.GetSitecoreRenderingContext();
            if (request.Response.HasErrors)
            {
                foreach (var error in request.Response.Errors)
                {
                    switch (error)
                    {
                        case ItemNotFoundSitecoreLayoutServiceClientException notFound:
                            Response.StatusCode = (int)HttpStatusCode.NotFound;
                            return View("NotFound", request.Response.Content.Sitecore.Context);
                        case InvalidRequestSitecoreLayoutServiceClientException badRequest:
                        case CouldNotContactSitecoreLayoutServiceClientException transportError:
                        case InvalidResponseSitecoreLayoutServiceClientException serverError:
                        default:
                            throw error;
                    }
                }
            }

            return View(route);
        }

        /// <summary>
        /// Generate Static pages using GraphiNetor
        /// </summary>
        /// <returns></returns>
        [UseSitecoreRendering]
        public async Task<JsonResult> GeneratePages()
        {
            //Step 3 : Configure bootstrapper & run pipelines
            //Using Middleware as we need ComponentRender in SitecoreRenderingContext
            // and mapping httpContext to Viewrender context
            _viewRenderer.httpContext = HttpContext;

            //Configuring Sitecore Pipelines (CopyAssets & Pagespipeline & running it async
            await _bootstrapper.ConfigureSitecorePipelines(_viewRenderer, _serviceprovider)
                           .RunAsync();

            //Deploying to Netlify
            await Task.Run(() => _bootstrapper.NetlifyDeploy(_serviceprovider));
            return new JsonResult(new { Status = "ok" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            return View(new ErrorViewModel
            {
                IsInvalidRequest = exceptionHandlerPathFeature?.Error is InvalidRequestSitecoreLayoutServiceClientException
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Healthz()
        {
            // TODO: Do we want to add logic here to confirm connectivity with SC etc?

            return Ok("Healthy");
        }
    }
}