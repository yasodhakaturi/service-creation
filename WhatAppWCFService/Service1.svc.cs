using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.ServiceModel.Activation;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Drawing.Imaging;
//using AntsCode.Util;
using System.Configuration;
using System.Collections;
using Newtonsoft.Json.Converters;
using System.Net.Mail;
using WhatsAppApi;
using WhatsAppPort;
using WhatAppWCFService;
using System.Windows.Forms;




namespace WhatAppWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string RequestCode(string number, out string password, string method)
        {
            //method = "sms";
            if (WhatsAppApi.Register.WhatsRegisterV2.RequestCode(number, out password, method))
            return  password;
            else
                return null;
        }
        public string RegisterCode(string number, string code)
        {
            
            string pwd = WhatsAppApi.Register.WhatsRegisterV2.RegisterCode(number, code);

            return pwd;
        }
        public string CheckLogin(string number, string password)
        {

            if (WhatsAppPort.frmLogin.CheckLogin(number, password))
                return "success";
            else
                return "fail";
         }

        public string UserExists(string number, string NickName)
          
         {

           var user = User.UserExists(number, NickName);

           return JsonConvert.SerializeObject(user);
          }
        
        
        public string SendMessage(string number, string Message)
        {

         string fulljid = WhatsAppApi.ApiBase.GetJID(number);

            try
            {
                //WhatSocket.Instance.SendMessage(fulljid, Message);

                string key = WhatsAppApi.WhatsApp.ISendMessage(fulljid, Message);
                return "Messsage Sent successfully";
            }
            catch
            {
                return "Message Delivary Failure";
            }
                
        }
    }
}
