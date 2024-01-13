using MicroRabbit.Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers;
[Route("api/Banking")]
[ApiController]
public class BankingController(IAccountService accountService) : ControllerBase
{
  
    [HttpGet("/account")]
    public IActionResult GetAccounts()
    { 
        var accounts = accountService.GetAccounts();
        return Ok(accounts);
    }
}