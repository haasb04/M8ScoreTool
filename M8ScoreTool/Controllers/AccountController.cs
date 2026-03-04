using M8ScoreTool.Models;
using Microsoft.AspNetCore.Mvc;

namespace M8ScoreTool.Controllers;

public class AccountController : Controller
{
    private const string AuthSessionKey = "SimpleAuth.Authenticated";
    private const string PasswordEnvVar = "M8SCORETOOL_PASSWORD";
    private readonly IConfiguration _configuration;

    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Login(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model, string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string configuredPassword =
            Environment.GetEnvironmentVariable(PasswordEnvVar) ?? string.Empty;
        if (string.IsNullOrWhiteSpace(configuredPassword))
        {
            ModelState.AddModelError(
                string.Empty,
                $"Server password is not configured. Set environment variable '{PasswordEnvVar}'."
            );
            return View(model);
        }

        if (model.Password == configuredPassword)
        {
            HttpContext.Session.SetString(AuthSessionKey, "true");
            return RedirectToLocal(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Invalid password.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LogOff()
    {
        HttpContext.Session.Remove(AuthSessionKey);
        return RedirectToAction(nameof(Login));
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}
