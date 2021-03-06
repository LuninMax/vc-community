﻿using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Publish, "VirtoImportDatabase", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishImportDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
        {
            base.Publish(dbconnection, data, sample, reduced, strategy);
            string connection = dbconnection;
            SafeWriteDebug("ConnectionString: " + connection);

            using (var db = new EFImportingRepository(connection))
            {
                SqlImportDatabaseInitializer initializer;

                if (sample)
                {
                    SafeWriteVerbose("Running sample scripts");
                    initializer = new SqlImportDatabaseInitializer();
                }
                else
                {
                    SafeWriteVerbose("Running minimum scripts");
                    initializer = new SqlImportDatabaseInitializer();
                }

                initializer.InitializeDatabase(db);
            }
        }
    }
}
