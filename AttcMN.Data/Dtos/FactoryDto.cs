using AttcMN.Data.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.Data.Dtos
{
    public class FactoryDto : BaseDto
    {
        [Excel(Name = "FacId")]
        public int? FacId { get; set; }

        [Excel(Name = "FacName")]
        [Required(ErrorMessage = "用户账号不能为空"), MaxLength(300, ErrorMessage = "用户账号长度不能超过30个字符")]
        public string? FacName { get; set; }

        [Excel(Name = "FacAddress")]
        [Required(ErrorMessage = "用户账号不能为空"), MaxLength(400, ErrorMessage = "用户账号长度不能超过30个字符")]
        public string? FacAddress { get; set; }

        [Excel(Name = "ShortName")]
        [Required(ErrorMessage = "用户账号不能为空"), MaxLength(50, ErrorMessage = "用户账号长度不能超过30个字符")]
        public string? ShortName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
