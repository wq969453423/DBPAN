using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DBPAN.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Net;

namespace DBPAN
{
    class QuartzJob : IJob
    {
        static string Cookie_1 = @"BIDUPSID=6DCF0AD69BFDBDCE9A29D7D6CB775A03; PSTM=1635331950; __yjs_duid=1_6635b9286b3d8c3b8c70308b04c2d7fb1635332686937; PANWEB=1; secu=1; pan_login_way=1; STOKEN=a8018f4da32fd2aa16dba4635fee3d1c21f1eec8faada9a28e8656c833a3f979; BAIDUID=6B1CF4BC0434C2BF5572680428637305:FG=1; BDORZ=FFFB88E999055A3F8A630C64834BD6D0; MCITY=-:; BAIDUID_BFESS=6B1CF4BC0434C2BF5572680428637305:FG=1; BA_HECTOR=2l8g840ka58l248hlb1h6k35g0q; BDCLND=yEPpzoELd3IZp7WbEFUL1nXfVjbbM2qftP7VGFVEMi0=; csrfToken=gOJNOk13izSXhYO9NaMJnEUi; Hm_lvt_7a3960b6f067eb0085b7f96ff5e660b0=1649662760,1649836402,1650961972,1651129185; PANPSC=11724610623255895207:eawLHinWIle3NrdyHuDDxjuv496EomYGNTrWoUCXsZTG/7Eb8llnhKPHXHumiLanw4oUalVAnBXEf2NR44l1ZH8jef7JsKoBEzh0OPEF2WlIt1giwoNsjMfZU3aFgpvQQ0baVohat62t6WY25u2uG0VqH25aAQJnHz1VonG3amuisT4vkN/x9KL5jVfu52v0baC532AuNi1GiBt6rgD9UiZTnz2RR6t+UiolHQD3rqJ89Veg5I7xCzuoAeCl9TBG; Hm_lpvt_7a3960b6f067eb0085b7f96ff5e660b0=1651129429; ndut_fmt=A6ED491C740CE0033B40A590DA3FB86788445DA7A6A23B7242CEDBD20B3A80E0; ab_sr=1.0.1_ODk0Y2VjYjA4MzVmYzk5Y2YzZDcwZGYxMGE1ZTNkNmNlMzc2YWVlODk1OTY2OWQ3Y2UzZmE2NmMzMTBiODAyMDg5ZWU3M2Y0OGJmZGEzNmMyN2JjNGJiM2E2MjRjZWFlNDM2YWRhYTYzN2FhYzYzZmQ2YTM0ZDNiNDNhN2Q1NzRiODY2MTQ5ZDkxMzMzMGNkMTM1ODhmMTFhMDIyM2U4Ng==";
        static string Cookie_2 = @"XFI=490e43ee-d1f2-b1d5-0a47-4325e6d9b67a; XFCS=3C99177759493363DFEB23DAD24AC610A87E3BC2BCC6A7DE6214B7D6868F26C1; BIDUPSID=6DCF0AD69BFDBDCE9A29D7D6CB775A03; PSTM=1651818484; PANWEB=1; BAIDUID=CCA87BFC02FB7AF65AC18D2FF1800894:FG=1; BDSFRCVID_BFESS=PVCOJeC62C4Fz9rDr21rK741ijTt0djTH6aoBG1W7c1fETg-6hCyEG0PLU8g0KubGrIvogKKLgOTHULF_2uxOjjg8UtVJeC6EG0Ptf8g0f5; H_BDCLCKID_SF_BFESS=tbCfoC8aJI_3jtOY5-__-4_tbh_X5-RLfaRp5h7F5l8-hl8mQRCBKx0XDGQHB5-JtC5B-P-M5ncxOKQphU5nDqbW-NQh0l3MHRc35-TN3KJm8MK9bT3v5tD1K4rq2-biW2JM2MbdQlRP_IoG2Mn8M4bb3qOpBtQmJeTxoUJ25DnJhbLGe4bK-TrBjH8etx5; BDORZ=FFFB88E999055A3F8A630C64834BD6D0; csrfToken=HnQ1j7ugPzP2vrwT86e-K5Op; Hm_lvt_7a3960b6f067eb0085b7f96ff5e660b0=1652855209,1652927615,1653554752,1654493845; BDCLND=%2FO%2BInqVsWH2497z7F7LhVIc76ccI6hrYcxwvf3wyLPQ%3D; H_PS_PSSID=31660_26350; delPer=0; PSINO=6; BAIDUID_BFESS=CCA87BFC02FB7AF65AC18D2FF1800894:FG=1; BA_HECTOR=0gag81258h2h2k0h8k1h9r6j615; BDRCVFR[Ter2S3H5o_D]=mk3SLVN4HKm; ZFY=RU2JpTPxGHRwGHZfY:AFqHmezGRmFg6tvabe4:A29nCHU:C; Hm_lpvt_7a3960b6f067eb0085b7f96ff5e660b0=1654497368; ndut_fmt=3A2F0396667F8556837084E50B725F88D0A7B9491381458638790BCFA66C37E2; ab_sr=1.0.1_ZDBiMTE2NWFiZjMzODdkZTU3NDNmMTBmNTMzYjdhMDc1NTlmYjA2M2RkYjRjZjZiMjk0NTA5YjczMTNlZTkyOTkyMTU3MjU2MzZjYzkwYzEzMjRiNGNhNzYwODhjYjAwNjNkOWM1MmE1OWIxM2NjZTE3MWE1ZmFhZWM4MTg4NDg4OWIzNGVhZDRiNWRmM2ZmZjI5YzE1OTgyNDA5MDBlNQ==";
        static string mainurl_v4_1 = "https://pan.baidu.com/share/list?uk=1102487041658&shareid=44750878449&order=other&desc=1&showempty=0&web=1&page=1&num=100&dir=%2Fsharelink1102487041658-418975818175612%2Fv4&t=0.40250305672514264&channel=chunlei&web=1&app_id=250528&bdstoken=&logid=NkIxQ0Y0QkMwNDM0QzJCRjU1NzI2ODA0Mjg2MzczMDU6Rkc9MQ==&clienttype=0&dp-logid=93351400152465930006";
        static string mainurl_v4_2 = "https://pan.baidu.com/share/list?uk=1100877385123&shareid=5250062146&order=other&desc=1&showempty=0&web=1&page=1&num=100&dir=%2FV4&t=0.07502994316364453&channel=chunlei&web=1&app_id=250528&bdstoken=&logid=Q0NBODdCRkMwMkZCN0FGNjVBQzE4RDJGRjE4MDA4OTQ6Rkc9MQ==&clienttype=0&dp-logid=89511200414542540020";

        static string Cookie_v3 = "XFI=7cc22f81-63aa-4698-edf0-03e5ce64be80; XFCS=A7AB6CF27FC915B97F9D4479F391AB78BBD331785514B6949B7ACBB9BCF2E8FC; XFT=f+IHPVjvjNvyHWnXJ46mStbk2PyVCviJK5z77YYYQok=; BIDUPSID=6DCF0AD69BFDBDCE9A29D7D6CB775A03; PSTM=1651818484; PANWEB=1; BAIDUID=CCA87BFC02FB7AF65AC18D2FF1800894:FG=1; BDSFRCVID_BFESS=PVCOJeC62C4Fz9rDr21rK741ijTt0djTH6aoBG1W7c1fETg-6hCyEG0PLU8g0KubGrIvogKKLgOTHULF_2uxOjjg8UtVJeC6EG0Ptf8g0f5; H_BDCLCKID_SF_BFESS=tbCfoC8aJI_3jtOY5-__-4_tbh_X5-RLfaRp5h7F5l8-hl8mQRCBKx0XDGQHB5-JtC5B-P-M5ncxOKQphU5nDqbW-NQh0l3MHRc35-TN3KJm8MK9bT3v5tD1K4rq2-biW2JM2MbdQlRP_IoG2Mn8M4bb3qOpBtQmJeTxoUJ25DnJhbLGe4bK-TrBjH8etx5; BDORZ=FFFB88E999055A3F8A630C64834BD6D0; csrfToken=HnQ1j7ugPzP2vrwT86e-K5Op; Hm_lvt_7a3960b6f067eb0085b7f96ff5e660b0=1652855209,1652927615,1653554752,1654493845; H_PS_PSSID=31660_26350; delPer=0; PSINO=6; BAIDUID_BFESS=CCA87BFC02FB7AF65AC18D2FF1800894:FG=1; BA_HECTOR=0gag81258h2h2k0h8k1h9r6j615; BDRCVFR[Ter2S3H5o_D]=mk3SLVN4HKm; ZFY=RU2JpTPxGHRwGHZfY:AFqHmezGRmFg6tvabe4:A29nCHU:C; BDCLND=ez8X7MpCpCwsygj1Bd7wppZEXUne5eEfLqiWWIz6AYo%3D; Hm_lpvt_7a3960b6f067eb0085b7f96ff5e660b0=1654501803; ndut_fmt=69E5FEBBB7F4AF0308A031789EEBCC828C5FC54EF3A7A369EE62CB53BEEEA4F2; ab_sr=1.0.1_MTc2NGFkZmNhZDQ1YWVmMDBhZWMwZDhmYzVmZTg2NGY2NmViZTk2NTUwZjRiZWNiODY1NWU1MzhlNWRiMzU0MTU4NzE3Njg2MmYwNzdkOTE4ZjE4Yjk0ZjNhZDNjNDRhY2YyMTkyMDMzZjhkMjczMDlhMTYyNzEzNDE1NDg4NGFlNThmY2NhZWE4YzY1NGNjMTk5OGVlM2FmMDAzM2FmNA==";
        static string mainurl_v3_1 = "https://pan.baidu.com/share/list?uk=1102520480527&shareid=58463903397&order=other&desc=1&showempty=0&web=1&page=1&num=100&dir=%2Fv3%2FBefore+repair&t=0.2804634661551815&channel=chunlei&web=1&app_id=250528&bdstoken=&logid=Q0NBODdCRkMwMkZCN0FGNjVBQzE4RDJGRjE4MDA4OTQ6Rkc9MQ==&clienttype=0&dp-logid=47824400477607920021";
        static string mainurl_v3_2 = "https://pan.baidu.com/share/list?uk=1102520480527&shareid=58463903397&order=other&desc=1&showempty=0&web=1&page=1&num=100&dir=%2Fv3%2FAfter+repair&t=0.2804634661551815&channel=chunlei&web=1&app_id=250528&bdstoken=&logid=Q0NBODdCRkMwMkZCN0FGNjVBQzE4RDJGRjE4MDA4OTQ6Rkc9MQ==&clienttype=0&dp-logid=47824400477607920021";
        static IDbConnection connection=null;
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {
                Console.WriteLine(DateTime.Now+"_开始_");
                await Main();
                Console.WriteLine(DateTime.Now + "_结束_");
            });
        }


        public async Task Main() {
            List<MainModel> listModel = new List<MainModel>();
            await GetBDPAN(listModel, mainurl_v4_1, "v4_1", Cookie_1);
            await GetBDPAN(listModel, mainurl_v4_2, "v4_2", Cookie_2);
            List<MainModel> listModel_v3 = new List<MainModel>();
            await GetBDPAN(listModel_v3, mainurl_v3_1, "v3", Cookie_v3);
            await GetBDPAN(listModel_v3, mainurl_v3_2, "v3", Cookie_v3);

            List<MainModel> inputDbModel = new List<MainModel>();
            decimal sizeGb = 0;
            await Task.Run(() =>
            {
                listModel.ForEach(e =>
                {
                    int index = e.path.IndexOf("/v4/");
                    int lostindex = e.path.LastIndexOf("/");
                    if (index > 0 && lostindex > 0)
                    {
                        e.folder = e.path.Substring(index + 4, lostindex - index - 4);
                    }
                    sizeGb = Math.Round(Convert.ToDecimal(e.size) / 1024 / 1024 / 1024, 3);
                    e.size = sizeGb.ToString();
                    e.tips = "v4";
                });
                listModel_v3.ForEach(e =>
                {
                    int index = e.path.IndexOf("/v3/");
                    int lostindex = e.path.LastIndexOf("/");
                    if (index > 0 && lostindex > 0)
                    {
                        e.folder = e.path.Substring(index + 4, lostindex - index - 4);
                    }
                    sizeGb = Math.Round(Convert.ToDecimal(e.size) / 1024 / 1024 / 1024, 3);
                    e.size = sizeGb.ToString();
                    e.tips = "v3";
                });
                inputDbModel.AddRange(listModel_v3);
                inputDbModel.AddRange(listModel);
            });


            sizeGb = listModel.Sum(e => decimal.Parse(e.size));
            await AddPanData(inputDbModel);
        }

        public static async Task GetBDPAN(List<MainModel> listModel, string mainurl, string type, string Cookie)
        {
            Console.WriteLine(DateTime.Now + "_进入读取操作_");
            var mainlist = await GetAsync(mainurl, "main", Cookie);
            for (int i = 0; i < mainlist.Count; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    string listurl = string.Empty;
                    if (type == "v4_1")
                    {
                        listurl = $@"https://pan.baidu.com/share/list?uk=1102487041658&shareid=44750878449&order=other&desc=1&showempty=0&web=1&page={j + 1}&num=100&dir={mainlist[i].path}&t=0.3699776405839794&channel=chunlei&web=1&app_id=250528&bdstoken=&logid=NkIxQ0Y0QkMwNDM0QzJCRjU1NzI2ODA0Mjg2MzczMDU6Rkc9MQ==&clienttype=0&dp-logid=23339100164593380021";
                    }
                    else if (type == "v4_2")
                    {
                        listurl = $@"https://pan.baidu.com/share/list?uk=1100877385123&shareid=5250062146&order=other&desc=1&showempty=0&web=1&page={j + 1}&num=100&dir={mainlist[i].path}&t=0.3699776405839794&channel=chunlei&web=1&app_id=250528&bdstoken=&logid=NkIxQ0Y0QkMwNDM0QzJCRjU1NzI2ODA0Mjg2MzczMDU6Rkc9MQ==&clienttype=0&dp-logid=23339100164593380021";
                    }
                    else if (type == "v3")
                    {
                        listurl = $@"https://pan.baidu.com/share/list?uk=1102520480527&shareid=58463903397&order=other&desc=1&showempty=0&web=1&page={j + 1}&num=100&dir={mainlist[i].path}&t=0.3699776405839794&channel=chunlei&web=1&app_id=250528&bdstoken=&logid=NkIxQ0Y0QkMwNDM0QzJCRjU1NzI2ODA0Mjg2MzczMDU6Rkc9MQ==&clienttype=0&dp-logid=23339100164593380021";
                    }
                    var list = await Task.Run(() => GetAsync(listurl, "main", Cookie));
                    listModel.AddRange(list);
                    if (list.Count < 100)
                    {
                        break;
                    }
                }
            }
        }

        public static async Task<List<MainModel>> GetAsync(string url, string type, string cookie)
        {

            try
            {
                string result = string.Empty;
                HttpWebRequest reqS = (HttpWebRequest)WebRequest.Create(url);
                reqS.Method = "GET";

                //添加请求头
                reqS.Headers.Add("Cookie", cookie);
                reqS.ContentType = "application/json; charset=utf-8";

                HttpWebResponse resS = (HttpWebResponse)reqS.GetResponse();
                Stream myResponseStream = resS.GetResponseStream();
                StreamReader streamReader = await Task.Run(() => { return new StreamReader(myResponseStream); });
                result = streamReader.ReadToEnd();
                streamReader.Close();
                myResponseStream.Close();

                JObject theResult = await Task.Run(() => { return (JObject)JsonConvert.DeserializeObject(result); });

                string finalResult = theResult["list"].ToString();

                List<MainModel> listS = await Task.Run(() => { return JsonConvert.DeserializeObject<List<MainModel>>(finalResult); });

                return listS;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task AddPanData(List<MainModel> listModel)
        {
            if (connection == null)
            {
                connection = CreateConnection();
            }
            using (var db = connection)
            {
                Console.WriteLine(DateTime.Now + "_进入数据库操作_");
                var Model = (await db.GetListAsync<MainModel>()).ToList();
                List<MainModel> addModel = new List<MainModel>();
                await Task.Run(() => addModel = listModel.Where(e => !Model.Any(c => c.server_filename == e.server_ctime && c.tips == e.tips)).ToList());
                string addstr = "insert into filesname values(null,@fs_id,@path,@category,@isdir,@server_filename,@size,@folder,@tips)";
                var res = await db.ExecuteAsync(addstr, addModel);
            }
        }


        public IDbConnection CreateConnection()
        {
            IDbConnection connection = new MySqlConnection("Server=101.33.200.33;Port=3306;Database=ar_base;Uid=root;Pwd=sqlking;pooling=false;Allow User Variables=True;");
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);//不然默认识别SQL server的语法，在MySQL中报错
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
    }
}
