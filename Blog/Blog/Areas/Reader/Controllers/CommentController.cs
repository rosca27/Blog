using Blog.DataAccess.Data;
using Blog.DataAccess.Repository.IRepository;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Areas.Reader.Controllers;
[Area("Reader")]
public class CommentController : Controller
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _db;
    public CommentController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ApplicationDbContext db)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _db = db;
    }
    public IActionResult Index(int? id)
    {
        Post post = _db.Posts.FirstOrDefault(p => p.Id == id);
        IEnumerable<Comment> comments = _unitOfWork.Comment.GetAll("User");
        comments = comments.Where(p => p.Post == post);

        return View(comments);
    }
    public IActionResult Delete(int? id)
    {
        Comment comment= _unitOfWork.Comment.GetFirstOrDefault(p => p.Id == id, "Post");
        int idC = comment.Post.Id;
        if (comment!=null)
        {
            _unitOfWork.Comment.Remove(comment);
            _unitOfWork.Save();
        }

        return RedirectToAction("Index");
    }
}

