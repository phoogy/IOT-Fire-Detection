using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace IOT_Fire_Detection.Services
{
    public class temp
    {
        public async Task<object> getLatestTemp()
        {
            //var options = new CredentialProfileOptions()
            //{
            //    AccessKey = "access_key",
            //    SecretKey = "secret_key"
            //};

            //var profile = new CredentialProfile("shared_profile", options);
            //profile.Region = RegionEndpoint.USWest2;
            //var sharedFile = new SharedCredentialsFile();
            //sharedFile.RegisterProfile(profile);

            var awsDynamoDb = new AmazonDynamoDBClient();

            //var response = await awsDynamoDb.ListTablesAsync();

            //var response = await awsDynamoDb.DescribeTableAsync("iot_sensor_data");
            Table table = Table.LoadTable(awsDynamoDb, "iot_sensor_data");
            //var response= table.Attributes;
            //var response = await awsDynamoDb.ScanAsync("iot_sensor_data",null);
            var scanFilter = new ScanFilter();
            var response = table.Scan(scanFilter);
            var results = response.Matches;

            return response;


        //var chain = new CredentialProfileStoreChain("c:\\Users\\sdkuser\\customCredentialsFile.ini");
        //    AWSCredentials awsCredentials;
        //    if (chain.TryGetAWSCredentials("basic_profile", out awsCredentials))
        //    {
        //        // Use awsCredentials
        //    }
        //    return new object();
        }
    }
}
