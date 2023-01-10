﻿using Blog.DataAccess.Data;
using Blog.DataAccess.Repository.IRepository;
using Blog.Models;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    private readonly NavigationManager _nav;
    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, ApplicationDbContext db,UserManager<IdentityUser> userManager, NavigationManager nav)
    {
        _logger = logger;
        _unitOfWork= unitOfWork;
        _db = db;
        _userManager= userManager;
        _nav = nav;
    }

  
    
    public IActionResult Index(int? id, int? page = 1)
    {
        List<PostUserVM> postUserVmList = new List<PostUserVM>(); 
        int pageC = 1;
        IEnumerable<Post> postsFromDb = _unitOfWork.Post.GetAll("User,Tags");
        int? tagEnable = null;
        if(id != null)
        {
            tagEnable = id;
			Tag tag = _unitOfWork.Tag.GetFirstOrDefault(x => x.Id == id);
			postsFromDb = postsFromDb.Where(x => x.Tags.Contains(tag));
		}
        int lenghtList = postsFromDb.ToList().Count();
        postsFromDb = postsFromDb.Skip((int)((page - 1) * 2)).Take(2);
        lenghtList = Decimal.ToInt32((decimal)Math.Ceiling((float)lenghtList / 2));

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
                snippet = snippet,
                MaxPages = lenghtList,
                CurrentPage = (int)page,
                TagEnable = tagEnable
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
                snippet = snippet,

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
        return View("Details", post);
    }
    public IActionResult Details(int? id)
    {
        User user = new();
        if (User.Identity.IsAuthenticated)
        {
            var claimsIdentiy = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentiy.FindFirst(ClaimTypes.NameIdentifier);
            user = (User)_userManager.Users.FirstOrDefault(x => x.Id == claim.Value);
        }
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
            CommentList = comments,
            User = user
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