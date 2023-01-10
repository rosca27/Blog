using Blog.DataAccess.Data;
using Blog.DataAccess.Repository.IRepository;
using Blog.Models;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Blog.Areas.Reader;
[Area("Reader")]

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, ApplicationDbContext db,UserManager<IdentityUser> userManager )
    {
        _logger = logger;
        _unitOfWork= unitOfWork;
        _db = db;
        _userManager= userManager;
    }

    public IActionResult Index()
    {
        List<PostUserVM> postUserVmList = new List<PostUserVM>();
        IEnumerable<Post> postsFromDb = _unitOfWork.Post.GetAll("User,Tags");
        foreach (Post post in postsFromDb)
        {
            string snippet = "";
            for(int i = 0; i< 200; i++)
            {
                if(i == post.Content.Length - 2)
                {
                    break;
                }
                snippet = snippet + post.Content[i];
            }
            PostUserVM postUserVM = new()
            {
                Post = post,
                snippet= snippet
            };
            if(postUserVM != null)
            {
                postUserVmList.Add(postUserVM);
            }
			
        }
        if (postUserVmList != null)
        {
            return View(postUserVmList);
        }
        return View();
    }

    public IActionResult TagIndex(int? id)
    {
        IEnumerable<Post> objPostList = _unitOfWork.Post.GetAll("Tags,Comments,User");
        Tag tag = _unitOfWork.Tag.GetFirstOrDefault(x => x.Id == id);
        objPostList = objPostList.Where(x => x.Tags.Contains(tag));
        List<PostUserVM> postUserVmList = new List<PostUserVM>();

        foreach (Post post in objPostList)
        {
            string snippet = "";
            for (int i = 0; i < 200; i++)
            {
                if (i == post.Content.Length - 2)
                {
                    break;
                }
                snippet = snippet + post.Content[i];
            }
            PostUserVM postUserVM = new()
            {
                Post = post,
                snippet = snippet
            };
            if (postUserVM != null)
            {
                postUserVmList.Add(postUserVM);
            }

        }
        if (postUserVmList != null)
        {
            return View("Index",postUserVmList);
        }
        return View();
    }

    public IActionResult Like(int? id)
    {
        Post post = _unitOfWork.Post.GetFirstOrDefault(x => x.Id == id, "User,Tags");
        post.Likes += 1;
        _unitOfWork.Post.Update(post);
        _unitOfWork.Save();
        return RedirectToAction("Details", post);
    }
    public IActionResult Details(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Post post = _unitOfWork.Post.GetFirstOrDefault(x => x.Id == id, "Tags,Comments,User");

        if (post == null)
        {
            return NotFound();
        }
        IEnumerable<Comment> comments = _unitOfWork.Comment.GetAll("User,Post");
        comments = comments.Where(x => x.Post == post);
        PostCommentVM postComment = new()
        {
            Post = post,
            CommentList = comments
        };
        return View(postComment);
    }

    [HttpPost]
    public IActionResult AddComment(int? id, string message)
    {

        Post post = _unitOfWork.Post.GetFirstOrDefault(x => x.Id == id,"User,Comments,Tags");
        var claimsIdentiy = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentiy.FindFirst(ClaimTypes.NameIdentifier);
        User user = (User)_userManager.Users.FirstOrDefault(x => x.Id == claim.Value);

        if (post == null)
        {
            return NotFound();
        }
        Comment comment = new()
        {
            Content = message,
            User = user,
            Post = post
        };
        _db.Comments.Add(comment);
        post.Comments.Add(comment);
        _db.Posts.Update(post);
        _db.SaveChanges();
        return RedirectToAction("Details",post);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}