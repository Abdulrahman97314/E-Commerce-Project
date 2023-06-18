using AdminPanal.Helpers;
using AdminPanal.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Helpers;
using Store.Core.Specifications;
using Store.Core;
using Store.Core.Entities;

namespace AdminPanal.Controllers
{
    public class BrandController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductBrand brand)
        {
            if (ModelState.IsValid)
            {
                var existingBrand = await unitOfWork.Repository<ProductBrand>().GetFirstOrDefaultAsync(b => b.Name == brand.Name);
                if (existingBrand != null)
                {
                    ModelState.AddModelError("Name", "A brand with the same name already exists.");
                }
                else
                {
                    await unitOfWork.Repository<ProductBrand>().AddAsync(brand);
                    await unitOfWork.CompleteAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(brand);
        }
        public async Task<IActionResult> Update(int id)
        {
            var brand = await unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            return View(brand);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductBrand brand)
        {
            if (ModelState.IsValid)
            {
                var existingBrand = await unitOfWork.Repository<ProductBrand>().GetFirstOrDefaultAsync(b => b.Name == brand.Name);
                if (existingBrand != null)
                {
                    ModelState.AddModelError("Name", "A brand with the same name already exists.");
                }
                else
                {
                    unitOfWork.Repository<ProductBrand>().Update(brand);
                    await unitOfWork.CompleteAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(brand);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            unitOfWork.Repository<ProductBrand>().Delete(brand);
            await unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }
    }
}
