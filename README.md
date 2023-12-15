# AspCombine-Create-EditPages
instead of using two methods and two views in asp.net core this is more easy way using Upsert and combine the two actions methods in one 


if you have an images or files in you method add this :

  if(productVM.Product.Id == 0) 
  {
      _unitOfWork.Product.Add(productVM.Product);
  }

  else
  {
      _unitOfWork.Product.Update(productVM.Product);
  }

  
