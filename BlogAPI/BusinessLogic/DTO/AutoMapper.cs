using AutoMapper;
using BusinessLogic.DTO;
using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NhaMayTram.BusinessLogic.Dto
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<PostDTO, TblPost>()
                .ReverseMap();
        }
    }
}
