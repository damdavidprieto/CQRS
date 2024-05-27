using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.DTOs
{
    public class ResultListDto<T>
    {
        public PaginationDto<T> Data { get; set; }
        public int Code { get; set; } = 0;
        public List<string> Messages { get; set; } = new List<string>();
    }
    public class ResultDto<T>
    {
        public T Data { get; set; }
        public int Code { get; set; } = 0;
        public List<string> Messages { get; set; } = new List<string>();
    }
    public class ResultDto
    {
        public int Code { get; set; } = 0;
        public List<string> Messages { get; set; } = new List<string>();
    }
}
