using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageOptimizer.Web.ViewModels
{
    public class ImagenCreateVM
    {
        [Display(Name = "Tipo de imagen")]
        public List<SelectListItem> TiposImagen { get; set; }

        public string TipoImagenId { get; set; }
    }
}
