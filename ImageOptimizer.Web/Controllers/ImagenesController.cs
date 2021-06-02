using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;

namespace ImageOptimizer.Web.Controllers
{
    public class ImagenesController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public ImagenesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormFile file)
        {
            string resultado = string.Empty;
            resultado = SaveFile(file);

            if (resultado != string.Empty) {
                ModelState.AddModelError("", resultado);
            }

  
            return View();
        }


        public string SaveFile(IFormFile file)
        {
            string FullPath = string.Empty;
            string RelativePath = string.Empty;
            string FileName = string.Empty;
            string resultado = string.Empty;
            int Width = 0;
            int Height = 0;

            try
            {
            
                //Subieron imagen 
                if (file != null)
                {

      //Validar tipo de la imagen
                    if (file.ContentType.ToLower() == "image/jpeg" ||
                file.ContentType.ToLower() == "image/jpg" )
                    {
                        using (var image = System.Drawing.Image.FromStream(file.OpenReadStream()))
                        {
                            Width = image.Width;
                            Height = image.Height;
                        }
                        //Validar tamaño de la imagen
                        if (Width == 1024 && Height == 640)
                        {


                            using var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
                            image.Mutate(x => x.Resize(256, 256));
                           

                            FileName = Path.GetFileNameWithoutExtension(file.FileName);
                          
                            string FileExtension = Path.GetExtension(file.FileName);  
                            //Guardar version de imagen con distinto tamaño
                            image.Save(string.Concat(FileName, "256x256", FileExtension));
                            FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + FileName.Trim() + FileExtension;
                            string UploadPath = String.Concat(_environment.WebRootPath, "\\", "images", "\\");
                            RelativePath = String.Concat("images", "\\", "marcas", "\\", FileName);
                            FullPath = Path.Combine(UploadPath, FileName);
                            using (var stream = System.IO.File.Create(FullPath))
                            {
                                file.CopyTo(stream);
                            }
                        
                        }
                        else {
                            resultado = "Solo se permite cargar imagenes de tamaño 1024 x 640";                    
                        }
                    }
                    else
                    {
                         resultado = "Solo se permite cargar imagenes tipo JPG, JPEG";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }

            return resultado;
        }

    }
}
