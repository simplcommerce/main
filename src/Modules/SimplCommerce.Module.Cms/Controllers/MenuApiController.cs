﻿using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Cms.Models;
using SimplCommerce.Module.Cms.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SimplCommerce.Module.Cms.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/menus")]
    public class MenuApiController : Controller
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<MenuItem> _menuItemRepository;

        public MenuApiController(IRepository<Menu> menuRepository, IRepository<MenuItem> menuItemRepository)
        {
            _menuRepository = menuRepository;
            _menuItemRepository = menuItemRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var menuList = _menuRepository.Query().ToList();
            return Json(menuList);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var menu = _menuRepository.Query().Include(x => x.MenuItems).ThenInclude(m => m.Entity).FirstOrDefault(x => x.Id == id);
            if(menu == null)
            {
                return NotFound();
            }

            var model = new MenuForm
            {
                Id = menu.Id,
                Name = menu.Name,
                IsPublished = menu.IsPublished,
                Items = menu.MenuItems.Select(x => new MenuItemForm
                {
                    Id = x.Id,
                    EntityId = x.EntityId,
                    ParentId = x.ParentId,
                    Name = x.Entity == null ? x.Name : x.Entity.Name,
                    CustomLink = x.CustomLink
                }).ToList()
            };

            return Json(model);
        }

        [HttpPost("{id}/add-items")]
        public IActionResult AddItem(long id, [FromBody] IList<MenuItemForm> model)
        {
            if (ModelState.IsValid)
            {
                var menu = _menuRepository.Query().Include(x => x.MenuItems).FirstOrDefault(x => x.Id == id);

                foreach (var item in model)
                {
                    var menuItem = new MenuItem
                    {
                        Menu = menu,
                        CustomLink = item.CustomLink,
                        EntityId = item.EntityId,
                        ParentId = item.ParentId
                    };

                    menu.MenuItems.Add(menuItem);
                }

                _menuRepository.SaveChange();
                return Ok();
            }
            return new BadRequestObjectResult(ModelState);
        }

        [HttpDelete("delete-item/{id}")]
        public IActionResult DeleteItem(long id)
        {
            var menuItem = _menuItemRepository.Query().FirstOrDefault(x => x.Id == id);
            if (menuItem == null)
            {
                return new NotFoundResult();
            }

            _menuItemRepository.Remove(menuItem);
            _menuItemRepository.SaveChange();

            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] MenuForm model)
        {
            if (ModelState.IsValid)
            {
                var menu = new Menu
                {
                    Name = model.Name,
                    IsPublished = model.IsPublished
                };

                _menuRepository.Add(menu);
                _menuRepository.SaveChange();

                return Ok();
            }
            return new BadRequestObjectResult(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] MenuForm model)
        {
            if (ModelState.IsValid)
            {
                var menu = _menuRepository.Query().Include(x => x.MenuItems).FirstOrDefault(x => x.Id == id);
                menu.Name = model.Name;
                menu.IsPublished = model.IsPublished;
                foreach(var item in menu.MenuItems)
                {
                    var modelMenuItem = model.Items.First(x => x.Id == item.Id);
                    item.EntityId = modelMenuItem.EntityId;
                    item.Name = modelMenuItem.Name;
                    item.CustomLink = modelMenuItem.CustomLink;
                    item.ParentId = modelMenuItem.ParentId;
                }

                _menuRepository.SaveChange();

                return Ok();
            }

            return new BadRequestObjectResult(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var menu = _menuRepository.Query().FirstOrDefault(x => x.Id == id);
            if (menu == null)
            {
                return new NotFoundResult();
            }

            _menuRepository.Remove(menu);
            _menuRepository.SaveChange();

            return Ok();
        }
    }
}
