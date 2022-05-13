using BlogAPI.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogAPI.GenericRepository;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private BlogDbContext DB = new BlogDbContext();
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
        public List<TblPost> GetAllPosts()
        {
            List<TblPost> Posts = (List<TblPost>)repository.GetAll();
            return Posts;
        }

        [HttpGet]
        [Route("GetPostsByDateRange")]
        public List<TblPost> GetPostsByDateRange(string From, string To)
        {
            return DB.TblPost.Where(P=>DateTime.Compare((DateTime)P.CreateTime, DateTime.Parse(From)) >=0 && DateTime.Compare((DateTime)P.CreateTime, DateTime.Parse(To)) <=0).ToList();
        }

        [HttpGet]
        [Route("GetPostByID")]
        public TblPost GetPostByID(string PostID)
        {
            TblPost Result = DB.TblPost.FirstOrDefault(P => P.PostId == PostID);
            if (Result == null) RedirectToAction("Blog", "ExceptionElert", new { ExceptionType = "Not found"});
            return DB.TblPost.Find(PostID);
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
                PostID += string.Format("{0:####}", DB.TblPost.Count());
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

        [HttpGet]
        [Route("ExceptionElert")]
        public string ExceptionElert(string ExceptionType)
        {
            return ExceptionType;
        }
    }
}
