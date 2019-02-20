using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtripForward;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string content1 = @"GET / HTTP/1.1
Accept: text/html, application/xhtml+xml, */*
Accept-Language: zh-CN
User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
Accept-Encoding: gzip, deflate
Host: 192.168.100.175:8003
DNT: 1
Connection: Keep-Alive

";

            string content2 = @"GET /RTCM32_GGB HTTP/1.1
User-Agent: NTRIP ZHDGPS
Accept: */*
Connection: close
Authorization: Basic aG5qeGN3MDAzNjY6NjAxMzczYw==

";

            string content2_1 = @"GET /RTCM32_GGB HTTP/1.1
User-Agent: NTRIP ZHDGPS
Accept: */*
Connection: close
Authorization: Basic dGVzdDE6dGVzdDE=

";
            string content3 = @"$GPGGA,063822.00,2807.6460170,N,11258.8251229,E,2,26,0.6,178.809,M,-13.809,M,260.8,3577*62" + Environment.NewLine;

            //UserDAL userDAL = new UserDAL();
            //USER user = userDAL.FindLoginUserByID(new Guid("25CCD307-B801-4251-BD00-CA2881E20314"));
            //Console.WriteLine(MsgHelper.CheckRequestType(content3));
            //Console.WriteLine(MsgHelper.GetMountPoint(content2));
            //Console.WriteLine(MsgHelper.GetUserAgent(content2));
            //Console.WriteLine(MsgHelper.ReplaceAuthorization(content2_1, "hnjxcw00366", "601373c").CompareTo(content2));
            //Console.WriteLine(MsgHelper.GetLng(content3));
            //Console.WriteLine(MsgHelper.GetLat(content3));
            Console.ReadLine();
            
        }
    }
}
