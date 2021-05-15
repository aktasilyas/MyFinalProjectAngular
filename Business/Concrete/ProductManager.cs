using Business.Abstract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities;
using Core.Utilities.Business;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
       // ICategoryService _categoryService;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
          //  _categoryService = categoryService;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {

           IResult result= BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfProductOfCountOfCategoryCorrect(product.CategoryID),CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }
         _productDal.Add(product);
          return new SuccessResult(Messages.ProductAdded);

          
         
        }

        public IResult Delete(int id)
        {
            var product = _productDal.Get(p => p.ProductID == id);
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        public IDataResult<List<Product>> GetAll()
        {
            //buraya is kodlari yazilir
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            _productDal.GetAll();
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);

        }
        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
              
                return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryID == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductID == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        public IResult Update(Product product)
        {
            if (CheckIfProductOfCountOfCategoryCorrect(product.CategoryID).Success)
            {
                _productDal.Update(product);
                return new SuccessResult(Messages.ProductUpdated);
            }
            return new ErrorResult();
        }
        private IResult CheckIfProductOfCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryID == categoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }
        private IResult CheckIfProductNameExists(String productName)
        {
            //ayni urun isminden baska bir kayit varmi
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            //ayni urun isminden baska bir kayit varmi
            /* var result = _categoryService.GetAll();

             if (result.Data.Count>15)
             {
                 return new ErrorResult(Messages.CategoryLimitExceded);
             }*/
            return new SuccessResult();
        }

       
    }
}
