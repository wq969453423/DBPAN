using Dapper;
using DBPAN.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DBPAN
{
    class Program
    {
        static async Task Main(string[] args) {
            QuartzStartUp quartz = new QuartzStartUp();
            await quartz.Start();

            Console.ReadLine();
            for (int i = 0; i <= 0;)
            {
                Console.ReadLine();
            }
        }
    }
}
