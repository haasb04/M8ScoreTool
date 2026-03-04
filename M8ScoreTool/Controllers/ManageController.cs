using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using M8ScoreTool.Models;

namespace M8ScoreTool.Controllers;

[Authorize]
public class ManageController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ManageController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index(ManageMessageId? message)
    {
        ViewBag.StatusMessage =
            message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
            : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
            : message == ManageMessageId.Error ? "An error has occurred."
            : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
            : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
            : "";

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return View("Error");
        }

        var model = new IndexViewModel
        {
            HasPassword = !string.IsNullOrWhiteSpace(user.PasswordHash),
            PhoneNumber = user.PhoneNumber,
            TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
            Logins = await _userManager.GetLoginsAsync(user),
            BrowserRemembered = false
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveLogin(string loginProvider, string providerKey)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
        }

        var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
        if (result.Succeeded)
        {
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.RemoveLoginSuccess });
        }

        return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
    }

    public IActionResult AddPhoneNumber()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        user.PhoneNumber = model.Number;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            AddErrors(result);
            return View(model);
        }

        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnableTwoFactorAuthentication()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);
        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DisableTwoFactorAuthentication()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult VerifyPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return View("Error");
        }

        return View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        user.PhoneNumber = model.PhoneNumber;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            AddErrors(result);
            return View(model);
        }

        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
    }

    public async Task<IActionResult> RemovePhoneNumber()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        user.PhoneNumber = null;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            AddErrors(result);
            return View(model);
        }

        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
    }

    public IActionResult SetPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
        if (!result.Succeeded)
        {
            AddErrors(result);
            return View(model);
        }

        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
    }

    public async Task<IActionResult> ManageLogins(ManageMessageId? message)
    {
        ViewBag.StatusMessage =
            message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
            : message == ManageMessageId.Error ? "An error has occurred."
            : "";

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return View("Error");
        }

        var userLogins = await _userManager.GetLoginsAsync(user);
        ViewBag.ShowRemoveButton = !string.IsNullOrWhiteSpace(user.PasswordHash) || userLogins.Count > 1;

        return View(new ManageLoginsViewModel
        {
            CurrentLogins = userLogins,
            OtherLogins = new List<AuthenticationScheme>()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LinkLogin(string provider)
    {
        return RedirectToAction(nameof(ManageLogins));
    }

    public IActionResult LinkLoginCallback()
    {
        return RedirectToAction(nameof(ManageLogins));
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    public enum ManageMessageId
    {
        AddPhoneSuccess,
        ChangePasswordSuccess,
        SetTwoFactorSuccess,
        SetPasswordSuccess,
        RemoveLoginSuccess,
        RemovePhoneSuccess,
        Error
    }
}
