using ClinicalPharmaSystem.DataContext;
using ClinicalPharmaSystem.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;

public class MenuViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _contextAccessor;

    public MenuViewComponent( IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public IViewComponentResult Invoke()
    {
        // Retrieve menu items from session
        var menuItemsJson = _contextAccessor.HttpContext.Session.GetString("MenuItems");

        if (menuItemsJson != null)
        {
            var menuItems = JsonSerializer.Deserialize<List<MenuViewModel>>(menuItemsJson);
            return View(menuItems);
        }

        // Handle the case where menu items are not available in the session
        return View(new List<MenuViewModel>());
    }
}
