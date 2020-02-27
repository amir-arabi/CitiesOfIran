using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Linq;

namespace CitiesOfIran
{
    class Program
    {
        private const string inputPath = @".\res\GEO97.xlsx";
        private const string outputPath = @".\res\CitiesOfIran.json";
        static void Main()
        {
            var keyValuePairs = new SortedDictionary<string, SortedSet<string>>();
            using var excelPackage = new ExcelPackage(new FileInfo(inputPath));
            var worksheet = excelPackage.Workbook.Worksheets["تقسیمات کشوری 1397"];
            string state, city;
            int count = 2;
            for (int i = 0; i < 33; i++)
            {
                var citiesSortedSet = new SortedSet<string>();
                do
                {
                    state = worksheet.Cells["A" + count].Text.Trim();
                    city = worksheet.Cells["B" + count].Text.Trim();
                    if (!string.IsNullOrEmpty(city))
                    {
                        citiesSortedSet.Add(city);
                    }
                    count++;
                } while (state == worksheet.Cells["A" + count].Text.Trim());
                if (!string.IsNullOrEmpty(state) && !keyValuePairs.ContainsKey(state))
                {
                    keyValuePairs.Add(state, citiesSortedSet);
                }
            }
            var models = new List<Model>();
            foreach (var (key, value) in keyValuePairs)
            {
                models.Add(new Model(key, value.ToList()));
            }
            File.WriteAllText(outputPath, JsonToUtf8String(ref models));
        }

        private static string JsonToUtf8String<T>(ref T t)
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            return Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(t, t.GetType(), jsonOptions));
        }
    }
}
