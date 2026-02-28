using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Common
{
    public sealed class PagedResult<T>
    {
        //making a generic class for reports and posts both 
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total {  get; set; }
        //generating empty list to feed the post for pagination
        public List<T> Items { get; set; } = new();
    }
}
