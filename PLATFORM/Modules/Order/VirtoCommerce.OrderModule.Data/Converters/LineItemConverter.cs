﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using Omu.ValueInjecter;
using VirtoCommerce.OrderModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class LineItemConverter
	{
		public static LineItem ToCoreModel(this LineItemEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new LineItem();
			retVal.InjectFrom(entity);

			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);
			if (entity.Discounts != null && entity.Discounts.Any())
			{
				retVal.Discount = entity.Discounts.First().ToCoreModel();
			}
		
			return retVal;
		}

		public static LineItem ToCoreModel(this coreModel.LineItem lineItem)
		{
			if (lineItem == null)
				throw new ArgumentNullException("lineItem");

			var retVal = new LineItem();
			retVal.InjectFrom(lineItem);

			retVal.IsGift = lineItem.IsGift;
            retVal.BasePrice = lineItem.ListPrice;
			retVal.Price = lineItem.PlacedPrice;
			retVal.Tax = lineItem.TaxTotal;
			retVal.FulfillmentLocationCode = lineItem.FulfillmentLocationCode;

			if(lineItem.Discounts != null)
			{
				retVal.Discount = lineItem.Discounts.Select(x => x.ToCoreModel()).FirstOrDefault();
			}
			return retVal;
		}

		public static LineItemEntity ToDataModel(this LineItem lineItem)
		{
			if (lineItem == null)
				throw new ArgumentNullException("lineItem");

			var retVal = new LineItemEntity();
			retVal.InjectFrom(lineItem);
			retVal.Currency = lineItem.Currency.ToString();
			if(lineItem.Discount != null)
			{
				retVal.Discounts = new ObservableCollection<DiscountEntity>(new DiscountEntity[] { lineItem.Discount.ToDataModel() });
			}
			return retVal;
		}

		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this LineItemEntity source, LineItemEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");


			var patchInjectionPolicy = new PatchInjection<LineItemEntity>(x => x.BasePrice, x => x.Price,
																		  x => x.Quantity, x => x.DiscountAmount, x => x.Tax, x => x.ShipmentId, x => x.Height, x => x.Length,
																		 x => x.Width, x => x.MeasureUnit, x => x.WeightUnit, x => x.Weight);
			target.InjectFrom(patchInjectionPolicy, source);


			if (!source.Discounts.IsNullCollection())
			{
				source.Discounts.Patch(target.Discounts, new DiscountComparer(), (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
			}
		}

	}
}
