using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;
using BusinessLogic.DTO;
using BusinessLogic.GenericRepository;
using DataAcess;
using System.Threading.Tasks;
using DataAcess.Entities;
using AutoMapper;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IGenericRepository<TblPost> _repositoryPost;
        public PostController()
        {
            this._repositoryPost = new GenericRepository<TblPost>();
        }
        //Multiple constructors accepting all given argument types have been found
        //Although there are a constructor without argument and a constructor have argument
        //public PostController(IGenericRepository<TblPost> repository)
        //{
        //    this.repository = repository;
        //}
        public Boolean CheckLoginStatus()
        {
            //To do something...
            Boolean Status = true;
            HttpContext.Items["CurrentUser"] = "ptsang";
            return Status;
        }

        [HttpGet]
        [Route("GetAllPosts")]
        public IActionResult GetAllPosts()
        {
            List<TblPost> Posts = (List<TblPost>)_repositoryPost.GetAll();
            return Ok(Posts);
        }

        [HttpGet]
        [Route("GetPostByID")]
        public IActionResult GetPostByID(string PostID)
        {
            try
            {
                TblPost Result = _repositoryPost.GetById(PostID);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetPostsByDateRange")]
        public List<TblPost> GetPostsByDateRange(string From, string To)
        {
            List<TblPost> Result = (List<TblPost>)_repositoryPost.GetAll();
            return Result.Where(P => DateTime.Compare((DateTime)P.CreateTime, DateTime.Parse(From)) >= 0 && DateTime.Compare((DateTime)P.CreateTime, DateTime.Parse(To)) <= 0).ToList();
        }

        /// <summary>
        /// Create PostID by category post ID
        /// </summary>
        /// <param name="CateID">Category ID</param>
        /// <returns></returns>
        public string CreatePostIDByCate(string CateID)
        {
            string PostID = CateID;
            do
            {
                PostID += string.Format("{0:000#}", _repositoryPost.GetAll().Where(R=>R.Category == CateID).Count());
            } while (_repositoryPost.GetAll().Any(P => P.PostId == PostID));
            return PostID;
        }

        /// <summary>
        /// Convert Title to MetaTitle using to do URL good for SEO
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public string TitleToMetaTitle(string Title)
        {
            string Metatitle = Title.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < Metatitle.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(Metatitle[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(Metatitle[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            sb = sb.Replace(' ', '-');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        [HttpPost]
        [Route("CreatePost")]
        public IActionResult CreatePost([FromBody] TblPost model)
        {
             
            
            if (!CheckLoginStatus()) return Unauthorized();
            else
            {
                try
                {
                    TblPost Post = new TblPost
                    {
                        PostId = CreatePostIDByCate(model.Category),
                        Title = model.Title,
                        MetaTitle = TitleToMetaTitle(model.Title),
                        Category = model.Category,
                        Imagepath = model.Imagepath,
                        Description = model.Description,
                        Content = model.Content,
                        Author = model.Author,
                        Status = model.Status,
                        CreateTime = DateTime.Now,
                        LastUpdate = DateTime.Now,
                        Log = "<i>" + DateTime.Now + "</i>: " + "Article created by " + model.Author
                    };
                    _repositoryPost.Insert(Post);
                    _repositoryPost.Save();
                    return Ok(new { Success = true, Data = Post });
                }
                catch (Exception EX)
                {
                    return UnprocessableEntity(new { Success = false, Data = EX.GetType().Name });
                }
            }
        }

        [HttpPut]
        [Route("UpdatePost")]
        public IActionResult UpdatePost([FromBody] TblPost model)
        {
            if (!CheckLoginStatus()) return Unauthorized();
            else
            {
                try
                {
                    TblPost PostToEdit = _repositoryPost.GetById(model.PostId);
                    if (PostToEdit == null) return NotFound();
                    string Change = "";
                    if(PostToEdit.Title != model.Title)
                    {
                        PostToEdit.Title = model.Title;
                        Change += "Title";
                    }
                    if (PostToEdit.MetaTitle != model.MetaTitle)
                    {
                        PostToEdit.MetaTitle = model.MetaTitle;
                        if(Change == "") Change += "MetaTitle";
                        else Change += ", MetaTitle";
                    }
                    if (PostToEdit.Category != model.Category)
                    {
                        PostToEdit.Category = model.Category;
                        if (Change == "") Change += "Category";
                        else Change += ", Category";
                    }
                    if (PostToEdit.Imagepath != model.Imagepath)
                    {
                        PostToEdit.Imagepath = model.Imagepath;
                        if (Change == "") Change += "Imagepath";
                        else Change += ", Imagepath";
                    }
                    if (PostToEdit.Description != model.Description)
                    {
                        PostToEdit.Description = model.Description;
                        if (Change == "") Change += "Description";
                        else Change += ", Description";
                    }
                    if (PostToEdit.Content != model.Content)
                    {
                        PostToEdit.Content = model.Content;
                        if (Change == "") Change += "Content";
                        else Change += ", Content";
                    }
                    if (PostToEdit.Status != model.Status)
                    {
                        PostToEdit.Status = model.Status;
                        if (Change == "") Change += "Status";
                        else Change += ", Status";
                    }
                    if(Change!="")
                    {
                        PostToEdit.LastUpdate = DateTime.Now;
                        PostToEdit.Log += "<br/><i>" + DateTime.Now + "</i>: " + "Article edited by " + HttpContext.Items["CurrentUser"] + " [<i>" + Change + "</i>]";
                        _repositoryPost.Update(PostToEdit);
                        _repositoryPost.Save();
                        return Ok(new { Success = true, Data = PostToEdit });
                    }
                    else
                    {
                        return UnprocessableEntity(new { Success = false, Data = "No changes" });
                    }
                }
                catch (Exception EX)
                {
                    return UnprocessableEntity(new { Success = false, Data = EX.GetType().Name });
                }
            }
        }
    }
}

