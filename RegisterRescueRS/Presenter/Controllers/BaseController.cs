
using System.Text.Json.Serialization;
using RegisterRescueRS.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace RegisterRescueRS.Presenter.Controllers;

public class BaseController : ControllerBase
{
    protected readonly IServiceProvider serviceProvider;


    public BaseController(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

}