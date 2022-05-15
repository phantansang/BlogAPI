using BlogAPI.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogAPI.GenericRepository;
using System.Net.Http;
using System.Web;
using System.Net;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private BlogDbContext DB = new BlogDbContext();

        private Boolean CheckLogin()
        {
            //Demo code
            HttpContext.Items["CurrentUser"] = "ptsang";
            return true;
        }
        private IGenericRepository<TblPost> repository = null;
        public PostController()
        {
            this.repository = new GenericRepository<TblPost>();
        }
        //Multiple constructors accepting all given argument types have been found
        //Although there are a constructor without argument and a constructor have argument
        //public PostController(IGenericRepository<TblPost> repository)
        //{
        //    this.repository = repository;
        //}
        [HttpGet]
        [Route("GetAllPosts")]
        public IActionResult GetAllPosts()
        {
            List<TblPost> Posts = (List<TblPost>)repository.GetAll();
            return Ok(Posts);
        }

        [HttpGet]
        [Route("GetPostByID")]
        public IActionResult GetPostByID(string PostID)
        {
            try
            {
                TblPost Result = repository.GetById(PostID);
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
            List<TblPost> Result = (List<TblPost>)repository.GetAll();
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
                PostID += string.Format("{0:000#}", DB.TblPost.Count());
            } while (DB.TblPost.Any(P => P.PostId == PostID));
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
            if (!CheckLogin()) return Unauthorized();
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
                    repository.Insert(Post);
                    repository.Save();
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
            if (!CheckLogin()) return Unauthorized();
            else
            {
                try
                {
                    TblPost PostToEdit = repository.GetById(model.PostId);
                    if (PostToEdit == null) return NotFound();
                    PostToEdit.Title = model.Title;
                    PostToEdit.MetaTitle = model.MetaTitle;
                    PostToEdit.Category = model.Category;
                    PostToEdit.Imagepath = model.Imagepath;
                    PostToEdit.Description = model.Description;
                    PostToEdit.Content = model.Content;
                    PostToEdit.Status = model.Status;
                    PostToEdit.LastUpdate = DateTime.Now;
                    PostToEdit.Log += "<br/><i>" + DateTime.Now + "</i>: " + "Article edited by " + HttpContext.Items["CurrentUser"];
                    repository.Update(PostToEdit);
                    repository.Save();
                    return Ok(new { Success = true, Data = PostToEdit });
                }
                catch (Exception EX)
                {
                    return UnprocessableEntity(new { Success = false, Data = EX.GetType().Name });
                }
            }
        }
    }
}

