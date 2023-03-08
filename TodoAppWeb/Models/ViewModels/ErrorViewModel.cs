using Microsoft.AspNetCore.Mvc;

namespace TodoAppWeb.Models.ViewModels
{
    public class ErrorViewModel
    {
        public StatusCodeResult? ErrorCode { get; set; }
    }
}