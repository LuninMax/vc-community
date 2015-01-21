﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient.Extensions
{
    using System.Configuration;

    public static class ReviewsClientExtnesion
    {
        public static ReviewsClient CreateReviewsClient(this CommerceClients source)
        {
            return source.CreateReviewsClient(ClientContext.Session.CatalogId, ClientContext.Session.Language);
        }

        public static ReviewsClient CreateReviewsClient(this CommerceClients source, string catalogId, string language)
        {
            // http://localhost/admin/api/mp/{0}/{1}/
            var connectionString = ClientContext.Configuration.ConnectionString + String.Format("{0}/{1}/", catalogId, language);
            return CreateReviewClientWithUri(source, connectionString);
        }

        public static ReviewsClient CreateReviewClientWithUri(this CommerceClients source, string serviceUrl)
        {
            var connectionString = serviceUrl;
            var subscriptionKey = ConfigurationManager.AppSettings["vc-public-apikey"];
            var client = new ReviewsClient(new Uri(connectionString), new AzureSubscriptionMessageProcessingHandler(subscriptionKey, "secret"));
            return client;
        }
    }
}