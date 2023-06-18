using AdminPanal.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Helpers;
using Store.Core.Specifications;
using Store.Core;
using Store.Core.Entities;
using AdminPanal.Helpers;

public class ProductController : Controller
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;

    public ProductController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
    }
    private async Task<Pagination<ProductViewModel>> GetProductPagination(ProductSpecPrams specPrams)
    {
        specPrams ??= new ProductSpecPrams();
        var spec = new ProductWithBrandAndTypeSpecification(specPrams);
        var products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
        var mappedProducts = mapper.Map<IReadOnlyList<ProductViewModel>>(products);
        var countSpec = new ProductWithFilterationForCountSpecification(specPrams);
        var count = await unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);
        var pagination = new Pagination<ProductViewModel>(specPrams.PageIndex, specPrams.PageSize, count, mappedProducts);
        return pagination;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var productSpecPrams = new ProductSpecPrams();
        var pagination = await GetProductPagination(productSpecPrams);
        return View(pagination);
    }

    [HttpPost]
    public async Task<IActionResult> Index(ProductSpecPrams specPrams)
    {
        var pagination = await GetProductPagination(specPrams);
        return View(pagination);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "Products");
            var mappedProduct = mapper.Map<Product>(productViewModel);
            await unitOfWork.Repository<Product>().AddAsync(mappedProduct);
            await unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }
        return View(productViewModel);
    }
    public async Task<IActionResult> Update(int id)
    {
        var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
        var mappedProduct = mapper.Map<ProductViewModel>(product);
        return View(mappedProduct);
    }
    [HttpPost]
    public async Task<IActionResult> Update(ProductViewModel productViewModel)
    {
        if(ModelState.IsValid)
        {
            PictureSettings.DeleteFile(productViewModel.PictureUrl, "Products");
            productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "Products");
            var mappedProduct = mapper.Map<Product>(productViewModel);
            unitOfWork.Repository<Product>().Update(mappedProduct);
            await unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }
        return View(productViewModel);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var product =await unitOfWork.Repository<Product>().GetByIdAsync(id);
        PictureSettings.DeleteFile(product.PictureUrl, "Products");
        unitOfWork.Repository<Product>().Delete(product);
        await unitOfWork.CompleteAsync();
        return RedirectToAction("Index");
    }

}