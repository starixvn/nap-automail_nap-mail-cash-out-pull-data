using Fanex.MailDelivery.Core.Models;
using Fanex.COPullData.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.COPullData.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class ExecuteController : ControllerBase
{
    private readonly MainService service;

    public ExecuteController(MainService service)
    {
        this.service = service;
    }

    [HttpGet]
    public JobMessage Get()
    {
        var result = service.Execute();
        return result;
    }
}