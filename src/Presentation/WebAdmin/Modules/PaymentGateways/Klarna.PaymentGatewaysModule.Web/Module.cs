﻿using Klarna.PaymentGatewaysModule.Web.Controllers;
using Klarna.PaymentGatewaysModule.Web.Managers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Settings;

namespace Klarna.PaymentGatewaysModule.Web
{
	public class Module : IModule
	{
		private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
		{
			_container = container;
		}

		public void Initialize()
		{
			var settingsManager = ServiceLocator.Current.GetInstance<ISettingsManager>();

			var klarnaEid = settingsManager.GetValue("Klarna.PaymentGateway.Credentials.AppKey", 0);
			var klarnaSecret = settingsManager.GetValue("Klarna.PaymentGateway.Credentials.SecretKey", string.Empty);

			//var paymentGatewayManager = ServiceLocator.Current.GetInstance<IPaymentGatewayManager>();

			var klarnaPaymentGateway = new KlarnaPaymentGatewayImpl(klarnaEid, klarnaSecret);

			//paymentGatewayManager.RegisterGateway(klarnaPaymentGateway);

			_container.RegisterType<KlarnaGatewayController>(new InjectionConstructor(klarnaPaymentGateway, klarnaEid, klarnaSecret));
		}
	}
}