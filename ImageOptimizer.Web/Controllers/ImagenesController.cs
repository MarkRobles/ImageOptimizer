using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using ImageOptimizer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageOptimizer.Data.Models;

namespace ImageOptimizer.Web.Controllers
{
    public class ImagenesController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly List<TipoImagen> tiposImagen;

        public ImagenesController(IWebHostEnvironment environment)
        {
            _environment = environment;
            tiposImagen  = new List<TipoImagen> {
            new TipoImagen{Id="JPEG",Descripcion="JPEG",Predeterminado=true },
            new TipoImagen{Id="PNG",Descripcion="PNG",Predeterminado=false },
            new TipoImagen{Id="WEBP",Descripcion="WEBP",Predeterminado=false },
            new TipoImagen{Id="GIF",Descripcion="GIF",Predeterminado=false }
            };
        }
        public IActionResult Index()
        {
          
            return View();
        }


        public IActionResult Create()
        {
           

            var vm = new ImagenCreateVM();
            vm.TiposImagen = tiposImagen.Select(c => new SelectListItem() { Text = $"{c.Descripcion} ", Value = c.Id.ToString(),Selected=c.Predeterminado }).ToList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ImagenCreateVM vm,IFormFile file)
        {
            string resultado = string.Empty;
            resultado = SaveFile(file,vm.TipoImagenId);

            if (resultado != string.Empty)
            {
                ModelState.AddModelError("", resultado);
            }



            vm.TiposImagen = tiposImagen.Select(c => new SelectListItem() { Text = $"{c.Descripcion} ", Value = c.Id.ToString() }).ToList();

            return View(vm);
        }


        public  void CreateImageBreakPoints(SixLabors.ImageSharp.Image image, string FileName,string TipoImagen)
        {
            string FullPath = string.Empty;
            int Width = 0;
            int Height = 0;// SixLabors.ImageSharp- Esta libreria define la altura correspondiente para mantener el aspec ratio si pones 0 en  Height o Width
            try
            {

                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileExtension = Path.GetExtension(FileName);

                FileExtension = TipoImagen;
                FileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), "-" + FileName.Trim());
                string UploadPath = String.Concat(_environment.WebRootPath, "\\", "images", "\\");

                Width = image.Width / 2;//No entiendo porque lo dividen entre 2 pero asi esta en la documentacion de la libreria jeje

                FullPath = Path.Combine(UploadPath, string.Concat(FileName,"_", "360px", FileExtension));
                using var image360 = image;
                image360.Mutate(x => x.Resize(360, Height));
                // image360.ToBase64String() esto seguro me servira cuando lo integre a otro proyecto 
                image360.Save(FullPath);//En la documentacion de la libreria se guardan con quality 75 por default


                FullPath = Path.Combine(UploadPath, string.Concat(FileName, "_", "576px", FileExtension));
                using var image576 = image;
                image576.Mutate(x => x.Resize(576, Height));
                // image576.ToBase64String() esto seguro me servira cuando lo integre a otro proyecto 
                image576.Save(FullPath);//En la documentacion de la libreria se guardan con quality 75 por default

                FullPath = Path.Combine(UploadPath, string.Concat(FileName,"_", "768px", FileExtension));
                using var image768 = image;
                image768.Mutate(x => x.Resize(768, Height));
                image768.Save(FullPath);

                FullPath = Path.Combine(UploadPath, string.Concat(FileName,"_", "992px", FileExtension));
                using var image992 = image;
                image992.Mutate(x => x.Resize(992, Height));
                image992.Save(FullPath);


                FullPath = Path.Combine(UploadPath, string.Concat(FileName,"_", "1200px", FileExtension));
                using var image1200 = image;
                image1200.Mutate(x => x.Resize(1200, Height));
                image1200.Save(FullPath);

            
            }
            catch (Exception)
            {

                throw;
            }
            

  

        }

            public string SaveFile(IFormFile file,string TipoImagen)
        {
            string FullPath = string.Empty;
            string RelativePath = string.Empty;
            string FileName = string.Empty;
            string resultado = string.Empty;
            int Width = 0;
            int Height = 0;
            SixLabors.ImageSharp.Image imageSix;

            try
            {

                //Subieron imagen 
                if (file != null)
                {

                    FileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string FileExtension = Path.GetExtension(file.FileName);

                    FileExtension = string.Concat(".", TipoImagen);
                    FileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), "-" + FileName.Trim());
                    string UploadPath = String.Concat(_environment.WebRootPath, "\\", "images", "\\");
                    FullPath = Path.Combine(UploadPath, string.Concat(FileName, FileExtension));


                    if (TipoImagen.ToLower().Contains("webp"))
                    {
                        // load the WEBP file in an instance of Image
                        var image = Aspose.Imaging.Image.Load(file.OpenReadStream());
                        // create an instance of JpegOptions
                        var options = new Aspose.Imaging.ImageOptions.WebPOptions();
                        FullPath = Path.Combine(UploadPath, string.Concat(FileName, FileExtension));
                        // save WEBP as a JPEG
                        image.Save(FullPath, options);
       
                        return resultado;
                    }

                    if (file.ContentType.ToLower().Contains("webp") )
                    {
                        // load the WEBP file in an instance of Image
                        var image = Aspose.Imaging.Image.Load(file.OpenReadStream());
                        // create an instance of JpegOptions
                        var options = new Aspose.Imaging.ImageOptions.JpegOptions();
                        FullPath = Path.Combine(UploadPath, string.Concat(FileName, FileExtension));
                        // save WEBP as a JPEG
                        image.Save(FullPath, options);
                         imageSix = SixLabors.ImageSharp.Image.Load(FullPath);
                    }
                    else {
                        imageSix = SixLabors.ImageSharp.Image.Load(file.OpenReadStream() );
                    }

                   
                    CreateImageBreakPoints(imageSix, file.FileName, FileExtension);

                    //Validar tipo de la imagen
                    //    if (file.ContentType.ToLower() == "image/jpeg" ||
                    //file.ContentType.ToLower() == "image/jpg")
                    //    {
                 //   using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
                      //  {
                         //   Width = image.Width;

                                       //https://docs.sixlabors.com/articles/imagesharp/resize.html

                        //Validar tamaño de la imagen
                        //if (Width >= 1200)
                        //{




                            //}
                            //else
                            //{
                            //    resultado = "Solo se permite cargar imagenes de ancho mayor o igual a 1200px";
                            //}
                      //  }

                    //}
                    //else
                    //{
                    //    resultado = "Solo se permite cargar imagenes tipo JPG, JPEG";
                    //}
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
