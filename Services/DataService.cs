using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace IOT_Fire_Detection.Services
{
    public class DataService
    {
        private static readonly AmazonDynamoDBClient awsDynamoDb = new AmazonDynamoDBClient();
        private static readonly Table table = Table.LoadTable(awsDynamoDb, "iot_sensor_data");
        
        public async Task<Document> getLatest() {
            var config = new ScanOperationConfig(){
                Limit = 1,
                ConsistentRead = true
            };
            var search = table.Scan(config);
            var list = await search.GetNextSetAsync();
            return list.First();
        }

        public async Task<List<Document>> getLastMinute(){
            return await getLastData(DateTime.Now.AddMinutes(-1));
        }

        public async Task<List<Document>> getLastHour(){
            return await getLastData(DateTime.Now.AddHours(-1));
        }

        public async Task<List<Document>> getLastDay(){
            return await getLastData(DateTime.Now.AddDays(-1));
        }

        public async Task<List<Document>> getLastWeek(){
            return await getLastData(DateTime.Now.AddDays(-7));
        }

        public async Task<List<Document>> getLastMonth(){
            return await getLastData(DateTime.Now.AddMonths(-1));
        }

        private async Task<List<Document>> getLastData(DateTime time)
        {
            AttributeValue attributeValue = new AttributeValue();
            attributeValue.N = time.ToString("yyyyMMddHHmmss");
            Condition con = new Condition();
            con.ComparisonOperator = ComparisonOperator.GE;
            con.AttributeValueList.Add(attributeValue);
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("timestamp", con);
            var config = new ScanOperationConfig();
            config.ConsistentRead = true;
            config.Filter = scanFilter;
            var search = table.Scan(config);
            var list = await search.GetNextSetAsync();
            return list;
        }
    }
}
