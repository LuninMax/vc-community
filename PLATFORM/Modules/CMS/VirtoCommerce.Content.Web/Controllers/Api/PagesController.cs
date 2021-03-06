﻿using System.Net;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Content.Web.Controllers.Api
{
	#region

	using System;
	using System.Linq;
	using System.Web.Http;
	using System.Web.Http.Description;
	using VirtoCommerce.Content.Data.Services;
	using VirtoCommerce.Content.Web.Models;
	using VirtoCommerce.Content.Web.Converters;
	using System.Collections.Generic;
	using VirtoCommerce.Web.Utilities;

	#endregion

	[RoutePrefix("api/cms/{storeId}")]
	[CheckPermission(Permission = PredefinedPermissions.Query)]
	public class PagesController : ApiController
	{
		#region Fields

		private IPagesService _pagesService;

		#endregion

		#region Constructors and Destructors

		public PagesController(Func<string, IPagesService> serviceFactory, ISettingsManager settingsManager)
		{
			if (serviceFactory == null)
			{
				throw new ArgumentNullException("serviceFactory");
			}

			if (settingsManager == null)
			{
				throw new ArgumentNullException("settingsManager");
			}

			var chosenRepository = settingsManager.GetValue(
				"VirtoCommerce.Content.MainProperties.PagesRepositoryType",
				string.Empty);

			var pagesService = serviceFactory.Invoke(chosenRepository);

			_pagesService = pagesService;
		}

		#endregion

		[HttpGet]
		[ResponseType(typeof(IEnumerable<Page>))]
		[Route("pages")]
		public IHttpActionResult GetPages(string storeId, [FromUri]GetPagesCriteria criteria)
		{
			var items = _pagesService.GetPages(storeId, criteria.ToCoreModel()).Select(s => s.ToWebModel());
			return Ok(items);
		}

		[HttpGet]
		[ResponseType(typeof(GetPagesResponse))]
		[Route("pages/folders")]
		public IHttpActionResult GetFolders(string storeId)
		{
			var items = _pagesService.GetPages(storeId, null);

			return Ok(items.ToWebModel());
		}

		[HttpGet]
		[ResponseType(typeof(Page))]
		[ClientCache(Duration = 30)]
		[Route("pages/{language}/{*pageName}")]
		public IHttpActionResult GetPage(string storeId, string language, string pageName)
		{
			var item = _pagesService.GetPage(storeId, pageName, language);
			if (item == null)
			{
				return StatusCode(HttpStatusCode.NoContent);
			}

			return Ok(item.ToWebModel());
		}

		[HttpGet]
		[ResponseType(typeof(CheckNameResponse))]
		[Route("pages/checkname")]
		public IHttpActionResult CheckName(string storeId, [FromUri]string pageName, [FromUri]string language)
		{
			var result = _pagesService.CheckList(storeId, pageName, language);
			var response = new CheckNameResponse { Result = result };
			return Ok(response);
		}

		[HttpPost]
		[Route("pages")]
		[CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult SaveItem(string storeId, Page page)
		{
			if (!string.IsNullOrEmpty(page.FileUrl))
			{
				using(var webClient = new WebClient())
				{
					var byteContent = webClient.DownloadData(page.FileUrl);
					page.ByteContent = byteContent;
				}
			}

			_pagesService.SavePage(storeId, page.ToCoreModel());
			return Ok();
		}

		[HttpDelete]
		[Route("pages")]
		[CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult DeleteItem(string storeId, [FromUri]string[] pageNamesAndLanguges)
		{
			_pagesService.DeletePage(storeId, PagesUtility.GetShortPageInfoFromString(pageNamesAndLanguges).ToArray());
			return Ok();
		}
	}
}