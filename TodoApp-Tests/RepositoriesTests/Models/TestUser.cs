using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp_Tests.RepositoriesTests.Models
{
    public class TestUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string PasswordHash
        {
            get
            {
                return Password.GetHashCode().ToString();
            }
        }
    }
}
