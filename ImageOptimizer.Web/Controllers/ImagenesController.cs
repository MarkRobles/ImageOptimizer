﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                        using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
                        {
                            Width = image.Width;//No entiendo porque lo dividen entre 2 pero asi esta en la documentacion de la libreria jeje
                            Height = 0;// SixLabors.ImageSharp- Esta libreria define la altura correspondiente para mantener el aspec ratio si pones 0 en  Height o Width
                            //https://docs.sixlabors.com/articles/imagesharp/resize.html

                            //Validar tamaño de la imagen
                            if (Width >= 1200)
                            {

                                Width = image.Width / 2;//No entiendo porque lo dividen entre 2 pero asi esta en la documentacion de la libreria jeje

                                FileName = Path.GetFileNameWithoutExtension(file.FileName);
                                string FileExtension = Path.GetExtension(file.FileName);
                                FileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), "-" + FileName.Trim());
                                string UploadPath = String.Concat(_environment.WebRootPath, "\\", "images", "\\");

                                FullPath = Path.Combine(UploadPath, string.Concat(FileName, "576", FileExtension));
                                using var image576 = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
                                image576.Mutate(x => x.Resize(576, Height));
                                image576.Save(FullPath);

                                FullPath = Path.Combine(UploadPath, string.Concat(FileName, "768", FileExtension));
                                using var image768 = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
                                image768.Mutate(x => x.Resize(768, Height));
                                image768.Save(FullPath);

                                FullPath = Path.Combine(UploadPath, string.Concat(FileName, "992", FileExtension));
                                using var image992 = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
                                image992.Mutate(x => x.Resize(992, Height));
                                image992.Save(FullPath);


                                FullPath = Path.Combine(UploadPath, string.Concat(FileName, "1200", FileExtension));
                                using var image1200 = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
                                image1200.Mutate(x => x.Resize(1200, Height));
                                image1200.Save(FullPath);


                                //FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + FileName.Trim() ;

                                //RelativePath = String.Concat("images", "\\", "marcas", "\\", FileName);
                                //FullPath = Path.Combine(UploadPath,string.Concat( FileName , FileExtension));
                                //using (var stream = System.IO.File.Create(FullPath))
                                //{
                                //    file.CopyTo(stream);
                                //}

                            }
                            else
                            {
                                resultado = "Solo se permite cargar imagenes de ancho mayor o igual a 1200px";
                            }
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
