//Khalid Dhaafi
//please not if you want to add images or pdf files this will not work you will need to add if and else for update or edit method read the ReadMe file
namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork db)
        {
            _unitOfWork = db; //this is to get db connection and apply opartions you do'nt need to write the sql Commnads 
        }


        public IActionResult Index() //URL Routing : IS Product/Index
        {            //then you  will need to pass this object to the view 
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();  //asing the _db object to the Product Model
            return View(objProductList); //to pass it to the View
        }

        //this is Get Action Mehod 
        public IActionResult Upsert(int? id)

        {
            ProductVM productVM = new()
            {
                CatgoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem

             {
                 Text = u.Name,
                 Value = u.Id.ToString()
             }),
            Product = new Product()
            };
            if(id == null || id == 0)
            {
                //for create 
                return View(productVM);
            }

            else
            { 
                //for update
                productVM.Product = _unitOfWork.Product.Get(u=>u.Id==id);
                return View(productVM);
            }
         
        }
        //This is a post action Method
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file) //this is in the index forecah loop
        {
          
            if (ModelState.IsValid) //this is will check with data annotaiton with the model
            {
                _unitOfWork.Product.Add(productVM.Product); // to add to DB
                _unitOfWork.Save(); // to save changes 
                TempData["success"] = "Catgory create successfully"; //this is temp data
                return RedirectToAction("Index");// to go to the orginal page using Rediract 
            }
            else
            {
                productVM.CatgoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem

                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM); //rerutn to View
            }
    
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound(); // You will need an error page for this 
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id); // Find category by ID
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj); // to add to DB
            _unitOfWork.Save(); // to save changes 
            TempData["success"] = "Catgory Delete successfully";
            return RedirectToAction("Index");
        }
    }
}
