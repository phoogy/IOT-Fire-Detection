using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace IOT_Fire_Detection.Services {
    public class DataService {
        private static readonly AmazonDynamoDBClient awsDynamoDb = new AmazonDynamoDBClient();
        private static readonly Table table = Table.LoadTable(awsDynamoDb, "iot_sensor_data");

        public async Task<Document> getLatest() {
            var config = new ScanOperationConfig() {
                Limit = 1,
                ConsistentRead = true
            };
            var search = table.Scan(config);
            var list = await search.GetNextSetAsync();
            return list.First();
        }

        public async Task<List<Document>> get60Seconds(DateTime startDateTime) {
            var list = new List<Document>();
            var data = await getData(startDateTime, startDateTime.AddSeconds(60));
            for (int i = 0; i < 60; i++)
            {
                var doc = data.Find(x => {
                    DateTime test;
                    if (DateTime.TryParseExact(x["timestamp"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out test))
                        return test.ToString("yyyyMMddHHmmss") == startDateTime.AddDays(i).ToString("yyyyMMddHHmmss");
                    return false;
                });
                if (doc != null)
                    list.Add(doc);
            }
            return list;
        }

        public async Task<List<Document>> get60Minutes(DateTime startDateTime) {
            var list = new List<Document>();
            var data = await getData(startDateTime, startDateTime.AddMinutes(60));
            for (int i = 0; i < 60; i++)
            {
                var doc = data.Find(x => {
                    DateTime test;
                    if (DateTime.TryParseExact(x["timestamp"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out test))
                        return test.ToString("yyyyMMddHHmm") == startDateTime.AddDays(i).ToString("yyyyMMddHHmm");
                    return false;
                });
                if (doc != null)
                    list.Add(doc);
            }
            return list;
        }

        public async Task<List<Document>> get24Hours(DateTime startDateTime) {
            var list = new List<Document>();
            var data = await getData(startDateTime, startDateTime.AddHours(24));
            for (int i = 0; i < 24; i++)
            {
                var doc = data.Find(x => {
                    DateTime test;
                    if (DateTime.TryParseExact(x["timestamp"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out test))
                        return test.ToString("yyyyMMddHH") == startDateTime.AddDays(i).ToString("yyyyMMddHH");
                    return false;
                });
                if (doc != null)
                    list.Add(doc);
            }
            return list;
        }

        public async Task<List<Document>> get7Days(DateTime startDateTime) {
            var list = new List<Document>();
            var data = await getData(startDateTime, startDateTime.AddDays(7));
            for (int i = 0; i < 7; i++) {
                var doc = data.Find(x => {
                    DateTime test;
                    if (DateTime.TryParseExact(x["timestamp"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out test))
                        return test.ToString("yyyyMMdd") == startDateTime.AddDays(i).ToString("yyyyMMdd");
                    return false;
                });
                if(doc != null)
                    list.Add(doc);
            }
            return list;
        }

        public async Task<List<Document>> get30Days(DateTime startDateTime) {
            var list = new List<Document>();
            var data = await getData(startDateTime, startDateTime.AddDays(30));
            for (int i = 0; i < 30; i++)
            {
                var doc = data.Find(x => {
                    DateTime test;
                    if (DateTime.TryParseExact(x["timestamp"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out test))
                        return test.ToString("yyyyMMdd") == startDateTime.AddDays(i).ToString("yyyyMMdd");
                    return false;
                });
                if (doc != null)
                    list.Add(doc);
            }
            return list;
        }

        public async Task<List<Document>> getData(DateTime startDateTime, DateTime endDateTime) {

            AttributeValue startAttr = new AttributeValue();
            startAttr.N = startDateTime.ToString("yyyyMMddHHmmss");
            Condition startCon = new Condition();
            startCon.ComparisonOperator = ComparisonOperator.GE;
            startCon.AttributeValueList.Add(startAttr);

            AttributeValue endAttr = new AttributeValue();
            endAttr.N = endDateTime.ToString("yyyyMMddHHmmss");
            Condition endCon = new Condition();
            endCon.ComparisonOperator = ComparisonOperator.GE;
            endCon.AttributeValueList.Add(endAttr);

            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("timestamp", startCon);
            scanFilter.AddCondition("timestamp", endCon);

            var config = new ScanOperationConfig();
            config.ConsistentRead = true;
            config.Filter = scanFilter;
            var search = table.Scan(config);
            var list = await search.GetNextSetAsync();
            list.Reverse();
            return list;
        }
    }
}
