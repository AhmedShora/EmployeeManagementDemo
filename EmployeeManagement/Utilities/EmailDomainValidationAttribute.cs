
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Utilities
{
    public class EmailDomainValidationAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        //Custom validation attribute
        public EmailDomainValidationAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            string [] strings= value.ToString().Split("@");
            return strings[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}
