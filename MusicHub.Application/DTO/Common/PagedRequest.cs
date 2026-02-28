using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Common
{
    public sealed class PagedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public void Normalize(int MaxPageSize = 50)
        {
            //ensures valid rules
            if (Page < 1) Page = 1;
            if (PageSize < 0) PageSize = 1 ;
            if(PageSize > MaxPageSize) PageSize = MaxPageSize ;
        }
    }
}
