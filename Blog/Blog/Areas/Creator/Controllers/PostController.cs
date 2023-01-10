using Blog.DataAccess.Data;
using Blog.DataAccess.Repository.IRepository;
using Blog.Models;
using Blog.Models.ViewModels;
using Blog.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Blog.Areas.Creator.Controllers;
[Area("Creator")]
[Authorize(Roles = SD.Role_Creator + "," + SD.Role_Admin)]
public class PostController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;
	private readonly UserManager<IdentityUser> _userManager;

	public PostController(IUnitOfWork unitOfWork, ApplicationDbContext db, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _db = db;
        _hostEnvironment = hostEnvironment;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        var claimsIdentiy = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentiy.FindFirst(ClaimTypes.NameIdentifier);
        IEnumerable<Post> objPostList;
        if (User.IsInRole(SD.Role_Admin))
        {
            objPostList = _db.Posts.Include(x => x.User);
        }
        else
        {
            objPostList = _db.Posts.Include(x => x.User).Where(x => x.User.Id == claim.Value);
        }
        return View(objPostList);
    }
    //GET
    public IActionResult Create()
    {
        
        PostVM postVM = new()
        {
            Post = new (),
            TagList = _unitOfWork.Tag.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            })
        };

        return View(postVM);
    }

    //Post
    [HttpPost]
    public IActionResult Create(PostVM obj, IFormFile? file)
    {
        var claimsIdentiy = (ClaimsIdentity)User.Identity;
		var claim = claimsIdentiy.FindFirst(ClaimTypes.NameIdentifier);
		User user = (User)_userManager.Users.FirstOrDefault(x => x.Id == claim.Value);

        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\posts");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                obj.Post.Picture = @"\images\posts\" + fileName + extension;

            }

			_unitOfWork.Post.Add(obj.Post);
            _unitOfWork.Save();

            foreach (String tag in obj.TagString)
            {
                Tag tagFind = _unitOfWork.Tag.GetFirstOrDefault(x => x.Id == int.Parse(tag));
           
                obj.Post.Tags.Add(tagFind);

            }
            obj.Post.User = user;
            _db.SaveChanges();
			return RedirectToAction("Index");
        }
        return View(obj);
    }

    //GET 
    public IActionResult Edit(int? id)
    {
		PostVM postVM = new()
        {
            Post = new Post (),
            TagList = _unitOfWork.Tag.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            TagString = new List<string>()
        };
        
        IEnumerable<Post> posts = _unitOfWork.Post.GetAll();
        if (id == null || id == 0)
        {
            return NotFound();
        }
        postVM.Post = _db.Posts.Include(x => x.Tags).Include(y => y.User).FirstOrDefault(x => x.Id == id);
        foreach(Tag tag in postVM.Post.Tags) { 

            postVM.TagString.Add(tag.Id.ToString());
        }
        
        if (postVM.Post == null)
        {
            return NotFound();
        }

        return View("Create",postVM);
    }

    //POST
    [HttpPost]
    public IActionResult Edit(PostVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\posts");
                var extension = Path.GetExtension(file.FileName);

                if (obj.Post.Picture != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Post.Picture.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
              
				using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                obj.Post.Picture = @"\images\products\" + fileName + extension;
            }
        }
        List<Tag> tags = new List<Tag>();
		foreach (String tag in obj.TagString)
		{
			Tag tagFind = _unitOfWork.Tag.GetFirstOrDefault(x => x.Id == int.Parse(tag));
			tags.Add(tagFind);

		}
        Post post = _db.Posts.Include(x => x.Tags).FirstOrDefault(x => x.Id == obj.Post.Id);
        post.Tags = new List<Tag>();
        post.Content = obj.Post.Content;
        post.Title = obj.Post.Title;
		_unitOfWork.Post.Update(post);
		_unitOfWork.Save();
		post.Tags = tags;
        post.UpdateDate= DateTime.Now;
		_db.Posts.Update(post);
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int? id)
    {
        if(id == null)
        {
            return NotFound();
        }
        Post post = _db.Posts.Include(x => x.Comments).FirstOrDefault(x => x.Id == id);
        _unitOfWork.Post.Remove(post);
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }
}

