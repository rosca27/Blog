using Blog.DataAccess.Data;
using Blog.DataAccess.Repository.IRepository;
using Blog.Models;
using Blog.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Areas.Admin.Controllers;
[Area("Admin")]

public class TagController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _db;

    public TagController(IUnitOfWork unitOfWork, ApplicationDbContext db)
    {
        _unitOfWork = unitOfWork;
        _db = db;
    }
    public IActionResult Index()
    {
        IEnumerable<Tag> objTagList = _unitOfWork.Tag.GetAll();
        return View(objTagList);
    }

    //GET
    public IActionResult Create()
    {
        return View();
    }

    //Post
    [HttpPost]
    public IActionResult Create(Tag obj)
    {
        if (ModelState.IsValid)
        {
            _db.Tags.Add(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //GET 
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var TagFromDb = _unitOfWork.Tag.GetFirstOrDefault(x => x.Id == id);
        if (TagFromDb == null) {
            return NotFound();
        }

        return View(TagFromDb);
    }

    //Post
    [HttpPost]
    public IActionResult Edit(Tag obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Tag.Update(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        return View(obj);
    }

    //GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var TagFromDb = _unitOfWork.Tag.GetFirstOrDefault(x => x.Id == id);

        if (TagFromDb == null)
        {
            return NotFound();
        }
        return View(TagFromDb);
    }

    //Post
    [HttpPost]
    public IActionResult Delete(Tag obj)
    {
        _unitOfWork.Tag.Remove(obj);
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }
}

