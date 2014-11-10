﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Services;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    public class ItemsController : ApiController
    {
        private readonly IItemService _itemsService;
		private readonly IPropertyService _propertyService;
    
        public ItemsController(IItemService itemsService, IPropertyService propertyService)
        {
            _itemsService = itemsService;
			_propertyService = propertyService;
        }

        // GET: api/items/5
		[ResponseType(typeof(webModel.Product))]
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            var item = _itemsService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

			var allCategoryProperties = _propertyService.GetCategoryProperties(item.CategoryId);
			var retVal = item.ToWebModel(allCategoryProperties);
         
            return Ok(retVal);
        }

        // GET: api/items/getnewitem
        [HttpGet]
		[ResponseType(typeof(webModel.Product))]
        public IHttpActionResult GetNewItem(string catalogId = null, string categoryId = null)
        {
			var newProduct = new webModel.Product
			{
				Name = "New product",
				Code =  Guid.NewGuid().ToString().Substring(0, 5),
				CategoryId = categoryId,
				CatalogId = catalogId
			};
			var retVal = _itemsService.Create(newProduct.ToModuleModel()).ToWebModel();
			return Ok(retVal);
        }

        // GET: api/items/getnewvariation
        [HttpGet]
		[ResponseType(typeof(webModel.Product))]
        public IHttpActionResult GetNewVariation(string itemId)
        {
			var mainProduct = _itemsService.GetById(itemId);
			var newVariation = new webModel.Product
			{
				Name = "New variation",
				Code = Guid.NewGuid().ToString().Substring(0, 5),
				CategoryId = mainProduct.CategoryId,
				CatalogId = mainProduct.CatalogId,
				TitularItemId = itemId
			};
			var retVal = _itemsService.Create(newVariation.ToModuleModel()).ToWebModel();
			return Ok(retVal);
        }

		[HttpPost]
		[ResponseType(typeof(void))]
		public IHttpActionResult Post(webModel.Product product)
		{
			UpdateProduct(product);
			return StatusCode(HttpStatusCode.NoContent);
		}

		[HttpPost]
		[ResponseType(typeof(void))]
		public IHttpActionResult Delete(string[] ids)
		{
			_itemsService.Delete(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}

		private void UpdateProduct(webModel.Product product)
		{
			var moduleProduct = product.ToModuleModel();
			_itemsService.Update(new moduleModel.CatalogProduct[] { moduleProduct });
		}
    }
}